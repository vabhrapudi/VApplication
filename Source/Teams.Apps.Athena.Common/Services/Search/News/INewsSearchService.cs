// <copyright file="INewsSearchService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search.News
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// News search service provider to fetch news based on search and filter criteria.
    /// </summary>
    public interface INewsSearchService
    {
        /// <summary>
        /// Get news list as per search and filter criteria.
        /// </summary>
        /// <param name="searchParametersDTO">Search parameters for enhanced searching.</param>
        /// <returns>List of news.</returns>
        Task<IEnumerable<NewsEntity>> GetNewsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Run the indexer on demand.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task RunIndexerOnDemandAsync();
    }
}
