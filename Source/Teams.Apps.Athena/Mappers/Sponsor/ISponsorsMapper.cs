// <copyright file="ISponsorsMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to sponsors.
    /// </summary>
    public interface ISponsorsMapper
    {
        /// <summary>
        /// Maps sponsors entity model to sponsors view model.
        /// </summary>
        /// <param name="sponsorsEntity">The sponsors entity model.</param>
        /// <returns>The sponsors entity view model.</returns>
        SponsorDTO MapForViewModel(SponsorEntity sponsorsEntity);
    }
}
