// <copyright file="DiscoveryTreeHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Services.Keywords;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Services.MicrosoftGraph;

    /// <summary>
    /// The helper methods related to discovery tree.
    /// </summary>
    public class DiscoveryTreeHelper : IDiscoveryTreeHelper
    {
        private const string KeywordsSeparator = " ";
        private const string SemicolonSeparator = ";";
        private const string DbFieldTypeString = "STRING";
        private const string DbFieldTypeInteger = "INTEGER";
        private const string DbFieldTypeArray = "ARRAY";
        private const string DbFieldTypeDate = "DATE";

        /// <summary>
        /// The filter Id for news filter in AthenaFilters.json.
        /// </summary>
        private const int NewsFilterId = 131;

        /// <summary>
        /// The filter Id for communities filter in AthenaFilters.json.
        /// </summary>
        private const int CommunitiesFilterId = 155;

        /// <summary>
        /// The filter Id for partners filter in AthenaFilters.json.
        /// </summary>
        private const int PartnersFilterId = 169;

        /// <summary>
        /// The filter Id for events filter in AthenaFilters.json.
        /// </summary>
        private const int EventsFilterId = 159;

        /// <summary>
        /// The filter Id for personnel filter in AthenaFilters.json.
        /// </summary>
        private const int PersonnelFilterId = 110;

        /// <summary>
        /// The filter Id for sponsors filter in AthenaFilters.json.
        /// </summary>
        private const int SponsorsFilterId = 8;

        /// <summary>
        /// The filter Id for other information resources filter in AthenaFilters.json.
        /// </summary>
        private const int OtherInformationResourcesFilterId = 120;

        /// <summary>
        /// The filter Id for tools and software filter in AthenaFilters.json.
        /// </summary>
        private const int ToolsAndSoftwareFilterId = 166;

        /// <summary>
        /// The 'All' filter query.
        /// </summary>
        private const string AllFilterQuery = "All eq -1";

        /// <summary>
        /// The instance of <see cref="FilterQueryHelper"/> class.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// The instance of <see cref="SponsorHelper"/> class.
        /// </summary>
        private readonly ISponsorHelper sponsorHelper;

        /// <summary>
        /// The instance of <see cref="UsersSearchService"/> class.
        /// </summary>
        private readonly IUsersSearchService usersSearchService;

        /// <summary>
        /// The instance of <see cref="DiscoveryTreeFiltersBlobRepository"/> class.
        /// </summary>
        private readonly IDiscoveryTreeFiltersBlobRepository discoveryTreeFiltersBlobRepository;

        /// <summary>
        /// The instance of <see cref="NodeTypeForDiscoveryTreeBlobRepository"/> class.
        /// </summary>
        private readonly INodeTypeForDiscoveryTreeBlobRepository nodeTypeForDiscoveryTreeBlobRepository;

        /// <summary>
        /// The instance of <see cref="UserRepository"/> class.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// The instance of <see cref="UserService"/> class.
        /// </summary>
        private readonly IUserService userGraphService;

        /// <summary>
        /// The instance of <see cref="UserGraphServiceMapper"/> class.
        /// </summary>
        private readonly IUserGraphServiceMapper userGraphServiceMapper;

        /// <summary>
        /// The instance of <see cref="KeywordsSearchService"/> class.
        /// </summary>
        private readonly IKeywordsSearchService keywordsSearchServices;

        /// <summary>
        /// Holds the instance of <see cref="ResearchProjectHelper"/> class.
        /// </summary>
        private readonly IResearchProjectHelper researchProjectHelper;

        /// <summary>
        /// Holds the instance of <see cref="ResearchRequestHelper"/> class.
        /// </summary>
        private readonly IResearchRequestHelper researchRequestHelper;

        /// <summary>
        /// Holds the instance of <see cref="PartnerMapper"/> class.
        /// </summary>
        private readonly IPartnerHelper partnerHelper;

        /// <summary>
        /// Holds the instance of <see cref="EventHelper"/> class.
        /// </summary>
        private readonly IEventHelper eventHelper;

        /// <summary>
        /// Holds the instance of <see cref="ResearchProposalHelper"/> class.
        /// </summary>
        private readonly IResearchProposalHelper researchProposalHelper;

        /// <summary>
        /// Holds the instance of <see cref="CoiHelper"/> class.
        /// </summary>
        private readonly ICoiHelper coiHelper;

        /// <summary>
        /// Holds the instance of <see cref="NewsHelper"/> class.
        /// </summary>
        private readonly INewsHelper newsHelper;

        /// <summary>
        /// Holds the instance of <see cref="UserSettingsHelper"/> class.
        /// </summary>
        private readonly IUserSettingsHelper userSettingsHelper;

        /// <summary>
        /// Holds the instance of <see cref="AthenaInfoResourcesHelper"/> class.
        /// </summary>
        private readonly IAthenaInfoResourcesHelper athenaInfoResourcesHelper;

        /// <summary>
        /// Holds the instance of <see cref="AthenaToolHelper"/> class.
        /// </summary>
        private readonly IAthenaToolHelper athenaToolHelper;

        /// <summary>
        /// Holds the instance of <see cref="UserBotConversationRepository"/> class.
        /// </summary>
        private readonly IUserBotConversationRepository userBotConversationRepository;

        /// <summary>
        /// Holds the instance of <see cref="UserGraphServiceHelper"/> class.
        /// </summary>
        private readonly IUserGraphServiceHelper userGraphServiceHelper;

        /// <summary>
        /// Holds the instance of <see cref="UserSettingsMapper"/> class.
        /// </summary>
        private readonly IUserSettingsMapper userSettingsMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryTreeHelper"/> class.
        /// </summary>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="usersSearchService">The instance of <see cref="UsersSearchService"/> class.</param>
        /// <param name="nodeTypeForDiscoveryTreeBlobRepository">The instance of <see cref="NodeTypeForDiscoveryTreeBlobRepository"/> class.</param>
        /// <param name="discoveryTreeFiltersBlobRepository">The instance of <see cref="DiscoveryTreeFiltersBlobRepository"/> class.</param>
        /// <param name="userGraphService">The instance of <see cref="UserService"/>.</param>
        /// <param name="userGraphServiceMapper">The instance of <see cref="UserGraphServiceMapper"/>.</param>
        /// <param name="userRepository">The instance of user repository accessors.</param>
        /// <param name="keywordsSearchServices">The instance of keywords search services.</param>
        /// <param name="sponsorHelper">The instance of <see cref="SponsorHelper"/> class.</param>
        /// <param name="researchProjectHelper">The instance of <see cref="ResearchProjectHelper"/> class.</param>
        /// <param name="researchRequestHelper">The instance of <see cref="ResearchRequestHelper"/> class.</param>
        /// <param name="partnerHelper">The instance of <see cref="PartnerHelper"/> class.</param>
        /// <param name="eventHelper">The instance of <see cref="EventHelper"/> class.</param>
        /// <param name="researchProposalHelper">The instance of <see cref="ResearchProposalHelper"/> class.</param>
        /// <param name="coiHelper">The instance of <see cref="CoiHelper"/> class.</param>
        /// <param name="newsHelper">The instance of <see cref="NewsHelper"/> class.</param>
        /// <param name="userSettingsHelper">The instance of <see cref="UserSettingsHelper"/> class.</param>
        /// <param name="athenaInfoResourcesHelper">The instance of <see cref="AthenaInfoResourcesHelper"/> class.</param>
        /// <param name="athenaToolHelper">The instance of <see cref="AthenaToolHelper"/> class.</param>
        /// <param name="userBotConversationRepository">The instance of <see cref="UserBotConversationRepository"/> class.</param>
        /// <param name="userGraphServiceHelper">The instance of <see cref="UserGraphServiceHelper"/> class.</param>
        /// <param name="userSettingsMapper">The instance of <see cref="UserSettingsMapper"> class.</param>
        public DiscoveryTreeHelper(
            IFilterQueryHelper filterQueryHelper,
            IUsersSearchService usersSearchService,
            IUserService userGraphService,
            IUserGraphServiceMapper userGraphServiceMapper,
            IUserRepository userRepository,
            IKeywordsSearchService keywordsSearchServices,
            INodeTypeForDiscoveryTreeBlobRepository nodeTypeForDiscoveryTreeBlobRepository,
            IDiscoveryTreeFiltersBlobRepository discoveryTreeFiltersBlobRepository,
            ISponsorHelper sponsorHelper,
            IResearchProjectHelper researchProjectHelper,
            IResearchRequestHelper researchRequestHelper,
            IPartnerHelper partnerHelper,
            IEventHelper eventHelper,
            IResearchProposalHelper researchProposalHelper,
            ICoiHelper coiHelper,
            INewsHelper newsHelper,
            IUserSettingsHelper userSettingsHelper,
            IAthenaInfoResourcesHelper athenaInfoResourcesHelper,
            IUserBotConversationRepository userBotConversationRepository,
            IUserGraphServiceHelper userGraphServiceHelper,
            IUserSettingsMapper userSettingsMapper,
            IAthenaToolHelper athenaToolHelper)
        {
            this.filterQueryHelper = filterQueryHelper;
            this.usersSearchService = usersSearchService;
            this.discoveryTreeFiltersBlobRepository = discoveryTreeFiltersBlobRepository;
            this.userGraphService = userGraphService;
            this.userGraphServiceMapper = userGraphServiceMapper;
            this.userRepository = userRepository;
            this.keywordsSearchServices = keywordsSearchServices;
            this.nodeTypeForDiscoveryTreeBlobRepository = nodeTypeForDiscoveryTreeBlobRepository;
            this.sponsorHelper = sponsorHelper;
            this.researchProjectHelper = researchProjectHelper;
            this.researchRequestHelper = researchRequestHelper;
            this.partnerHelper = partnerHelper;
            this.eventHelper = eventHelper;
            this.researchProposalHelper = researchProposalHelper;
            this.coiHelper = coiHelper;
            this.newsHelper = newsHelper;
            this.userSettingsHelper = userSettingsHelper;
            this.athenaInfoResourcesHelper = athenaInfoResourcesHelper;
            this.athenaToolHelper = athenaToolHelper;
            this.userBotConversationRepository = userBotConversationRepository;
            this.userGraphServiceHelper = userGraphServiceHelper;
            this.userSettingsMapper = userSettingsMapper;
        }

        /// <summary>
        /// Gets a discovery tree filters from blob storage.
        /// </summary>
        /// <returns>A task representing get discovery tree node data.</returns>
        public async Task<IEnumerable<DiscoveryTreeFilterItems>> GetDiscoveryTreeFilters()
        {
            return await this.discoveryTreeFiltersBlobRepository.GetBlobJsonFileContentAsync(DiscoveryTreeFiltersBlobMetadata.FileName);
        }

        /// <inheritdoc/>
        public async Task<DiscoveryTreeData> GetDiscoveryTreeNodeData(IEnumerable<int> keywords)
        {
            var discoveryTreeNodeData = new DiscoveryTreeData();

            if (keywords.IsNullOrEmpty())
            {
                return discoveryTreeNodeData;
            }

            // Get research projects.
            discoveryTreeNodeData.ResearchProjects = await this.researchProjectHelper.GetResearchProjectsByKeywordsAsync(keywords);

            // Get research requests.
            discoveryTreeNodeData.ResearchRequests = await this.researchRequestHelper.GetResearchRequestsByKeywordsAsync(keywords);

            // Get sponsors.
            discoveryTreeNodeData.Sponsors = await this.sponsorHelper.GetSponsorsByKeywordsAsync(keywords);

            // Get Athena partners.
            discoveryTreeNodeData.Partners = await this.partnerHelper.GetPartnersByKeywordsAsync(keywords);

            // Get research proposals.
            discoveryTreeNodeData.ResearchProposals = await this.researchProposalHelper.GetResearchProposalsByKeywordsAsync(keywords);

            // Get cois.
            discoveryTreeNodeData.Cois = await this.coiHelper.GetApprovedCoiRequestsByKeywordIdsAsync(keywords);

            // Get news articles.
            discoveryTreeNodeData.NewsArticles = await this.newsHelper.GetApprovedNewsArticlesByKeywordIdsAsync(keywords);

            // Get Athena events.
            discoveryTreeNodeData.Events = await this.eventHelper.GetEventsByKeywordsIdsAsync(keywords);

            // Get users.
            discoveryTreeNodeData.Users = await this.userSettingsHelper.GetUsersByKeywordIds(keywords);

            return discoveryTreeNodeData;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserDetails>> GetUsersByKeywordIds(IEnumerable<int> keywordIds)
        {
            var userDetails = new List<UserDetails>();
            var userFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(UserEntity.Keywords), keywordIds);

            var userSearchParametersDto = new SearchParametersDTO
            {
                Filter = userFilter,
            };

            var users = await this.usersSearchService.GetUsersAsync(userSearchParametersDto);
            foreach (var user in users)
            {
                string userProfilePhoto = null;
                if (!string.IsNullOrEmpty(user.UserId))
                {
                    userProfilePhoto = await this.userGraphService.GetUserProfilePhotoAsync(user.UserId);
                }

                userDetails.Add(this.userGraphServiceMapper.MapToUserDetailsViewModel(user, userProfilePhoto));
            }

            return userDetails;
        }

        /// <inheritdoc/>
        public async Task<UserEntity> FollowResourceAsync(IEnumerable<int> keywordIds, string userId)
        {
            userId = userId ?? throw new ArgumentNullException(nameof(userId));

            var userEntity = await this.userRepository.GetUserDetailsByUserIdAsync(userId);
            if (userEntity == null)
            {
                var userBotConversationEntity = await this.userBotConversationRepository.GetAsync(UserBotConversationTableMetadata.PartitionKey, userId);
                if (userBotConversationEntity == null)
                {
                    return null;
                }

                var userDetails = await this.userGraphServiceHelper.GetUsersAsync(new string[] { userId });
                if (userDetails == null)
                {
                    return null;
                }

                var userCreateDetails = this.userSettingsMapper.MapForCreateModel(userDetails.FirstOrDefault());
                userEntity = await this.userRepository.CreateOrUpdateAsync(userCreateDetails);
            }

            var keywordsFilterQuery = this.filterQueryHelper.GetFilterCondition(nameof(KeywordEntity.KeywordId), keywordIds.Select(i => i.ToString(CultureInfo.InvariantCulture)));
            var keywordsSearchParametersDto = new SearchParametersDTO
            {
                Filter = keywordsFilterQuery,
            };

            try
            {
                var keywords = await this.keywordsSearchServices.GetKeywordsAsync(keywordsSearchParametersDto);
                if (!keywords.IsNullOrEmpty())
                {
                    var existingKeywordNames = string.IsNullOrEmpty(userEntity.KeywordNames) ? new List<string>() : userEntity.KeywordNames.Split(SemicolonSeparator).ToList();
                    var existingKeywordIds = string.IsNullOrEmpty(userEntity.Keywords) ? new List<string>() : userEntity.Keywords.Split(KeywordsSeparator).ToList();

                    foreach (var keyword in keywords)
                    {
                        if (!existingKeywordIds.Contains(keyword.KeywordId))
                        {
                            existingKeywordNames.Add(keyword.Title);
                            existingKeywordIds.Add(keyword.KeywordId);
                        }
                    }

                    userEntity.KeywordNames = string.Join(SemicolonSeparator, existingKeywordNames.Select(keywordName => keywordName));
                    userEntity.Keywords = string.Join(KeywordsSeparator, existingKeywordIds.Select(keywordId => keywordId));

                    return await this.userRepository.CreateOrUpdateAsync(userEntity);
                }

                return userEntity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a discovery tree node type from blob storage.
        /// </summary>
        /// <returns>A task representing get discovery tree node data.</returns>
        public async Task<IEnumerable<NodeType>> GetDiscoveryTreeNodeTypeAsync()
        {
            return await this.nodeTypeForDiscoveryTreeBlobRepository.GetBlobJsonFileContentAsync(NodeTypeForDiscoveryTreeBlobMetadata.FileName);
        }

        /// <inheritdoc/>
        public async Task<DiscoveryTreeData> FindOrFilterDiscoveryTreeResourcesAsync(IEnumerable<string> searchTexts, IEnumerable<int> searchKeywordIds, IEnumerable<DiscoveryTreeSelectedFilter> selectedFilters)
        {
            var discoveryTreeNodeData = new DiscoveryTreeData();

            if (selectedFilters.IsNullOrEmpty())
            {
                return discoveryTreeNodeData;
            }

            var nodeTypes = await this.GetDiscoveryTreeNodeTypeAsync();

            if (nodeTypes.IsNullOrEmpty())
            {
                throw new Exception("Failed to get node types while finding and filtering discovery tree resources");
            }

            var dictionary = new Dictionary<int, Dictionary<string, List<string>>>();
            var filterTypesByFile = new Dictionary<string, List<string>>();

            if (!selectedFilters.IsNullOrEmpty())
            {
                foreach (var filter in selectedFilters)
                {
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaResearchProjects, nodeTypes, filterTypesByFile);
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaResearchRequests, nodeTypes, filterTypesByFile);
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaResearchProposals, nodeTypes, filterTypesByFile);
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaPartners, nodeTypes, filterTypesByFile);
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaEvents, nodeTypes, filterTypesByFile);
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaCommunities, nodeTypes, filterTypesByFile);
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaNewsArticles, nodeTypes, filterTypesByFile);
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaSponsors, nodeTypes, filterTypesByFile);
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaUsers, nodeTypes, filterTypesByFile);
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaInfoResources, nodeTypes, filterTypesByFile);
                    this.PrepareDictionary(filter, dictionary, FileNames.AthenaTools, nodeTypes, filterTypesByFile);
                }
            }

            var searchParameters = new SearchParametersDTO
            {
                Filter = null,
                IsGetAllRecords = true,
            };

            var isSearch = !searchTexts.IsNullOrEmpty() || !searchKeywordIds.IsNullOrEmpty();

            // Get research projects.
            string researchProjectsQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaResearchProjects);
            bool isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaResearchProjects, filterTypesByFile);

            if (!isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(researchProjectsQuery)
                    || (!string.IsNullOrWhiteSpace(researchProjectsQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(researchProjectsQuery, searchTexts, searchKeywordIds, nameof(ResearchProjectEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.ResearchProjects = await this.researchProjectHelper.GetResearchProjectsAsync(searchParameters);
            }

            // Get research requests.
            string researchRequestsQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaResearchRequests);
            isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaResearchRequests, filterTypesByFile);

            if (!isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(researchRequestsQuery)
                    || (!string.IsNullOrWhiteSpace(researchRequestsQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(researchRequestsQuery, searchTexts, searchKeywordIds, nameof(ResearchRequestEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.ResearchRequests = await this.researchRequestHelper.GetResearchRequestsAsync(searchParameters);
            }

            // Get research proposals.
            string researchProposalsQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaResearchProposals);
            isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaResearchProposals, filterTypesByFile);

            if (!isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(researchProposalsQuery)
                    || (!string.IsNullOrWhiteSpace(researchProposalsQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(researchProposalsQuery, searchTexts, searchKeywordIds, nameof(ResearchProposalEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.ResearchProposals = await this.researchProposalHelper.GetResearchProposalsAsync(searchParameters);
            }

            // Get partners.
            string partnersQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaPartners);
            bool isPartnersFilterSelected = selectedFilters.Any(selectedFilter => selectedFilter.Type == PartnersFilterId);
            isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaPartners, filterTypesByFile);

            if (isPartnersFilterSelected
                && !isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(partnersQuery)
                    || (!string.IsNullOrWhiteSpace(partnersQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(partnersQuery, searchTexts, searchKeywordIds, nameof(PartnerEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.Partners = await this.partnerHelper.GetPartnersAsync(searchParameters);
            }

            // Get events.
            string eventsQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaEvents);
            bool isEventsFilterSelected = selectedFilters.Any(selectedFilter => selectedFilter.Type == EventsFilterId);
            isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaEvents, filterTypesByFile);

            if (isEventsFilterSelected
                && !isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(eventsQuery)
                    || (!string.IsNullOrWhiteSpace(eventsQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(eventsQuery, searchTexts, searchKeywordIds, nameof(EventEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.Events = await this.eventHelper.GetEventsAsync(searchParameters);
            }

            // Get Athena communities.
            string athenaCommunitiesQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaCommunities);
            bool isCommunitiesFilterSelected = selectedFilters.Any(selectedFilter => selectedFilter.Type == CommunitiesFilterId);
            isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaCommunities, filterTypesByFile);

            if (isCommunitiesFilterSelected
                && !isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(athenaCommunitiesQuery)
                    || (!string.IsNullOrWhiteSpace(athenaCommunitiesQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(athenaCommunitiesQuery, searchTexts, searchKeywordIds, nameof(CommunityOfInterestEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.Cois = await this.coiHelper.GetCoiRequestsAsync(searchParameters);
            }

            // Get Athena news articles.
            string athenaNewsArticlesQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaNewsArticles);
            bool isNewsFilterSelected = selectedFilters.Any(selectedFilter => selectedFilter.Type == NewsFilterId);
            isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaNewsArticles, filterTypesByFile);

            if (isNewsFilterSelected
                && !isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(athenaNewsArticlesQuery)
                    || (!string.IsNullOrWhiteSpace(athenaNewsArticlesQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(athenaNewsArticlesQuery, searchTexts, searchKeywordIds, nameof(NewsEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.NewsArticles = await this.newsHelper.GetNewsAsync(searchParameters);
            }

            // Get Athena sponsors.
            string athenaSponsorsQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaSponsors);
            bool isSponsorsFilterSelected = selectedFilters.Any(selectedFilter => selectedFilter.Type == SponsorsFilterId);
            isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaSponsors, filterTypesByFile);

            if (isSponsorsFilterSelected
                && !isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(athenaSponsorsQuery)
                    || (!string.IsNullOrWhiteSpace(athenaSponsorsQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(athenaSponsorsQuery, searchTexts, searchKeywordIds, nameof(SponsorEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.Sponsors = await this.sponsorHelper.GetSponsorsAsync(searchParameters);
            }

            // Get Athena users.
            string athenaUsersQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaUsers);
            bool isPersonnelFilterSelected = selectedFilters.Any(selectedFilter => selectedFilter.Type == PersonnelFilterId);
            isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaUsers, filterTypesByFile);

            if (isPersonnelFilterSelected
                && !isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(athenaUsersQuery)
                    || (!string.IsNullOrWhiteSpace(athenaUsersQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(athenaUsersQuery, searchTexts, searchKeywordIds, nameof(UserEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.Users = await this.userSettingsHelper.GetUsersAsync(searchParameters);
            }

            // Get Athena info resources.
            string athenaInfoResourcesQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaInfoResources);
            bool isOtherInformationResourcesFilterSelected = selectedFilters.Any(selectedFilter => selectedFilter.Type == OtherInformationResourcesFilterId);
            isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaInfoResources, filterTypesByFile);

            if (isOtherInformationResourcesFilterSelected
                && !isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(athenaInfoResourcesQuery)
                    || (!string.IsNullOrWhiteSpace(athenaInfoResourcesQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(athenaInfoResourcesQuery, searchTexts, searchKeywordIds, nameof(AthenaInfoResourceEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.AthenaInfoResources = await this.athenaInfoResourcesHelper.GetAthenaInfoResourcesAsync(searchParameters);
            }

            // Get Athena tools.
            string athenaToolsQuery = this.GenerateFinalFilterQuery(dictionary, FileNames.AthenaTools);
            bool isToolsAndSoftwareFilterSelected = selectedFilters.Any(selectedFilter => selectedFilter.Type == ToolsAndSoftwareFilterId);
            isOnlyDateQueryExists = this.IsOnlyDateQueryExists(FileNames.AthenaTools, filterTypesByFile);

            if (isToolsAndSoftwareFilterSelected
                && !isOnlyDateQueryExists
                && (!string.IsNullOrWhiteSpace(athenaToolsQuery)
                    || (!string.IsNullOrWhiteSpace(athenaToolsQuery) && isSearch)
                    || (!selectedFilters.Any() && isSearch)))
            {
                var filterQuery = this.GetFinalEntityFilterQuery(athenaToolsQuery, searchTexts, searchKeywordIds, nameof(AthenaToolEntity.Keywords));

                searchParameters.Filter = filterQuery;
                discoveryTreeNodeData.AthenaTools = await this.athenaToolHelper.GetAthenaToolsAsync(searchParameters);
            }

            return discoveryTreeNodeData;
        }

        /// <inheritdoc/>
        public string GetFilterStringByType(DiscoveryTreeFilterItems filter, string jsonFileName, IEnumerable<NodeType> nodeTypes, Dictionary<string, List<string>> filterTypesByFile)
        {
            string dbFieldType = filter.DbFieldType.ToUpperInvariant();
            string dbFieldTypeString = null;
            string query = null;

            switch (dbFieldType)
            {
                case DbFieldTypeString:
                    dbFieldTypeString = DbFieldTypeString;
                    query = string.Join(" ", $"{filter.DbField}", "eq", $"'{filter.DbValue.First()}'");
                    break;

                case DbFieldTypeInteger:
                    dbFieldTypeString = DbFieldTypeInteger;
                    query = string.Join($" {TableOperators.Or} ", filter.DbValue.Select(x => string.Join(" ", filter.DbField, "eq", x)));
                    break;

                case DbFieldTypeArray:
                    dbFieldTypeString = DbFieldTypeArray;
                    query = this.filterQueryHelper.GetFilterConditionForExactStringMatch(filter.DbField, filter.DbValue);
                    break;

                case DbFieldTypeDate:
                    var nodeType = nodeTypes.Where(x => x.JsonFile.Trim().ToUpperInvariant() == jsonFileName.Trim().ToUpperInvariant()).FirstOrDefault();

                    if (string.IsNullOrEmpty(nodeType?.DateFieldName))
                    {
                        return null;
                    }
                    else
                    {
                        query = this.GetDateFilterQuery(filter, nodeType.DateFieldName);

                        if (query != null)
                        {
                            dbFieldTypeString = DbFieldTypeDate;
                        }
                    }

                    break;

                case "NA":
                    if (filter.DbValue.First() == -1)
                    {
                        return AllFilterQuery;
                    }

                    return null;
                default:
                    return null;
            }

            if (dbFieldTypeString != null)
            {
                if (filterTypesByFile.ContainsKey(jsonFileName))
                {
                    filterTypesByFile[jsonFileName].Add(dbFieldTypeString);
                }
                else
                {
                    filterTypesByFile.Add(jsonFileName, new List<string> { dbFieldTypeString });
                }
            }

            return query;
        }

        /// <inheritdoc/>
        public void PrepareDictionary(DiscoveryTreeSelectedFilter filter, Dictionary<int, Dictionary<string, List<string>>> dictionary, string resultType, IEnumerable<NodeType> nodeTypes, Dictionary<string, List<string>> filterTypesByFile)
        {
            var entityFilters = filter
                .Filters
                .Where(x => x.DbEntity.Split(",").Any(y => y.Trim() == resultType));

            var resultList = this.GetFilterList(entityFilters, resultType, nodeTypes, filterTypesByFile);

            if (dictionary.ContainsKey(filter.Type))
            {
                if (dictionary[filter.Type].ContainsKey(resultType))
                {
                    dictionary[filter.Type][resultType].AddRange(resultList);
                }
                else
                {
                    dictionary[filter.Type].Add(resultType, resultList);
                }
            }
            else
            {
                dictionary.Add(filter.Type, new Dictionary<string, List<string>> { { resultType, resultList } });
            }
        }

        /// <inheritdoc/>
        public List<string> GetFilterList(IEnumerable<DiscoveryTreeFilterItems> filters, string jsonFileName, IEnumerable<NodeType> nodeTypes, Dictionary<string, List<string>> filterTypesByFile)
        {
            var filterStrings = new List<string>();

            foreach (var filter in filters)
            {
                var filterString = this.GetFilterStringByType(filter, jsonFileName, nodeTypes, filterTypesByFile);

                if (!filterString.IsNullOrEmpty())
                {
                    filterStrings.Add(filterString);
                }
            }

            return filterStrings;
        }

        /// <summary>
        /// Generates the filter query.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="resultType">The result type entity.</param>
        /// <returns>Returns the query string.</returns>
        private string GenerateFinalFilterQuery(Dictionary<int, Dictionary<string, List<string>>> dictionary, string resultType)
        {
            Dictionary<string, List<string>> andQuery = new Dictionary<string, List<string>>();

            foreach (var keyValuePair in dictionary)
            {
                var valueOrList = keyValuePair.Value[resultType];

                if (valueOrList.Any())
                {
                    var resultWithOrCondition = "(" + string.Join($" {TableOperators.Or} ", valueOrList) + ")";

                    if (andQuery.ContainsKey(resultType))
                    {
                        andQuery[resultType].Add(resultWithOrCondition);
                    }
                    else
                    {
                        andQuery.Add(resultType, new List<string> { resultWithOrCondition });
                    }
                }
            }

            if (andQuery.ContainsKey(resultType))
            {
                var isApplyAllQuery = andQuery.All(x => x.Value.All(y => y.Contains(AllFilterQuery, StringComparison.InvariantCultureIgnoreCase)));

                if (isApplyAllQuery)
                {
                    return AllFilterQuery;
                }
                else
                {
                    andQuery[resultType] = andQuery[resultType].Where(x => !x.Contains(AllFilterQuery, StringComparison.InvariantCultureIgnoreCase)).ToList();

                    if (andQuery[resultType].Any())
                    {
                        return string.Join($" {TableOperators.And} ", andQuery[resultType]);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the date filter query.
        /// </summary>
        /// <param name="filter">The date filter item.</param>
        /// <param name="dateFieldName">The date field name.</param>
        /// <returns>The date filter query.</returns>
        private string GetDateFilterQuery(DiscoveryTreeFilterItems filter, string dateFieldName)
        {
            DateTime date = DateTime.UtcNow;

            switch (filter.FilterId)
            {
                // From last week.
                case 502:
                    date = date.AddDays(-7);
                    break;

                // From last month.
                case 503:
                    date = date.AddMonths(-1);
                    break;

                // From last 6 months.
                case 504:
                    date = date.AddMonths(-6);
                    break;

                // From last 12 months.
                case 104:
                    date = date.AddYears(-1);
                    break;

                // From last 2 years.
                case 105:
                    date = date.AddYears(-2);
                    break;

                // From last 5 years.
                case 106:
                    date = date.AddYears(-5);
                    break;

                // From last 10 years.
                case 107:
                    date = date.AddYears(-10);
                    break;

                // From last 20 years.
                case 108:
                    date = date.AddYears(-20);
                    break;

                default:
                    return null;
            }

            string zuluDateString = date.ToZuluTimeFormatWithStartOfDay();
            return this.filterQueryHelper.GetFilterConditionForDate(dateFieldName, "ge", new[] { zuluDateString });
        }

        /// <summary>
        /// Indicates whether date filter is applied by file type.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="filterTypesByFile">The collection of filter types by file.</param>
        /// <returns>Returns whether collection for a file contains all date filters.</returns>
        private bool IsOnlyDateQueryExists(string fileName, Dictionary<string, List<string>> filterTypesByFile)
        {
            if (filterTypesByFile.ContainsKey(fileName))
            {
                return filterTypesByFile[fileName].All(dbFieldType => dbFieldType == DbFieldTypeDate);
            }

            return false;
        }

        private string GetFinalSearchQuery(IEnumerable<string> searchTexts, IEnumerable<int> searchKeywordIds, string keywordSearchColumnName)
        {
            var keywordsFilter = string.Empty;

            if (!searchKeywordIds.IsNullOrEmpty())
            {
                keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(keywordSearchColumnName, searchKeywordIds);
            }

            var searchTextsFilter = string.Empty;

            if (!searchTexts.IsNullOrEmpty())
            {
                searchTextsFilter = this.filterQueryHelper.GetFilterConditionForExactMatch(searchTexts);
            }

            if (!searchTexts.IsNullOrEmpty() && !searchKeywordIds.IsNullOrEmpty())
            {
                return this.filterQueryHelper.CombineFilters(searchTextsFilter, keywordsFilter, TableOperators.Or);
            }
            else if (!searchTexts.IsNullOrEmpty())
            {
                return searchTextsFilter;
            }
            else if (!searchKeywordIds.IsNullOrEmpty())
            {
                return keywordsFilter;
            }

            return string.Empty;
        }

        private string GetFinalEntityFilterQuery(string entityFilterQuery, IEnumerable<string> searchTexts, IEnumerable<int> searchKeywordIds, string keywordSearchColumnName)
        {
            var isSearch = !searchTexts.IsNullOrEmpty() || !searchKeywordIds.IsNullOrEmpty();
            var searchQuery = string.Empty;

            if (isSearch)
            {
                searchQuery = this.GetFinalSearchQuery(searchTexts, searchKeywordIds, nameof(ResearchProjectEntity.Keywords));
            }

            if (entityFilterQuery == AllFilterQuery || string.IsNullOrEmpty(entityFilterQuery))
            {
                if (isSearch)
                {
                    return searchQuery;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (isSearch)
                {
                    return this.filterQueryHelper.CombineFilters(searchQuery, entityFilterQuery, TableOperators.And);
                }
                else
                {
                    return entityFilterQuery;
                }
            }
        }
    }
}