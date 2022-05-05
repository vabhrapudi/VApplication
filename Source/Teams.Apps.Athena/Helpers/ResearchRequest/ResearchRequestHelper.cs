// <copyright file="ResearchRequestHelper.cs" company="NPS Foundation">
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
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The helper class for research request entities.
    /// </summary>
    public class ResearchRequestHelper : IResearchRequestHelper
    {
        /// <summary>
        /// The instance of filter query helper.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// The instance of research request search service.
        /// </summary>
        private readonly IResearchRequestsSearchService researchRequestsSearchService;

        /// <summary>
        /// The instance of research request mapper.
        /// </summary>
        private readonly IResearchRequestMapper researchRequestMapper;

        /// <summary>
        /// The instance of research request repository.
        /// </summary>
        private readonly IResearchRequestsRepository researchRequestsRepository;

        /// <summary>
        /// The instance of resource feedback repository to store user ratings.
        /// </summary>
        private readonly IResourceFeedbackRepository resourceFeedbackRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchRequestHelper"/> class.
        /// </summary>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="researchRequestsSearchService">The instance of <see cref="ResearchRequestsSearchService"/> class.</param>
        /// <param name="researchRequestMapper">The instance of <see cref="ResearchRequestMapper"/> class.</param>
        /// <param name="researchRequestsRepository">The instance of <see cref="ResearchRequestsRepository"/> class.</param>
        /// <param name="resourceFeedbackRepository">The instance of <see cref="ResourceFeedbackRepository"/> class.</param>
        public ResearchRequestHelper(
            IFilterQueryHelper filterQueryHelper,
            IResearchRequestsSearchService researchRequestsSearchService,
            IResearchRequestMapper researchRequestMapper,
            IResearchRequestsRepository researchRequestsRepository,
            IResourceFeedbackRepository resourceFeedbackRepository)
        {
            this.filterQueryHelper = filterQueryHelper;
            this.researchRequestsSearchService = researchRequestsSearchService;
            this.researchRequestMapper = researchRequestMapper;
            this.researchRequestsRepository = researchRequestsRepository;
            this.resourceFeedbackRepository = resourceFeedbackRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResearchRequestViewDTO>> GetResearchRequestsByKeywordsAsync(IEnumerable<int> keywordIds)
        {
            var researchRequestsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(ResearchRequestEntity.Keywords), keywordIds);

            var researchRequestsSearchParametersDto = new SearchParametersDTO
            {
                IsGetAllRecords = true,
                Filter = researchRequestsFilter,
            };

            var researchRequests = await this.researchRequestsSearchService.GetResearchRequestsAsync(researchRequestsSearchParametersDto);
            return researchRequests.Select(x => this.researchRequestMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResearchRequestViewDTO>> GetResearchRequestsAsync(SearchParametersDTO searchParametersDTO)
        {
            var researchRequests = await this.researchRequestsSearchService.GetResearchRequestsAsync(searchParametersDTO);
            return researchRequests.Select(x => this.researchRequestMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<ResearchRequestViewDTO> GetResearchRequestByTableIdAsync(string researchRequestTableId, string userAadObjectId)
        {
            researchRequestTableId = researchRequestTableId ?? throw new ArgumentNullException(nameof(researchRequestTableId));
            var researchRequestEntity = await this.researchRequestsRepository.GetAsync(ResearchRequestsTableMetadata.PartitionKey, researchRequestTableId);
            var researchRequestDTO = this.researchRequestMapper.MapForViewModel(researchRequestEntity);
            var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.ResearchRequest, new List<string> { researchRequestTableId }, userAadObjectId);
            if (resourceFeedback.Any())
            {
                researchRequestDTO.UserRating = resourceFeedback.FirstOrDefault().Rating;
            }

            return researchRequestDTO;
        }

        /// <inheritdoc/>
        public async Task RateResearchRequestAsync(string researchRequestTableId, int rating, string userAadObjectId)
        {
            researchRequestTableId = researchRequestTableId ?? throw new ArgumentNullException(nameof(researchRequestTableId), "Research request table Id cannot be null.");
            var researchRequest = await this.researchRequestsRepository.GetAsync(ResearchRequestsTableMetadata.PartitionKey, researchRequestTableId);

            if (researchRequest != null)
            {
                var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.ResearchRequest, new List<string> { researchRequestTableId }, userAadObjectId);
                if (resourceFeedback.Any())
                {
                    var feedback = resourceFeedback.FirstOrDefault();
                    if (feedback.Rating > rating)
                    {
                        researchRequest.SumOfRatings -= feedback.Rating - rating;
                    }
                    else
                    {
                        researchRequest.SumOfRatings += rating - feedback.Rating;
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
                        ResourceId = researchRequestTableId,
                        ResourceTypeId = (int)Itemtype.ResearchRequest,
                    };

                    researchRequest.SumOfRatings += rating;
                    researchRequest.NumberOfRatings += 1;
                    await this.resourceFeedbackRepository.InsertOrMergeAsync(feedback);
                }
            }

            var avg = (decimal)researchRequest.SumOfRatings / (decimal)researchRequest.NumberOfRatings;
#pragma warning disable CA1305 // Culture provider is not required as its a number to string conversion
            researchRequest.AverageRating = avg.ToString("0.0");
#pragma warning restore CA1305 // Culture provider is not required as its a number to string conversion
            await this.researchRequestsRepository.CreateOrUpdateAsync(researchRequest);
            await this.researchRequestsSearchService.RunIndexerOnDemandAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResearchRequestViewDTO>> GetResearchRequestsAsync(IEnumerable<int> keywords, DateTime fromDate, int? count)
        {
            var dateFilter = this.filterQueryHelper.GetFilterConditionForDate(nameof(ResearchRequestEntity.LastUpdate), QueryComparisons.GreaterThanOrEqual, new[] { fromDate.ToZuluTimeFormatWithStartOfDay() });
            var keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(ResearchRequestEntity.Keywords), keywords);

            var filter = this.filterQueryHelper.CombineFilters(dateFilter, keywordsFilter, TableOperators.And);

            var searchParametersDto = new SearchParametersDTO
            {
                TopRecordsCount = count,
                Filter = filter,
                OrderBy = new List<string> { $"{nameof(ResearchRequestEntity.LastUpdate)} desc" },
            };

            var researchRequests = await this.researchRequestsSearchService.GetResearchRequestsAsync(searchParametersDto);
            return researchRequests.Select(x => this.researchRequestMapper.MapForViewModel(x));
        }
    }
}
