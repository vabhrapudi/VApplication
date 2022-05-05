// <copyright file="ActivityRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Models.Enums;

    /// <summary>
    /// Repository to store activity data for adaptive cards sent in personal or team scope.
    /// </summary>
    public class ActivityRepository : BaseRepository<ActivityEntity>, IActivityRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public ActivityRepository(
            ILogger<ActivityRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: ActivityTableMetadata.TableName,
                  defaultPartitionKey: ActivityTableMetadata.DefaultPartitionKey,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public string GetPartitionKey(string itemId, Itemtype itemType)
        {
            return $"{itemId}${(int)itemType}";
        }
    }
}
