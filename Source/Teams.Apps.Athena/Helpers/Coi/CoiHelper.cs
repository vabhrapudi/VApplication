// <copyright file="CoiHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Models.Enums;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Repositories.ResourceFeedback;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Constants;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Services.MicrosoftGraph;

    /// <summary>
    /// Provides helper methods associated with COI entity operations.
    /// </summary>
    public class CoiHelper : ICoiHelper
    {
        /// <summary>
        /// The instance of <see cref="CoiMapper"/> class.
        /// </summary>
        private readonly ICoiMapper coiMapper;

        /// <summary>
        /// The instance of <see cref="CoiRepository"/> class.
        /// </summary>
        private readonly ICoiRepository coiRepository;

        /// <summary>
        /// The instance of <see cref="CoiSearchService"/> class.
        /// </summary>
        private readonly ICoiSearchService coiSearchService;

        /// <summary>
        /// The instance of <see cref="FilterQueryHelper"/> class.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// The instance of <see cref="TeamService"/> class.
        /// </summary>
        private readonly ITeamService microsoftTeamsTeamService;

        /// <summary>
        /// The instance of <see cref="UserService"/> class.
        /// </summary>
        private readonly IUserService userGraphService;

        /// <summary>
        /// The instance of <see cref="UserGraphServiceMapper"/> class.
        /// </summary>
        private readonly IUserGraphServiceMapper userGraphServiceMapper;

        /// The instance of <see cref="ResourceFeedbackRepository"/> class.
        /// </summary>
        private readonly IResourceFeedbackRepository resourceFeedbackRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoiHelper"/> class.
        /// </summary>
        /// <param name="coiMapper">The instance of <see cref="ICoiMapper"/>.</param>
        /// <param name="coiRepository">The instance of <see cref="ICoiRepository"/>.</param>
        /// <param name="coiSearchService">The instance of <see cref="CoiSearchService"/>.</param>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/>.</param>
        /// <param name="microsoftTeamsTeamService">The instance of <see cref="TeamService"/> class.</param>
        /// <param name="userGraphService">The instance of <see cref="UserService"/> class.</param>
        /// <param name="userGraphServiceMapper">The instance of <see cref="UserGraphServiceMapper"/> class.</param>
        /// <param name="resourceFeedbackRepository">The instance of <see cref="ResourceFeedbackRepository"/> class.</param>
        public CoiHelper(
            ICoiMapper coiMapper,
            ICoiRepository coiRepository,
            ICoiSearchService coiSearchService,
            IFilterQueryHelper filterQueryHelper,
            ITeamService microsoftTeamsTeamService,
            IUserService userGraphService,
            IUserGraphServiceMapper userGraphServiceMapper,
            IResourceFeedbackRepository resourceFeedbackRepository)
        {
            this.coiMapper = coiMapper;
            this.coiRepository = coiRepository;
            this.coiSearchService = coiSearchService;
            this.filterQueryHelper = filterQueryHelper;
            this.microsoftTeamsTeamService = microsoftTeamsTeamService;
            this.userGraphService = userGraphService;
            this.userGraphServiceMapper = userGraphServiceMapper;
            this.resourceFeedbackRepository = resourceFeedbackRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CoiEntityDTO>> GetActiveCoiRequestsAsync(string searchText, int pageNumber, CoiSortColumn sortColumn, SortOrder sortOrder, IEnumerable<int> statusFilterValues, Guid userAadId)
        {
            var searchServiceParameters = new SearchParametersDTO
            {
                PageCount = (int)pageNumber,
                SearchString = searchText?.Trim().EscapeSpecialCharacters(),
                CoiSortColumn = sortColumn,
                SortOrder = sortOrder,
                Filter = this.filterQueryHelper.GetActiveCoiRequestsFilterCondition(statusFilterValues, userAadId),
            };

            var activeCoiRequests = await this.coiSearchService.GetCommunityOfInterestsAsync(searchServiceParameters);

            var activeCoiRequestsDTOs = activeCoiRequests
                .Select(coiRequest => this.coiMapper.MapForViewModel(coiRequest));

            return activeCoiRequestsDTOs;
        }

        /// <summary>
        /// Gets all COI requests created by user which are approved.
        /// </summary>
        /// <param name="keywords">The status filter values.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IEnumerable<CoiEntityDTO>> GetApprovedCoiRequestAsync(IEnumerable<KeywordEntity> keywords)
        {
            var keywordList = new List<string>();
            keywordList = keywords.Select(keyword => keyword.Title).ToList();

            var searchServiceParameters = new SearchParametersDTO
            {
                IsGetAllRecords = true,
                Filter = this.filterQueryHelper.GetApprovedCoiRequestsFilterCondition(keywordList),
            };

            var approvedCoiRequests = await this.coiSearchService.GetCommunityOfInterestsAsync(searchServiceParameters);

            var approvedCoiRequestsDTOs = approvedCoiRequests
                .Select(coiRequest => this.coiMapper.MapForViewModel(coiRequest));

            return approvedCoiRequestsDTOs;
        }

        /// <inheritdoc/>
        public async Task<CoiEntityDTO> GetCoiRequestAsync(Guid coiRequestId)
        {
            var coiRequest = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, coiRequestId.ToString());

            if (coiRequest == null || coiRequest.IsDeleted)
            {
                return null;
            }

            return this.coiMapper.MapForViewModel(coiRequest);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CoiEntityDTO>> GetApprovedCoiRequestsByKeywordIdsAsync(IEnumerable<int> keywordIds)
        {
            var coisKeywordsFilterQuery = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(CommunityOfInterestEntity.Keywords), keywordIds);
            var approvedCoisFilterQuery = this.filterQueryHelper.GetFilterCondition(nameof(CommunityOfInterestEntity.Status), new[] { (int)CoiRequestStatus.Approved });

            var coisFilterQuery = this.filterQueryHelper.CombineFilters(coisKeywordsFilterQuery, approvedCoisFilterQuery, TableOperators.And);

            var coiSearchParametersDto = new SearchParametersDTO
            {
                IsGetAllRecords = true,
                Filter = coisFilterQuery,
            };

            var cois = await this.coiSearchService.GetCommunityOfInterestsAsync(coiSearchParametersDto);
            var coiDTOs = cois.Select(x => this.coiMapper.MapForViewModel(x));
            var coiViewDTOs = new List<CoiEntityDTO>();
            foreach (var coi in coiDTOs)
            {
                if (!string.IsNullOrEmpty(coi.TeamId))
                {
                    try
                    {
                        var channelId = await this.microsoftTeamsTeamService.GetPrimaryChannelIdOfTeamAsync(coi.TeamId);
                        coi.ChannelId = channelId;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                if (!string.IsNullOrEmpty(coi.CreatedBy))
                {
                    try
                    {
                        var users = await this.userGraphService.GetUsersAsync(new string[] { coi.CreatedBy });
                        var userDetails = users.Select(user => this.userGraphServiceMapper.MapToViewModel(user)).ToList();
                        coi.CreatedByName = users.ToList().FirstOrDefault().DisplayName;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                coiViewDTOs.Add(coi);
            }

            return coiViewDTOs;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CoiEntityDTO>> GetCoiRequestsAsync(SearchParametersDTO searchParametersDTO)
        {
            var cois = await this.coiSearchService.GetCommunityOfInterestsAsync(searchParametersDTO);
            var coiDTOs = cois.Select(x => this.coiMapper.MapForViewModel(x));
            var coiViewDTOs = new List<CoiEntityDTO>();
            foreach (var coi in coiDTOs)
            {
                if (!string.IsNullOrEmpty(coi.TeamId))
                {
                    try
                    {
                        var channelId = await this.microsoftTeamsTeamService.GetPrimaryChannelIdOfTeamAsync(coi.TeamId);
                        coi.ChannelId = channelId;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                if (!string.IsNullOrEmpty(coi.CreatedBy))
                {
                    try
                    {
                        var users = await this.userGraphService.GetUsersAsync(new string[] { coi.CreatedBy });
                        var userDetails = users.Select(user => this.userGraphServiceMapper.MapToViewModel(user)).ToList();
                        coi.CreatedByName = users.ToList().FirstOrDefault().DisplayName;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                coiViewDTOs.Add(coi);
            }

            return coiViewDTOs;
        }

        /// <inheritdoc/>
        public async Task<CoiEntityDTO> GetCoiByTableIdAsync(string coiTableId, string userAadObjectId)
        {
            coiTableId = coiTableId ?? throw new ArgumentNullException(nameof(coiTableId));
            var coiEntity = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, coiTableId);
            var coiEntityDTO = this.coiMapper.MapForViewModel(coiEntity);
            var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.COI, new List<string> { coiTableId }, userAadObjectId);
            if (resourceFeedback.Any())
            {
                coiEntityDTO.UserRating = resourceFeedback.FirstOrDefault().Rating;
            }

            return coiEntityDTO;
        }

        /// <inheritdoc/>
        public async Task RateCoiAsync(string coiTableId, int rating, string userAadObjectId)
        {
            coiTableId = coiTableId ?? throw new ArgumentNullException(nameof(coiTableId), "Coi table Id cannot be null.");
            var coiEntity = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, coiTableId);

            if (coiEntity != null)
            {
                var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.COI, new List<string> { coiTableId }, userAadObjectId);
                if (resourceFeedback.Any())
                {
                    var feedback = resourceFeedback.FirstOrDefault();
                    if (feedback.Rating > rating)
                    {
                        coiEntity.SumOfRatings -= feedback.Rating - rating;
                    }
                    else
                    {
                        coiEntity.SumOfRatings += rating - feedback.Rating;
                    }

                    feedback.Rating = rating;
                    await this.resourceFeedbackRepository.CreateOrUpdateAsync(feedback);
                }
                else
                {
                    var feedback = new ResourceFeedback
                    {
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userAadObjectId,
                        FeedbackId = Guid.NewGuid().ToString(),
                        Rating = rating,
                        ResourceId = coiTableId,
                        ResourceTypeId = (int)Itemtype.COI,
                    };

                    coiEntity.SumOfRatings += rating;
                    coiEntity.NumberOfRatings += 1;
                    await this.resourceFeedbackRepository.InsertOrMergeAsync(feedback);
                }
            }

            var avg = (decimal)coiEntity.SumOfRatings / (decimal)coiEntity.NumberOfRatings;
#pragma warning disable CA1305 // Culture provider is not required as its a number to string conversion
            coiEntity.AverageRating = avg.ToString("0.0");
#pragma warning restore CA1305 // Culture provider is not required as its a number to string conversion
            await this.coiRepository.CreateOrUpdateAsync(coiEntity);
            await this.coiSearchService.RunIndexerOnDemandAsync();
        }

        /// <inheritdoc/>
        public async Task<CoiEntityDTO> GetCoiDetailsAsync(Guid teamId)
        {
            var teamIdFilterCondition = this.filterQueryHelper.GetFilterCondition(nameof(CommunityOfInterestEntity.TeamId), new[] { teamId.ToString() });

            var coiDetails = await this.coiRepository.GetWithFilterAsync(teamIdFilterCondition);

            if (!coiDetails.Any())
            {
                return null;
            }

            return this.coiMapper.MapForViewModel(coiDetails.First());
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CoiEntityDTO>> GetApprovedCoiRequestsAsync(IEnumerable<int> keywords, DateTime fromDate, int? count)
        {
            var updatedOnDateFilter = this.filterQueryHelper.GetFilterConditionForDate(nameof(CommunityOfInterestEntity.UpdatedOn), QueryComparisons.GreaterThanOrEqual, new[] { fromDate.ToZuluTimeFormatWithStartOfDay() });
            var keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(CommunityOfInterestEntity.Keywords), keywords);
            var approvedCoiFilter = this.filterQueryHelper.GetFilterCondition(nameof(CommunityOfInterestEntity.Status), new List<int> { (int)CoiRequestStatus.Approved });

            var searchParametersDto = new SearchParametersDTO
            {
                TopRecordsCount = count,
                Filter = this.filterQueryHelper.CombineFilters(new[] { updatedOnDateFilter, keywordsFilter, approvedCoiFilter }, TableOperators.And),
                OrderBy = new List<string> { $"{nameof(CommunityOfInterestEntity.UpdatedOn)} desc" },
            };

            var cois = await this.coiSearchService.GetCommunityOfInterestsAsync(searchParametersDto);
            return cois.Select(x => this.coiMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CoiEntityDTO>> GetApprovedCoiRequestsCreatedInAthenaAppAsync()
        {
            string approvedCoiFilter = this.filterQueryHelper.GetFilterCondition(nameof(CommunityOfInterestEntity.Status), new[] { (int)CoiRequestStatus.Approved });
            string sourceAthenaFilter = this.filterQueryHelper.GetFilterCondition(nameof(CommunityOfInterestEntity.CoiId), new[] { Constants.SourceAthena });
            string teamIdIsNotNullFilter = this.filterQueryHelper.GetNotEqualFilterCondition(nameof(CommunityOfInterestEntity.TeamId), null);
            string teamIdIsNotEmptyFilter = this.filterQueryHelper.GetNotEqualFilterCondition(nameof(CommunityOfInterestEntity.TeamId), string.Empty);

            string filter = this.filterQueryHelper
                .CombineFilters(new[] { approvedCoiFilter, sourceAthenaFilter, teamIdIsNotNullFilter, teamIdIsNotEmptyFilter }, TableOperators.And);

            var cois = await this.coiRepository.GetWithFilterAsync(filter);

            return cois.Select(x => this.coiMapper.MapForViewModel(x));
        }
    }
}
