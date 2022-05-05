// <copyright file="IStudyDepartmentHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provides helper methods for study department entity.
    /// </summary>
    public interface IStudyDepartmentHelper
    {
        /// <summary>
        /// Gets the study departments.
        /// </summary>
        /// <returns>Returns study departments.</returns>
        Task<IEnumerable<StudyDepartmentEntity>> GetStudyDepartmentsAsync();
    }
}