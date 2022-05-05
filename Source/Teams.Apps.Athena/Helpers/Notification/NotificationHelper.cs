// <copyright file="NotificationHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Logging;
    using Polly;
    using Polly.Contrib.WaitAndRetry;
    using Polly.Retry;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Manages notification related operations.
    /// </summary>
    public class NotificationHelper : INotificationHelper
    {
        /// <summary>
        /// Default value for channel activity to send notifications
        /// </summary>
        private const string TeamsBotChannelId = "msteams";

        /// <summary>
        /// Represents retry count
        /// </summary>
        private const int RetryCount = 2;

        /// <summary>
        /// Instance of IBot framework HTTP adapter.
        /// </summary>
        private readonly IBotFrameworkHttpAdapter botFrameworkHttpAdapter;

        /// <summary>
        /// Holds the Microsoft app credentials
        /// </summary>
        private readonly MicrosoftAppCredentials microsoftAppCredentials;

        /// <summary>
        /// Instance of logger to log event and errors.
        /// </summary>
        private readonly ILogger<NotificationHelper> logger;

        /// <summary>
        /// Retry policy with jitter, retry twice with a jitter delay of up to 1.5 sec. Retry for HTTP 429(transient error)/502 bad gateway.
        /// </summary>
        /// <remarks>
        /// Reference: https://github.com/Polly-Contrib/Polly.Contrib.WaitAndRetry#new-jitter-recommendation.
        /// </remarks>
        private readonly AsyncRetryPolicy retryPolicy = Policy.Handle<ErrorResponseException>(
            ex => ex.Response.StatusCode == HttpStatusCode.TooManyRequests || ex.Response.StatusCode == HttpStatusCode.BadGateway)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(1500), RetryCount));

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationHelper"/> class.
        /// </summary>
        /// <param name="botFrameworkHttpAdapter">The bot adapter.</param>
        /// <param name="microsoftAppCredentials">The Microsoft app credentials.</param>
        /// <param name="logger">Instance of logger to log event and errors.</param>
        public NotificationHelper(
            IBotFrameworkHttpAdapter botFrameworkHttpAdapter,
            MicrosoftAppCredentials microsoftAppCredentials,
            ILogger<NotificationHelper> logger)
        {
            this.botFrameworkHttpAdapter = botFrameworkHttpAdapter;
            this.microsoftAppCredentials = microsoftAppCredentials;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<string> SendNotificationInPersonalScopeAsync(Conversation userBotConversation, Attachment card, bool isUpdateCard = false, string activityId = null)
        {
            if (userBotConversation == null
                || string.IsNullOrWhiteSpace(userBotConversation.ConversationId)
                || string.IsNullOrWhiteSpace(userBotConversation.UserId.ToString())
                || string.IsNullOrWhiteSpace(userBotConversation.ServiceUrl)
                || card == null)
            {
                this.logger.LogError("Unable to send notification in personal scope as some of the conversation details are not available.");
                return null;
            }

            if (isUpdateCard && string.IsNullOrEmpty(activityId))
            {
                this.logger.LogError("Failed to update card as null or empty activity Id was received.");
                throw new ArgumentNullException(nameof(activityId), "Activity Id cannot be null in case of updating an existing card.");
            }

            string activityResourceId = null;

            try
            {
                MicrosoftAppCredentials.TrustServiceUrl(userBotConversation.ServiceUrl);

                var conversationReference = new ConversationReference()
                {
                    Bot = new ChannelAccount() { Id = $"28:{this.microsoftAppCredentials.MicrosoftAppId}" },
                    ChannelId = TeamsBotChannelId,
                    Conversation = new ConversationAccount() { Id = userBotConversation.ConversationId },
                    ServiceUrl = userBotConversation.ServiceUrl,
                };

                var botFrameworkAdapter = this.botFrameworkHttpAdapter as BotFrameworkAdapter;

                await this.retryPolicy.ExecuteAsync(async () =>
                {
                    await botFrameworkAdapter.ContinueConversationAsync(
                      this.microsoftAppCredentials.MicrosoftAppId,
                      conversationReference,
                      async (turnContext, cancellationToken) =>
                      {
                          if (isUpdateCard)
                          {
                              var activity = MessageFactory.Attachment(card);
                              activity.Id = activityId;

                              var resource = await turnContext.UpdateActivityAsync(activity);
                              activityResourceId = resource.Id;
                          }
                          else
                          {
                              var resource = await turnContext.SendActivityAsync(MessageFactory.Attachment(card), cancellationToken);
                              activityResourceId = resource.Id;
                          }
                      },
                      CancellationToken.None);
                });
            }
#pragma warning disable CA1031 // Catching general exception to continue sending notifications.
            catch (Exception ex)
#pragma warning restore CA1031 // Catching general exception to continue sending notifications.
            {
                this.logger.LogError(ex, $"Failed to send notification to user {userBotConversation.UserId} in personal scope.");
            }

            return activityResourceId;
        }

        /// <inheritdoc/>
        public async Task<string> SendNotificationInTeamScopeAsync(TeamEntity teamDetails, Attachment card, bool isUpdateCard = false, string activityId = null)
        {
            if (teamDetails == null
                || string.IsNullOrWhiteSpace(teamDetails.ServiceUrl)
                || string.IsNullOrWhiteSpace(teamDetails.TeamId))
            {
                this.logger.LogError("Some of the team details are not available in order to send notification in team scope.");
                throw new ArgumentNullException(nameof(teamDetails), "Some of the team details are not valid.");
            }

            if (card == null)
            {
                this.logger.LogError("The attachment card in not available in order to send notification in team scope.");
                throw new ArgumentNullException(nameof(card), "Card attachment cannot be null.");
            }

            if (isUpdateCard && string.IsNullOrEmpty(activityId))
            {
                this.logger.LogError("Failed to update card as null or empty activity Id was received.");
                throw new ArgumentNullException(nameof(activityId), "Activity Id cannot be null in case of updating an existing card.");
            }

            MicrosoftAppCredentials.TrustServiceUrl(teamDetails.ServiceUrl);

            var conversationReference = new ConversationReference()
            {
                ChannelId = TeamsBotChannelId,
                Bot = new ChannelAccount() { Id = this.microsoftAppCredentials.MicrosoftAppId },
                ServiceUrl = teamDetails.ServiceUrl,
                Conversation = new ConversationAccount() { ConversationType = ConversationType.Channel, IsGroup = true, Id = teamDetails.TeamId },
            };

            string activityResourceId = null;

            try
            {
                await (this.botFrameworkHttpAdapter as BotFrameworkAdapter).ContinueConversationAsync(
                    this.microsoftAppCredentials.MicrosoftAppId,
                    conversationReference,
                    async (conversationTurnContext, conversationCancellationToken) =>
                    {
                        if (isUpdateCard)
                        {
                            var activity = MessageFactory.Attachment(card);
                            activity.Id = activityId;

                            var resource = await conversationTurnContext.UpdateActivityAsync(activity);
                            activityResourceId = resource.Id;
                        }
                        else
                        {
                            var resource = await conversationTurnContext.SendActivityAsync(MessageFactory.Attachment(card));
                            activityResourceId = resource.Id;
                        }
                    },
                    CancellationToken.None);
            }
#pragma warning disable CA1031 // Catching general exception to continue sending notifications.
            catch (Exception ex)
#pragma warning restore CA1031 // Catching general exception to continue sending notifications.
            {
                this.logger.LogError(ex, $"Failed to send notification in team scope for team Id {teamDetails.TeamId}.");
            }

            return activityResourceId;
        }
    }
}
