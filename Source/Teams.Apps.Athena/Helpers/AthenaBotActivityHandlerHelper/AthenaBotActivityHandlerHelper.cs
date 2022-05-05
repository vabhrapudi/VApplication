// <copyright file="AthenaBotActivityHandlerHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Athena.Models;
    using Newtonsoft.Json.Linq;
    using Teams.Apps.Athena.Bot;
    using Teams.Apps.Athena.Common;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Resources;
    using UserRequestType = Teams.Apps.Athena.Common.Models.UserRequestType;

    /// <summary>
    /// Helper for handling bot related activities.
    /// </summary>
    public class AthenaBotActivityHandlerHelper : IAthenaBotActivityHandlerHelper
    {
        /// <summary>
        /// Instance to send logs to the Application Insights service.
        /// </summary>
        private readonly ILogger<AthenaBotActivityHandler> logger;

        /// <summary>
        /// Provides insert and delete operations for team entity.
        /// </summary>
        private readonly ITeamRepository teamRepository;

        /// <summary>
        /// Provides insert and delete operations for user-Bot conversation entity.
        /// </summary>
        private readonly IUserBotConversationRepository userBotConversationRepository;

        /// <summary>
        /// Provides helper methods for bot adaptive card.
        /// </summary>
        private readonly IAdaptiveCardHelper adaptiveCardHelper;

        /// <summary>
        /// The Bot options.
        /// </summary>
        private readonly IOptions<BotSettings> botOptions;

        /// <summary>
        /// The current cultures' string localizer.
        /// </summary>
        private readonly IStringLocalizer<Strings> localizer;

        /// <summary>
        /// The instance of <see cref="UserSettingsHelper"/> class.
        /// </summary>
        private readonly IUserSettingsHelper userSettingsHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaBotActivityHandlerHelper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="teamRepository">Provides insert and delete operations for team configuration entity.</param>
        /// <param name="userBotConversationRepository">The instance of <see cref="UserBotConversationRepository"/>.</param>
        /// <param name="adaptiveCardHelper">Card helper.</param>
        /// <param name="botOptions">The Bot options.</param>
        /// <param name="localizer">The current culture's string localizer.</param>
        /// <param name="userSettingsHelper">The instance of <see cref="UserSettingsHelper"/> class.</param>
        public AthenaBotActivityHandlerHelper(
            ILogger<AthenaBotActivityHandler> logger,
            ITeamRepository teamRepository,
            IUserBotConversationRepository userBotConversationRepository,
            IAdaptiveCardHelper adaptiveCardHelper,
            IOptions<BotSettings> botOptions,
            IStringLocalizer<Strings> localizer,
            IUserSettingsHelper userSettingsHelper)
        {
            this.logger = logger;
            this.teamRepository = teamRepository;
            this.userBotConversationRepository = userBotConversationRepository;
            this.adaptiveCardHelper = adaptiveCardHelper;
            this.botOptions = botOptions;
            this.localizer = localizer;
            this.userSettingsHelper = userSettingsHelper;
        }

        /// <inheritdoc/>
        public async Task OnBotInstalledInPersonalAsync(ITurnContext<IConversationUpdateActivity> turnContext)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext), "Turncontext cannot be null.");

            this.logger.LogInformation($"Installing Bot in personal scope for user {turnContext.Activity.From.AadObjectId}.");

            var activity = turnContext.Activity;
            var userDetails = await TeamsInfo.GetMemberAsync(turnContext, activity.From.Id);

            var userBotConversation = new UserBotConversationEntity
            {
                UserId = userDetails.AadObjectId,
                ConversationId = activity.Conversation.Id,
                ServiceUrl = activity.ServiceUrl,
                BotInstalledOn = DateTime.UtcNow,
            };

            var responseEntity = await this.userBotConversationRepository.CreateOrUpdateAsync(userBotConversation);

            if (responseEntity == null)
            {
                this.logger.LogInformation($"Failed to install Bot in personal scope for user {turnContext.Activity.From.AadObjectId}.");
                return;
            }

            this.logger.LogInformation($"Bot successfully installed in personal scope for user {turnContext.Activity.From.AadObjectId}.");

            var activityId = await this.adaptiveCardHelper.SendWelcomeCardInPersonalScope(userBotConversation);

            if (activityId == null)
            {
                this.logger.LogError($"Failed to send welcome card to user {userDetails.Id} after bot is installed.");
            }
            else
            {
                this.logger.LogInformation($"Sent welcome card to user {userDetails.Id} after bot is installed.");
            }
        }

        /// <inheritdoc/>
        public async Task OnBotInstalledInTeamAsync(ITurnContext<IConversationUpdateActivity> turnContext)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));

            var activity = turnContext.Activity;

            // Storing team information to storage.
            var teamsDetails = activity.TeamsGetTeamInfo();

            if (teamsDetails == null)
            {
                this.logger.LogInformation($"Unable to store bot installation state in storage. The team details are not available in Bot conversation activity.");
                return;
            }

            this.logger.LogInformation($"Installing Bot in team {teamsDetails.Id}.");

            TeamEntity teamEntity = new TeamEntity
            {
                TeamId = teamsDetails.Id,
                BotInstalledOn = DateTime.UtcNow,
                ServiceUrl = activity.ServiceUrl,
                ConversationId = activity.Conversation.Id,
                GroupId = teamsDetails.AadGroupId,
            };

            var teamDetails = await this.teamRepository.CreateOrUpdateAsync(teamEntity);

            if (teamDetails == null)
            {
                this.logger.LogInformation($"Failed to install Bot in team {teamsDetails.Id}.");
                return;
            }

            this.logger.LogInformation($"Bot successfully installed in team {teamsDetails.Id}.");
        }

        /// <inheritdoc/>
        public async Task OnBotUninstalledFromTeamAsync(ITurnContext<IConversationUpdateActivity> turnContext)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext), "Turncontext cannot be null.");

            var teamsChannelData = turnContext.Activity.GetChannelData<TeamsChannelData>();
            var teamId = teamsChannelData.Team.Id;

            this.logger.LogInformation($"Uninstalling Bot from team Id {teamId}.");

            try
            {
                var teamEntity = await this.teamRepository.GetAsync(TeamTableMetadata.TeamPartitionKey, teamId);
                if (teamEntity == null)
                {
                    this.logger.LogError($"Failed to uninstall Bot from team '{teamId}' as team details are not found.");
                    return;
                }

                // Deleting team information from storage when bot is uninstalled from a team.
                await this.teamRepository.DeleteAsync(teamEntity);
                this.logger.LogInformation($"Bot uninstalled from team Id {teamId}.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to delete team details from storage for team {teamId} while uninstalling Bot.");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<TaskModuleResponse> OnTaskModuleFetchRequestAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
            taskModuleRequest = taskModuleRequest ?? throw new ArgumentNullException(nameof(taskModuleRequest));

            var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, CancellationToken.None);

            if (member == null)
            {
                return this.GetTaskModuleResponse(new Uri($"{this.botOptions.Value.AppBaseUri}/error"), this.localizer.GetString("TaskModuleErrorTitle"));
            }

            var activityValue = JObject.FromObject(taskModuleRequest.Data);
            var adaptiveCardData = activityValue?.ToObject<AdaptiveCardSubmitActionData>();

            if (adaptiveCardData == null)
            {
                this.logger.LogInformation("Value obtained from task module fetch action is null.");
            }

            var botCommand = adaptiveCardData.BotCommand;

            switch (botCommand)
            {
                case BotCommand.ViewCoiRequestDetails:
                    var taskModuleRequestUrl = new Uri($"{this.botOptions.Value.AppBaseUri}{Routes.GetApprovedOrRejectRequestRoute(adaptiveCardData.CoiTableId, UserRequestType.CoiRequest)}");
                    return this.GetTaskModuleResponse(taskModuleRequestUrl, this.localizer.GetString("ViewCoiDetailsTaskModuleTitle"));

                case BotCommand.ViewNewsArticleRequestDetails:
                    taskModuleRequestUrl = new Uri($"{this.botOptions.Value.AppBaseUri}{Routes.GetApprovedOrRejectRequestRoute(adaptiveCardData.NewsTableId, UserRequestType.NewsArticleRequest)}");
                    return this.GetTaskModuleResponse(taskModuleRequestUrl, this.localizer.GetString("ViewNewsArticleDetailsTaskModuleTitle"));

                case BotCommand.ViewReadonlyCoiRequestDetails:
                    taskModuleRequestUrl = new Uri($"{this.botOptions.Value.AppBaseUri}{Routes.GetReadonlyRequestRoute(adaptiveCardData.CoiTableId, UserRequestType.CoiRequest)}");
                    return this.GetTaskModuleResponse(taskModuleRequestUrl, this.localizer.GetString("ViewCoiDetailsTaskModuleTitle"));

                case BotCommand.ViewReadonlyNewsArticleRequestDetails:
                    taskModuleRequestUrl = new Uri($"{this.botOptions.Value.AppBaseUri}{Routes.GetReadonlyRequestRoute(adaptiveCardData.NewsTableId, UserRequestType.NewsArticleRequest)}");
                    return this.GetTaskModuleResponse(taskModuleRequestUrl, this.localizer.GetString("ViewNewsArticleDetailsTaskModuleTitle"));

                default:
                    return this.GetTaskModuleResponse(new Uri($"{this.botOptions.Value.AppBaseUri}{Routes.ErrorPage}"), this.localizer.GetString("TaskModuleErrorTitle"));
            }
        }

        /// <inheritdoc/>
        public TaskModuleResponse GetTaskModuleResponse(Uri url, string title, int height = 600, int width = 600)
        {
            return new TaskModuleResponse
            {
                Task = new TaskModuleContinueResponse
                {
                    Value = new TaskModuleTaskInfo()
                    {
                        Url = url?.ToString(),
                        Height = height,
                        Width = width,
                        Title = title,
                    },
                },
            };
        }

        /// <inheritdoc/>
        public async Task OnBotUninstalledInPersonalAsync(ITurnContext<IConversationUpdateActivity> turnContext)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext), "Turncontext cannot be null.");

            var userAadId = turnContext.Activity.From.AadObjectId;

            this.logger.LogInformation($"Uninstalling Bot in personal scope for user {userAadId}.");

            try
            {
                var userDetails = await this.userBotConversationRepository.GetAsync(UserBotConversationTableMetadata.PartitionKey, userAadId);

                if (userDetails == null)
                {
                    this.logger.LogInformation($"Failed to uninstalled Bot in personal scope as details weren't found for user {userAadId}.");
                    return;
                }

                await this.userBotConversationRepository.DeleteAsync(userDetails);
                await this.userSettingsHelper.DeleteUserSettingsAsync(userAadId);

                this.logger.LogInformation($"Uninstalled Bot in personal scope for user {userAadId}.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Something went wrong while uninstalling Bot for user {userAadId}.");
                throw;
            }
        }
    }
}