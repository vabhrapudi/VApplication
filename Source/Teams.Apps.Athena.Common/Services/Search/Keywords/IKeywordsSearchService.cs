// <copyright file="IKeywordsSearchService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Keywords
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Keywords search service provider to fetch keywords.
    /// </summary>
    public interface IKeywordsSearchService
    {
        /// <summary>
        /// Get keywords as per search criteria.
        /// </summary>
        /// <param name="searchParametersDTO">Advanced search parameters.</param>
        /// <returns>Returns keywords</returns>
        Task<IEnumerable<KeywordEntity>> GetKeywordsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Run the indexer on demand.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task RunIndexerOnDemandAsync();
    }
}
