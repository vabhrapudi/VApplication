// <copyright file="IHomeConfigurationHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing home tab configuration.
    /// </summary>
    public interface IHomeConfigurationHelper
    {
        /// <summary>
        /// Gets home configuration article details by article Id.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="articleId">The home tab configuration article Id.</param>
        /// <returns>Returns home tab configuration details.</returns>
        Task<HomeConfigurationArticleDTO> GetHomeConfigurationByArticleIdAsync(string teamId, string articleId);

        /// <summary>
        /// Creates new home configuration article.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="homeConfigurationArticleDTO">The home configuration article DTO model.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <returns>Returns home tab configuration details.</returns>
        Task<HomeConfigurationArticleDTO> CreateHomeConfigurationArticleAsync(string teamId, HomeConfigurationArticleDTO homeConfigurationArticleDTO, string userAadId);

        /// <summary>
        /// Updates home configuration article.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="homeConfigurationArticleDTO">The home configuration article view DTO model.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <returns>Returns home tab configuration details.</returns>
        Task<HomeConfigurationArticleDTO> UpdateHomeConfigurationArticleAsync(string teamId, HomeConfigurationArticleDTO homeConfigurationArticleDTO, string userAadId);

        /// <summary>
        /// Gets the home configuration articles configured for a team.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>The collection of home configuration articles.</returns>
        Task<IEnumerable<HomeConfigurationArticleDTO>> GetHomeConfigurationArticlesAsync(Guid teamId);

        /// <summary>
        /// Deletes the home configuration articles of a team.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="articleIds">The article Ids.</param>
        /// <returns>A task representing delete articles operation.</returns>
        Task DeleteHomeConfigurationArticlesAsync(Guid teamId, IEnumerable<Guid> articleIds);
    }
}
