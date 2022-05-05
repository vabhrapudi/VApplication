// <copyright file="IResearchProjectsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes repository operations related to research projects.
    /// </summary>
    public interface IResearchProjectsRepository : IRepository<ResearchProjectEntity>
    {
        /// <summary>
        /// Validates whether a research project with same title already exists.
        /// </summary>
        /// <param name="title">The research project title.</param>
        /// <returns>Returns true if the research project with same title already exists. Else returns false.</returns>
        Task<ResearchProjectEntity> GetMatchingResearchProjectAsync(string title);

        /// <summary>
        /// Gets ResearchProject by ResearchProjectId.
        /// </summary>
        /// <param name="researchProjectId">The research project Id.</param>
        /// <returns>Returns  ResearchProject record if exists </returns>
        Task<ResearchProjectEntity> GetResearchProjectByProjectIdAsync(int researchProjectId);
    }
}
