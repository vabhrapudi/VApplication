// <copyright file="MyCollectionsRepository.cs" company="NPS Foundation">
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
    /// Repository of the collection data stored in the table storage.
    /// </summary>
    public class MyCollectionsRepository : BaseRepository<MyCollectionsEntity>, IMyCollectionsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyCollectionsRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public MyCollectionsRepository(
            ILogger<MyCollectionsRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: MyCollectionsTableMetadata.TableName,
                  defaultPartitionKey: MyCollectionsTableMetadata.MyCollectionsPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MyCollectionsEntity>> GetCollectionsByUserIdAsync(string userAadId)
        {
            var requestCreatedByUserFilterCondition = TableQuery.GenerateFilterCondition("CreatedBy", QueryComparisons.Equal, userAadId);
            return await this.GetWithFilterAsync(requestCreatedByUserFilterCondition);
        }

        /// <inheritdoc/>
        public async Task<MyCollectionsEntity> GetMyCollectionAsync(string name)
        {
            var collectionNameFilter = TableQuery.GenerateFilterCondition(
                        nameof(MyCollectionsEntity.Name),
                        QueryComparisons.Equal,
                        name);
            var collections = await this.GetWithFilterAsync(collectionNameFilter);
            return collections.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<MyCollectionsEntity> GetMyCollectionAsync(string name, string collectionId)
        {
            var collectionNameFilter = TableQuery.GenerateFilterCondition(
                        nameof(MyCollectionsEntity.Name),
                        QueryComparisons.Equal,
                        name);
            var collectionIdFilter = TableQuery.GenerateFilterCondition(
                        nameof(MyCollectionsEntity.CollectionId),
                        QueryComparisons.NotEqual,
                        collectionId);
            var queryFilter = TableQuery.CombineFilters(collectionNameFilter, TableOperators.And, collectionIdFilter);
            var collections = await this.GetWithFilterAsync(queryFilter);
            return collections.FirstOrDefault();
        }
    }
}