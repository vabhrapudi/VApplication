// <copyright file="QuickAccessRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the quick access stored in the table storage.
    /// </summary>
    public class QuickAccessRepository : BaseRepository<QuickAccessEntity>, IQuickAccessRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickAccessRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public QuickAccessRepository(
            ILogger<QuickAccessRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: QuickAccessTableMetadata.TableName,
                  defaultPartitionKey: QuickAccessTableMetadata.PartitionKey,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<QuickAccessEntity>> GetQuickAccessListByUserIdAsync(string userId)
        {
            var userIdFilter = TableQuery.GenerateFilterCondition(
                nameof(QuickAccessEntity.UserId), QueryComparisons.Equal, userId);

            return await this.GetWithFilterAsync(userIdFilter);
        }

        /// <inheritdoc/>
        public async Task<QuickAccessEntity> GetQuickAccessItemByTaxonomyIdAsync(string taxonomyId, string userId)
        {
            var taxonomyIdFilter = TableQuery.GenerateFilterCondition(
                nameof(QuickAccessEntity.TaxonomyId), QueryComparisons.Equal, taxonomyId);

            var userIdFilter = TableQuery.GenerateFilterCondition(
                nameof(QuickAccessEntity.UserId), QueryComparisons.Equal, userId);

            var filter = TableQuery.CombineFilters(taxonomyIdFilter, TableOperators.And, userIdFilter);

            var quickAccessEntities = await this.GetWithFilterAsync(filter);
            return quickAccessEntities.FirstOrDefault();
        }
    }
}