// <copyright file="IAthenaEventMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes model mapper methods related to Athena events.
    /// </summary>
    public interface IAthenaEventMapper
    {
        /// <summary>
        /// Maps Athena event entity model to view model.
        /// </summary>
        /// <param name="eventEntity">The Athena event entity model.</param>
        /// <returns>The Athena entity view model.</returns>
        public EventDTO MapForViewModel(EventEntity eventEntity);
    }
}
