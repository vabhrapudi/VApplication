// <copyright file="INewsBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The interface which expose methods to get news.
    /// </summary>
    public interface INewsBlobRepository : IBlobBaseRepository<IEnumerable<NewsJsonModel>>
    {
        /// <summary>
        /// Gets the news json data based on timestamp of last successful run to be insered in table.
        /// </summary>
        /// <param name="lastRunAt">The last date time at which the sync job ran.</param>
        /// <returns>The news json data that needs to be created or updated.</returns>
        Task<IEnumerable<NewsJsonModel>> GetNewsJsonData(DateTime lastRunAt);
    }
}
