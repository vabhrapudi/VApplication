// <copyright file="ISpecialtyHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provides helper methods for managing specialty entity.
    /// </summary>
    public interface ISpecialtyHelper
    {
        /// <summary>
        /// Gets the specialty details.
        /// </summary>
        /// <returns>Returns specialty details.</returns>
        Task<IEnumerable<SpecialtyEntity>> GetSpecialtyDetailsAsync();
    }
}