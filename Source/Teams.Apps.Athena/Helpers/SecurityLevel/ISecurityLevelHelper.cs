// <copyright file="ISecurityLevelHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes helper methods related to security level operations.
    /// </summary>
    public interface ISecurityLevelHelper
    {
        /// <summary>
        /// Gets the security levels.
        /// </summary>
        /// <returns>The collection of security level.</returns>
        Task<IEnumerable<SecurityLevels>> GetSecurityLevelsAsync();
    }
}