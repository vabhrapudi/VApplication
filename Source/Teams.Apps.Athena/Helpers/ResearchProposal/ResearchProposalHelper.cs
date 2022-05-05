// <copyright file="ResearchProposalHelper.cs" company="NPS Foundation">
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

    /// <summary>
    /// The helper class for managing research proposal entities.
    /// </summary>
    public class ResearchProposalHelper : IResearchProposalHelper
    {
        /// <summary>
        /// The instance of filter query helper.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// The instance of research proposal search service.
        /// </summary>
        private readonly IResearchProposalsSearchService researchProposalsSearchService;

        /// <summary>
        /// The instance of research proposal mapper.
        /// </summary>
        private readonly IResearchProposalMapper researchProposalMapper;

        /// <summary>
        /// The instance of research proposal repository.
        /// </summary>
        private readonly IResearchProposalsRepository researchProposalsRepository;

        /// <summary>
        /// The instance of resource feedback repository to store user ratings.
        /// </summary>
        private readonly IResourceFeedbackRepository resourceFeedbackRepository;

        /// <summary>
        /// The instance of user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchProposalHelper"/> class.
        /// </summary>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="researchProposalsSearchService">The instance of <see cref="ResearchProposalsSearchService"/> class.</param>
        /// <param name="researchProposalMapper">The instance of <see cref="ResearchProposalMapper"/> class.</param>
        /// <param name="researchProposalsRepository">The instance of <see cref="ResearchProposalsRepository"/> class.</param>
        /// <param name="resourceFeedbackRepository">The instance of <see cref="ResourceFeedbackRepository"/> class.</param>
        /// <param name="userRepository">The instance of <see cref="UserRepository"/> class.</param>
        public ResearchProposalHelper(
            IFilterQueryHelper filterQueryHelper,
            IResearchProposalsSearchService researchProposalsSearchService,
            IResearchProposalMapper researchProposalMapper,
            IResearchProposalsRepository researchProposalsRepository,
            IResourceFeedbackRepository resourceFeedbackRepository,
            IUserRepository userRepository)
        {
            this.filterQueryHelper = filterQueryHelper;
            this.researchProposalsSearchService = researchProposalsSearchService;
            this.researchProposalMapper = researchProposalMapper;
            this.researchProposalsRepository = researchProposalsRepository;
            this.resourceFeedbackRepository = resourceFeedbackRepository;
            this.userRepository = userRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResearchProposalViewDTO>> GetResearchProposalsByKeywordsAsync(IEnumerable<int> keywordIds)
        {
            var researchProposalsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(ResearchProposalEntity.Keywords), keywordIds);

            var researchProposalsSearchParametersDto = new SearchParametersDTO
            {
                IsGetAllRecords = true,
                Filter = researchProposalsFilter,
            };

            var researchProposals = await this.researchProposalsSearchService.GetResearchProposalsAsync(researchProposalsSearchParametersDto);
            var reserchProposalDTO = new List<ResearchProposalViewDTO>();
            foreach (var researchProposal in researchProposals)
            {
                var researchProposalDto = this.researchProposalMapper.MapForViewModel(researchProposal);
                if (researchProposalDto.SubmitterId != -1)
                {
                    var userDetails = await this.userRepository.GetUserDetailsByExternalUserIdAsync(researchProposalDto.SubmitterId);
                    if (userDetails != null)
                    {
                        researchProposalDto.SubmittedBy = userDetails.UserDisplayName;
                    }
                }
                else
                {
                    var userDetails = await this.userRepository.GetUserDetailsByUserIdAsync(researchProposalDto.UserId);
                    if (userDetails != null)
                    {
                        researchProposalDto.SubmittedBy = userDetails.UserDisplayName;
                    }
                }

                reserchProposalDTO.Add(researchProposalDto);
            }

            return reserchProposalDTO;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResearchProposalViewDTO>> GetResearchProposalsAsync(SearchParametersDTO searchParametersDTO)
        {
            var researchProposals = await this.researchProposalsSearchService.GetResearchProposalsAsync(searchParametersDTO);
            var reserchProposalDTO = new List<ResearchProposalViewDTO>();
            foreach (var researchProposal in researchProposals)
            {
                var researchProposalDto = this.researchProposalMapper.MapForViewModel(researchProposal);
                if (researchProposalDto.SubmitterId != -1)
                {
                    var userDetails = await this.userRepository.GetUserDetailsByExternalUserIdAsync(researchProposalDto.SubmitterId);
                    if (userDetails != null)
                    {
                        researchProposalDto.SubmittedBy = userDetails.UserDisplayName;
                    }
                }
                else
                {
                    var userDetails = await this.userRepository.GetUserDetailsByUserIdAsync(researchProposalDto.UserId);
                    if (userDetails != null)
                    {
                        researchProposalDto.SubmittedBy = userDetails.UserDisplayName;
                    }
                }

                reserchProposalDTO.Add(researchProposalDto);
            }

            return reserchProposalDTO;
        }

        /// <inheritdoc/>
        public async Task<ResearchProposalEntity> CreateResearchProposalAsync(ResearchProposalCreateDTO researchProposalCreateDTO, string userAadId)
        {
            researchProposalCreateDTO = researchProposalCreateDTO ?? throw new ArgumentNullException(nameof(researchProposalCreateDTO));

            var existingReseachProposal = await this.researchProposalsRepository.GetResearchProposalAsync(researchProposalCreateDTO.Title.Trim());

            // Reseach proposal can not be created if research project with the same title already exists.
            if (existingReseachProposal != null)
            {
                return null;
            }

            var researchProposalDetails = this.researchProposalMapper.MapForCreateModel(researchProposalCreateDTO, userAadId);
            var userDetails = await this.userRepository.GetUserDetailsByUserIdAsync(userAadId);
            if (userDetails != null)
            {
                researchProposalDetails.SubmitterId = userDetails.ExternalUserId;
            }
            else
            {
                researchProposalDetails.SubmitterId = Constants.SourceAthena;
            }

            await this.researchProposalsRepository.CreateOrUpdateAsync(researchProposalDetails);
            return researchProposalDetails;
        }

        /// <inheritdoc/>
        public async Task<ResearchProposalViewDTO> GetResearchProposalByTableIdAsync(string researchProposalTableId, string userAadObjectId)
        {
            researchProposalTableId = researchProposalTableId ?? throw new ArgumentNullException(nameof(researchProposalTableId));
            var researchProposalEntity = await this.researchProposalsRepository.GetAsync(ResearchProposalsTableMetadata.PartitionKey, researchProposalTableId);
            var researchProposalDTO = this.researchProposalMapper.MapForViewModel(researchProposalEntity);
            var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.ResearchProposal, new List<string> { researchProposalTableId }, userAadObjectId);
            if (resourceFeedback.Any())
            {
                researchProposalDTO.UserRating = resourceFeedback.FirstOrDefault().Rating;
            }

            return researchProposalDTO;
        }

         /// <inheritdoc/>
        public async Task RateResearchProposalAsync(string researchProposalTableId, int rating, string userAadObjectId)
        {
            researchProposalTableId = researchProposalTableId ?? throw new ArgumentNullException(nameof(researchProposalTableId), "Research proposal table Id cannot be null.");
            var researchProposal = await this.researchProposalsRepository.GetAsync(ResearchProposalsTableMetadata.PartitionKey, researchProposalTableId);

            if (researchProposal != null)
            {
                var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.ResearchProposal, new List<string> { researchProposalTableId }, userAadObjectId);
                if (resourceFeedback.Any())
                {
                    var feedback = resourceFeedback.FirstOrDefault();
                    if (feedback.Rating > rating)
                    {
                        researchProposal.SumOfRatings -= feedback.Rating - rating;
                    }
                    else
                    {
                        researchProposal.SumOfRatings += rating - feedback.Rating;
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
                        ResourceId = researchProposalTableId,
                        ResourceTypeId = (int)Itemtype.ResearchProposal,
                    };

                    researchProposal.SumOfRatings += rating;
                    researchProposal.NumberOfRatings += 1;
                    await this.resourceFeedbackRepository.InsertOrMergeAsync(feedback);
                }
            }

            var avg = (decimal)researchProposal.SumOfRatings / (decimal)researchProposal.NumberOfRatings;
#pragma warning disable CA1305 // Culture provider is not required as its a number to string conversion
            researchProposal.AverageRating = avg.ToString("0.0");
#pragma warning restore CA1305 // Culture provider is not required as its a number to string conversion
            await this.researchProposalsRepository.CreateOrUpdateAsync(researchProposal);
            await this.researchProposalsSearchService.RunIndexerOnDemandAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ResearchProposalViewDTO>> GetResearchProposalsAsync(IEnumerable<int> keywords, DateTime fromDate, int? count)
        {
            var dateFilter = this.filterQueryHelper.GetFilterConditionForDate(nameof(ResearchProposalEntity.LastUpdate), QueryComparisons.GreaterThanOrEqual, new[] { fromDate.ToZuluTimeFormatWithStartOfDay() });
            var keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(ResearchProposalEntity.Keywords), keywords);

            var filter = this.filterQueryHelper.CombineFilters(dateFilter, keywordsFilter, TableOperators.And);

            var searchParametersDto = new SearchParametersDTO
            {
                TopRecordsCount = count,
                Filter = filter,
                OrderBy = new List<string> { $"{nameof(ResearchProposalEntity.LastUpdate)} desc" },
            };

            var researchProposals = await this.researchProposalsSearchService.GetResearchProposalsAsync(searchParametersDto);
            return researchProposals.Select(x => this.researchProposalMapper.MapForViewModel(x));
        }
    }
}
