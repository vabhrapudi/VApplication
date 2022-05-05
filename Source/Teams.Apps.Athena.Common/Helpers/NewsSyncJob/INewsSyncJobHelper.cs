// <copyright file="INewsSyncJobHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Helpers
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides the helper method for news sync job.
    /// </summary>
    public interface INewsSyncJobHelper
    {
        /// <summary>
        /// Creates or Updates the news data from json to table.
        /// </summary>
        /// <param name="lastRunAt">The last date time at which the sync job ran.</param>
        /// <returns>The task representing creation or updation of news data operation.</returns>
        Task CreateOrUpdateNewsJsonDataAsync(DateTime lastRunAt);
    }
}
