// <copyright file="AthenaFeedbackSearchService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
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
    /// Feedback Search service which helps in creating index, indexer and data source if it doesn't exist
    /// for indexing table which will be used for searching and filtering feedbacks.
    /// </summary>
    public class AthenaFeedbackSearchService : IDisposable, IAthenaFeedbackSearchService
    {
        /// <summary>
        /// Azure Search service maximum search result count for feedbacks.
        /// </summary>
        private const int ApiSearchResultCount = 30;

        /// <summary>
        /// Default search indexer schedule duration.
        /// </summary>
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
        private readonly ILogger<AthenaFeedbackSearchService> logger;

        /// <summary>
        /// Connection string for table storage.
        /// </summary>
        private readonly string tableStorageConnectionString;

        /// <summary>
        /// Flag: Has Dispose already been called?
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaFeedbackSearchService"/> class.
        /// </summary>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        /// <param name="searchServiceOptions">Options used to create the search service index client.</param>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="searchServiceClient">Search service client dependency injection.</param>
        public AthenaFeedbackSearchService(
            IOptions<RepositoryOptions> repositoryOptions,
            IOptions<SearchServiceOptions> searchServiceOptions,
            ILogger<AthenaFeedbackSearchService> logger,
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
                AthenaFeedbackSearchServiceMetadata.IndexName,
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

        /// <inheritdoc/>
        public async Task<IEnumerable<AthenaFeedbackEntity>> GetAthenaFeedbacksAsync(SearchParametersDTO searchParametersDTO)
        {
            searchParametersDTO = searchParametersDTO ?? throw new ArgumentNullException(nameof(searchParametersDTO), "Search parameter is null");

            await this.EnsureInitializedAsync();

            searchParametersDTO.SkipRecords = searchParametersDTO.PageCount * ApiSearchResultCount;

            var searchParameters = this.InitializeSearchParameters(searchParametersDTO);
            var postSearchResult = await this.searchIndexClient.Documents.SearchAsync<AthenaFeedbackEntity>(searchParametersDTO.SearchString.EscapeSpecialCharacters(), searchParameters);
            SearchContinuationToken continuationToken = null;
            var feedbacks = new List<AthenaFeedbackEntity>();

            if (postSearchResult?.Results != null)
            {
                feedbacks.AddRange(postSearchResult.Results.Select(p => p.Document));
                continuationToken = postSearchResult.ContinuationToken;
            }

            while (continuationToken != null)
            {
                var searchResult = await this.searchIndexClient.Documents.ContinueSearchAsync<AthenaFeedbackEntity>(continuationToken);

                if (searchResult?.Results != null)
                {
                    feedbacks.AddRange(searchResult.Results.Select(p => p.Document));
                    continuationToken = searchResult.ContinuationToken;
                }
            }

            return feedbacks;
        }

        /// <inheritdoc/>
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
                        await this.searchServiceClient.Indexers.RunAsync(AthenaFeedbackSearchServiceMetadata.IndexerName);
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
            if (await this.searchServiceClient.Indexes.ExistsAsync(AthenaFeedbackSearchServiceMetadata.IndexName))
            {
                await this.searchServiceClient.Indexes.DeleteAsync(AthenaFeedbackSearchServiceMetadata.IndexName);
            }

            var tableIndex = new Index()
            {
                Name = AthenaFeedbackSearchServiceMetadata.IndexName,
                Fields = FieldBuilder.BuildForType<AthenaFeedbackEntity>(),
            };
            await this.searchServiceClient.Indexes.CreateAsync(tableIndex);
        }

        /// <summary>
        /// Create data source if it doesn't exist in Azure Search service.
        /// </summary>
        /// <returns><see cref="Task"/> That represents data source is added to Azure Search service.</returns>
        private async Task CreateDataSourceAsync()
        {
            if (await this.searchServiceClient.DataSources.ExistsAsync(AthenaFeedbackSearchServiceMetadata.DataSourceName))
            {
                return;
            }

            var dataSource = DataSource.AzureTableStorage(
                AthenaFeedbackSearchServiceMetadata.DataSourceName,
                this.tableStorageConnectionString,
                AthenaFeedbackTableNames.TableName);

            await this.searchServiceClient.DataSources.CreateAsync(dataSource);
        }

        /// <summary>
        /// Create indexer if it doesn't exist in Azure Search service.
        /// </summary>
        /// <returns><see cref="Task"/> That represents indexer is created if not available in Azure Search service.</returns>
        private async Task CreateIndexerAsync()
        {
            if (await this.searchServiceClient.Indexers.ExistsAsync(AthenaFeedbackSearchServiceMetadata.IndexerName))
            {
                await this.searchServiceClient.Indexers.DeleteAsync(AthenaFeedbackSearchServiceMetadata.IndexerName);
            }

            var indexer = new Indexer()
            {
                Name = AthenaFeedbackSearchServiceMetadata.IndexerName,
                DataSourceName = AthenaFeedbackSearchServiceMetadata.DataSourceName,
                TargetIndexName = AthenaFeedbackSearchServiceMetadata.IndexName,
                Schedule = new IndexingSchedule(TimeSpan.FromMinutes(SearchIndexingIntervalInMinutes)),
            };

            await this.searchServiceClient.Indexers.CreateAsync(indexer);
            await this.searchServiceClient.Indexers.RunAsync(AthenaFeedbackSearchServiceMetadata.IndexerName);
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
                Top = ApiSearchResultCount,
                Skip = searchParametersDTO.SkipRecords.HasValue ? searchParametersDTO.SkipRecords : 0,
                Select = new[]
                {
                    nameof(AthenaFeedbackEntity.FeedbackId),
                    nameof(AthenaFeedbackEntity.Feedback),
                    nameof(AthenaFeedbackEntity.Details),
                    nameof(AthenaFeedbackEntity.CreatedAt),
                    nameof(AthenaFeedbackEntity.CreatedBy),
                    nameof(AthenaFeedbackEntity.Category),
                    nameof(AthenaFeedbackEntity.Type),
                },
                Filter = searchParametersDTO.Filter,
            };

            if (searchParametersDTO.SortByFilter == (int)AthenaFeedbackSortByItems.Recent)
            {
                searchParameters.OrderBy = new List<string> { $"{nameof(AthenaFeedbackEntity.CreatedAt)} desc" };
            }
            else if (searchParametersDTO.SortByFilter == (int)AthenaFeedbackSortByItems.Category)
            {
                searchParameters.OrderBy = new List<string> { $"{nameof(AthenaFeedbackEntity.Category)} asc", $"{nameof(AthenaFeedbackEntity.CreatedAt)} desc" };
            }
            else if (searchParametersDTO.SortByFilter == (int)AthenaFeedbackSortByItems.FeeedbackType)
            {
                searchParameters.OrderBy = new List<string> { $"{nameof(AthenaFeedbackEntity.Type)} asc", $"{nameof(AthenaFeedbackEntity.CreatedAt)} desc" };
            }

            if (searchParametersDTO.IsGetAllRecords)
            {
                searchParameters.Top = null;
            }

            return searchParameters;
        }
    }
}