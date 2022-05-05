// <copyright file="IResearchProposalHelper.cs" company="NPS Foundation">
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
    /// Provides helper methods for managing research proposal entity.
    /// </summary>
    public interface IResearchProposalHelper
    {
        /// <summary>
        /// Gets the research proposals by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The collection of <see cref="ResearchProposalViewDTO"/>.</returns>
        Task<IEnumerable<ResearchProposalViewDTO>> GetResearchProposalsByKeywordsAsync(IEnumerable<int> keywordIds);

        /// <summary>
        /// Gets research proposals.
        /// </summary>
        /// <param name="searchParametersDTO">The advanced search parameters.</param>
        /// <returns>The research proposals.</returns>
        Task<IEnumerable<ResearchProposalViewDTO>> GetResearchProposalsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Creates a research proposal entity.
        /// </summary>
        /// <param name="researchProposalCreateDTO">The research proposal create entity.</param>
        /// <param name="userAadId">The user add Id.</param>
        /// <returns>Returns research proposal entity.</returns>
        Task<ResearchProposalEntity> CreateResearchProposalAsync(ResearchProposalCreateDTO researchProposalCreateDTO, string userAadId);

        /// <summary>
        /// Gets a research proposal by table Id.
        /// </summary>
        /// <param name="researchProposalTableId">The research proposal table Id of the research project to fetch.</param>
        /// <param name="userAadObjectId">The user aad object Id.</param>
        /// <returns>Returns research proposal entity details.</returns>
        Task<ResearchProposalViewDTO> GetResearchProposalByTableIdAsync(string researchProposalTableId, string userAadObjectId);

        /// <summary>
        /// Stores rating of user for a research proposal.
        /// </summary>
        /// <param name="researchProposalTableId">The research proposal table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="userAadObjectId">User Id who submitted rating.</param>
        /// <returns>Returns task indicating operation result.</returns>
        Task RateResearchProposalAsync(string researchProposalTableId, int rating, string userAadObjectId);

        /// <summary>
        /// Gets the research proposals.
        /// </summary>
        /// <param name="keywords">The collection of keywords.</param>
        /// <param name="fromDate">The date and time from which research proposals to get.</param>
        /// <param name="count">The number of research proposals to get.</param>
        /// <returns>The collection of research proposals.</returns>
        Task<IEnumerable<ResearchProposalViewDTO>> GetResearchProposalsAsync(IEnumerable<int> keywords, DateTime fromDate, int? count);
    }
}