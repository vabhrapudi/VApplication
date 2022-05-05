// <copyright file="AdminHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Graph;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Common.Services.Search.News;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Services.AdaptiveCard;
    using Teams.Apps.Athena.Services.MicrosoftGraph;

    /// <summary>
    /// Provides helper methods for managing approval request.
    /// </summary>
    public class AdminHelper : IAdminHelper
    {
        /// <summary>
        /// The instance of news model repository.
        /// </summary>
        private readonly INewsRepository newsRepository;

        /// <summary>
        /// The instance of community of interest model repository.
        /// </summary>
        private readonly ICoiRepository communityOfInterestRepository;

        /// <summary>
        /// The instance of COI model mapper.
        /// </summary>
        private readonly ICoiMapper coiMapper;

        /// <summary>
        /// The instance of news entity model mapper.
        /// </summary>
        private readonly INewsMapper newsMapper;

        /// <summary>
        /// The instance of COI search service.
        /// </summary>
        private readonly ICoiSearchService coiSearchService;

        /// <summary>
        /// The instance of News search service.
        /// </summary>
        private readonly INewsSearchService newsSearchService;

        private readonly ITeamService microsoftTeamsTeamService;

        /// <summary>
        /// Provides methods to fetch user conversation details.
        /// </summary>
        private readonly IUserBotConversationRepository userBotConversationRepository;

        /// <summary>
        /// Provides methods to generate adaptive cards.
        /// </summary>
        private readonly IAdaptiveCardService adaptiveCardService;

        /// <summary>
        /// Provides methods to send notification.
        /// </summary>
        private readonly INotificationHelper notificationHelper;

        private readonly IAdaptiveCardHelper adaptiveCardHelper;

        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminHelper"/> class.
        /// </summary>
        /// <param name="newsRepository">The instance of news repository accessors.</param>
        /// <param name="communityOfInterestRepository">The instance of coi repository accessors.</param>
        /// <param name="coiMapper">Coi request mapper.</param>
        /// <param name="newsMapper">News request mapper</param>
        /// <param name="coiSearchService">The instance of COI search service.</param>
        /// <param name="newsSearchService">The instance of News search service.</param
        /// <param name="microsoftTeamsTeamService">The instance of <see cref="TeamService"/>.</param>
        /// <param name="userBotConversationRepository">Provides methods to fetch user conversation details</param>
        /// <param name="adaptiveCardService">Provides methods to generate adaptive cards.</param>
        /// <param name="notificationHelper">Provides methods to send notification.</param>
        /// <param name="adaptiveCardHelper">The instance of <see cref="AdaptiveCardHelper"/> class.</param>
        /// <param name="userService">The instance of <see cref="UserService"/> class.</param>
        public AdminHelper(
            INewsRepository newsRepository,
            ICoiRepository communityOfInterestRepository,
            ICoiMapper coiMapper,
            INewsMapper newsMapper,
            ICoiSearchService coiSearchService,
            INewsSearchService newsSearchService,
            IUserBotConversationRepository userBotConversationRepository,
            IAdaptiveCardService adaptiveCardService,
            INotificationHelper notificationHelper,
            ITeamService microsoftTeamsTeamService,
            IAdaptiveCardHelper adaptiveCardHelper,
            IUserService userService)
        {
            this.newsRepository = newsRepository;
            this.communityOfInterestRepository = communityOfInterestRepository;
            this.coiMapper = coiMapper;
            this.newsMapper = newsMapper;
            this.coiSearchService = coiSearchService;
            this.newsSearchService = newsSearchService;
            this.userBotConversationRepository = userBotConversationRepository;
            this.adaptiveCardService = adaptiveCardService;
            this.notificationHelper = notificationHelper;
            this.microsoftTeamsTeamService = microsoftTeamsTeamService;
            this.adaptiveCardHelper = adaptiveCardHelper;
            this.userService = userService;
        }

        /// <summary>
        /// Gets coi request details by Id.
        /// </summary>
        /// <param name="requestId">Get coi request id.</param>
        /// <returns>Returns request details</returns>
        public async Task<CoiEntityDTO> GetCoiRequestByIdAsync(string requestId)
        {
            requestId = requestId ?? throw new ArgumentNullException(nameof(requestId));
            var response = await this.communityOfInterestRepository.GetAsync(CoiTableMetadata.PartitionKey, requestId);

            if (response == null)
            {
                return null;
            }

            return this.coiMapper.MapForViewModel(response);
        }

        /// <summary>
        /// Gets news request details by Id.
        /// </summary>
        /// <param name="requestId">Get news request id.</param>
        /// <returns>Returns request details</returns>
        public async Task<NewsEntityDTO> GetNewsRequestByIdAsync(string requestId)
        {
            requestId = requestId ?? throw new ArgumentNullException(nameof(requestId));
            var response = await this.newsRepository.GetAsync(NewsTableMetadata.NewsPartitionKey, requestId);

            if (response == null)
            {
                return null;
            }

            return this.newsMapper.MapForViewModel(response);
        }

        /// <inheritdoc/>
        public async Task<bool> ApproveOrRejectCoiRequestsAsync(IEnumerable<Guid> coiRequestIds, bool isApprove, string rejectComments)
        {
            var coiRequestsFilter = this.communityOfInterestRepository
                .GetRowKeysFilter(coiRequestIds.Select(requestId => requestId.ToString()));

            var coiRequests = await this.communityOfInterestRepository.GetWithFilterAsync(coiRequestsFilter);

            if (coiRequests.IsNullOrEmpty())
            {
                return false;
            }

            var pendingCoiRequests = coiRequests.Where(coiRequest => coiRequest.Status == (int)CoiRequestStatus.Pending);

            if (pendingCoiRequests.IsNullOrEmpty())
            {
                return false;
            }

            var requestsToBeUpdated = new List<CommunityOfInterestEntity>();

            foreach (var pendingCoiRequest in pendingCoiRequests)
            {
                if (isApprove)
                {
                    var teamId = await this.microsoftTeamsTeamService.CreateTeamAsync(
                                                pendingCoiRequest.CoiName.Trim(),
                                                pendingCoiRequest.CoiDescription.Trim(),
                                                pendingCoiRequest.Type == (int)CoiTeamType.Private ? TeamVisibilityType.Private : TeamVisibilityType.Public,
                                                Guid.Parse(pendingCoiRequest.CreatedByObjectId));

                    // Change status to 'Approved' only when the team was created successfully.
                    if (teamId == null)
                    {
                        continue;
                    }

                    pendingCoiRequest.Status = (int)RequestStatus.Approved;
                    pendingCoiRequest.TeamId = teamId;
                }
                else
                {
                    pendingCoiRequest.Status = (int)CoiRequestStatus.Rejected;
                    pendingCoiRequest.AdminComment = rejectComments;
                }

                pendingCoiRequest.UpdatedOn = DateTime.UtcNow;

                requestsToBeUpdated.Add(pendingCoiRequest);
            }

            await this.communityOfInterestRepository.BatchInsertOrMergeAsync(requestsToBeUpdated);
            await this.coiSearchService.RunIndexerOnDemandAsync();

            // Update adaptive cards.
            if (!requestsToBeUpdated.IsNullOrEmpty())
            {
                var userIds = requestsToBeUpdated.GroupBy(x => x.CreatedByObjectId).Where(x => !string.IsNullOrEmpty(x.Key)).Select(x => x.Key);
                var userDetails = await this.userService.GetUsersAsync(userIds);

                foreach (var request in requestsToBeUpdated)
                {
                    await this.adaptiveCardHelper.SendCoiRequestCardToCreatorAsync(request, true);

                    var userDisplayName = userDetails.Where(x => x.Id == request.CreatedByObjectId)?.FirstOrDefault();

                    if (userDisplayName != null)
                    {
                        await this.adaptiveCardHelper.SendNewCoiRequestCardToAdminTeamAsync(request, userDisplayName.DisplayName, true);
                    }
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> ApproveOrRejectNewsArticleRequestsAsync(IEnumerable<Guid> newsArticleRequestIds, bool isApprove, string rejectComments, bool? makeNewsArticleImportant = null)
        {
            var newsRequestsFilter = this.newsRepository
                .GetRowKeysFilter(newsArticleRequestIds.Select(requestId => requestId.ToString()));

            var newsArticleRequests = await this.newsRepository.GetWithFilterAsync(newsRequestsFilter);

            if (newsArticleRequests.IsNullOrEmpty())
            {
                return false;
            }

            var pendingNewsArticleRequests = newsArticleRequests
                .Where(newsArticleRequest => newsArticleRequest.Status == (int)NewsArticleRequestStatus.Pending);

            if (pendingNewsArticleRequests.IsNullOrEmpty())
            {
                return false;
            }

            var requestsToBeUpdated = new List<NewsEntity>();

            foreach (var pendingNewsArticleRequest in pendingNewsArticleRequests)
            {
                if (isApprove)
                {
                    pendingNewsArticleRequest.Status = (int)CoiRequestStatus.Approved;

                    if (makeNewsArticleImportant.HasValue)
                    {
                        pendingNewsArticleRequest.IsImportant = makeNewsArticleImportant.Value;
                    }
                }
                else
                {
                    pendingNewsArticleRequest.Status = (int)CoiRequestStatus.Rejected;
                    pendingNewsArticleRequest.AdminComment = rejectComments;
                }

                pendingNewsArticleRequest.UpdatedAt = DateTime.UtcNow;

                requestsToBeUpdated.Add(pendingNewsArticleRequest);
            }

            await this.newsRepository.BatchInsertOrMergeAsync(requestsToBeUpdated);
            await this.newsSearchService.RunIndexerOnDemandAsync();

            // Update adaptive cards.
            if (!requestsToBeUpdated.IsNullOrEmpty())
            {
                var userIds = requestsToBeUpdated.GroupBy(x => x.CreatedBy).Where(x => !string.IsNullOrEmpty(x.Key)).Select(x => x.Key);
                var userDetails = await this.userService.GetUsersAsync(userIds);

                foreach (var request in requestsToBeUpdated)
                {
                    await this.adaptiveCardHelper.SendNewsRequestCardToCreatorAsync(request, true);

                    var userDisplayName = userDetails.Where(x => x.Id == request.CreatedBy)?.FirstOrDefault();

                    if (userDisplayName != null)
                    {
                        await this.adaptiveCardHelper.SendNewNewsArticleRequestCardToAdminTeamAsync(request, userDisplayName.DisplayName, true);
                    }
                }
            }

            return true;
        }
    }
}