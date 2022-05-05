// <copyright file="EventHelper.cs" company="NPS Foundation">
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
    /// The helper methods related to Athena events.
    /// </summary>
    public class EventHelper : IEventHelper
    {
        /// <summary>
        /// The instance of filter query helper.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// The instance of athena events search service.
        /// </summary>
        private readonly IAthenaEventsSearchService athenaEventsSearchService;

        /// <summary>
        /// The instance of athena event mapper.
        /// </summary>
        private readonly IAthenaEventMapper athenaEventMapper;

        /// <summary>
        /// The instance of events repository.
        /// </summary>
        private readonly IEventsRepository eventsRepository;

        /// <summary>
        /// The instance of resource feedback repository to store user ratings.
        /// </summary>
        private readonly IResourceFeedbackRepository resourceFeedbackRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHelper"/> class.
        /// </summary>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="athenaEventsSearchService">The instance of <see cref="AthenaEventsSearchService"/> class.</param>
        /// <param name="athenaEventMapper">The instance of <see cref="AthenaEventMapper"/> class.</param>
        /// <param name="eventsRepository">The instance of <see cref="EventsRepository"/> class.</param>
        /// <param name="resourceFeedbackRepository">The instance of <see cref="ResourceFeedbackRepository"/> class.</param>
        public EventHelper(
            IFilterQueryHelper filterQueryHelper,
            IAthenaEventsSearchService athenaEventsSearchService,
            IAthenaEventMapper athenaEventMapper,
            IEventsRepository eventsRepository,
            IResourceFeedbackRepository resourceFeedbackRepository)
        {
            this.filterQueryHelper = filterQueryHelper;
            this.athenaEventsSearchService = athenaEventsSearchService;
            this.athenaEventMapper = athenaEventMapper;
            this.eventsRepository = eventsRepository;
            this.resourceFeedbackRepository = resourceFeedbackRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventDTO>> GetEventsByKeywordsIdsAsync(IEnumerable<int> keywordIds)
        {
            var eventsByKeywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(EventEntity.Keywords), keywordIds);

            var eventsSearchParametersDto = new SearchParametersDTO
            {
                IsGetAllRecords = true,
                Filter = eventsByKeywordsFilter,
            };

            var events = await this.athenaEventsSearchService.GetEventsAsync(eventsSearchParametersDto);
            return events.Select(x => this.athenaEventMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventDTO>> GetEventsAsync(SearchParametersDTO searchParametersDTO)
        {
            var events = await this.athenaEventsSearchService.GetEventsAsync(searchParametersDTO);
            return events.Select(x => this.athenaEventMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<EventDTO> GetEventByTableIdAsync(string eventTableId, string userAadObjectId)
        {
            eventTableId = eventTableId ?? throw new ArgumentNullException(nameof(eventTableId));
            var eventEntity = await this.eventsRepository.GetAsync(EventsTableMetadata.PartitionKey, eventTableId);
            var eventDTO = this.athenaEventMapper.MapForViewModel(eventEntity);
            var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.Event, new List<string> { eventTableId }, userAadObjectId);
            if (resourceFeedback.Any())
            {
                eventDTO.UserRating = resourceFeedback.FirstOrDefault().Rating;
            }

            return eventDTO;
        }

        /// <inheritdoc/>
        public async Task RateEventAsync(string eventTableId, int rating, string userAadObjectId)
        {
            eventTableId = eventTableId ?? throw new ArgumentNullException(nameof(eventTableId), "Event table Id cannot be null.");
            var eventEntity = await this.eventsRepository.GetAsync(EventsTableMetadata.PartitionKey, eventTableId);

            if (eventEntity != null)
            {
                var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.Event, new List<string> { eventTableId }, userAadObjectId);
                if (resourceFeedback.Any())
                {
                    var feedback = resourceFeedback.FirstOrDefault();
                    if (feedback.Rating > rating)
                    {
                        eventEntity.SumOfRatings -= feedback.Rating - rating;
                    }
                    else
                    {
                        eventEntity.SumOfRatings += rating - feedback.Rating;
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
                        ResourceId = eventTableId,
                        ResourceTypeId = (int)Itemtype.Event,
                    };

                    eventEntity.SumOfRatings += rating;
                    eventEntity.NumberOfRatings += 1;
                    await this.resourceFeedbackRepository.InsertOrMergeAsync(feedback);
                }
            }

            var avg = (decimal)eventEntity.SumOfRatings / (decimal)eventEntity.NumberOfRatings;
#pragma warning disable CA1305 // Culture provider is not required as its a number to string conversion
            eventEntity.AverageRating = avg.ToString("0.0");
#pragma warning restore CA1305 // Culture provider is not required as its a number to string conversion
            await this.eventsRepository.CreateOrUpdateAsync(eventEntity);
            await this.athenaEventsSearchService.RunIndexerOnDemandAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventDTO>> GetEventsAsync(IEnumerable<int> keywords, DateTime fromDate, int? count)
        {
            var dateFilter = this.filterQueryHelper.GetFilterConditionForDate(nameof(EventEntity.LastUpdate), QueryComparisons.GreaterThanOrEqual, new[] { fromDate.ToZuluTimeFormatWithStartOfDay() });
            var keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(EventEntity.Keywords), keywords);

            var filter = this.filterQueryHelper.CombineFilters(dateFilter, keywordsFilter, TableOperators.And);

            var searchParametersDto = new SearchParametersDTO
            {
                TopRecordsCount = count,
                Filter = filter,
                OrderBy = new List<string> { $"{nameof(EventEntity.LastUpdate)} desc" },
            };

            var events = await this.athenaEventsSearchService.GetEventsAsync(searchParametersDto);
            return events.Select(x => this.athenaEventMapper.MapForViewModel(x));
        }
    }
}
