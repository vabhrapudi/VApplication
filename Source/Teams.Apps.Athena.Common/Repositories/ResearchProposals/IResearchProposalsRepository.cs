// <copyright file="IResearchProposalsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes repository methods for database operations related to research proposal.
    /// </summary>
    public interface IResearchProposalsRepository : IRepository<ResearchProposalEntity>
    {
        /// <summary>
        /// Gets the research proposal by title.
        /// </summary>
        /// <param name="title">The research proposal title.</param>
        /// <returns>Returns the research proposal entity.</returns>
        Task<ResearchProposalEntity> GetResearchProposalAsync(string title);

        /// <summary>
        /// Gets the research proposal by Id.
        /// </summary>
        /// <param name="researchProposalId">The research proposal Id.</param>
        /// <returns>Returns the research proposal entity.</returns>
        Task<ResearchProposalEntity> GetResearchProposalByIdAsync(int researchProposalId);
    }
}
