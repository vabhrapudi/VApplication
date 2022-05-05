// <copyright file="CommentsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the comments stored in the table storage.
    /// </summary>
    public class CommentsRepository : BaseRepository<CommentsEntity>, ICommentsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public CommentsRepository(
            ILogger<CommentsRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: CommentsTableMetadata.TableName,
                  defaultPartitionKey: CommentsTableMetadata.PartitionKey,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CommentsEntity>> GetCommentsByResourceTypeAsync(string resourceTableId, int resourceTypeId)
        {
            var resourceTypeFilter = TableQuery.GenerateFilterConditionForInt(
                nameof(CommentsEntity.ResourceTypeId), QueryComparisons.Equal, resourceTypeId);
            var resourceTableIdFilter = TableQuery.GenerateFilterCondition(
                nameof(CommentsEntity.ResourceTableId), QueryComparisons.Equal, resourceTableId);
            var finalFilter = TableQuery.CombineFilters(resourceTypeFilter, TableOperators.And, TableQuery.CombineFilters(resourceTableIdFilter, TableOperators.And, resourceTypeFilter));

            return await this.GetWithFilterAsync(finalFilter);
        }
    }
}