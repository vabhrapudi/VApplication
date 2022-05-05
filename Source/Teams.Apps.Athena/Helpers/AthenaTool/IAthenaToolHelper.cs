// <copyright file="IAthenaToolHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing Athena tools.
    /// </summary>
    public interface IAthenaToolHelper
    {
        /// <summary>
        /// Gets Athena tools details.
        /// </summary>
        /// <param name="searchParametersDTO">The advanced search parameters.</param>
        /// <returns>Returns the collection of Athena tool model </returns>
        Task<IEnumerable<AthenaToolDTO>> GetAthenaToolsAsync(SearchParametersDTO searchParametersDTO);
    }
}
