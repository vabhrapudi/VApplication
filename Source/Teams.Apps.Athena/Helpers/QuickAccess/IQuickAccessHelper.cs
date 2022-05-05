// <copyright file="IQuickAccessHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The helper methods provider for quick access.
    /// </summary>
    public interface IQuickAccessHelper
    {
        /// <summary>
        /// Gets the list of quick access items.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>The quick access list.</returns>
        Task<IEnumerable<QuickAccessEntity>> GetQuickAccessListAsync(string userId);

        /// <summary>
        /// Adds quick access item.
        /// </summary>
        /// <param name="quickAccessItem">The quick access item.</param>
        /// <param name="userId">User Id.</param>
        /// <returns>The created comment.</returns>
        Task<QuickAccessEntity> AddQuickAccessItemAsync(QuickAccessItemCreateDTO quickAccessItem, string userId);

        /// <summary>
        /// Deletes the quick access item.
        /// </summary>
        /// <param name="quickAccessItemId">The quick access item Id.</param>
        /// <returns>Returns task indicating operation result.</returns>
        Task DeleteQuickAccessItemAsync(string quickAccessItemId);
    }
}