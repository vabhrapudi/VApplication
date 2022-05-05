// <copyright file="ISponsorRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for Sponsor Data Repository.
    /// </summary>
    public interface ISponsorRepository : IRepository<SponsorEntity>
    {
        /// <summary>
        /// Gets the sponsor by Id.
        /// </summary>
        /// <param name="sponsorId">The Sponsor Id.</param>
        /// <returns>Returns the Sponsor entity.</returns>
        Task<SponsorEntity> GetSponsorByIdAsync(int sponsorId);
    }
}
