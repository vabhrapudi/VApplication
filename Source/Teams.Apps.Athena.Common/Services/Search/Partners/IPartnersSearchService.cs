// <copyright file="IPartnersSearchService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to Athena partners search service.
    /// </summary>
    public interface IPartnersSearchService
    {
        /// <summary>
        /// Gets the Athena partners.
        /// </summary>
        /// <param name="searchParametersDTO">The search parameters for enhanced searching.</param>
        /// <returns>The collection of <see cref="PartnerEntity"/>.</returns>
        Task<IEnumerable<PartnerEntity>> GetPartnersAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Run the indexer on demand.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task RunIndexerOnDemandAsync();
    }
}
