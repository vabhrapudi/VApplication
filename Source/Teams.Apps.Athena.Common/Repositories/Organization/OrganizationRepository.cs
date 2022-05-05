// <copyright file="OrganizationRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories.Organization
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the organization data stored in the table storage.
    /// </summary>
    public class OrganizationRepository : BaseRepository<OrganizationEntity>, IOrganizationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public OrganizationRepository(
            ILogger<OrganizationRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: OrganizationTableNames.TableName,
                  defaultPartitionKey: OrganizationTableNames.OrganizationPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}