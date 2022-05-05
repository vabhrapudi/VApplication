// <copyright file="NewsSearchService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search.News
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Rest.Azure;
    using Polly;
    using Polly.Contrib.WaitAndRetry;
    using Polly.Retry;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Models.Enums;
    using Teams.Apps.Athena.Common.Repositories;
    using Index = Microsoft.Azure.Search.Models.Index;

    /// <summary>
    /// News Search service which helps in creating index, indexer and data source if it doesn't exist
    /// for indexing table which will be used for searching and filtering news.
    /// </summary>
    public class NewsSearchService : IDisposable, INewsSearchService
    {
        /// <summary>
        /// Azure Search service maximum search result count for news.
        /// </summary>
        private const int ApiSearchResultCount = 30;

        // Default search indexer schedule duration
        private const int SearchIndexingIntervalInMinutes = 5;

        /// <summary>
        /// Retry policy with jitter.
        /// </summary>
        /// <remarks>
        /// Reference: https://github.com/Polly-Contrib/Polly.Contrib.WaitAndRetry#new-jitter-recommendation.
        /// </remarks>
        private readonly AsyncRetryPolicy retryPolicy;

        /// <summary>
        /// Used to initialize task.
        /// </summary>
        private readonly Lazy<Task> initializeTask;

        /// <summary>
        /// Instance of Azure Search service client.
        /// </summary>
        private readonly ISearchServiceClient searchServiceClient;

        /// <summary>
        /// Instance of Azure Search index client.
        /// </summary>
        private readonly ISearchIndexClient searchIndexClient;

        /// <summary>
        /// Instance to send logs to the Application Insights service.
        /// </summary>
        private readonly ILogger<NewsSearchService> logger;

        /// <summary>
        /// Connection string for table storage.
        /// </summary>
        private readonly string tableStorageConnectionString;

        /// <summary>
        /// Flag: Has Dispose already been called?
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsSearchService"/> class.
        /// </summary>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        /// <param name="searchServiceOptions">Options used to create the search service index client.</param>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="searchServiceClient">Search service client dependency injection.</param>
        public NewsSearchService(
            IOptions<RepositoryOptions> repositoryOptions,
            IOptions<SearchServiceOptions> searchServiceOptions,
            ILogger<NewsSearchService> logger,
            ISearchServiceClient searchServiceClient)
        {
            repositoryOptions = repositoryOptions ?? throw new ArgumentNullException(nameof(repositoryOptions));
            searchServiceOptions = searchServiceOptions ?? throw new ArgumentNullException(nameof(searchServiceOptions));
            this.initializeTask = new Lazy<Task>(() => this.InitializeAsync());
            this.tableStorageConnectionString = repositoryOptions.Value.StorageAccountConnectionString;
            this.initializeTask = new Lazy<Task>(() => this.InitializeAsync());
            this.logger = logger;
            this.searchServiceClient = searchServiceClient;
            this.searchIndexClient = new SearchIndexClient(
                searchServiceOptions.Value.SearchServiceName,
                NewsSearchServiceNames.IndexName,
                new SearchCredentials(searchServiceOptions.Value.SearchServiceQueryApiKey));
            this.retryPolicy = Policy.Handle<CloudException>(
                ex => (int)ex.Response.StatusCode == StatusCodes.Status409Conflict ||
                (int)ex.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                .WaitAndRetryAsync(Backoff.LinearBackoff(TimeSpan.FromMilliseconds(2000), 2));
        }

        /// <summary>
        /// Dispose search service instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get news list as per search and filter criteria.
        /// </summary>
        /// <param name="searchParametersDTO">Search parameters for enhanced searching.</param>
        /// <returns>List of news.</returns>
        public async Task<IEnumerable<NewsEntity>> GetNewsAsync(SearchParametersDTO searchParametersDTO)
        {
            searchParametersDTO = searchParametersDTO ?? throw new ArgumentNullException(nameof(searchParametersDTO), "Search parameter is null");

            await this.EnsureInitializedAsync();

            searchParametersDTO.SkipRecords = searchParametersDTO.PageCount * ApiSearchResultCount;

            var searchParameters = this.InitializeSearchParameters(searchParametersDTO);
            var postSearchResult = await this.searchIndexClient.Documents.SearchAsync<NewsEntity>(searchParametersDTO.SearchString, searchParameters);
            SearchContinuationToken continuationToken = null;
            var news = new List<NewsEntity>();

            if (postSearchResult?.Results != null)
            {
                news.AddRange(postSearchResult.Results.Select(p => p.Document));
                continuationToken = postSearchResult.ContinuationToken;
            }

            while (continuationToken != null)
            {
                var searchResult = await this.searchIndexClient.Documents.ContinueSearchAsync<NewsEntity>(continuationToken);

                if (searchResult?.Results != null)
                {
                    news.AddRange(searchResult.Results.Select(p => p.Document));
                    continuationToken = searchResult.ContinuationToken;
                }
            }

            return news;
        }

        /// <summary>
        /// Run the indexer on demand.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        public async Task RunIndexerOnDemandAsync()
        {
            // Retry once after 1 second if conflict occurs during indexer run.
            // If conflict occurs again means another index run is in progress and it will index data for which first failure occurred.
            // Hence ignore second conflict and continue.
            var requestId = Guid.NewGuid().ToString();

            try
            {
                await this.retryPolicy.ExecuteAsync(async () =>
                {
                    try
                    {
                        this.logger.LogInformation($"On-demand indexer run request #{requestId} - start");
                        await this.searchServiceClient.Indexers.RunAsync(NewsSearchServiceNames.IndexerName);
                        this.logger.LogInformation($"On-demand indexer run request #{requestId} - complete");
                    }
                    catch (CloudException ex)
                    {
                        this.logger.LogError(ex, $"Failed to run on-demand indexer run for request #{requestId}: {ex.Message}");
                        throw;
                    }
                });
            }
            catch (CloudException ex)
            {
                this.logger.LogError(ex, $"Failed to run on-demand indexer for retry. Request #{requestId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">True if already disposed else false.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.searchServiceClient.Dispose();
                this.searchIndexClient.Dispose();
            }

            this.disposed = true;
        }

        /// <summary>
        /// Creates Index, Data Source and Indexer for search service.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        private async Task RecreateSearchServiceIndexAsync()
        {
            try
            {
                await this.CreateSearchIndexAsync();
                await this.CreateDataSourceAsync();
                await this.CreateIndexerAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create index, indexer and data source if doesn't exist.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        private async Task InitializeAsync()
        {
            try
            {
                await this.RecreateSearchServiceIndexAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Failed to initialize Azure Search Service: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Create index in Azure Search service if it doesn't exist.
        /// </summary>
        /// <returns><see cref="Task"/> That represents index is created if it is not created.</returns>
        private async Task CreateSearchIndexAsync()
        {
            if (await this.searchServiceClient.Indexes.ExistsAsync(NewsSearchServiceNames.IndexName))
            {
                await this.searchServiceClient.Indexes.DeleteAsync(NewsSearchServiceNames.IndexName);
            }

            var tableIndex = new Index()
            {
                Name = NewsSearchServiceNames.IndexName,
                Fields = FieldBuilder.BuildForType<NewsEntity>(),
            };
            await this.searchServiceClient.Indexes.CreateAsync(tableIndex);
        }

        /// <summary>
        /// Create data source if it doesn't exist in Azure Search service.
        /// </summary>
        /// <returns><see cref="Task"/> That represents data source is added to Azure Search service.</returns>
        private async Task CreateDataSourceAsync()
        {
            if (await this.searchServiceClient.DataSources.ExistsAsync(NewsSearchServiceNames.DataSourceName))
            {
                return;
            }

            var dataSource = DataSource.AzureTableStorage(
                NewsSearchServiceNames.DataSourceName,
                this.tableStorageConnectionString,
                NewsTableMetadata.TableName,
                query: null,
                new SoftDeleteColumnDeletionDetectionPolicy("IsDeleted", true));

            await this.searchServiceClient.DataSources.CreateAsync(dataSource);
        }

        /// <summary>
        /// Create indexer if it doesn't exist in Azure Search service.
        /// </summary>
        /// <returns><see cref="Task"/> That represents indexer is created if not available in Azure Search service.</returns>
        private async Task CreateIndexerAsync()
        {
            if (await this.searchServiceClient.Indexers.ExistsAsync(NewsSearchServiceNames.IndexerName))
            {
                await this.searchServiceClient.Indexers.DeleteAsync(NewsSearchServiceNames.IndexerName);
            }

            var indexer = new Indexer()
            {
                Name = NewsSearchServiceNames.IndexerName,
                DataSourceName = NewsSearchServiceNames.DataSourceName,
                TargetIndexName = NewsSearchServiceNames.IndexName,
                Schedule = new IndexingSchedule(TimeSpan.FromMinutes(SearchIndexingIntervalInMinutes)),
            };

            await this.searchServiceClient.Indexers.CreateAsync(indexer);
            await this.searchServiceClient.Indexers.RunAsync(NewsSearchServiceNames.IndexerName);
        }

        /// <summary>
        /// Initialization of InitializeAsync method which will help in indexing.
        /// </summary>
        /// <returns>Represents an asynchronous operation.</returns>
        private Task EnsureInitializedAsync()
        {
            return this.initializeTask.Value;
        }

        /// <summary>
        /// Initialization of search service parameters which will help in searching the documents.
        /// </summary>
        /// <param name="searchParametersDTO">Search parameters.</param>
        /// <returns>Represents an search parameter object.</returns>
        private SearchParameters InitializeSearchParameters(SearchParametersDTO searchParametersDTO)
        {
            SearchParameters searchParameters = new SearchParameters()
            {
                Top = searchParametersDTO.TopRecordsCount.HasValue ? searchParametersDTO.TopRecordsCount.Value : ApiSearchResultCount,
                Skip = searchParametersDTO.SkipRecords.HasValue ? searchParametersDTO.SkipRecords : 0,
                Select = new[]
                {
                    nameof(NewsEntity.TableId),
                    nameof(NewsEntity.NewsId),
                    nameof(NewsEntity.Title),
                    nameof(NewsEntity.Abstract),
                    nameof(NewsEntity.Body),
                    nameof(NewsEntity.ExternalLink),
                    nameof(NewsEntity.ImageURL),
                    nameof(NewsEntity.Keywords),
                    nameof(NewsEntity.CreatedAt),
                    nameof(NewsEntity.CreatedBy),
                    nameof(NewsEntity.IsImportant),
                    nameof(NewsEntity.SumOfRatings),
                    nameof(NewsEntity.NumberOfRatings),
                    nameof(NewsEntity.AverageRating),
                    nameof(NewsEntity.Status),
                    nameof(NewsEntity.AdminComment),
                    nameof(NewsEntity.NodeTypeId),
                    nameof(NewsEntity.PublishedDate),
                    nameof(NewsEntity.UpdatedAt),
                    nameof(NewsEntity.NewsSourceId),
                },
                SearchFields = new[] { nameof(NewsEntity.Title), nameof(NewsEntity.KeywordNames), nameof(NewsEntity.Keywords), nameof(NewsEntity.Abstract), nameof(NewsEntity.Body), },
                Filter = searchParametersDTO.Filter,
            };

            if (searchParametersDTO.IsGetAllRecords)
            {
                searchParameters.Top = null;
            }

            if (!searchParametersDTO.SearchFields.IsNullOrEmpty())
            {
                searchParameters.SearchFields = searchParametersDTO.SearchFields;
            }
            else
            {
                // default search news by title and kewords.
                searchParameters.SearchFields = new[] { nameof(NewsEntity.Title), nameof(NewsEntity.KeywordNames) };
            }

            if (searchParametersDTO.SortByFilter == (int)SortBy.Recent)
            {
                searchParameters.OrderBy = new List<string> { $"{nameof(NewsEntity.PublishedDate)} desc" };
            }
            else if (searchParametersDTO.SortByFilter == (int)SortBy.RatingHighToLow)
            {
                searchParameters.OrderBy = new List<string> { $"{nameof(NewsEntity.AverageRating)} desc", $"{nameof(NewsEntity.PublishedDate)} desc" };
            }
            else if (searchParametersDTO.SortByFilter == (int)SortBy.Significance)
            {
                searchParameters.OrderBy = new List<string> { $"{nameof(NewsEntity.IsImportant)} desc", $"{nameof(NewsEntity.PublishedDate)} desc" };
            }

            if (searchParametersDTO.NewsArticleSortColumn != NewsArticleSortColumn.None)
            {
                var orderByColumns = this.GetOrderByExtression(searchParametersDTO.NewsArticleSortColumn, searchParametersDTO.SortOrder);
                if (orderByColumns.Any())
                {
                    searchParameters.OrderBy = orderByColumns;
                }
            }

            if (!searchParametersDTO.OrderBy.IsNullOrEmpty())
            {
                searchParameters.OrderBy = searchParametersDTO.OrderBy;
            }

            return searchParameters;
        }

        /// <summary>
        /// Gets the order by expression.
        /// </summary>
        /// <param name="sortColumn">The column to be sorted.</param>
        /// <param name="sortOrder">The sort sort.</param>
        /// <returns>The order by expression.</returns>
        private string[] GetOrderByExtression(NewsArticleSortColumn sortColumn, SortOrder sortOrder)
        {
            switch (sortColumn)
            {
                case NewsArticleSortColumn.Title:
                    return new[] { $"{nameof(NewsEntity.Title)} {(sortOrder == SortOrder.Ascending ? "asc" : "desc")}" };

                case NewsArticleSortColumn.Status:
                    return new[] { $"{nameof(NewsEntity.Status)} {(sortOrder == SortOrder.Ascending ? "asc" : "desc")}" };

                case NewsArticleSortColumn.CreatedAt:
                    return new[] { $"{nameof(NewsEntity.CreatedAt)} {(sortOrder == SortOrder.Ascending ? "asc" : "desc")}" };

                default:
                    return new[] { $"{nameof(NewsEntity.IsImportant)} desc, {nameof(NewsEntity.CreatedAt)} {(sortOrder == SortOrder.Ascending ? "asc" : "desc")}" };
            }
        }
    }
}