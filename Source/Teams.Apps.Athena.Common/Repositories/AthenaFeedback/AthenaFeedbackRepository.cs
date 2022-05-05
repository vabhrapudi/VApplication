// <copyright file="AthenaFeedbackRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the team data stored in the table storage.
    /// </summary>
    public class AthenaFeedbackRepository : BaseRepository<AthenaFeedbackEntity>, IAthenaFeedbackRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaFeedbackRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public AthenaFeedbackRepository(
            ILogger<AthenaFeedbackRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: AthenaFeedbackTableNames.TableName,
                  defaultPartitionKey: AthenaFeedbackTableNames.AthenaFeedbackPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}