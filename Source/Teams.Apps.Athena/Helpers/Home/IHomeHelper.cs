// <copyright file="IHomeHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes the helper methods related to home tab.
    /// </summary>
    public interface IHomeHelper
    {
        /// <summary>
        /// Gets the active home status bar details configured for the team.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>The home status bar details.</returns>
        Task<HomeStatusBarConfigurationDTO> GetActiveHomeStatusBarDetailsAsync(Guid teamId);

        /// <summary>
        /// Gets the active home status bar details configured for Athena central team.
        /// </summary>
        /// <returns>The home status bar details.</returns>
        Task<HomeStatusBarConfigurationDTO> GetActiveHomeStatusBarDetailsForCentralTeamAsync();

        /// <summary>
        /// Gets the articles configured for 'New to Athena' section for team.
        /// </summary>
        /// <param name="teamId">The team Id of which articles to get.</param>
        /// <returns>The collection of home articles.</returns>
        Task<IEnumerable<HomeConfigurationArticleDTO>> GetNewToAthenaArticlesAsync(Guid teamId);

        /// <summary>
        /// Gets the articles configured for 'New to Athena' section for Athena central team.
        /// </summary>
        /// <returns>The collection of home articles.</returns>
        Task<IEnumerable<HomeConfigurationArticleDTO>> GetNewToAthenaArticlesForCentralTeamAsync();

        /// <summary>
        /// Gets the user's daily briefing articles of rolling last 7 days for Athena Central team.
        /// </summary>
        /// <param name="userAadId">The logged-in user's AAD Id.</param>
        /// <returns>The user's daily briefing articles.</returns>
        Task<IEnumerable<DailyBriefingHomeArticleDTO>> GetDailyBriefingArticlesOfUserForCentralTeamAsync(string userAadId);

        /// <summary>
        /// Gets the user's daily briefing articles of rolling last 7 days for COI team.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="userAadId">The logged-in user's AAD Id.</param>
        /// <returns>The user's daily briefing articles.</returns>
        Task<IEnumerable<DailyBriefingHomeArticleDTO>> GetDailyBriefingArticlesOfUserForCoiTeamAsync(Guid teamId, string userAadId);
    }
}
