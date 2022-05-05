// <copyright file="IResearchProjectHelper.cs" company="NPS Foundation">
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
    /// Provides helper methods for managing research project entity.
    /// </summary>
    public interface IResearchProjectHelper
    {
        /// <summary>
        /// Creates a research project entity.
        /// </summary>
        /// <param name="researchProjectCreateDTO">The research project create entity.</param>
        /// <returns>Returns research project entity.</returns>
        Task<ResearchProjectEntity> CreateResearchProjectAsync(ResearchProjectCreateDTO researchProjectCreateDTO);

        /// <summary>
        /// Gets a research project by table Id.
        /// </summary>
        /// <param name="researchProjectTableId">The research project table Id of the research project to fetch.</param>
        /// <param name="userAadObjectId">The user aad object Id.</param>
        /// <returns>Returns research project entity details.</returns>
        Task<ResearchProjectDTO> GetResearchProjectByIdAsync(string researchProjectTableId, string userAadObjectId);

        /// <summary>
        /// Gets the research projects by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The research project matching-in provided keywords.</returns>
        Task<IEnumerable<ResearchProjectDTO>> GetResearchProjectsByKeywordsAsync(IEnumerable<int> keywordIds);

        /// <summary>
        /// Stores rating of user for a research project.
        /// </summary>
        /// <param name="researchProjectTableId">The research project table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="userAadObjectId">User Id who submitted rating.</param>
        /// <returns>Returns task indicating operation result.</returns>
        Task RateResearchProjectAsync(string researchProjectTableId, int rating, string userAadObjectId);

        /// <summary>
        /// Gets the research project based on advanced search parameters.
        /// </summary>
        /// <param name="searchParametersDTO">The search parameters.</param>
        /// <returns>The research projects.</returns>
        Task<IEnumerable<ResearchProjectDTO>> GetResearchProjectsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Gets the research projects.
        /// </summary>
        /// <param name="keywords">The collection of keywords.</param>
        /// <param name="fromDate">The date and time from which research projects to get.</param>
        /// <param name="count">The number of research projects to get.</param>
        /// <returns>The collection of research projects.</returns>
        Task<IEnumerable<ResearchProjectDTO>> GetResearchProjectsAsync(IEnumerable<int> keywords, DateTime fromDate, int? count);
    }
}