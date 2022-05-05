// <copyright file="IAthenaInfoResourcesHelper.cs" company="NPS Foundation">
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
    /// The helper methods related to Athena info resources.
    /// </summary>
    public interface IAthenaInfoResourcesHelper
    {
        /// <summary>
        /// Gets the Athena info resources.
        /// </summary>
        /// <param name="searchParametersDTO">The advanced search parameters.</param>
        /// <returns>The collection of Athena info resources.</returns>
        Task<IEnumerable<AthenaInfoResourceDTO>> GetAthenaInfoResourcesAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Gets the Athena info resources.
        /// </summary>
        /// <param name="keywords">The collection of keywords.</param>
        /// <param name="fromDate">The date and time from which info resources to get.</param>
        /// <param name="count">The number of info resources to get.</param>
        /// <returns>The collection of info resources.</returns>
        Task<IEnumerable<AthenaInfoResourceDTO>> GetAthenaInfoResourcesAsync(IEnumerable<int> keywords, DateTime fromDate, int? count);
    }
}
