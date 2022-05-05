// <copyright file="IQuickAccessRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for quick access repository.
    /// </summary>
    public interface IQuickAccessRepository : IRepository<QuickAccessEntity>
    {
        /// <summary>
        /// Gets the list of quick access items.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <returns>The quick access list.</returns>
        Task<IEnumerable<QuickAccessEntity>> GetQuickAccessListByUserIdAsync(string userId);

        /// <summary>
        /// Gets the quick access item by taxonomy Id for the given user Id.
        /// </summary>
        /// <param name="taxonomyId">The taxonomy Id.</param>
        /// <param name="userId">User Id.</param>
        /// <returns>The quick access item.</returns>
        Task<QuickAccessEntity> GetQuickAccessItemByTaxonomyIdAsync(string taxonomyId, string userId);
    }
}