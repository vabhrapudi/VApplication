// <copyright file="IAthenaToolMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to Athena tools.
    /// </summary>
    public interface IAthenaToolMapper
    {
        /// <summary>
        /// Maps Athena tool entity model to Athena tool view model.
        /// </summary>
        /// <param name="athenaToolEntity"> The Athena tool entity.</param>
        /// <returns>Returns the Athena tool view model.</returns>
        AthenaToolDTO MapForViewModel(AthenaToolEntity athenaToolEntity);
    }
}
