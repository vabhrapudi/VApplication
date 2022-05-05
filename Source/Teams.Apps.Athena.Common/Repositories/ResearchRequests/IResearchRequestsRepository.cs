// <copyright file="IResearchRequestsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes repository operations related to research projects.
    /// </summary>
    public interface IResearchRequestsRepository : IRepository<ResearchRequestEntity>
    {
        /// <summary>
        /// Gets user details by researchRequest Id.
        /// </summary>
        /// <param name="researchRequestId">ResearchRequest Id.</param>
        /// <returns>Returns ResearchRequest details.</returns>
        Task<ResearchRequestEntity> GetResearchRequestDetailsByIdAsync(int researchRequestId);
    }
}
