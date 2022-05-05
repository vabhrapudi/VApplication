// <copyright file="IGraduationDegreeHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provides helper methods for managing graduation degree entity.
    /// </summary>
    public interface IGraduationDegreeHelper
    {
        /// <summary>
        /// Gets the degrees of graduation.
        /// </summary>
        /// <returns>Returns graduation degrees.</returns>
        Task<IEnumerable<GraduationDegree>> GetGraduationDegreesAsync();
    }
}