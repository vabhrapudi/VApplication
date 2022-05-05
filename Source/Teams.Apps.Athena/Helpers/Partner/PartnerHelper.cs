// <copyright file="PartnerHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Models.Enums;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Repositories.ResourceFeedback;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The helper methods related to Athena partners entity.
    /// </summary>
    public class PartnerHelper : IPartnerHelper
    {
        /// <summary>
        /// The instance of filter query helper.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// The instance of partner serach service.
        /// </summary>
        private readonly IPartnersSearchService partnersSearchService;

        /// <summary>
        /// The instance of partner mapper.
        /// </summary>
        private readonly IPartnerMapper partnerMapper;

        /// <summary>
        /// The instance of partner repository.
        /// </summary>
        private readonly IPartnersRepository partnersRepository;

        /// <summary>
        /// The instance of resource feedback repository to store user ratings.
        /// </summary>
        private readonly IResourceFeedbackRepository resourceFeedbackRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerHelper"/> class.
        /// </summary>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="partnersSearchService">The instance of <see cref="PartnersSearchService"/> class.</param>
        /// <param name="partnerMapper">The instance of <see cref="PartnerMapper"/> class.</param>
        /// <param name="partnersRepository">The instance of <see cref="PartnersRepository"/> class.</param>
        /// <param name="resourceFeedbackRepository">The instance of <see cref="ResourceFeedbackRepository"/> class.</param>
        public PartnerHelper(
            IFilterQueryHelper filterQueryHelper,
            IPartnersSearchService partnersSearchService,
            IPartnerMapper partnerMapper,
            IPartnersRepository partnersRepository,
            IResourceFeedbackRepository resourceFeedbackRepository)
        {
            this.filterQueryHelper = filterQueryHelper;
            this.partnersSearchService = partnersSearchService;
            this.partnerMapper = partnerMapper;
            this.partnersRepository = partnersRepository;
            this.resourceFeedbackRepository = resourceFeedbackRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PartnerDTO>> GetPartnersByKeywordsAsync(IEnumerable<int> keywordIds)
        {
            var partnersByKeywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(PartnerEntity.Keywords), keywordIds);

            var partnersSearchParametersDto = new SearchParametersDTO
            {
                IsGetAllRecords = true,
                Filter = partnersByKeywordsFilter,
            };

            var partners = await this.partnersSearchService.GetPartnersAsync(partnersSearchParametersDto);
            return partners.Select(x => this.partnerMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PartnerDTO>> GetPartnersAsync(SearchParametersDTO searchParametersDTO)
        {
            var partners = await this.partnersSearchService.GetPartnersAsync(searchParametersDTO);
            return partners.Select(x => this.partnerMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<PartnerDTO> GetPartnerByTableIdAsync(string partnerTableId, string userAadObjectId)
        {
            partnerTableId = partnerTableId ?? throw new ArgumentNullException(nameof(partnerTableId));
            var partnerEntity = await this.partnersRepository.GetAsync(PartnersTableMetadata.PartitionKey, partnerTableId);
            var partnerDTO = this.partnerMapper.MapForViewModel(partnerEntity);
            var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.Partner, new List<string> { partnerTableId }, userAadObjectId);
            if (resourceFeedback.Any())
            {
                partnerDTO.UserRating = resourceFeedback.FirstOrDefault().Rating;
            }

            return partnerDTO;
        }

        /// <inheritdoc/>
        public async Task RatePartnerAsync(string partnerTableId, int rating, string userAadObjectId)
        {
            partnerTableId = partnerTableId ?? throw new ArgumentNullException(nameof(partnerTableId), "Partner table Id cannot be null.");
            var partnerEntity = await this.partnersRepository.GetAsync(PartnersTableMetadata.PartitionKey, partnerTableId);

            if (partnerEntity != null)
            {
                var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.Partner, new List<string> { partnerTableId }, userAadObjectId);
                if (resourceFeedback.Any())
                {
                    var feedback = resourceFeedback.FirstOrDefault();
                    if (feedback.Rating > rating)
                    {
                        partnerEntity.SumOfRatings -= feedback.Rating - rating;
                    }
                    else
                    {
                        partnerEntity.SumOfRatings += rating - feedback.Rating;
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
                        ResourceId = partnerTableId,
                        ResourceTypeId = (int)Itemtype.Partner,
                    };

                    partnerEntity.SumOfRatings += rating;
                    partnerEntity.NumberOfRatings += 1;
                    await this.resourceFeedbackRepository.InsertOrMergeAsync(feedback);
                }
            }

            var avg = (decimal)partnerEntity.SumOfRatings / (decimal)partnerEntity.NumberOfRatings;
#pragma warning disable CA1305 // Culture provider is not required as its a number to string conversion
            partnerEntity.AverageRating = avg.ToString("0.0");
#pragma warning restore CA1305 // Culture provider is not required as its a number to string conversion
            await this.partnersRepository.CreateOrUpdateAsync(partnerEntity);
            await this.partnersSearchService.RunIndexerOnDemandAsync();
        }
    }
}
