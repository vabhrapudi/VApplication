// <copyright file="CoiSearchService.cs" company="NPS Foundation">
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
    /// The COI search service which helps in creating index, indexer and data source if it doesn't exist
    /// for indexing table which will be used for searching and filtering COIs.
    /// </summary>
    public class CoiSearchService : IDisposable, ICoiSearchService
    {
        /// <summary>
        /// Azure Search service maximum search result count for COI.
        /// </summary>
        private const int SearchResultsCountPerPage = 30;

        private const string SoftDeleteColumnNameIsDeleted = "IsDeleted";

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
        private readonly ILogger<CoiSearchService> logger;

        /// <summary>
        /// Connection string for table storage.
        /// </summary>
        private readonly string tableStorageConnectionString;

        /// <summary>
        /// Flag: Has Dispose already been called?
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoiSearchService"/> class.
        /// </summary>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        /// <param name="searchServiceOptions">The options used to create search service index client.</param>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="searchServiceClient">The instance of <see cref="SearchServiceClient"/>.</param>
        public CoiSearchService(
            IOptions<RepositoryOptions> repositoryOptions,
            IOptions<SearchServiceOptions> searchServiceOptions,
            ILogger<CoiSearchService> logger,
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
                CoiSearchServiceMetadata.IndexName,
                new SearchCredentials(searchServiceOptions.Value.SearchServiceQueryApiKey));

            this.retryPolicy = Policy.Handle<CloudException>(
                ex => (int)ex.Response.StatusCode == StatusCodes.Status409Conflict ||
                (int)ex.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                .WaitAndRetryAsync(Backoff.LinearBackoff(TimeSpan.FromMilliseconds(2000), 2));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CommunityOfInterestEntity>> GetCommunityOfInterestsAsync(SearchParametersDTO searchParametersDTO)
        {
            searchParametersDTO = searchParametersDTO ?? throw new ArgumentNullException(nameof(searchParametersDTO), "Search parameter is null");

            await this.EnsureInitializedAsync();

            searchParametersDTO.SkipRecords = searchParametersDTO.PageCount * SearchResultsCountPerPage;

            var searchParameters = this.InitializeSearchParameters(searchParametersDTO);

            var postSearchResult = await this.searchIndexClient.Documents.SearchAsync<CommunityOfInterestEntity>(searchParametersDTO.SearchString, searchParameters);

            SearchContinuationToken continuationToken = null;
            var communityOfInterests = new List<CommunityOfInterestEntity>();

            if (postSearchResult?.Results != null)
            {
                communityOfInterests.AddRange(postSearchResult.Results.Select(p => p.Document));
                continuationToken = postSearchResult.ContinuationToken;
            }

            while (continuationToken != null)
            {
                var searchResult = await this.searchIndexClient.Documents.ContinueSearchAsync<CommunityOfInterestEntity>(continuationToken);

                if (searchResult?.Results != null)
                {
                    communityOfInterests.AddRange(searchResult.Results.Select(p => p.Document));
                    continuationToken = searchResult.ContinuationToken;
                }
            }

            return communityOfInterests;
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
                        await this.searchServiceClient.Indexers.RunAsync(CoiSearchServiceMetadata.IndexerName);
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
        /// Dispose search service instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
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
        /// <returns><see cref="Task"/> that represents index is created if it is not created.</returns>
        private async Task CreateSearchIndexAsync()
        {
            try
            {
                if (await this.searchServiceClient.Indexes.ExistsAsync(CoiSearchServiceMetadata.IndexName))
                {
                    await this.searchServiceClient.Indexes.DeleteAsync(CoiSearchServiceMetadata.IndexName);
                }

                var tableIndex = new Index()
                {
                    Name = CoiSearchServiceMetadata.IndexName,
                    Fields = FieldBuilder.BuildForType<CommunityOfInterestEntity>(),
                };

                await this.searchServiceClient.Indexes.CreateAsync(tableIndex);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Create data source if it doesn't exist in Azure Search service.
        /// </summary>
        /// <returns><see cref="Task"/> That represents data source is added to Azure Search service.</returns>
        private async Task CreateDataSourceAsync()
        {
            if (await this.searchServiceClient.DataSources.ExistsAsync(CoiSearchServiceMetadata.DataSourceName))
            {
                return;
            }

            var dataSource = DataSource.AzureTableStorage(
                CoiSearchServiceMetadata.DataSourceName,
                this.tableStorageConnectionString,
                CoiTableMetadata.TableName,
                query: null,
                new SoftDeleteColumnDeletionDetectionPolicy(SoftDeleteColumnNameIsDeleted, true));

            await this.searchServiceClient.DataSources.CreateAsync(dataSource);
        }

        /// <summary>
        /// Create indexer if it doesn't exist in Azure Search service.
        /// </summary>
        /// <returns><see cref="Task"/> That represents indexer is created if not available in Azure Search service.</returns>
        private async Task CreateIndexerAsync()
        {
            try
            {
                if (await this.searchServiceClient.Indexers.ExistsAsync(CoiSearchServiceMetadata.IndexerName))
                {
                    await this.searchServiceClient.Indexers.DeleteAsync(CoiSearchServiceMetadata.IndexerName);
                }

                var indexer = new Indexer()
                {
                    Name = CoiSearchServiceMetadata.IndexerName,
                    DataSourceName = CoiSearchServiceMetadata.DataSourceName,
                    TargetIndexName = CoiSearchServiceMetadata.IndexName,
                    Schedule = new IndexingSchedule(TimeSpan.FromMinutes(SearchIndexingIntervalInMinutes)),
                };

                await this.searchServiceClient.Indexers.CreateAsync(indexer);
                await this.searchServiceClient.Indexers.RunAsync(CoiSearchServiceMetadata.IndexerName);
            }
            catch (Exception ex)
            {
                throw;
            }
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
                    nameof(CommunityOfInterestEntity.TableId),
                    nameof(CommunityOfInterestEntity.CoiId),
                    nameof(CommunityOfInterestEntity.CoiName),
                    nameof(CommunityOfInterestEntity.CoiDescription),
                    nameof(CommunityOfInterestEntity.Keywords),
                    nameof(CommunityOfInterestEntity.Status),
                    nameof(CommunityOfInterestEntity.Type),
                    nameof(CommunityOfInterestEntity.CreatedOn),
                    nameof(CommunityOfInterestEntity.CreatedByObjectId),
                    nameof(CommunityOfInterestEntity.CreatedByUserPrincipalName),
                    nameof(CommunityOfInterestEntity.AdminComment),
                    nameof(CommunityOfInterestEntity.TeamId),
                    nameof(CommunityOfInterestEntity.NodeTypeId),
                    nameof(CommunityOfInterestEntity.SecurityLevel),
                    nameof(CommunityOfInterestEntity.Organization),
                    nameof(CommunityOfInterestEntity.WebSite),
                    nameof(CommunityOfInterestEntity.UpdatedOn),
                    nameof(CommunityOfInterestEntity.KeywordsText),
                },
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
                // default search COIs by name and kewords.
                searchParameters.SearchFields = new[] { nameof(CommunityOfInterestEntity.CoiName), nameof(CommunityOfInterestEntity.KeywordNames), nameof(CommunityOfInterestEntity.Keywords), nameof(CommunityOfInterestEntity.CoiDescription), nameof(CommunityOfInterestEntity.KeywordsText), };
            }

            if (!searchParametersDTO.OrderBy.IsNullOrEmpty())
            {
                searchParameters.OrderBy = searchParametersDTO.OrderBy;
            }
            else
            {
                searchParameters.OrderBy = this.GetOrderByExtression(searchParametersDTO.CoiSortColumn, searchParametersDTO.SortOrder);
            }

            return searchParameters;
        }

        /// <summary>
        /// Gets the order by expression.
        /// </summary>
        /// <param name="sortColumn">The column to be sorted.</param>
        /// <param name="sortOrder">The sort sort.</param>
        /// <returns>The order by expression.</returns>
        private string[] GetOrderByExtression(CoiSortColumn sortColumn, SortOrder sortOrder)
        {
            switch (sortColumn)
            {
                case CoiSortColumn.Name:
                    return new[] { $"{nameof(CommunityOfInterestEntity.CoiName)} {(sortOrder == SortOrder.Ascending ? "asc" : "desc")}" };

                case CoiSortColumn.Type:
                    return new[] { $"{nameof(CommunityOfInterestEntity.Type)} {(sortOrder == SortOrder.Ascending ? "asc" : "desc")}" };

                case CoiSortColumn.Status:
                    return new[] { $"{nameof(CommunityOfInterestEntity.Status)} {(sortOrder == SortOrder.Ascending ? "asc" : "desc")}" };

                default:
                    return new[] { $"{nameof(CommunityOfInterestEntity.CreatedOn)} {(sortOrder == SortOrder.Ascending ? "asc" : "desc")}" };
            }
        }
    }
}
