// <copyright file="AthenaEventsSearchService.cs" company="NPS Foundation">
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
    using Teams.Apps.Athena.Common.Repositories;
    using Index = Microsoft.Azure.Search.Models.Index;

    /// <summary>
    /// The Athena events search service which helps in creating index, indexer and data source if it doesn't exist
    /// for indexing table which will be used for searching and filtering Athena events.
    /// </summary>
    public class AthenaEventsSearchService : IDisposable, IAthenaEventsSearchService
    {
        private const int SearchResultsCountPerPage = 30;

        /// <summary>
        /// Default search indexer schedule duration.
        /// </summary>
        private const int SearchIndexingIntervalInMinutes = 5;

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
        private readonly ILogger<AthenaEventsSearchService> logger;

        /// <summary>
        /// Connection string for table storage.
        /// </summary>
        private readonly string tableStorageConnectionString;

        /// <summary>
        /// Retry policy with jitter.
        /// </summary>
        /// <remarks>
        /// Reference: https://github.com/Polly-Contrib/Polly.Contrib.WaitAndRetry#new-jitter-recommendation.
        /// </remarks>
        private readonly AsyncRetryPolicy retryPolicy;

        /// <summary>
        /// Flag: Has Dispose already been called?
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaEventsSearchService"/> class.
        /// </summary>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="searchServiceClient">Search service client dependency injection.</param>
        /// <param name="searchServiceOptions">Options used to create the search service index client.</param>
        public AthenaEventsSearchService(
            IOptions<RepositoryOptions> repositoryOptions,
            ILogger<AthenaEventsSearchService> logger,
            ISearchServiceClient searchServiceClient,
            IOptions<SearchServiceOptions> searchServiceOptions)
        {
            repositoryOptions = repositoryOptions ?? throw new ArgumentNullException(nameof(repositoryOptions));
            searchServiceOptions = searchServiceOptions ?? throw new ArgumentNullException(nameof(searchServiceOptions));

            this.initializeTask = new Lazy<Task>(() => this.InitializeAsync());
            this.tableStorageConnectionString = repositoryOptions.Value.StorageAccountConnectionString;
            this.logger = logger;

            this.searchServiceClient = searchServiceClient;
            this.searchIndexClient = new SearchIndexClient(
                searchServiceOptions.Value.SearchServiceName,
                AthenaEventsSearchServiceMetadata.IndexName,
                new SearchCredentials(searchServiceOptions.Value.SearchServiceQueryApiKey));
            this.retryPolicy = Policy.Handle<CloudException>(
                ex => (int)ex.Response.StatusCode == StatusCodes.Status409Conflict ||
                (int)ex.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                .WaitAndRetryAsync(Backoff.LinearBackoff(TimeSpan.FromMilliseconds(2000), 2));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<EventEntity>> GetEventsAsync(SearchParametersDTO searchParametersDTO)
        {
            await this.EnsureInitializedAsync();

            var searchParameters = this.InitializeSearchParameters(searchParametersDTO);

            SearchContinuationToken continuationToken = null;
            var events = new List<EventEntity>();

            var postSearchResult = await this.searchIndexClient.Documents.SearchAsync<EventEntity>(searchParametersDTO.SearchString, searchParameters);

            if (postSearchResult?.Results != null)
            {
                events.AddRange(postSearchResult.Results.Select(p => p.Document));
                continuationToken = postSearchResult.ContinuationToken;
            }

            while (continuationToken != null)
            {
                var searchResult = await this.searchIndexClient.Documents.ContinueSearchAsync<EventEntity>(continuationToken);

                if (searchResult?.Results != null)
                {
                    events.AddRange(searchResult.Results.Select(p => p.Document));
                    continuationToken = searchResult.ContinuationToken;
                }
                else
                {
                    continuationToken = null;
                }
            }

            return events;
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
                        await this.searchServiceClient.Indexers.RunAsync(AthenaEventsSearchServiceMetadata.IndexerName);
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
        /// Initialization of search service parameters which will help in searching the documents.
        /// </summary>
        /// <param name="searchParametersDTO">Search parameters.</param>
        /// <returns>Represents an search parameter object.</returns>
        private SearchParameters InitializeSearchParameters(SearchParametersDTO searchParametersDTO)
        {
            SearchParameters searchParameters = new SearchParameters()
            {
                Top = searchParametersDTO.TopRecordsCount.HasValue ? searchParametersDTO.TopRecordsCount.Value : SearchResultsCountPerPage,
                Skip = searchParametersDTO.SkipRecords.HasValue ? searchParametersDTO.SkipRecords : 0,
                Select = new[]
                {
                    nameof(EventEntity.TableId),
                    nameof(EventEntity.EventId),
                    nameof(EventEntity.NodeTypeId),
                    nameof(EventEntity.SecurityLevel),
                    nameof(EventEntity.Keywords),
                    nameof(EventEntity.KeywordsText),
                    nameof(EventEntity.LastUpdate),
                    nameof(EventEntity.DateOfEvent),
                    nameof(EventEntity.Title),
                    nameof(EventEntity.Description),
                    nameof(EventEntity.Organization),
                    nameof(EventEntity.Location),
                    nameof(EventEntity.WebSite),
                    nameof(EventEntity.OtherContactInfo),
                },
                Filter = searchParametersDTO.Filter,
            };

            if (searchParametersDTO.IsGetAllRecords)
            {
                searchParameters.Top = null;
            }

            if (!searchParametersDTO.OrderBy.IsNullOrEmpty())
            {
                searchParameters.OrderBy = searchParametersDTO.OrderBy;
            }

            if (!searchParametersDTO.SearchFields.IsNullOrEmpty())
            {
                searchParameters.SearchFields = searchParametersDTO.SearchFields;
            }
            else
            {
                searchParameters.SearchFields = new[] { nameof(EventEntity.Title), nameof(EventEntity.Description), nameof(EventEntity.Keywords), nameof(EventEntity.KeywordsText), };
            }

            return searchParameters;
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
        /// Create index in Azure Search service if it doesn't exist.
        /// </summary>
        /// <returns><see cref="Task"/> That represents index is created if it is not created.</returns>
        private async Task CreateSearchIndexAsync()
        {
            if (await this.searchServiceClient.Indexes.ExistsAsync(AthenaEventsSearchServiceMetadata.IndexName))
            {
                await this.searchServiceClient.Indexes.DeleteAsync(AthenaEventsSearchServiceMetadata.IndexName);
            }

            var tableIndex = new Index()
            {
                Name = AthenaEventsSearchServiceMetadata.IndexName,
                Fields = FieldBuilder.BuildForType<EventEntity>(),
            };
            await this.searchServiceClient.Indexes.CreateAsync(tableIndex);
        }

        /// <summary>
        /// Create data source if it doesn't exist in Azure Search service.
        /// </summary>
        /// <returns><see cref="Task"/> That represents data source is added to Azure Search service.</returns>
        private async Task CreateDataSourceAsync()
        {
            if (await this.searchServiceClient.DataSources.ExistsAsync(AthenaEventsSearchServiceMetadata.DataSourceName))
            {
                return;
            }

            var dataSource = DataSource.AzureTableStorage(
                AthenaEventsSearchServiceMetadata.DataSourceName,
                this.tableStorageConnectionString,
                EventsTableMetadata.TableName);

            await this.searchServiceClient.DataSources.CreateAsync(dataSource);
        }

        /// <summary>
        /// Create indexer if it doesn't exist in Azure Search service.
        /// </summary>
        /// <returns><see cref="Task"/> That represents indexer is created if not available in Azure Search service.</returns>
        private async Task CreateIndexerAsync()
        {
            if (await this.searchServiceClient.Indexers.ExistsAsync(AthenaEventsSearchServiceMetadata.IndexerName))
            {
                await this.searchServiceClient.Indexers.DeleteAsync(AthenaEventsSearchServiceMetadata.IndexerName);
            }

            var indexer = new Indexer()
            {
                Name = AthenaEventsSearchServiceMetadata.IndexerName,
                DataSourceName = AthenaEventsSearchServiceMetadata.DataSourceName,
                TargetIndexName = AthenaEventsSearchServiceMetadata.IndexName,
                Schedule = new IndexingSchedule(TimeSpan.FromMinutes(SearchIndexingIntervalInMinutes)),
            };

            await this.searchServiceClient.Indexers.CreateAsync(indexer);
            await this.searchServiceClient.Indexers.RunAsync(AthenaEventsSearchServiceMetadata.IndexerName);
        }
    }
}
