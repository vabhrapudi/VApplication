// <copyright file="IAthenaInfoResourceMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to  Athena info resource.
    /// </summary>
    public interface IAthenaInfoResourceMapper
    {
        /// <summary>
        /// Maps Athena Info resource entity model to Athena Info resource view model.
        /// </summary>
        /// <param name="athenaInfoResourceEntity">The Athena Info resource entity model.</param>
        /// <returns>The Athena Info resource entity view model.</returns>
        AthenaInfoResourceDTO MapForViewModel(AthenaInfoResourceEntity athenaInfoResourceEntity);
    }
}
