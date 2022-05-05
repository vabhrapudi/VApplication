// <copyright file="IPartnerMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to research requests.
    /// </summary>
    public interface IPartnerMapper
    {
        /// <summary>
        /// Maps partners entity model to partners view model.
        /// </summary>
        /// <param name="partnerEntityModel">The partners entity model.</param>
        /// <returns>The news entity view model.</returns>
        PartnerDTO MapForViewModel(PartnerEntity partnerEntityModel);
    }
}
