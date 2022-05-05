// <copyright file="INewsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>
namespace Teams.Apps.Athena.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for News Data Repository.
    /// </summary>
    public interface INewsRepository : IRepository<NewsEntity>
    {
        /// <summary>
        /// Get active news article requests by Ids.
        /// </summary>
        /// <param name="newsArticleRequestIds">The collection of news article request Ids to get.</param>
        /// <param name="userAadId">The user AAD Id of which requests to get.</param>
        /// <returns>The collection of news article requests.</returns>
        Task<IEnumerable<NewsEntity>> GetActiveNewsArticlesAsync(IEnumerable<string> newsArticleRequestIds, Guid userAadId);

        /// <summary>
        /// Get news article requests by news Id.
        /// </summary>
        /// <param name="newsId">The news Id.</param>
        /// <returns>The collection of news article requests.</returns>
        Task<NewsEntity> GetNewsDetailsByNewsId(int newsId);
    }
}