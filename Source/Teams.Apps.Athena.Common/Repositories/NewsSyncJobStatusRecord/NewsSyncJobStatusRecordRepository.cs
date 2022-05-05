// <copyright file="NewsSyncJobStatusRecordRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the news sync status record data stored in the table storage.
    /// </summary>
    public class NewsSyncJobStatusRecordRepository : BaseRepository<NewsSyncJobStatusRecordEntity>, INewsSyncJobStatusRecordRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsSyncJobStatusRecordRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public NewsSyncJobStatusRecordRepository(
            ILogger<NewsSyncJobStatusRecordRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: NewsSyncJobStatusRecordTableMetadata.TableName,
                  defaultPartitionKey: NewsSyncJobStatusRecordTableMetadata.NewsSyncStatusRecordPartitionKey,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}
