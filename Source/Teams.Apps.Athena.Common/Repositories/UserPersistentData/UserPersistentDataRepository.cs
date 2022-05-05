// <copyright file="UserPersistentDataRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the user persistent data stored in the table storage.
    /// </summary>
    public class UserPersistentDataRepository : BaseRepository<UserPersistentDataEntity>, IUserPersistentDataRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPersistentDataRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public UserPersistentDataRepository(
            ILogger<UserPersistentDataRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: UserPersistentDataTableMetadata.TableName,
                  defaultPartitionKey: UserPersistentDataTableMetadata.PartitionKey,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}
