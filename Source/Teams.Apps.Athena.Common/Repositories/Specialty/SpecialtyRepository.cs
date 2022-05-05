// <copyright file="SpecialtyRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories.Specialty
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the specialty data stored in the table storage.
    /// </summary>
    public class SpecialtyRepository : BaseRepository<SpecialtyEntity>, ISpecialtyRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialtyRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public SpecialtyRepository(
            ILogger<SpecialtyRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: SpecialtyTableNames.TableName,
                  defaultPartitionKey: SpecialtyTableNames.SpecialtyPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}