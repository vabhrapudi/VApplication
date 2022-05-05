// <copyright file="AthenaBotActivityHandler.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Bot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Athena.Models;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Resources;

    /// <summary>
    /// The ActivityHandler is responsible for reacting to incoming events from Teams sent from BotFramework.
    /// </summary>
    public sealed class AthenaBotActivityHandler : TeamsActivityHandler
    {
        /// <summary>
        /// A set of key/value application configuration properties for Activity settings.
        /// </summary>
        private readonly IOptions<BotSettings> botOptions;

        /// <summary>
        /// Instance to send logs to the Application Insights service.
        /// </summary>
        private readonly ILogger<AthenaBotActivityHandler> logger;

        /// <summary>
        /// The current cultures' string localizer.
        /// </summary>
        private readonly IStringLocalizer<Strings> localizer;

        /// <summary>
        /// Instance of Application Insights Telemetry client.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Provides helper methods for bot related activities.
        /// </summary>
        private readonly IAthenaBotActivityHandlerHelper activityHandlerHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaBotActivityHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="localizer">The current cultures' string localizer.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client. </param>
        /// <param name="options">The options.</param>
        /// <param name="activityHandlerHelper">Provides helper methods for bot related activities.</param>
        public AthenaBotActivityHandler(
            ILogger<AthenaBotActivityHandler> logger,
            IStringLocalizer<Strings> localizer,
            TelemetryClient telemetryClient,
            IOptions<BotSettings> options,
            IAthenaBotActivityHandlerHelper activityHandlerHelper)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.telemetryClient = telemetryClient;
            this.botOptions = options;
            this.activityHandlerHelper = activityHandlerHelper;
        }

        /// <summary>
        /// Invoked when members other than this bot (like a user) are removed from the conversation.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
                this.RecordEvent(nameof(this.OnConversationUpdateActivityAsync), turnContext);

                var activity = turnContext.Activity;
                this.logger.LogInformation($"conversationType: {activity.Conversation.ConversationType}, membersAdded: {activity.MembersAdded?.Count}, membersRemoved: {activity.MembersRemoved?.Count}");

                if (activity.Conversation.ConversationType == ConversationTypes.Personal)
                {
                    if (activity.MembersAdded != null && activity.MembersAdded.Any(member => member.Id == activity.Recipient.Id))
                    {
                        await this.activityHandlerHelper.OnBotInstalledInPersonalAsync(turnContext);
                    }
                    else if (activity.MembersRemoved != null && activity.MembersRemoved.Any(member => member.Id == activity.Recipient.Id))
                    {
                        await this.activityHandlerHelper.OnBotUninstalledInPersonalAsync(turnContext);
                    }
                }
                else if (activity.Conversation.ConversationType == ConversationTypes.Channel)
                {
                    if (activity.MembersAdded != null && activity.MembersAdded.Any(member => member.Id == activity.Recipient.Id))
                    {
                        await this.activityHandlerHelper.OnBotInstalledInTeamAsync(turnContext);
                    }
                    else if (activity.MembersRemoved != null && activity.MembersRemoved.Any(member => member.Id == activity.Recipient.Id))
                    {
                        await this.activityHandlerHelper.OnBotUninstalledFromTeamAsync(turnContext);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Exception occurred while bot conversation update event.");
                throw;
            }
        }

        /// <inheritdoc/>
        protected override async Task<TaskModuleResponse> OnTeamsTaskModuleFetchAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));

            try
            {
                this.RecordEvent(nameof(this.OnTeamsTaskModuleFetchAsync), turnContext);
                return await this.activityHandlerHelper.OnTaskModuleFetchRequestAsync(turnContext, taskModuleRequest);
            }
#pragma warning disable CA1031 // Catching general exception to redirect user to error page.
            catch (Exception ex)
#pragma warning restore CA1031 // Catching general exception to redirect user to error page.
            {
                this.logger.LogError(ex, $"Error while opening task module for user Id {turnContext.Activity.From.AadObjectId}");
                return this.activityHandlerHelper.GetTaskModuleResponse(new Uri($"{this.botOptions.Value.AppBaseUri}/error"), this.localizer.GetString("TaskModuleErrorTitle"));
            }
        }

        /// <summary>
        /// Handle task module submit action.
        /// </summary>
        /// <param name="turnContext">turn context.</param>
        /// <param name="taskModuleRequest">task module request.</param>
        /// <param name="cancellationToken">cancellation token.</param>
        /// <returns>task module response.</returns>
        protected override async Task<TaskModuleResponse> OnTeamsTaskModuleSubmitAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            return default;
        }

        /// <summary>
        /// Records event data to Application Insights telemetry client
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="turnContext">Provides context for a turn in a bot.</param>
        private void RecordEvent(string eventName, ITurnContext turnContext)
        {
            var teamsChannelData = turnContext.Activity.GetChannelData<TeamsChannelData>();

            this.telemetryClient.TrackEvent(eventName, new Dictionary<string, string>
            {
                { "userId", turnContext.Activity.From.AadObjectId },
                { "tenantId", turnContext.Activity.Conversation.TenantId },
                { "teamId", teamsChannelData?.Team?.Id },
                { "channelId", teamsChannelData?.Channel?.Id },
            });
        }
    }
}