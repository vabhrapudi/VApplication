// <copyright file="ICoiRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes table operations related to COI repository.
    /// </summary>
    public interface ICoiRepository : IRepository<CommunityOfInterestEntity>
    {
        /// <summary>
        /// Gets all COIs which are not deleted.
        /// </summary>
        /// <param name="userAadId">The user AAD Id of which COIs to get.</param>
        /// <returns>The collection of active COIs.</returns>
        Task<IEnumerable<CommunityOfInterestEntity>> GetActiveCoiRequestsAsync(Guid userAadId);

        /// <summary>
        /// Get active COI requests by Ids.
        /// </summary>
        /// <param name="coiRequestIds">The collection of COI request Ids to get.</param>
        /// <param name="userAadId">The user AAD Id of which requests to get.</param>
        /// <returns>The collection of COI requests.</returns>
        Task<IEnumerable<CommunityOfInterestEntity>> GetActiveCoiRequestsAsync(IEnumerable<string> coiRequestIds, Guid userAadId);

        /// <summary>
        /// Gets the COIs by name.
        /// </summary>
        /// <param name="name">The name of COI to get.</param>
        /// <returns>The collection of COIs.</returns>
        Task<IEnumerable<CommunityOfInterestEntity>> GetActiveCoiRequestsAsync(string name);

        /// <summary>
        /// Gets filter string of coi based on team id.
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns>The fiter condition.</returns>
        string GetCoiFilterAsync(string teamId);

        /// <summary>
        /// Gets the COI by Id
        /// </summary>
        /// <param name="coiId"> COI Id</param>
        /// <returns>The COI record</returns>
        Task<CommunityOfInterestEntity> GetCommunityByIdAsync(int coiId);
    }
}