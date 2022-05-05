// <copyright file="AthenaInfoResourcesRepository.cs" company="NPS Foundation">
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
    /// The repository for managing table operations related to Athena info resources.
    /// </summary>
    public class AthenaInfoResourcesRepository : BaseRepository<AthenaInfoResourceEntity>, IAthenaInfoResourcesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaInfoResourcesRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public AthenaInfoResourcesRepository(
            ILogger<AthenaInfoResourcesRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaInfoResourcesTableMetadata.TableName,
                 AthenaInfoResourcesTableMetadata.PartitionKey,
                 repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<AthenaInfoResourceEntity> GetInfoResourceByResourceIdAsync(int infoResourceId)
        {
            var infoResourceIdFilter = TableQuery.GenerateFilterConditionForInt(
                        nameof(AthenaInfoResourceEntity.InfoResourceId),
                        QueryComparisons.Equal,
                        infoResourceId);
            var infoResource = await this.GetWithFilterAsync(infoResourceIdFilter);
            return infoResource.FirstOrDefault();
        }
    }
}
