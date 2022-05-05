// <copyright file="IQuickAccessMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods that manages quick access entity model mappings.
    /// </summary>
    public interface IQuickAccessMapper
    {
        /// <summary>
        /// Gets quick access model to be inserted in database.
        /// </summary>
        /// <param name="quickAccessItem">The quick access item.</param>
        /// <param name="userId">User Id.</param>
        /// <returns>Returns a quick access entity model.</returns>
        QuickAccessEntity MapForCreateModel(QuickAccessItemCreateDTO quickAccessItem, string userId);
    }
}
