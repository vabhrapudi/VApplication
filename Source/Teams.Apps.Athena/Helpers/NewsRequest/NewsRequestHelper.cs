// <copyright file="NewsRequestHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Services.Search.News;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods associated with news entity operations.
    /// </summary>
    public class NewsRequestHelper : INewsRequestHelper
    {
        private readonly INewsMapper newsMapper;

        private readonly INewsRepository newsRepository;

        private readonly IAdaptiveCardHelper adaptiveCardHelper;

        private readonly INewsSearchService newsSearchService;

        private readonly IFilterQueryHelper filterQueryHelper;

        private readonly IUserGraphServiceHelper userGraphServiceHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRequestHelper"/> class.
        /// </summary>
        /// <param name="newsMapper">The instance of <see cref="NewsMapper"/> class.</param>
        /// <param name="newsRepository">The instance of <see cref="NewsRepository"/> class.</param>
        /// <param name="adaptiveCardHelper">The instance of <see cref="AdaptiveCardHelper"/> class.</param>
        /// <param name="newsSearchService">The instance of <see cref="NewsSearchService"/>.</param>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/>.</param>
        /// <param name="userGraphServiceHelper">The instance of <see cref="UserGraphServiceHelper"/>.</param>
        public NewsRequestHelper(
            INewsMapper newsMapper,
            INewsRepository newsRepository,
            IAdaptiveCardHelper adaptiveCardHelper,
            INewsSearchService newsSearchService,
            IFilterQueryHelper filterQueryHelper,
            IUserGraphServiceHelper userGraphServiceHelper)
        {
            this.newsMapper = newsMapper;
            this.newsRepository = newsRepository;
            this.adaptiveCardHelper = adaptiveCardHelper;
            this.newsSearchService = newsSearchService;
            this.filterQueryHelper = filterQueryHelper;
            this.userGraphServiceHelper = userGraphServiceHelper;
        }

        /// <inheritdoc/>
        public async Task<NewsEntityDTO> CreateNewsArticleRequestAsync(NewsEntityDTO newsArticleRequestDetails, Guid userAadId, string upn, string userName)
        {
            if (newsArticleRequestDetails == null)
            {
                throw new ArgumentNullException(nameof(newsArticleRequestDetails));
            }

            var newsArticleRequest = await this.GetNewsArticleRequestAsync(newsArticleRequestDetails.Title.Trim());

            // Request can not be created if active news article with the same title already exists.
            if (newsArticleRequest != null)
            {
                return null;
            }

            var newsArticleRequestToCreate = this.newsMapper.MapForCreateModel(newsArticleRequestDetails, userAadId, upn);

            var newNewsArticleRequest = await this.newsRepository.CreateOrUpdateAsync(newsArticleRequestToCreate);

            if (newNewsArticleRequest != null)
            {
                await this.newsSearchService.RunIndexerOnDemandAsync();
                await this.adaptiveCardHelper.SendNewNewsArticleRequestCardToAdminTeamAsync(newNewsArticleRequest, userName);
                await this.adaptiveCardHelper.SendNewsRequestCardToCreatorAsync(newNewsArticleRequest);
            }

            return this.newsMapper.MapForViewModel(newNewsArticleRequest);
        }

        /// <inheritdoc/>
        public async Task<NewsEntityDTO> CreateDraftNewsArticleRequestAsync(DraftNewsEntityDTO draftNewsArticleRequestDetails, Guid userAadId, string upn)
        {
            if (draftNewsArticleRequestDetails == null)
            {
                throw new ArgumentNullException(nameof(draftNewsArticleRequestDetails));
            }

            var newsArticleRequest = await this.GetNewsArticleRequestAsync(draftNewsArticleRequestDetails.Title.Trim());

            // Request can not be created if active news article with the same title already exists.
            if (newsArticleRequest != null)
            {
                return null;
            }

            var draftNewsArticleRequestToCreate = this.newsMapper.MapForCreateDraftModel(draftNewsArticleRequestDetails, userAadId, upn);

            var draftNewsArticleRequest = await this.newsRepository.CreateOrUpdateAsync(draftNewsArticleRequestToCreate);

            await this.newsSearchService.RunIndexerOnDemandAsync();

            return this.newsMapper.MapForViewModel(draftNewsArticleRequest);
        }

        /// <inheritdoc/>
        public async Task DeleteNewsArticleRequestsAsync(IEnumerable<string> newsArticleRequestsIds, Guid userAadId)
        {
            if (newsArticleRequestsIds.IsNullOrEmpty())
            {
                throw new ArgumentException("The news article Ids must be provided in order to delete requests.", nameof(newsArticleRequestsIds));
            }

            var newsArticleRequestsToBeDeleted = await this.newsRepository.GetActiveNewsArticlesAsync(newsArticleRequestsIds, userAadId);

            // The news article requests can be deleted only if the request status is 'Draft' or 'Pending'.
            newsArticleRequestsToBeDeleted = newsArticleRequestsToBeDeleted
                .Where(newsRequest => newsRequest.Status == (int)NewsArticleRequestStatus.Draft || newsRequest.Status == (int)NewsArticleRequestStatus.Pending);

            if (!newsArticleRequestsToBeDeleted.Any())
            {
                return;
            }

            foreach (var newsArtcileRequest in newsArticleRequestsToBeDeleted)
            {
                newsArtcileRequest.IsDeleted = true;
                newsArtcileRequest.UpdatedAt = DateTime.UtcNow;
            }

            await this.newsRepository.BatchInsertOrMergeAsync(newsArticleRequestsToBeDeleted);
            await this.newsSearchService.RunIndexerOnDemandAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NewsEntityDTO>> GetActiveNewsArticlesAsync(string searchText, int pageNumber, NewsArticleSortColumn sortColumn, SortOrder sortOrder, IEnumerable<int> statusFilterValues, Guid userAadId)
        {
            var searchServiceParameters = new SearchParametersDTO
            {
                PageCount = pageNumber,
                SearchString = searchText?.Trim().EscapeSpecialCharacters(),
                NewsArticleSortColumn = sortColumn,
                SortOrder = sortOrder,
                Filter = this.filterQueryHelper.GetActiveNewsArticleRequestsFilterCondition(statusFilterValues, userAadId),
            };

            var activeNewsArticles = await this.newsSearchService.GetNewsAsync(searchServiceParameters);

            var activeNewsArticlesDTOs = activeNewsArticles
                .Select(newsArticleRequest => this.newsMapper.MapForViewModel(newsArticleRequest));

            return activeNewsArticlesDTOs;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NewsRequestViewDTO>> GetPendingForApprovalNewsArticlesAsync(string searchText, int pageNumber, NewsArticleSortColumn sortColumn, SortOrder sortOrder, List<int> statusFilterValues)
        {
            var result = new List<NewsRequestViewDTO>();
            var searchServiceParameters = new SearchParametersDTO
            {
                PageCount = pageNumber,
                SearchString = searchText?.Trim().EscapeSpecialCharacters(),
                NewsArticleSortColumn = sortColumn,
                SortOrder = sortOrder,
                Filter = this.filterQueryHelper.GetFilterCondition(nameof(CommunityOfInterestEntity.Status), statusFilterValues.Any() ? statusFilterValues : new List<int> { (int)RequestStatus.Pending, (int)RequestStatus.Approved, (int)RequestStatus.Rejected }),
            };

            var results = await this.newsSearchService.GetNewsAsync(searchServiceParameters);

            if (!results.IsNullOrEmpty())
            {
                var details = await this.userGraphServiceHelper.GetUsersAsync(results.Where(request => !string.IsNullOrEmpty(request.CreatedBy)).Select(request => request.CreatedBy));

                foreach (var news in results)
                {
                    var newsItem = this.newsMapper.MapForApprovalRequestViewModel(news);
                    newsItem.CreatedBy = details.Where(user => user.Id == news.CreatedBy).FirstOrDefault()?.DisplayName;
                    result.Add(newsItem);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public async Task<NewsEntityDTO> GetNewsArticleRequestAsync(Guid newsArticleTableId)
        {
            var newsArticleRequest = await this.newsRepository.GetAsync(NewsTableMetadata.NewsPartitionKey, newsArticleTableId.ToString());

            if (newsArticleRequest == null || newsArticleRequest.IsDeleted)
            {
                return null;
            }

            return this.newsMapper.MapForViewModel(newsArticleRequest);
        }

        /// <inheritdoc/>
        public async Task<NewsEntityDTO> SubmitDraftNewsArticleRequestAsync(NewsEntityDTO draftNewsArticleRequest, Guid userAadId, string upn, string userName)
        {
            if (draftNewsArticleRequest == null)
            {
                throw new ArgumentNullException(nameof(draftNewsArticleRequest));
            }

            var newsArticleRequest = await this.newsRepository.GetAsync(NewsTableMetadata.NewsPartitionKey, draftNewsArticleRequest.TableId);

            // End-user can update the request only when the request status is 'Draft'.
            if (newsArticleRequest == null || newsArticleRequest.Status != (int)NewsArticleRequestStatus.Draft)
            {
                return null;
            }

            var existingNewsArticleRequest = await this.GetNewsArticleRequestAsync(draftNewsArticleRequest.Title.Trim());

            // Request can not be created if active news article with the same title already exists.
            if (existingNewsArticleRequest != null && existingNewsArticleRequest.TableId != newsArticleRequest.TableId)
            {
                return null;
            }

            this.newsMapper.MapForUpdateModel(draftNewsArticleRequest, newsArticleRequest);
            newsArticleRequest.Status = (int)NewsArticleRequestStatus.Pending;

            var updatedNewsArticleRequest = await this.newsRepository.InsertOrMergeAsync(newsArticleRequest);

            if (updatedNewsArticleRequest != null)
            {
                await this.newsSearchService.RunIndexerOnDemandAsync();
                await this.adaptiveCardHelper.SendNewNewsArticleRequestCardToAdminTeamAsync(updatedNewsArticleRequest, userName);
                await this.adaptiveCardHelper.SendNewsRequestCardToCreatorAsync(updatedNewsArticleRequest);
            }

            return this.newsMapper.MapForViewModel(updatedNewsArticleRequest);
        }

        /// <inheritdoc/>
        public async Task<NewsEntityDTO> UpdateDraftNewsArticleRequestAsync(DraftNewsEntityDTO draftNewsArticleRequest, Guid userAadId, string upn)
        {
            if (draftNewsArticleRequest == null)
            {
                throw new ArgumentNullException(nameof(draftNewsArticleRequest));
            }

            var newsArticleRequest = await this.newsRepository.GetAsync(NewsTableMetadata.NewsPartitionKey, draftNewsArticleRequest.TableId);

            // End-user can update the request only when the request status is 'Draft'.
            if (newsArticleRequest == null || newsArticleRequest.Status != (int)NewsArticleRequestStatus.Draft)
            {
                return null;
            }

            var existingNewsArticleRequest = await this.GetNewsArticleRequestAsync(draftNewsArticleRequest.Title.Trim());

            // Request can not be created if active news article with the same title already exists.
            if (existingNewsArticleRequest != null && existingNewsArticleRequest.TableId != newsArticleRequest.TableId)
            {
                return null;
            }

            this.newsMapper.MapForUpdateModel(draftNewsArticleRequest, newsArticleRequest);

            var updatedNewsArticleRequest = await this.newsRepository.InsertOrMergeAsync(newsArticleRequest);

            await this.newsSearchService.RunIndexerOnDemandAsync();

            return this.newsMapper.MapForViewModel(updatedNewsArticleRequest);
        }

        /// <summary>
        /// Validates whether a news article request with same title already exists.
        /// </summary>
        /// <param name="title">The news article title.</param>
        /// <returns>Returns true if the news article requests already exists. Else returns false.</returns>
        private async Task<NewsEntity> GetNewsArticleRequestAsync(string title)
        {
            var searchServiceParameters = new SearchParametersDTO
            {
                PageCount = 0,
                Filter = this.filterQueryHelper.GetFilterCondition(nameof(NewsEntity.Title), new List<string> { title.Trim() }),
            };

            var newsArticles = await this.newsSearchService.GetNewsAsync(searchServiceParameters);

            return newsArticles.FirstOrDefault();
        }
    }
}