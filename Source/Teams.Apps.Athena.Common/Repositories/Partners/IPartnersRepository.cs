// <copyright file="IPartnersRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes repository operations related to partners.
    /// </summary>
    public interface IPartnersRepository : IRepository<PartnerEntity>
    {
        /// <summary>
        /// Gets Partner details by partner Id.
        /// </summary>
        /// <param name="partnerId">Partner Id.</param>
        /// <returns>Returns Partner details.</returns>
        Task<PartnerEntity> GetPartnerDetailsByPartnerIdAsync(int partnerId);
    }
}
