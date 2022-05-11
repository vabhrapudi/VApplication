// <copyright file="INewsHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Models.Enums;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing news.
    /// </summary>
    public interface INewsHelper
    {
        /// <summary>
        /// Gets news item by table Id.
        /// </summary>
        /// <param name="tableId">The news table Id.</param>
        /// <param name="userAadObjectId">Current user object Id.</param>
        /// <returns>Returns news details.</returns>
        Task<NewsEntityDTO> GetNewsByTableIdAsync(string tableId, string userAadObjectId);

        /// <summary>
        /// Gets news items.
        /// </summary>
        /// <param name="searchString">The searched string.</param>
        /// <param name="pageCount">>Page count for which post needs to be fetched.</param>
        /// <param name="sortBy">0 for recent, 1 for significance and 2 for rating high to low for news. Refer <see cref="SortBy"/> for values.</param>
        /// <param name="newsFilters">Holds the filter parameters for news.</param>
        /// <param name="userObjectId">Current user object Id.</param>
        /// <returns>Returns array news details.</returns>
        Task<IEnumerable<NewsEntityDTO>> GetNewsAsync(string searchString, int pageCount, int sortBy, NewsFilterParametersDTO newsFilters, string userObjectId);

        /// <summary>
        /// Gets coi news items.
        /// </summary>
        /// <param name="teamId">Team Id.</param>
        /// <param name="searchString">The searched string.</param>
        /// <param name="pageCount">>Page count for which post needs to be fetched.</param>
        /// <param name="sortBy">0 for recent, 1 for significance and 2 for rating high to low for news. Refer <see cref="SortBy"/> for values.</param>
        /// <param name="newsFilters">Holds the filter parameters for news.</param>
        /// <param name="userObjectId">Current user object Id.</param>
        /// <returns>Returns array news details.</returns>
        Task<IEnumerable<NewsEntityDTO>> GetCoiNewsAsync(string teamId, string searchString, int pageCount, int sortBy, NewsFilterParametersDTO newsFilters, string userObjectId);

        /// <summary>
        /// Submits rating for a news item.
        /// </summary>
        /// <param name="tableId">The news table Id.</param>
        /// <param name="rating">Rating provided by user.</param>
        /// <param name="userAadObjectId">User Id who submitted rating.</param>
        /// <returns>Returns task indicating operation result.</returns>
        Task RateNewsAsync(string tableId, int rating, string userAadObjectId);

        /// <summary>
        /// Gets the approved news articles by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The news articles.</returns>
        Task<IEnumerable<NewsEntityDTO>> GetApprovedNewsArticlesByKeywordIdsAsync(IEnumerable<int> keywordIds);

        /// <summary>
        /// Gets the news articles.
        /// </summary>
        /// <param name="searchParametersDTO">Advanced search parameters.</param>
        /// <returns>The collection of news articles.</returns>
        Task<IEnumerable<NewsEntityDTO>> GetNewsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Gets the approved news articles.
        /// </summary>
        /// <param name="keywords">The collection of keywords.</param>
        /// <param name="fromDate">The date and time from which news articles to get.</param>
        /// <param name="count">The number of news articles to get.</param>
        /// <returns>The collection of news articles.</returns>
        Task<IEnumerable<NewsEntityDTO>> GetApprovedNewsArticlesAsync(IEnumerable<int> keywords, DateTime fromDate, int? count);

        /// <summary>
        /// Gets the node types for news.
        /// </summary>
        /// <returns>The collection of new node types.</returns>
        Task<IEnumerable<NodeType>> GetNodeTypesForNewsAsync();

        /// <summary>
        /// Gets the news keyword Ids.
        /// </summary>
        /// <returns>The collection of news keyword Ids.</returns>
        Task<IEnumerable<int>> GetNewsKeywordIdsAsync();

        /// <summary>
        /// Updates the news article.
        /// </summary>
        /// <param name="tableId">The table Id of news article.</param>
        /// <param name="isImportant">Indicates if the news aricle is important.</param>
        /// <returns>Returns the updated news article.</returns>
        Task<NewsEntityDTO> UpdateNewsAsync(string tableId, bool isImportant);
    }
}