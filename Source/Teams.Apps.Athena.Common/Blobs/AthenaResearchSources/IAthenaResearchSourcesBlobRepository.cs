// <copyright file="IAthenaResearchSourcesBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to Athena research sources blob repository.
    /// </summary>
    public interface IAthenaResearchSourcesBlobRepository : IBlobBaseRepository<IEnumerable<AthenaResearchSource>>
    {
        /// <summary>
        /// Returns the research Source by research source Id.
        /// </summary>
        /// <param name="researchSourceId">The research source Id.</param>
        /// <returns>The research Source.</returns>
        Task<AthenaResearchSource> GetResearchSourceById(int researchSourceId);
    }
}