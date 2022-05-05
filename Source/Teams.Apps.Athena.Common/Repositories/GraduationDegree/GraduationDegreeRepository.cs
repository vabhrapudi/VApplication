// <copyright file="GraduationDegreeRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories.GraduationDegree
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the graduation degree data stored in the table storage.
    /// </summary>
    public class GraduationDegreeRepository : BaseRepository<GraduationDegree>, IGraduationDegreeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraduationDegreeRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public GraduationDegreeRepository(
            ILogger<GraduationDegreeRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: GraduationDegreeTableNames.TableName,
                  defaultPartitionKey: GraduationDegreeTableNames.GraduationDegreePartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}