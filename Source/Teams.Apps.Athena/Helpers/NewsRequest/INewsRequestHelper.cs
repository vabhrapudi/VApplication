// <copyright file="INewsRequestHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes helper methods related to news request operations.
    /// </summary>
    public interface INewsRequestHelper
    {
        /// <summary>
        /// Creates a new news article request.
        /// </summary>
        /// <param name="newsArticleRequestDetails">The details of news article request to be created.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <param name="upn">The logged-in user's user principle name (UPN).</param>
        /// <param name="userName">The logged-in user's name.</param>
        /// <returns>The details of newly created news article request.</returns>
        Task<NewsEntityDTO> CreateNewsArticleRequestAsync(NewsEntityDTO newsArticleRequestDetails, Guid userAadId, string upn, string userName);

        /// <summary>
        /// Creates a new draft news article request.
        /// </summary>
        /// <param name="draftNewsArticleRequestDetails">The draft news article request details.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <param name="upn">The logged-in user's user principle name (UPN).</param>
        /// <returns>The details of newly created draft news article request.</returns>
        Task<NewsEntityDTO> CreateDraftNewsArticleRequestAsync(DraftNewsEntityDTO draftNewsArticleRequestDetails, Guid userAadId, string upn);

        /// <summary>
        /// Gets all news articles created by user which are not deleted. If search text is provided, then search
        /// requests by title and keywords.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="pageNumber">The page number for which requests to be fetched.</param>
        /// <param name="sortColumn">The sort column of type <see cref="NewsArticleSortColumn"/>.</param>
        /// <param name="sortOrder">The sort order of type <see cref="SortOrder"/>.</param>
        /// <param name="statusFilterValues">The status filter values.</param>
        /// <param name="userAadId">The user AAD Id of which requests to get.</param>
        /// <returns>The collection of news articles.</returns>
        Task<IEnumerable<NewsEntityDTO>> GetActiveNewsArticlesAsync(string searchText, int pageNumber, NewsArticleSortColumn sortColumn, SortOrder sortOrder, IEnumerable<int> statusFilterValues, Guid userAadId);

        /// <summary>
        /// Deletes news article requests in batch.
        /// </summary>
        /// <param name="newsArticleRequestsIds">The collection of news article requests Ids to be deleted.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <returns>A task indicating requests delete operation.</returns>
        Task DeleteNewsArticleRequestsAsync(IEnumerable<string> newsArticleRequestsIds, Guid userAadId);

        /// <summary>
        /// Gets an active news article request by Id.
        /// </summary>
        /// <param name="newsArticleTableId">The news article table Id to get.</param>
        /// <returns>The news request details.</returns>
        Task<NewsEntityDTO> GetNewsArticleRequestAsync(Guid newsArticleTableId);

        /// <summary>
        /// Submits a draft news article request.
        /// </summary>
        /// <param name="draftNewsArticleRequest">The draft news article details to be submitted.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <param name="upn">The logged-in user's user principle name (UPN).</param>
        /// <param name="userName">The logged-in user's name.</param>
        /// <returns>The submitted news article request details.</returns>
        Task<NewsEntityDTO> SubmitDraftNewsArticleRequestAsync(NewsEntityDTO draftNewsArticleRequest, Guid userAadId, string upn, string userName);

        /// <summary>
        /// Updates a draft news article request.
        /// </summary>
        /// <param name="draftNewsArticleRequest">The draft news article details to be updated.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <param name="upn">The logged-in user's user principle name (UPN).</param>
        /// <returns>The updated draft news article request details.</returns>
        Task<NewsEntityDTO> UpdateDraftNewsArticleRequestAsync(DraftNewsEntityDTO draftNewsArticleRequest, Guid userAadId, string upn);

        /// <summary>
        /// Search pending news items.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <param name="pageNumber">Page number for which reuqests needs to be fetched.</param>
        /// <param name="sortColumn">Column name by which requests needs to be sorted.</param>
        /// <param name="sortOrder">Order by which sorting needs to be done.</param>
        /// <param name="statusFilterValues">Selected status filter.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IEnumerable<NewsRequestViewDTO>> GetPendingForApprovalNewsArticlesAsync(string searchText, int pageNumber, NewsArticleSortColumn sortColumn, SortOrder sortOrder, List<int> statusFilterValues);
    }
}