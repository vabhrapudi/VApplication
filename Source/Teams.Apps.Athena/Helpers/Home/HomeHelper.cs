// <copyright file="HomeHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Athena.Models;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The helper method related to home tab.
    /// </summary>
    public class HomeHelper : IHomeHelper
    {
        private const int UserDailyBriefingArticlesCount = 15;
        private const int UserDailyBriefingArticlesRollingDays = 7;

        private readonly IHomeConfigurationsRepository homeConfigurationRepository;
        private readonly IHomeConfigurationMapper homeConfigurationMapper;
        private readonly IHomeStatusBarConfigurationRepository homeStatusBarConfigurationRepository;
        private readonly IHomeStatusBarConfigurationMapper homeStatusBarConfigurationMapper;
        private readonly IUserSettingsHelper userSettingsHelper;
        private readonly IResearchProjectHelper researchProjectHelper;
        private readonly IResearchRequestHelper researchRequestHelper;
        private readonly IEventHelper eventHelper;
        private readonly IResearchProposalHelper researchProposalHelper;
        private readonly ICoiHelper coiHelper;
        private readonly INewsHelper newsHelper;
        private readonly IAthenaInfoResourcesHelper athenaInfoResourcesHelper;
        private readonly IHomeMapper homeMapper;
        private readonly ITeamRepository teamRepository;
        private readonly IOptions<BotSettings> botOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeHelper"/> class.
        /// </summary>
        /// <param name="homeConfigurationMapper">The instance of <see cref="HomeConfigurationMapper"/> class.</param>
        /// <param name="homeConfigurationRepository">The instance of <see cref="HomeConfigurationsRepository"/> class.</param>
        /// <param name="homeStatusBarConfigurationRepository">The instance of <see cref="HomeStatusBarConfigurationRepository"/> class.</param>
        /// <param name="homeStatusBarConfigurationMapper">The instance of <see cref="HomeStatusBarConfigurationMapper"/> class.</param>
        /// <param name="researchProjectHelper">The instance of <see cref="ResearchProjectHelper"/> class.</param>
        /// <param name="researchRequestHelper">The instance of <see cref="ResearchRequestHelper"/> class.</param>
        /// <param name="eventHelper">The instance of <see cref="EventHelper"/> class.</param>
        /// <param name="researchProposalHelper">The instance of <see cref="ResearchProposalHelper"/> class.</param>
        /// <param name="coiHelper">The instance of <see cref="CoiHelper"/> class.</param>
        /// <param name="newsHelper">The instance of <see cref="NewsHelper"/> class.</param>
        /// <param name="userSettingsHelper">The instance of <see cref="UserSettingsHelper"/> class.</param>
        /// <param name="athenaInfoResourcesHelper">The instance of <see cref="AthenaInfoResourcesHelper"/> class.</param>
        /// <param name="homeMapper">The instance of <see cref="HomeMapper"/> class.</param>
        /// <param name="teamRepository">The instance of <see cref="TeamRepository"/> class.</param>
        /// <param name="botOptions">The application configuration options.</param>
        public HomeHelper(
            IHomeConfigurationsRepository homeConfigurationRepository,
            IHomeConfigurationMapper homeConfigurationMapper,
            IHomeStatusBarConfigurationRepository homeStatusBarConfigurationRepository,
            IHomeStatusBarConfigurationMapper homeStatusBarConfigurationMapper,
            IResearchProjectHelper researchProjectHelper,
            IResearchRequestHelper researchRequestHelper,
            IEventHelper eventHelper,
            IResearchProposalHelper researchProposalHelper,
            ICoiHelper coiHelper,
            INewsHelper newsHelper,
            IUserSettingsHelper userSettingsHelper,
            IAthenaInfoResourcesHelper athenaInfoResourcesHelper,
            IHomeMapper homeMapper,
            ITeamRepository teamRepository,
            IOptions<BotSettings> botOptions)
        {
            this.homeConfigurationRepository = homeConfigurationRepository;
            this.homeConfigurationMapper = homeConfigurationMapper;
            this.homeStatusBarConfigurationRepository = homeStatusBarConfigurationRepository;
            this.homeStatusBarConfigurationMapper = homeStatusBarConfigurationMapper;
            this.researchProjectHelper = researchProjectHelper;
            this.researchRequestHelper = researchRequestHelper;
            this.eventHelper = eventHelper;
            this.researchProposalHelper = researchProposalHelper;
            this.coiHelper = coiHelper;
            this.newsHelper = newsHelper;
            this.userSettingsHelper = userSettingsHelper;
            this.athenaInfoResourcesHelper = athenaInfoResourcesHelper;
            this.homeMapper = homeMapper;
            this.teamRepository = teamRepository;
            this.botOptions = botOptions;
        }

        /// <inheritdoc/>
        public async Task<HomeStatusBarConfigurationDTO> GetActiveHomeStatusBarDetailsAsync(Guid teamId)
        {
            var homeStatusBarDetails = await this.homeStatusBarConfigurationRepository.GetAsync(HomeStatusBarConfigurationTableMetadata.PartitionKey, teamId.ToString());

            if (homeStatusBarDetails == null || !homeStatusBarDetails.IsActive)
            {
                return null;
            }

            return this.homeStatusBarConfigurationMapper.MapForViewModel(homeStatusBarDetails);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<HomeConfigurationArticleDTO>> GetNewToAthenaArticlesAsync(Guid teamId)
        {
            var newToAthenaArticles = await this.homeConfigurationRepository.GetAllAsync(teamId.ToString());

            return newToAthenaArticles
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => this.homeConfigurationMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DailyBriefingHomeArticleDTO>> GetDailyBriefingArticlesOfUserForCentralTeamAsync(string userAadId)
        {
            userAadId = userAadId ?? throw new ArgumentNullException(nameof(userAadId));

            var userDetails = await this.userSettingsHelper.GetUserByIdAsync(userAadId);

            if (userDetails == null || userDetails.Keywords.IsNullOrEmpty())
            {
                return Enumerable.Empty<DailyBriefingHomeArticleDTO>();
            }

            var articles = new List<DailyBriefingHomeArticleDTO>();

            var searchParametersDto = new SearchParametersDTO
            {
                TopRecordsCount = UserDailyBriefingArticlesCount,
            };

            var rollingDateAndTimeFrom = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(-UserDailyBriefingArticlesRollingDays);

            // News
            var news = await this.newsHelper.GetApprovedNewsArticlesAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(news.Select(x => this.homeMapper.MapForViewModel(x)));

            // Research projects
            var researchProjects = await this.researchProjectHelper.GetResearchProjectsAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(researchProjects.Select(x => this.homeMapper.MapForViewModel(x)));

            // Research requests
            var researchRequests = await this.researchRequestHelper.GetResearchRequestsAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(researchRequests.Select(x => this.homeMapper.MapForViewModel(x)));

            // Research proposals
            var researchProposals = await this.researchProposalHelper.GetResearchProposalsAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(researchProposals.Select(x => this.homeMapper.MapForViewModel(x)));

            // Events
            var events = await this.eventHelper.GetEventsAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(events.Select(x => this.homeMapper.MapForViewModel(x)));

            // COIs
            var cois = await this.coiHelper.GetApprovedCoiRequestsAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(cois.Select(x => this.homeMapper.MapForViewModel(x)));

            // Info resources
            var infoResources = await this.athenaInfoResourcesHelper.GetAthenaInfoResourcesAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(infoResources.Select(x => this.homeMapper.MapForViewModel(x)));

            return articles.OrderByDescending(x => x.UpdatedOn);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<DailyBriefingHomeArticleDTO>> GetDailyBriefingArticlesOfUserForCoiTeamAsync(Guid teamId, string userAadId)
        {
            userAadId = userAadId ?? throw new ArgumentNullException(nameof(userAadId));

            var userDetails = await this.userSettingsHelper.GetUserByIdAsync(userAadId);

            if (userDetails == null || userDetails.Keywords.IsNullOrEmpty())
            {
                return Enumerable.Empty<DailyBriefingHomeArticleDTO>();
            }

            var coiTeamDetails = await this.coiHelper.GetCoiDetailsAsync(teamId);

            // The COI team does not exists or COI team does not have keywords assigned.
            if (coiTeamDetails == null || coiTeamDetails.Keywords.IsNullOrEmpty())
            {
                return Enumerable.Empty<DailyBriefingHomeArticleDTO>();
            }

            // Get matching keywords.
            var keywords = coiTeamDetails.Keywords.Intersect(userDetails.Keywords);

            // If there are no matching keywords.
            if (!keywords.Any())
            {
                return Enumerable.Empty<DailyBriefingHomeArticleDTO>();
            }

            var articles = new List<DailyBriefingHomeArticleDTO>();

            var searchParametersDto = new SearchParametersDTO
            {
                TopRecordsCount = UserDailyBriefingArticlesCount,
            };

            var rollingDateAndTimeFrom = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddDays(-UserDailyBriefingArticlesRollingDays);

            // News
            var news = await this.newsHelper.GetApprovedNewsArticlesAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(news.Select(x => this.homeMapper.MapForViewModel(x)));

            // Research projects
            var researchProjects = await this.researchProjectHelper.GetResearchProjectsAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(researchProjects.Select(x => this.homeMapper.MapForViewModel(x)));

            // Research requests
            var researchRequests = await this.researchRequestHelper.GetResearchRequestsAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(researchRequests.Select(x => this.homeMapper.MapForViewModel(x)));

            // Research proposals
            var researchProposals = await this.researchProposalHelper.GetResearchProposalsAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(researchProposals.Select(x => this.homeMapper.MapForViewModel(x)));

            // Events
            var events = await this.eventHelper.GetEventsAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(events.Select(x => this.homeMapper.MapForViewModel(x)));

            // Info resources
            var infoResources = await this.athenaInfoResourcesHelper.GetAthenaInfoResourcesAsync(userDetails.Keywords, rollingDateAndTimeFrom, UserDailyBriefingArticlesCount);
            articles.AddRange(infoResources.Select(x => this.homeMapper.MapForViewModel(x)));

            return articles.OrderByDescending(x => x.UpdatedOn);
        }

        /// <inheritdoc/>
        public async Task<HomeStatusBarConfigurationDTO> GetActiveHomeStatusBarDetailsForCentralTeamAsync()
        {
            var adminTeamDetails = await this.teamRepository.GetAsync(TeamTableMetadata.TeamPartitionKey, this.botOptions.Value.AdminTeamId);

            if (adminTeamDetails == null || adminTeamDetails.GroupId.IsEmptyOrInvalidGuid())
            {
                return null;
            }

            return await this.GetActiveHomeStatusBarDetailsAsync(Guid.Parse(adminTeamDetails.GroupId));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<HomeConfigurationArticleDTO>> GetNewToAthenaArticlesForCentralTeamAsync()
        {
            var adminTeamDetails = await this.teamRepository.GetAsync(TeamTableMetadata.TeamPartitionKey, this.botOptions.Value.AdminTeamId);

            if (adminTeamDetails == null || adminTeamDetails.GroupId.IsEmptyOrInvalidGuid())
            {
                return Enumerable.Empty<HomeConfigurationArticleDTO>();
            }

            return await this.GetNewToAthenaArticlesAsync(Guid.Parse(adminTeamDetails.GroupId));
        }
    }
}
