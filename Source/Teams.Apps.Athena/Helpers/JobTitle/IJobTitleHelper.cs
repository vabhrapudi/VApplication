// <copyright file="IJobTitleHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provides helper methods for managing job title.
    /// </summary>
    public interface IJobTitleHelper
    {
        /// <summary>
        /// Gets all the job title data from json.
        /// </summary>
        /// <returns>Returns collection of Id and title of job title from json file.</returns>
        Task<IEnumerable<JobTitle>> GetJobTitlesAsync();
    }
}
