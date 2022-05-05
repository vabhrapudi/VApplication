// <copyright file="IKeywordsHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing keywords entity.
    /// </summary>
    public interface IKeywordsHelper
    {
        /// <summary>
        /// Gets the keywords.
        /// </summary>
        /// <param name="searchQuery">The search query string.</param>
        /// <returns>Returns keywords.</returns>
        Task<IEnumerable<KeywordEntity>> GetKeywordsAsync(string searchQuery);

        /// <summary>
        /// Gets the keyword Ids of COI team.
        /// </summary>
        /// <param name="teamId">Team Id.</param>
        /// <returns>Returns keywords.</returns>
        Task<IEnumerable<int>> GetCoiTeamKeywordsAsync(string teamId);

        /// <summary>
        /// Get keywords as per keyword Ids.
        /// </summary>
        /// <param name="keywordIds">Collections of keyword Ids.</param>
        /// <returns>List of Keywords.</returns>
        Task<IEnumerable<KeywordEntity>> GetKeywordsByKeywordIdsAsync(IEnumerable<int> keywordIds);

        /// <summary>
        /// Gets the all keywords list.
        /// </summary>
        /// <returns>Returns keywords.</returns>
        Task<IEnumerable<KeywordDTO>> GetAllKeywordsAsync();
    }
}
