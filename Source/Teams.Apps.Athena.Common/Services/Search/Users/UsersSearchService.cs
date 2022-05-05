// <copyright file="UsersSearchService.cs" company="NPS Foundation">
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
    /// The users search service which helps in creating index, indexer and data source if it doesn't exist
    /// for indexing table which will be used for searching and filtering Athena users.
    /// </summary>
    public class UsersSearchService : IDisposable, IUsersSearchService
    {
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
        private readonly ILogger<UsersSearchService> logger;

        /// <summary>
        /// Connection string for table storage.
        /// </summary>
        private readonly string tableStorageConnectionString;

        /// <summary>
        /// Flag: Has Dispose already been called?
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersSearchService"/> class.
        /// </summary>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="searchServiceClient">Search service client dependency injection.</param>
        /// <param name="searchServiceOptions">Options used to create the search service index client.</param>
        public UsersSearchService(
            IOptions<RepositoryOptions> repositoryOptions,
            ILogger<UsersSearchService> logger,
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
                UsersSearchServiceMetadata.IndexName,
                new SearchCredentials(searchServiceOptions.Value.SearchServiceQueryApiKey));

            this.retryPolicy = Policy.Handle<CloudException>(
                ex => (int)ex.Response.StatusCode == StatusCodes.Status409Conflict ||
                (int)ex.Response.StatusCode == StatusCodes.Status429TooManyRequests)
                .WaitAndRetryAsync(Backoff.LinearBackoff(TimeSpan.FromMilliseconds(2000), 2));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<UserEntity>> GetUsersAsync(SearchParametersDTO searchParametersDTO)
        {
            searchParametersDTO = searchParametersDTO ?? throw new ArgumentNullException(nameof(searchParametersDTO), "Search parameter is null");

            await this.EnsureInitializedAsync();

            searchParametersDTO.SkipRecords = searchParametersDTO.TopRecordsCount.HasValue ? (searchParametersDTO.PageCount * searchParametersDTO.TopRecordsCount) : null;

            var searchParameters = this.InitializeSearchParameters(
                searchParametersDTO.TopRecordsCount,
                searchParametersDTO.SkipRecords,
                searchParametersDTO.Filter);

            if (!searchParametersDTO.SearchFields.IsNullOrEmpty())
            {
                searchParameters.SearchFields = searchParametersDTO.SearchFields;
            }

            var postSearchResult = await this.searchIndexClient.Documents.SearchAsync<UserEntity>(searchParametersDTO.SearchString, searchParameters);

            SearchContinuationToken continuationToken = null;
            var users = new List<UserEntity>();

            if (postSearchResult?.Results != null)
            {
                users.AddRange(postSearchResult.Results.Select(p => p.Document));
                continuationToken = postSearchResult.ContinuationToken;
            }

            while (continuationToken != null)
            {
                var searchResult = await this.searchIndexClient.Documents.ContinueSearchAsync<UserEntity>(continuationToken);

                if (searchResult?.Results != null)
                {
                    users.AddRange(searchResult.Results.Select(p => p.Document));
                    continuationToken = searchResult.ContinuationToken;
                }
            }

            return users;
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
                        await this.searchServiceClient.Indexers.RunAsync(UsersSearchServiceMetadata.IndexerName);
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
        /// Initialization of search service parameters which will help in searching the documents.
        /// </summary>
        /// <param name="top">Number indicating top N count to be fetched.</param>
        /// <param name="skip">Number of records to skip.</param>
        /// <param name="filter">The filter string.</param>
        /// <returns>Represents an search parameter object.</returns>
        private SearchParameters InitializeSearchParameters(int? top, int? skip, string filter)
        {
            SearchParameters searchParameters = new SearchParameters()
            {
                Top = top,
                Skip = skip,
                Select = new[]
                {
                    nameof(UserEntity.TableId),
                    nameof(UserEntity.UserId),
                    nameof(UserEntity.FirstName),
                    nameof(UserEntity.MiddleName),
                    nameof(UserEntity.LastName),
                    nameof(UserEntity.Keywords),
                    nameof(UserEntity.KeywordsText),
                    nameof(UserEntity.UserDisplayName),
                    nameof(UserEntity.EmailAddress),
                    nameof(UserEntity.NodeTypeId),
                    nameof(UserEntity.DateAtPost),
                    nameof(UserEntity.RotationDate),
                    nameof(UserEntity.DateOfRank),
                    nameof(UserEntity.WebSite),
                    nameof(UserEntity.Advisors),
                    nameof(UserEntity.OtherContact),
                    nameof(UserEntity.JobTitle),
                    nameof(UserEntity.Service),
                    nameof(UserEntity.Rank),
                    nameof(UserEntity.Specialty),
                    nameof(UserEntity.PayGrade),
                    nameof(UserEntity.CurrentOrganization),
                    nameof(UserEntity.UnderGraduateDegree),
                    nameof(UserEntity.GradSchool),
                    nameof(UserEntity.GraduateDegreeProgram),
                    nameof(UserEntity.DeptOfStudy),
                },
                SearchFields = new[] { nameof(UserEntity.FirstName), nameof(UserEntity.MiddleName), nameof(UserEntity.LastName), nameof(UserEntity.UserDisplayName), nameof(UserEntity.KeywordNames), nameof(UserEntity.Keywords), nameof(UserEntity.KeywordsText), },
                Filter = filter,
            };

            searchParameters.OrderBy = new[] { nameof(UserEntity.FirstName) };

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
            if (await this.searchServiceClient.Indexes.ExistsAsync(UsersSearchServiceMetadata.IndexName))
            {
                await this.searchServiceClient.Indexes.DeleteAsync(UsersSearchServiceMetadata.IndexName);
            }

            var tableIndex = new Index()
            {
                Name = UsersSearchServiceMetadata.IndexName,
                Fields = FieldBuilder.BuildForType<UserEntity>(),
            };
            await this.searchServiceClient.Indexes.CreateAsync(tableIndex);
        }

        /// <summary>
        /// Create data source if it doesn't exist in Azure Search service.
        /// </summary>
        /// <returns><see cref="Task"/> That represents data source is added to Azure Search service.</returns>
        private async Task CreateDataSourceAsync()
        {
            if (await this.searchServiceClient.DataSources.ExistsAsync(UsersSearchServiceMetadata.DataSourceName))
            {
                return;
            }

            var dataSource = DataSource.AzureTableStorage(
                UsersSearchServiceMetadata.DataSourceName,
                this.tableStorageConnectionString,
                UserTableMetadata.TableName);

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
                if (await this.searchServiceClient.Indexers.ExistsAsync(UsersSearchServiceMetadata.IndexerName))
                {
                    await this.searchServiceClient.Indexers.DeleteAsync(UsersSearchServiceMetadata.IndexerName);
                }

                var indexer = new Indexer()
                {
                    Name = UsersSearchServiceMetadata.IndexerName,
                    DataSourceName = UsersSearchServiceMetadata.DataSourceName,
                    TargetIndexName = UsersSearchServiceMetadata.IndexName,
                    Schedule = new IndexingSchedule(TimeSpan.FromMinutes(SearchIndexingIntervalInMinutes)),
                };

                await this.searchServiceClient.Indexers.CreateAsync(indexer);
                await this.searchServiceClient.Indexers.RunAsync(UsersSearchServiceMetadata.IndexerName);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
