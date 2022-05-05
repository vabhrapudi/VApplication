// <copyright file="IResearchRequestsSearchService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to research requests search service.
    /// </summary>
    public interface IResearchRequestsSearchService
    {
        /// <summary>
        /// Gets the research requests.
        /// </summary>
        /// <param name="searchParametersDTO">The search parameters for enhanced searching.</param>
        /// <returns>The collection of <see cref="ResearchRequestEntity"/>.</returns>
        Task<IEnumerable<ResearchRequestEntity>> GetResearchRequestsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Run the indexer on demand.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task RunIndexerOnDemandAsync();
    }
}
