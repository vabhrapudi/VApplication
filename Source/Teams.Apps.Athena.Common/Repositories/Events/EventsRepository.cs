// <copyright file="EventsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The repository for managing table operations related to events.
    /// </summary>
    public class EventsRepository : BaseRepository<EventEntity>, IEventsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventsRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public EventsRepository(
            ILogger<EventsRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 EventsTableMetadata.TableName,
                 EventsTableMetadata.PartitionKey,
                 repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<EventEntity> GetEventDetailsByEventIdAsync(int eventId)
        {
                var eventFilter = TableQuery.GenerateFilterConditionForInt(
                           nameof(EventEntity.EventId),
                           QueryComparisons.Equal,
                           eventId);
                var eventRecord = await this.GetWithFilterAsync(eventFilter);
                return eventRecord.FirstOrDefault();
        }
    }
}
