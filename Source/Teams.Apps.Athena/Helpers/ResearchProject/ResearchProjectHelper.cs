// <copyright file="ResearchProjectHelper.cs" company="NPS Foundation">
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
    /// Provides helper methods for managing research project entity.
    /// </summary>
    public class ResearchProjectHelper : IResearchProjectHelper
    {
        /// <summary>
        /// The instance of research project model mapper.
        /// </summary>
        private readonly IResearchProjectMapper researchProjectMapper;

        /// <summary>
        /// The instance of research project repository.
        /// </summary>
        private readonly IResearchProjectsRepository researchProjectsRepository;

        /// <summary>
        /// Holds the instance of <see cref="FilterQueryHelper"/> class.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// Holds the instance of <see cref="ResearchProjectsSearchService"/> class.
        /// </summary>
        private readonly IResearchProjectsSearchService researchProjectsSearchService;

        /// <summary>
        /// The instance of resource feedback repository to store user ratings.
        /// </summary>
        private readonly IResourceFeedbackRepository resourceFeedbackRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchProjectHelper"/> class.
        /// </summary>
        /// <param name="researchProjectsRepository">The instance of <see cref="ResearchProjectsRepository"/> class.</param>
        /// <param name="researchProjectMapper">The instance of <see cref="ResearchProjectMapper"/> class.</param>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="researchProjectsSearchService">The instance of <see cref="ResearchProjectsSearchService"/> class.</param>
        /// <param name="resourceFeedbackRepository">The instance of <see cref="ResourceFeedbackRepository"/> class.</param>
        public ResearchProjectHelper(
            IResearchProjectsRepository researchProjectsRepository,
            IResearchProjectMapper researchProjectMapper,
            IFilterQueryHelper filterQueryHelper,
            IResearchProjectsSearchService researchProjectsSearchService,
            IResourceFeedbackRepository resourceFeedbackRepository)
        {
            this.researchProjectsRepository = researchProjectsRepository;
            this.researchProjectMapper = researchProjectMapper;
            this.filterQueryHelper = filterQueryHelper;
            this.researchProjectsSearchService = researchProjectsSearchService;
            this.resourceFeedbackRepository = resourceFeedbackRepository;
        }

        /// <inheritdoc/>
        public async Task<ResearchProjectEntity> CreateResearchProjectAsync(ResearchProjectCreateDTO researchProjectCreateDTO)
        {
            researchProjectCreateDTO = researchProjectCreateDTO ?? throw new ArgumentNullException(nameof(researchProjectCreateDTO));

            var existingReseachProject = await this.researchProjectsRepository.GetMatchingResearchProjectAsync(researchProjectCreateDTO.Title.Trim());

            // Reseach project can not be created if research project with the same title already exists.
            if (existingReseachProject != null)
            {
                return null;
            }

            var researchProjectDetails = this.researchProjectMapper.MapForCreateModel(researchProjectCreateDTO);
            await this.researchProjectsRepository.CreateOrUpdateAsync(researchProjectDetails);
            return researchProjectDetails;
        }

        /// <inheritdoc/>
        public async Task<ResearchProjectDTO> GetResearchProjectByIdAsync(string researchProjectTableId, string userAadObjectId)
        {
            researchProjectTableId = researchProjectTableId ?? throw new ArgumentNullException(nameof(researchProjectTableId));
            var researchProjectEntity = await this.researchProjectsRepository.GetAsync(ResearchProjectsTableMetadata.PartitionKey, researchProjectTableId);
            var researchProjectDTO = this.researchProjectMapper.MapForViewModel(researchProjectEntity);
            var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.ResearchProject, new List<string> { researchProjectTableId }, userAadObjectId);
            if (resourceFeedback.Any())
            {
                researchProjectDTO.UserRating = resourceFeedback.FirstOrDefault().Rating;
            }

            return researchProjectDTO;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResearchProjectDTO>> GetResearchProjectsByKeywordsAsync(IEnumerable<int> keywordIds)
        {
            var researchProjectsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(ResearchProjectEntity.Keywords), keywordIds);

            var researchProjectsSearchParametersDto = new SearchParametersDTO
            {
                IsGetAllRecords = true,
                Filter = researchProjectsFilter,
            };

            var researchProjects = await this.researchProjectsSearchService.GetResearchProjectsAsync(researchProjectsSearchParametersDto);
            return researchProjects.Select(x => this.researchProjectMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task RateResearchProjectAsync(string researchProjectTableId, int rating, string userAadObjectId)
        {
            researchProjectTableId = researchProjectTableId ?? throw new ArgumentNullException(nameof(researchProjectTableId), "Research project table Id cannot be null.");
            var researchProject = await this.researchProjectsRepository.GetAsync(ResearchProjectsTableMetadata.PartitionKey, researchProjectTableId);

            if (researchProject != null)
            {
                var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.ResearchProject, new List<string> { researchProjectTableId }, userAadObjectId);
                if (resourceFeedback.Any())
                {
                    var feedback = resourceFeedback.FirstOrDefault();
                    if (feedback.Rating > rating)
                    {
                        researchProject.SumOfRatings -= feedback.Rating - rating;
                    }
                    else
                    {
                        researchProject.SumOfRatings += rating - feedback.Rating;
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
                        ResourceId = researchProjectTableId,
                        ResourceTypeId = (int)Itemtype.ResearchProject,
                    };

                    researchProject.SumOfRatings += rating;
                    researchProject.NumberOfRatings += 1;
                    await this.resourceFeedbackRepository.InsertOrMergeAsync(feedback);
                }
            }

            var avg = (decimal)researchProject.SumOfRatings / (decimal)researchProject.NumberOfRatings;
#pragma warning disable CA1305 // Culture provider is not required as its a number to string conversion
            researchProject.AverageRating = avg.ToString("0.0");
#pragma warning restore CA1305 // Culture provider is not required as its a number to string conversion
            await this.researchProjectsRepository.CreateOrUpdateAsync(researchProject);
            await this.researchProjectsSearchService.RunIndexerOnDemandAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResearchProjectDTO>> GetResearchProjectsAsync(SearchParametersDTO searchParametersDTO)
        {
            var researchProjects = await this.researchProjectsSearchService.GetResearchProjectsAsync(searchParametersDTO);
            return researchProjects.Select(x => this.researchProjectMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResearchProjectDTO>> GetResearchProjectsAsync(IEnumerable<int> keywords, DateTime fromDate, int? count)
        {
            var dateFilter = this.filterQueryHelper.GetFilterConditionForDate(nameof(ResearchProjectEntity.LastUpdate), QueryComparisons.GreaterThanOrEqual, new[] { fromDate.ToZuluTimeFormatWithStartOfDay() });
            var keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(ResearchProjectEntity.Keywords), keywords);

            var filter = this.filterQueryHelper.CombineFilters(dateFilter, keywordsFilter, TableOperators.And);

            var searchParametersDto = new SearchParametersDTO
            {
                TopRecordsCount = count,
                Filter = filter,
                OrderBy = new List<string> { $"{nameof(ResearchProjectEntity.LastUpdate)} desc" },
            };

            var researchProjects = await this.researchProjectsSearchService.GetResearchProjectsAsync(searchParametersDto);
            return researchProjects.Select(x => this.researchProjectMapper.MapForViewModel(x));
        }
    }
}