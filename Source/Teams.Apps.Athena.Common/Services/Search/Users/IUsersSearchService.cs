// <copyright file="IUsersSearchService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The users search service provider.
    /// </summary>
    public interface IUsersSearchService
    {
        /// <summary>
        /// Gets the all Athena users.
        /// </summary>
        /// <param name="searchParametersDTO">The search parameters.</param>
        /// <returns>A task to get all Athena users operation.</returns>
        Task<IEnumerable<UserEntity>> GetUsersAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Run the indexer on demand.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task RunIndexerOnDemandAsync();
    }
}
