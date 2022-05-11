// <copyright file="NewsHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Models.Enums;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Repositories.ResourceFeedback;
    using Teams.Apps.Athena.Common.Services.Search.News;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing news.
    /// </summary>
    public class NewsHelper : INewsHelper
    {
        private const char KeywordIdsSeparator = ' ';

        /// <summary>
        /// The instance of <see cref="NewsMapper"/> class.
        /// </summary>
        private readonly INewsMapper newsMapper;

        /// <summary>
        /// The instance of <see cref="NewsRepository"/> class.
        /// </summary>
        private readonly INewsRepository newsRepository;

        /// <summary>
        /// The instance of <see cref="CoiRepository"/> class.
        /// </summary>
        private readonly ICoiRepository coiRepository;

        /// <summary>
        /// The instance of <see cref="NewsSearchService"/> class.
        /// </summary>
        private readonly INewsSearchService newsSearchService;

        /// <summary>
        /// The instance of <see cref="ResourceFeedbackRepository"/> class.
        /// </summary>
        private readonly IResourceFeedbackRepository resourceFeedbackRepository;

        /// <summary>
        /// The instance of <see cref="FilterQueryHelper"/> class.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// The instance of <see cref="UserGraphServiceHelper"/> class.
        /// </summary>
        private readonly IUserGraphServiceHelper userGraphServiceHelper;

        /// <summary>
        /// The instance of <see cref="NodeTypeForDiscoveryTreeBlobRepository"/> class.
        /// </summary>
        private readonly INodeTypeForDiscoveryTreeBlobRepository nodeTypeForDiscoveryTreeBlobRepository;

        /// <summary>
        /// The instance of <see cref="AthenaNewsKeywordsBlobRepository"/> class.
        /// </summary>
        private readonly IAthenaNewsKeywordsBlobRepository athenaNewsKeywordsBlobRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsHelper"/> class.
        /// </summary>
        /// <param name="newsMapper">The instance of <see cref="NewsMapper"/> class.</param>
        /// <param name="newsRepository">The instance of <see cref="NewsRepository"/> class.</param>
        /// <param name="coiRepository">The instance of <see cref="CoiRepository"/> class.</param>
        /// <param name="newsSearchService">The instance of <see cref="NewsSearchService"/> class.</param>
        /// <param name="resourceFeedbackRepository">The instance of <see cref="ResourceFeedbackRepository"/> class.</param>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="userGraphServiceHelper">The instance of <see cref="UserGraphServiceHelper"/> class.</param>
        /// <param name="nodeTypeForDiscoveryTreeBlobRepository">The instance of <see cref="NodeTypeForDiscoveryTreeBlobRepository"/> class.</param>
        /// <param name="athenaNewsKeywordsBlobRepository">The instance of <see cref="AthenaNewsKeywordsBlobRepository"/> class.</param>
        public NewsHelper(
            INewsMapper newsMapper,
            INewsRepository newsRepository,
            ICoiRepository coiRepository,
            INewsSearchService newsSearchService,
            IResourceFeedbackRepository resourceFeedbackRepository,
            IFilterQueryHelper filterQueryHelper,
            IUserGraphServiceHelper userGraphServiceHelper,
            INodeTypeForDiscoveryTreeBlobRepository nodeTypeForDiscoveryTreeBlobRepository,
            IAthenaNewsKeywordsBlobRepository athenaNewsKeywordsBlobRepository)
        {
            this.newsMapper = newsMapper;
            this.newsRepository = newsRepository;
            this.coiRepository = coiRepository;
            this.newsSearchService = newsSearchService;
            this.resourceFeedbackRepository = resourceFeedbackRepository;
            this.filterQueryHelper = filterQueryHelper;
            this.userGraphServiceHelper = userGraphServiceHelper;
            this.nodeTypeForDiscoveryTreeBlobRepository = nodeTypeForDiscoveryTreeBlobRepository;
            this.athenaNewsKeywordsBlobRepository = athenaNewsKeywordsBlobRepository;
        }

        /// <inheritdoc/>
        public async Task<NewsEntityDTO> GetNewsByTableIdAsync(string tableId, string userAadObjectId)
        {
            tableId = tableId ?? throw new ArgumentNullException(nameof(tableId), "News table Id cannot be null.");

            var news = await this.newsRepository.GetAsync(NewsTableMetadata.NewsPartitionKey, tableId);
            var newsItem = this.newsMapper.MapForViewModel(news);
            var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.News, new List<string> { tableId }, userAadObjectId);
            if (resourceFeedback.Any())
            {
                newsItem.UserRating = resourceFeedback.FirstOrDefault().Rating;
            }

            return newsItem;
        }

        /// <inheritdoc/>
        public async Task RateNewsAsync(string tableId, int rating, string userAadObjectId)
        {
            tableId = tableId ?? throw new ArgumentNullException(nameof(tableId), "News table Id cannot be null.");
            var news = await this.newsRepository.GetAsync(NewsTableMetadata.NewsPartitionKey, tableId);

            if (news != null)
            {
                var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync((int)Itemtype.News, new List<string> { tableId }, userAadObjectId);
                if (resourceFeedback.Any())
                {
                    var feedback = resourceFeedback.FirstOrDefault();
                    if (feedback.Rating > rating)
                    {
                        news.SumOfRatings -= feedback.Rating - rating;
                    }
                    else
                    {
                        news.SumOfRatings += rating - feedback.Rating;
                    }

                    feedback.Rating = rating;
                    await this.resourceFeedbackRepository.CreateOrUpdateAsync(feedback);
                }
                else
                {
                    var feedback = new ResourceFeedback
                    {
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userAadObjectId,
                        FeedbackId = Guid.NewGuid().ToString(),
                        Rating = rating,
                        ResourceId = tableId,
                        ResourceTypeId = (int)Itemtype.News,
                    };

                    news.SumOfRatings += rating;
                    news.NumberOfRatings += 1;
                    await this.resourceFeedbackRepository.InsertOrMergeAsync(feedback);
                }

                var avg = (decimal)news.SumOfRatings / (decimal)news.NumberOfRatings;
#pragma warning disable CA1305 // Culture provider is not required as its a number to string conversion
                news.AverageRating = avg.ToString("0.0");
#pragma warning restore CA1305 // Culture provider is not required as its a number to string conversion
                await this.newsRepository.CreateOrUpdateAsync(news);
                await this.newsSearchService.RunIndexerOnDemandAsync();
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NewsEntityDTO>> GetNewsAsync(string searchString, int pageCount, int sortBy, NewsFilterParametersDTO newsFilters, string userObjectId)
        {
            var results = new List<NewsEntityDTO>();
            var searchParamsDTO = new SearchParametersDTO
            {
                SearchString = searchString?.Trim().EscapeSpecialCharacters(),
                PageCount = pageCount,
                SortByFilter = sortBy,
                NewsArticleSortColumn = NewsArticleSortColumn.None,
                Filter = this.filterQueryHelper.GetFilterCondition(nameof(NewsEntity.Status), new List<int> { (int)NewsArticleRequestStatus.Approved }),
            };

            if (!newsFilters.Keywords.IsNullOrEmpty() && newsFilters.Keywords.Any())
            {
                var keywordFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(NewsEntity.Keywords), newsFilters.Keywords.Select(keyword => keyword.KeywordId));
                searchParamsDTO.Filter += $" and ({keywordFilter})";
            }

            if (!newsFilters.NewsTypes.IsNullOrEmpty() && newsFilters.NewsTypes.Any())
            {
                var newsTypeFilter = this.filterQueryHelper.GetFilterCondition(nameof(NewsEntity.NodeTypeId), newsFilters.NewsTypes.Select(newsType => newsType.NodeTypeId));
                searchParamsDTO.Filter += $" and ({newsTypeFilter})";
            }

            var newsList = await this.newsSearchService.GetNewsAsync(searchParamsDTO);
            if (!newsList.IsNullOrEmpty())
            {
                var details = await this.userGraphServiceHelper.GetUsersAsync(newsList.Where(request => !string.IsNullOrEmpty(request.CreatedBy)).Select(request => request.CreatedBy));
                var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync(1, newsList.Select(x => x.TableId), userObjectId);

                foreach (var news in newsList)
                {
                    var newsItem = this.newsMapper.MapForViewModel(news);
                    newsItem.CreatedBy = details.Where(user => user.Id == news.CreatedBy).FirstOrDefault()?.DisplayName;
                    var feedback = resourceFeedback.Where(x => x.ResourceId == news.TableId).FirstOrDefault();
                    if (feedback != null)
                    {
                        newsItem.UserRating = feedback.Rating;
                    }

                    results.Add(newsItem);
                }
            }

            return results;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NewsEntityDTO>> GetCoiNewsAsync(string teamId, string searchString, int pageCount, int sortBy, NewsFilterParametersDTO newsFilters, string userObjectId)
        {
            var results = new List<NewsEntityDTO>();
            var searchParamsDTO = new SearchParametersDTO
            {
                SearchString = searchString?.Trim().EscapeSpecialCharacters(),
                PageCount = pageCount,
                SortByFilter = sortBy,
                NewsArticleSortColumn = NewsArticleSortColumn.None,
                Filter = this.filterQueryHelper.GetFilterCondition(nameof(NewsEntity.Status), new List<int> { (int)NewsArticleRequestStatus.Approved }),
            };

            if (newsFilters.Keywords.IsNullOrEmpty() || !newsFilters.Keywords.Any())
            {
                var filterQuery = this.coiRepository.GetCoiFilterAsync(teamId);
                var coiRequest = await this.coiRepository.GetWithFilterAsync(filterQuery);

                if (coiRequest.Any() && coiRequest.FirstOrDefault() != null && !coiRequest.First().Keywords.IsNullOrEmpty())
                {
                    var coiKeywords = coiRequest.First().Keywords;
                    var coiKeywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(NewsEntity.Keywords), coiKeywords);
                    searchParamsDTO.Filter += $" and ({coiKeywordsFilter})";
                }
                else
                {
                    return results;
                }
            }
            else
            {
                var filterQuery = this.coiRepository.GetCoiFilterAsync(teamId);
                var coiRequest = await this.coiRepository.GetWithFilterAsync(filterQuery);

                if (coiRequest.Any() && coiRequest.FirstOrDefault() != null)
                {
                    var validKeywords = coiRequest.First().Keywords.Split(KeywordIdsSeparator).Intersect(newsFilters.Keywords.Select(keyword => keyword.KeywordId));

                    if (validKeywords.Any())
                    {
                        var keywordFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(NewsEntity.Keywords), validKeywords);
                        searchParamsDTO.Filter += $" and ({keywordFilter})";
                    }
                    else
                    {
                        return results;
                    }
                }
                else
                {
                    return results;
                }
            }

            if (!newsFilters.NewsTypes.IsNullOrEmpty() && newsFilters.NewsTypes.Any())
            {
                var newsTypeFilter = this.filterQueryHelper.GetFilterCondition(nameof(NewsEntity.NodeTypeId), newsFilters.NewsTypes.Select(newsType => newsType.NodeTypeId));
                searchParamsDTO.Filter += $" and ({newsTypeFilter})";
            }

            var newsList = await this.newsSearchService.GetNewsAsync(searchParamsDTO);
            if (!newsList.IsNullOrEmpty())
            {
                var resourceFeedback = await this.resourceFeedbackRepository.GetFeedbackByResourceTypeAsync(1, newsList.Select(x => x.TableId), userObjectId);

                foreach (var news in newsList)
                {
                    var t = this.newsMapper.MapForViewModel(news);
                    var feedback = resourceFeedback.Where(x => x.ResourceId == news.TableId).FirstOrDefault();
                    if (feedback != null)
                    {
                        t.UserRating = feedback.Rating;
                    }

                    results.Add(t);
                }
            }

            return results;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NewsEntityDTO>> GetApprovedNewsArticlesByKeywordIdsAsync(IEnumerable<int> keywordIds)
        {
            var newsKeywordsFilterQuery = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(NewsEntity.Keywords), keywordIds);
            var approvedNewsFilterQuery = this.filterQueryHelper.GetFilterCondition(nameof(NewsEntity.Status), new[] { (int)NewsArticleRequestStatus.Approved });

            var newsFilterQuery = this.filterQueryHelper.CombineFilters(newsKeywordsFilterQuery, approvedNewsFilterQuery, TableOperators.And);

            var newsSearchParametersDto = new SearchParametersDTO
            {
                IsGetAllRecords = true,
                Filter = newsFilterQuery,
            };

            var newsArticles = await this.newsSearchService.GetNewsAsync(newsSearchParametersDto);
            return newsArticles.Select(x => this.newsMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NewsEntityDTO>> GetNewsAsync(SearchParametersDTO searchParametersDTO)
        {
            var newsArticles = await this.newsSearchService.GetNewsAsync(searchParametersDTO);
            return newsArticles.Select(x => this.newsMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NewsEntityDTO>> GetApprovedNewsArticlesAsync(IEnumerable<int> keywords, DateTime fromDate, int? count)
        {
            var publishedDateFilter = this.filterQueryHelper.GetFilterConditionForDate(nameof(NewsEntity.PublishedDate), QueryComparisons.GreaterThanOrEqual, new[] { fromDate.ToZuluTimeFormatWithStartOfDay() });
            var keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(NewsEntity.Keywords), keywords);
            var approvedNewsFilter = this.filterQueryHelper.GetFilterCondition(nameof(NewsEntity.Status), new[] { (int)NewsArticleRequestStatus.Approved });

            var searchParametersDto = new SearchParametersDTO
            {
                TopRecordsCount = count,
                Filter = this.filterQueryHelper.CombineFilters(new[] { publishedDateFilter, keywordsFilter, approvedNewsFilter }, TableOperators.And),
                OrderBy = new List<string> { $"{nameof(NewsEntity.PublishedDate)} desc" },
            };

            var newsArticles = await this.newsSearchService.GetNewsAsync(searchParametersDto);
            return newsArticles.Select(x => this.newsMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NodeType>> GetNodeTypesForNewsAsync()
        {
            var nodeType = await this.nodeTypeForDiscoveryTreeBlobRepository.GetBlobJsonFileContentAsync(NodeTypeForDiscoveryTreeBlobMetadata.FileName);

            var coiNodeTypes = new List<NodeType>();
            if (nodeType != null)
            {
                coiNodeTypes = nodeType.Where(x => x.JsonFile == FileNames.AthenaNewsArticles).ToList();
            }

            return coiNodeTypes;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<int>> GetNewsKeywordIdsAsync()
        {
            var newsKeywordData = await this.athenaNewsKeywordsBlobRepository.GetBlobJsonFileContentAsync(AthenaNewsKeywordsBlobMetadata.FileName);
            if (!newsKeywordData.IsNullOrEmpty())
            {
                return newsKeywordData.First().Keywords;
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<NewsEntityDTO> UpdateNewsAsync(string tableId, bool isImportant)
        {
            var newsArticleRequest = await this.newsRepository.GetAsync(NewsTableMetadata.NewsPartitionKey, tableId);

            if (newsArticleRequest == null)
            {
                return null;
            }

            newsArticleRequest.IsImportant = isImportant;

            var updatedNewsArticleRequest = await this.newsRepository.InsertOrMergeAsync(newsArticleRequest);

            if (updatedNewsArticleRequest != null)
            {
                await this.newsSearchService.RunIndexerOnDemandAsync();
            }

            return this.newsMapper.MapForViewModel(updatedNewsArticleRequest);
        }
    }
}