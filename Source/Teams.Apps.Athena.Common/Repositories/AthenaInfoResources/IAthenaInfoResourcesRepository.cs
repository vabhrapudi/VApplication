// <copyright file="IAthenaInfoResourcesRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes repository operations related to Athena info resources.
    /// </summary>
    public interface IAthenaInfoResourcesRepository : IRepository<AthenaInfoResourceEntity>
    {
        /// <summary>
        /// Gets Athena info resource by InfoResourceId.
        /// </summary>
        /// <param name="infoResourceId">InfoResourceId.</param>
        /// <returns>Returns Athena info resource record if exists</returns>
        Task<AthenaInfoResourceEntity> GetInfoResourceByResourceIdAsync(int infoResourceId);
    }
}
