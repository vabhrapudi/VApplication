// <copyright file="IAthenaToolsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes repository operations related to Athena tools.
    /// </summary>
    public interface IAthenaToolsRepository : IRepository<AthenaToolEntity>
    {
        /// <summary>
        /// Gets AthenaTools by Id.
        /// </summary>
        /// <param name="toolId">Tool Id.</param>
        /// <returns>Returns Athena Tools record if exists</returns>
        Task<AthenaToolEntity> GetAthenaToolByIdAsync(int toolId);
    }
}
