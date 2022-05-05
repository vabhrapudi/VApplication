// <copyright file="SponsorHelper.cs" company="NPS Foundation">
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
    /// The helper methods related to sponsor.
    /// </summary>
    public class SponsorHelper : ISponsorHelper
    {
        /// <summary>
        /// The instance of sponsors search services to access search service.
        /// </summary>
        private readonly ISponsorsSearchService sponsorsSearchServices;

        /// <summary>
        /// The instance of filter query helper.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// The instance of sponsors mapper.
        /// </summary>
        private readonly ISponsorsMapper sponsorMapper;

        /// <summary>
        /// The instance of sponsor repository.
        /// </summary>
        private readonly ISponsorRepository sponsorRepository;

        /// <summary>
        /// The instance of resource feedback repository to store user ratings.
        /// </summary>
        private readonly IResourceFeedbackRepository resourceFeedbackRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SponsorHelper"/> class.
        /// </summary>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="sponsorsSearchServices">The instance of <see cref="SponsorsSearchService"/> class.</param>
        /// <param name="sponsorMapper">The instance of <see cref="SponsorsMapper"/> class.</param>
        /// <param name="sponsorRepository">The instance of <see cref="SponsorRepository"/> class.</param>
        /// <param name="resourceFeedbackRepository">The instance of <see cref="ResourceFeedbackRepository"/> class.</param>
        public SponsorHelper(
            IFilterQueryHelper filterQueryHelper,
            ISponsorsSearchService sponsorsSearchServices,
            ISponsorsMapper sponsorMapper,
            ISponsorRepository sponsorRepository,
            IResourceFeedbackRepository resourceFeedbackRepository)
        {
            this.filterQueryHelper = filterQueryHelper;
            this.sponsorsSearchServices = sponsorsSearchServices;
            this.sponsorMapper = sponsorMapper;
            this.sponsorRepository = sponsorRepository;
            this.resourceFeedbackRepository = resourceFeedbackRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SponsorDTO>> GetSponsorsByKeywordsAsync(IEnumerable<int> keywordIds)
        {
            var sponsorsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(SponsorEntity.Keywords), keywordIds);

            var sponsorsSearchParametersDto = new SearchParametersDTO
            {
                IsGetAllRecords = true,
                Filter = sponsorsFilter,
            };

            var sponsors = await this.sponsorsSearchServices.GetSponsorsAsync(sponsorsSearchParametersDto);
            return sponsors.Select(x => this.sponsorMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<SponsorDTO>> GetSponsorsAsync(SearchParametersDTO searchParametersDTO)
        {
            var sponsors = await this.sponsorsSearchServices.GetSponsorsAsync(searchParametersDTO);
            return sponsors.Select(x => this.sponsorMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<SponsorDTO> GetSponsorByTableIdAsync(string sponsorTableId, string userAadObjectId)
        {
            sponsorTableId = sponsorTableId ?? throw new ArgumentNullException(nameof(sponsorTableId));
            var sponsorEntity = await this.sponsorRepository.GetAsync(SponsorTableMetadata.SponsorPartitionKey, sponsorTableId);
            var sponsorDTO = this.sponsorMapper.MapForViewModel(sponsorEntity);
            var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.Sponsor, new List<string> { sponsorTableId }, userAadObjectId);
            if (resourceFeedback.Any())
            {
                sponsorDTO.UserRating = resourceFeedback.FirstOrDefault().Rating;
            }

            return sponsorDTO;
        }

        /// <inheritdoc/>
        public async Task RateSponsorAsync(string sponsorTableId, int rating, string userAadObjectId)
        {
            sponsorTableId = sponsorTableId ?? throw new ArgumentNullException(nameof(sponsorTableId), "Sponsor table Id cannot be null.");
            var sponsor = await this.sponsorRepository.GetAsync(SponsorTableMetadata.SponsorPartitionKey, sponsorTableId);

            if (sponsor != null)
            {
                var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.Sponsor, new List<string> { sponsorTableId }, userAadObjectId);
                if (resourceFeedback.Any())
                {
                    var feedback = resourceFeedback.FirstOrDefault();
                    if (feedback.Rating > rating)
                    {
                        sponsor.SumOfRatings -= feedback.Rating - rating;
                    }
                    else
                    {
                        sponsor.SumOfRatings += rating - feedback.Rating;
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
                        ResourceId = sponsorTableId,
                        ResourceTypeId = (int)Itemtype.Sponsor,
                    };

                    sponsor.SumOfRatings += rating;
                    sponsor.NumberOfRatings += 1;
                    await this.resourceFeedbackRepository.InsertOrMergeAsync(feedback);
                }
            }

            var avg = (decimal)sponsor.SumOfRatings / (decimal)sponsor.NumberOfRatings;
#pragma warning disable CA1305 // Culture provider is not required as its a number to string conversion
            sponsor.AverageRating = avg.ToString("0.0");
#pragma warning restore CA1305 // Culture provider is not required as its a number to string conversion
            await this.sponsorRepository.CreateOrUpdateAsync(sponsor);
            await this.sponsorsSearchServices.RunIndexerOnDemandAsync();
        }
    }
}