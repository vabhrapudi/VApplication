// <copyright file="FaqRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the FAQ data stored in the table storage.
    /// </summary>
    public class FaqRepository : BaseRepository<FaqEntity>, IFaqRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaqRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public FaqRepository(
            ILogger<FaqRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: FaqTableMetadata.TableName,
                  defaultPartitionKey: FaqTableMetadata.FaqPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}