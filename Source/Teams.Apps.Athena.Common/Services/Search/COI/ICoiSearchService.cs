// <copyright file="ICoiSearchService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes search services related to COI.
    /// </summary>
    public interface ICoiSearchService
    {
        /// <summary>
        /// Get COIs list as per search and filter criteria.
        /// </summary>
        /// <param name="searchParametersDTO">Search parameters for enhanced searching.</param>
        /// <returns>The collection of COIs.</returns>
        Task<IEnumerable<CommunityOfInterestEntity>> GetCommunityOfInterestsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Run the indexer on demand.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task RunIndexerOnDemandAsync();
    }
}
