// <copyright file="IAthenaNewsSourcesBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to Athena news sources blob repository.
    /// </summary>
    public interface IAthenaNewsSourcesBlobRepository : IBlobBaseRepository<IEnumerable<AthenaNewsSource>>
    {
        /// <summary>
        /// Returns the news Source by news source Id.
        /// </summary>
        /// <param name="newsSourceId">The news source Id.</param>
        /// <returns>The news Source</returns>
        Task<AthenaNewsSource> GetNewsSourceById(int newsSourceId);
    }
}
