// <copyright file="IResearchRequestHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing research request entity.
    /// </summary>
    public interface IResearchRequestHelper
    {
        /// <summary>
        /// Gets the research requests by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The collection of <see cref="ResearchRequestViewDTO"/>.</returns>
        Task<IEnumerable<ResearchRequestViewDTO>> GetResearchRequestsByKeywordsAsync(IEnumerable<int> keywordIds);

        /// <summary>
        /// Gets the research requests based on advanced search parameters.
        /// </summary>
        /// <param name="searchParametersDTO">The search parameters.</param>
        /// <returns>The research requests.</returns>
        Task<IEnumerable<ResearchRequestViewDTO>> GetResearchRequestsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Gets a research request by table Id.
        /// </summary>
        /// <param name="researchRequestTableId">The research request table Id of the research project to fetch.</param>
        /// <param name="userAadObjectId">The user aad object Id.</param>
        /// <returns>Returns research request entity details.</returns>
        Task<ResearchRequestViewDTO> GetResearchRequestByTableIdAsync(string researchRequestTableId, string userAadObjectId);

        /// <summary>
        /// Stores rating of user for a research request.
        /// </summary>
        /// <param name="researchRequestTableId">The research request table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="userAadObjectId">User Id who submitted rating.</param>
        /// <returns>Returns task indicating operation result.</returns>
        Task RateResearchRequestAsync(string researchRequestTableId, int rating, string userAadObjectId);

        /// <summary>
        /// Gets the research requests.
        /// </summary>
        /// <param name="keywords">The collection of keywords.</param>
        /// <param name="fromDate">The date and time from which research requests to get.</param>
        /// <param name="count">The number of research requests to get.</param>
        /// <returns>The collection of research requests.</returns>
        Task<IEnumerable<ResearchRequestViewDTO>> GetResearchRequestsAsync(IEnumerable<int> keywords, DateTime fromDate, int? count);
    }
}
