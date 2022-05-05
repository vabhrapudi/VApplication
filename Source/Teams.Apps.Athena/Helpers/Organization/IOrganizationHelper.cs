// <copyright file="IOrganizationHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provides helper methods for managing organization entity.
    /// </summary>
    public interface IOrganizationHelper
    {
        /// <summary>
        /// Gets the organizations.
        /// </summary>
        /// <returns>Returns organizations.</returns>
        Task<IEnumerable<OrganizationEntity>> GetOrganizationsAsync();
    }
}