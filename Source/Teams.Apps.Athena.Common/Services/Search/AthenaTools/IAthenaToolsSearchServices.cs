// <copyright file="IAthenaToolsSearchServices.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to Athena tools search service.
    /// </summary>
    public interface IAthenaToolsSearchServices
    {
        /// <summary>
        /// Gets Athena tools.
        /// </summary>
        /// <param name="searchParametersDTO">The search parameters for enhanced searching.</param>
        /// <returns>The collection of <see cref="AthenaToolEntity"/>.</returns>
        Task<IEnumerable<AthenaToolEntity>> GetAthenaToolsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Run the indexer on demand.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task RunIndexerOnDemandAsync();
    }
}
