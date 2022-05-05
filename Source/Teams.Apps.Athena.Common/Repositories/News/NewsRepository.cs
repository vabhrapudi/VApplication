// <copyright file="NewsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the news data stored in the table storage.
    /// </summary>
    public class NewsRepository : BaseRepository<NewsEntity>, INewsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public NewsRepository(
            ILogger<NewsRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: NewsTableMetadata.TableName,
                  defaultPartitionKey: NewsTableMetadata.NewsPartitionKey,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NewsEntity>> GetActiveNewsArticlesAsync(IEnumerable<string> newsArticleRequestIds, Guid userAadId)
        {
            var requestCreatedByUserFilterCondition = TableQuery.GenerateFilterCondition("CreatedBy", QueryComparisons.Equal, userAadId.ToString());
            var requestNotDeletedFilterCondition = TableQuery.GenerateFilterConditionForBool("IsDeleted", QueryComparisons.Equal, false);
            var newsArticleRequestIdsFilterCondition = this.GetRowKeysFilter(newsArticleRequestIds);

            var createdByAndIsDeletedFilter = TableQuery.CombineFilters(requestCreatedByUserFilterCondition, TableOperators.And, requestNotDeletedFilterCondition);

            var filter = TableQuery.CombineFilters(createdByAndIsDeletedFilter, TableOperators.And, newsArticleRequestIdsFilterCondition);

            return await this.GetWithFilterAsync(filter);
        }

        /// <inheritdoc/>
        public async Task<NewsEntity> GetNewsDetailsByNewsId(int newsId)
        {
            var newsIdFilter = TableQuery.GenerateFilterConditionForInt(nameof(NewsEntity.NewsId), QueryComparisons.Equal, newsId);
            var news = await this.GetWithFilterAsync(newsIdFilter);
            return news.FirstOrDefault();
        }
    }
}