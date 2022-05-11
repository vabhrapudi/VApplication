// <copyright file="FeedbackHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// /
    /// </summary>
    public class FeedbackHelper : IFeedbackHelper
    {
        /// <summary>
        /// The instance of <see cref="AthenaFeedbackRepository"/> class.
        /// </summary>
        private readonly IAthenaFeedbackRepository athenaFeedbackRepository;

        /// <summary>
        /// The instance of <see cref="AthenaFeedbackSearchService"/> class.
        /// </summary>
        private readonly IAthenaFeedbackSearchService athenaFeedbackSearchService;

        /// <summary>
        /// The instance of <see cref="UserGraphServiceHelper"/> class.
        /// </summary>
        private readonly IUserGraphServiceHelper userGraphServiceHelper;

        /// <summary>
        /// The instance of <see cref="AthenaFeedbackMapper"/> class.
        /// </summary>
        private readonly IAthenaFeedbackMapper athenaFeedbackMapper;

        /// <summary>
        /// The instance of <see cref="FilterQueryHelper"/> class.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackHelper"/> class.
        /// </summary>
        /// <param name="athenaFeedbackRepository">The instance of <see cref="AthenaFeedbackRepository"/> class.</param>
        /// <param name="athenaFeedbackSearchService">The instance of <see cref="AthenaFeedbackSearchService"/> class.</param>
        /// <param name="userGraphServiceHelper">The instance of <see cref="UserGraphServiceHelper"/> class.</param>
        /// <param name="athenaFeedbackMapper">The instance of <see cref="AthenaFeedbackMapper"/> class.</param>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        public FeedbackHelper(
            IAthenaFeedbackRepository athenaFeedbackRepository,
            IAthenaFeedbackSearchService athenaFeedbackSearchService,
            IUserGraphServiceHelper userGraphServiceHelper,
            IAthenaFeedbackMapper athenaFeedbackMapper,
            IFilterQueryHelper filterQueryHelper)
        {
            this.athenaFeedbackRepository = athenaFeedbackRepository;
            this.athenaFeedbackSearchService = athenaFeedbackSearchService;
            this.userGraphServiceHelper = userGraphServiceHelper;
            this.athenaFeedbackMapper = athenaFeedbackMapper;
            this.filterQueryHelper = filterQueryHelper;
        }

        /// <inheritdoc/>
        public async Task SaveAthenaFeedback(AthenaFeedbackCreateDTO athenaFeedbackCreateDTO, string userAadObjectId)
        {
            athenaFeedbackCreateDTO = athenaFeedbackCreateDTO ?? throw new ArgumentNullException(nameof(athenaFeedbackCreateDTO));
            if (string.IsNullOrEmpty(userAadObjectId))
            {
                throw new ArgumentNullException(nameof(userAadObjectId));
            }

            var feedbackEntity = this.athenaFeedbackMapper.MapForCreateModel(athenaFeedbackCreateDTO, userAadObjectId);

            await this.athenaFeedbackRepository.InsertOrMergeAsync(feedbackEntity);

            await this.athenaFeedbackSearchService.RunIndexerOnDemandAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AthenaFeedbackViewDTO>> GetAthenaFeedbacksAsync(int pageNumber,  int sortBy, IEnumerable<int> feedbackFilterValues)
        {
            var feedbacks = new List<AthenaFeedbackViewDTO>();

            var searchParamsDTO = new SearchParametersDTO
            {
                PageCount = pageNumber,
                SortByFilter = sortBy,
                Filter = this.filterQueryHelper.GetFilterCondition(nameof(AthenaFeedbackEntity.Feedback), feedbackFilterValues),
            };

            var feedbackEntities = await this.athenaFeedbackSearchService.GetAthenaFeedbacksAsync(searchParamsDTO);

            if (!feedbackEntities.IsNullOrEmpty())
            {
                var userDetails = await this.userGraphServiceHelper.GetUsersAsync(feedbackEntities.Where(request => !string.IsNullOrEmpty(request.CreatedBy)).Select(request => request.CreatedBy));

                foreach (var feedback in feedbackEntities)
                {
                    var userDetail = userDetails.Where(user => user.Id == feedback.CreatedBy).FirstOrDefault();
                    var feedbackViewDTO = this.athenaFeedbackMapper.MapForViewModel(feedback, userDetail);
                    feedbacks.Add(feedbackViewDTO);
                }
            }

            return feedbacks;
        }

        /// <inheritdoc/>
        public async Task<AthenaFeedbackViewDTO> GetAthenaFeedbackDetailsAsync(string feedbackId)
        {
            var feedbackEntity = await this.athenaFeedbackRepository.GetAsync(AthenaFeedbackTableNames.AthenaFeedbackPartition, feedbackId);
            if (feedbackEntity != null)
            {
                var userDetails = await this.userGraphServiceHelper.GetUsersAsync(new[] { feedbackEntity.CreatedBy });
                return this.athenaFeedbackMapper.MapForViewModel(feedbackEntity, userDetails.FirstOrDefault());
            }

            return null;
        }
    }
}
