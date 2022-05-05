// <copyright file="AdaptiveCardHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Athena.Models;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Models.Enums;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Services.AdaptiveCard;

    /// <summary>
    /// Provides helper methods related to adaptive cards.
    /// </summary>
    public class AdaptiveCardHelper : IAdaptiveCardHelper
    {
        private readonly IAdaptiveCardService adaptiveCardService;

        private readonly INotificationHelper notificationHelper;

        private readonly ITeamRepository teamRepository;

        private readonly IOptions<BotSettings> botOptions;

        private readonly IUserBotConversationRepository userBotConversationRepository;

        private readonly IActivityRepository activityRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveCardHelper"/> class.
        /// </summary>
        /// <param name="adaptiveCardService">The instance of <see cref="AdaptiveCardService"/>.</param>
        /// <param name="notificationHelper">The instance of <see cref="NotificationHelper"/>.</param>
        /// <param name="teamRepository">The instance of <see cref="TeamRepository"/>.</param>
        /// <param name="botOptions">The options for application configuration.</param>
        /// <param name="userBotConversationRepository">Provides methods to fetch user conversation details.</param>
        /// <param name="activityRepository">Instance of <see cref="ActivityRepository"/> class.</param>
        public AdaptiveCardHelper(
            IAdaptiveCardService adaptiveCardService,
            INotificationHelper notificationHelper,
            ITeamRepository teamRepository,
            IOptions<BotSettings> botOptions,
            IUserBotConversationRepository userBotConversationRepository,
            IActivityRepository activityRepository)
        {
            this.adaptiveCardService = adaptiveCardService;
            this.notificationHelper = notificationHelper;
            this.teamRepository = teamRepository;
            this.botOptions = botOptions;
            this.userBotConversationRepository = userBotConversationRepository;
            this.activityRepository = activityRepository;
        }

        /// <inheritdoc/>
        public Task<string> SendWelcomeCardInPersonalScope(UserBotConversationEntity userBotConversationDetails)
        {
            userBotConversationDetails = userBotConversationDetails ?? throw new ArgumentNullException(nameof(userBotConversationDetails));

            var welcomeCardAttachment = this.adaptiveCardService.GetWelcomeCard();

            var userBotConversation = new Conversation
            {
                UserId = Guid.Parse(userBotConversationDetails.UserId),
                ConversationId = userBotConversationDetails.ConversationId,
                ServiceUrl = userBotConversationDetails.ServiceUrl,
                BotInstalledOn = userBotConversationDetails.BotInstalledOn,
            };

            return this.notificationHelper.SendNotificationInPersonalScopeAsync(userBotConversation, welcomeCardAttachment);
        }

        /// <inheritdoc/>
        public async Task SendNewNewsArticleRequestCardToAdminTeamAsync(NewsEntity newsArticleDetails, string userName, bool isUpdateCard = false)
        {
            if (newsArticleDetails == null)
            {
                throw new ArgumentNullException(nameof(newsArticleDetails));
            }

            var teamDetails = await this.teamRepository.GetAsync(TeamTableMetadata.TeamPartitionKey, this.botOptions.Value.AdminTeamId);

            if (teamDetails == null)
            {
                return;
            }

            var newNewsArticleRequestAdaptiveCard = this.adaptiveCardService.GetNewNewsArticleRequestCard(newsArticleDetails, userName);

            if (isUpdateCard)
            {
                var filter = this.activityRepository.GetCustomColumnFilter(new[] { teamDetails.TeamId }, nameof(ActivityEntity.TeamId));
                var partitionKey = this.activityRepository.GetPartitionKey(newsArticleDetails.TableId, Itemtype.News);

                var activityCollection = await this.activityRepository
                    .GetWithFilterAsync(filter, partitionKey);

                var activityId = activityCollection?.FirstOrDefault()?.ActivityId;

                if (!string.IsNullOrEmpty(activityId))
                {
                    await this.notificationHelper
                        .SendNotificationInTeamScopeAsync(teamDetails, newNewsArticleRequestAdaptiveCard, true, activityId);
                }
            }
            else
            {
                // Send card to admin team.
                var activityId = await this.notificationHelper.SendNotificationInTeamScopeAsync(teamDetails, newNewsArticleRequestAdaptiveCard);

                if (!string.IsNullOrEmpty(activityId))
                {
                    var activity = new ActivityEntity
                    {
                        ActivityId = activityId,
                        ItemId = newsArticleDetails.TableId,
                        TeamId = teamDetails.TeamId,
                        ItemType = (int)Itemtype.News,
                    };

                    await this.activityRepository.CreateOrUpdateAsync(activity);
                }
            }
        }

        /// <inheritdoc/>
        public async Task SendNewCoiRequestCardToAdminTeamAsync(CommunityOfInterestEntity coiDetails, string userName, bool isUpdateCard = false)
        {
            if (coiDetails == null)
            {
                throw new ArgumentNullException(nameof(coiDetails));
            }

            var teamDetails = await this.teamRepository.GetAsync(TeamTableMetadata.TeamPartitionKey, this.botOptions.Value.AdminTeamId);

            if (teamDetails == null)
            {
                return;
            }

            var newCoiRequestAdaptiveCard = this.adaptiveCardService.GetNewCoiRequestCard(coiDetails, userName);

            if (isUpdateCard)
            {
                var filter = this.activityRepository.GetCustomColumnFilter(new[] { teamDetails.TeamId }, nameof(ActivityEntity.TeamId));
                var partitionKey = this.activityRepository.GetPartitionKey(coiDetails.TableId, Itemtype.COI);

                var activityCollection = await this.activityRepository
                    .GetWithFilterAsync(filter, partitionKey);

                var activityId = activityCollection?.FirstOrDefault()?.ActivityId;

                if (!string.IsNullOrEmpty(activityId))
                {
                    await this.notificationHelper
                        .SendNotificationInTeamScopeAsync(teamDetails, newCoiRequestAdaptiveCard, true, activityId);
                }
            }
            else
            {
                // Send card to admin team.
                var activityId = await this.notificationHelper.SendNotificationInTeamScopeAsync(teamDetails, newCoiRequestAdaptiveCard);

                if (!string.IsNullOrEmpty(activityId))
                {
                    var activity = new ActivityEntity
                    {
                        ActivityId = activityId,
                        ItemId = coiDetails.TableId,
                        TeamId = teamDetails.TeamId,
                        ItemType = (int)Itemtype.COI,
                    };

                    await this.activityRepository.CreateOrUpdateAsync(activity);
                }
            }
        }

        /// <inheritdoc/>
        public async Task SendCoiRequestCardToCreatorAsync(CommunityOfInterestEntity coiDetails, bool isUpdateCard = false)
        {
            if (coiDetails == null)
            {
                throw new ArgumentNullException(nameof(coiDetails));
            }

            var userDetails = await this.userBotConversationRepository.GetAsync(UserBotConversationTableMetadata.PartitionKey, coiDetails.CreatedByObjectId);

            if (userDetails == null)
            {
                return;
            }

            var newCoiRequestAdaptiveCard = this.adaptiveCardService.GetNewCoiRequestCard(coiDetails);

            var userBotConversation = new Conversation
            {
                UserId = Guid.Parse(userDetails.UserId),
                ConversationId = userDetails.ConversationId,
                ServiceUrl = userDetails.ServiceUrl,
            };

            if (isUpdateCard)
            {
                var filter = this.activityRepository.GetCustomColumnFilter(new[] { userDetails.UserId }, nameof(ActivityEntity.UserId));
                var partitionKey = this.activityRepository.GetPartitionKey(coiDetails.TableId, Itemtype.COI);

                var activityCollection = await this.activityRepository
                    .GetWithFilterAsync(filter, partitionKey);

                var activityId = activityCollection?.FirstOrDefault()?.ActivityId;

                if (!string.IsNullOrEmpty(activityId))
                {
                    await this.notificationHelper
                        .SendNotificationInPersonalScopeAsync(userBotConversation, newCoiRequestAdaptiveCard, true, activityId);
                }
            }
            else
            {
                // Send card to admin team.
                var activityId = await this.notificationHelper.SendNotificationInPersonalScopeAsync(userBotConversation, newCoiRequestAdaptiveCard);

                if (!string.IsNullOrEmpty(activityId))
                {
                    var activity = new ActivityEntity
                    {
                        ActivityId = activityId,
                        ItemId = coiDetails.TableId,
                        UserId = userDetails.UserId,
                        ItemType = (int)Itemtype.COI,
                    };

                    await this.activityRepository.CreateOrUpdateAsync(activity);
                }
            }
        }

        /// <inheritdoc/>
        public async Task SendNewsRequestCardToCreatorAsync(NewsEntity newsDetails, bool isUpdateCard = false)
        {
            if (newsDetails == null)
            {
                throw new ArgumentNullException(nameof(newsDetails));
            }

            var userDetails = await this.userBotConversationRepository.GetAsync(UserBotConversationTableMetadata.PartitionKey, newsDetails.CreatedBy);

            if (userDetails == null)
            {
                return;
            }

            var newNewsRequestAdaptiveCard = this.adaptiveCardService.GetNewNewsArticleRequestCard(newsDetails);

            var userBotConversation = new Conversation
            {
                UserId = Guid.Parse(userDetails.UserId),
                ConversationId = userDetails.ConversationId,
                ServiceUrl = userDetails.ServiceUrl,
            };

            if (isUpdateCard)
            {
                var filter = this.activityRepository.GetCustomColumnFilter(new[] { userDetails.UserId }, nameof(ActivityEntity.UserId));
                var partitionKey = this.activityRepository.GetPartitionKey(newsDetails.TableId, Itemtype.News);

                var activityCollection = await this.activityRepository
                    .GetWithFilterAsync(filter, partitionKey);

                var activityId = activityCollection?.FirstOrDefault()?.ActivityId;

                if (!string.IsNullOrEmpty(activityId))
                {
                    await this.notificationHelper
                        .SendNotificationInPersonalScopeAsync(userBotConversation, newNewsRequestAdaptiveCard, true, activityId);
                }
            }
            else
            {
                // Send card to admin team.
                var activityId = await this.notificationHelper.SendNotificationInPersonalScopeAsync(userBotConversation, newNewsRequestAdaptiveCard);

                if (!string.IsNullOrEmpty(activityId))
                {
                    var activity = new ActivityEntity
                    {
                        ActivityId = activityId,
                        ItemId = newsDetails.TableId,
                        UserId = userDetails.UserId,
                        ItemType = (int)Itemtype.News,
                    };

                    await this.activityRepository.CreateOrUpdateAsync(activity);
                }
            }
        }
    }
}