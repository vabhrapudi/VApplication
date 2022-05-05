// <copyright file="SyncJobRecordRepository.cs" company="NPS Foundation">
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
    /// Repository of the sync record data stored in the table storage.
    /// </summary>
    public class SyncJobRecordRepository : BaseRepository<SyncJobRecordEntity>, ISyncJobRecordRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncJobRecordRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public SyncJobRecordRepository(
            ILogger<SyncJobRecordRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: SyncJobRecordTableMetadata.TableName,
                  defaultPartitionKey: SyncJobRecordTableMetadata.SyncRecordPartitionKey,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<SyncJobRecordEntity> GetSyncJobRecordAsync(string syncJobName)
        {
            var syncJobNameFilter = TableQuery.GenerateFilterCondition(
                        nameof(SyncJobRecordEntity.SyncJob),
                        QueryComparisons.Equal,
                        syncJobName);
            var syncJobRecord = await this.GetWithFilterAsync(syncJobNameFilter);
            return syncJobRecord.FirstOrDefault();
        }
    }
}
