// <copyright file="IHomeStatusBarConfigurationMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to home status bar configuration.
    /// </summary>
    public interface IHomeStatusBarConfigurationMapper
    {
        /// <summary>
        /// Maps the home status bar configuration entity model to view model.
        /// </summary>
        /// <param name="homeStatusBarConfigurationEntity">The home status bar configuration entity model.</param>
        /// <returns>The home status bar configuration view model.</returns>
        HomeStatusBarConfigurationDTO MapForViewModel(HomeStatusBarConfigurationEntity homeStatusBarConfigurationEntity);

        /// <summary>
        /// Maps the home status bar configuration view model to entity model.
        /// </summary>
        /// <param name="homeStatusBarConfigurationDTO">The home status bar configuration view model.</param>
        /// <param name="teamId">The team Id.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <returns>The home status bar configuration entity model.</returns>
        HomeStatusBarConfigurationEntity MapForCreateModel(HomeStatusBarConfigurationDTO homeStatusBarConfigurationDTO, string teamId, string userAadId);

        /// <summary>
        /// Maps the home status bar configuration view model to entity model.
        /// </summary>
        /// <param name="homeStatusBarConfigurationDTO">The home status bar configuration view model.</param>
        /// <param name="homeStatusBarConfigurationEntity">The home status bar configuration entity model.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        void MapForUpdateModel(HomeStatusBarConfigurationDTO homeStatusBarConfigurationDTO, HomeStatusBarConfigurationEntity homeStatusBarConfigurationEntity, string userAadId);
    }
}
