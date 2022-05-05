// <copyright file="IHomeStatusBarConfigurationHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes the helper methods related to home status bar configuration.
    /// </summary>
    public interface IHomeStatusBarConfigurationHelper
    {
        /// <summary>
        /// Creates the home status bar configuration.
        /// </summary>
        /// <param name="homeStatusBarConfigurationDTO">The home status bar configuration details to update.</param>
        /// <param name="teamId">The team Id.</param>
        /// <param name="userAadId">The logged-in user's AAD Id.</param>
        /// <returns>The created home status bar configuration details.</returns>
        Task<HomeStatusBarConfigurationDTO> CreateHomeStatusBarConfigurationAsync(HomeStatusBarConfigurationDTO homeStatusBarConfigurationDTO, string teamId, string userAadId);

        /// <summary>
        /// Updates the home status bar configuration.
        /// </summary>
        /// <param name="homeStatusBarConfigurationDTO">The home status bar configuration details to update.</param>
        /// <param name="teamId">The team Id.</param>
        /// <param name="userAadId">The logged-in user's AAD Id.</param>
        /// <returns>The updated home status bar configuration details.</returns>
        Task<HomeStatusBarConfigurationDTO> UpdateHomeStatusBarConfigurationAsync(HomeStatusBarConfigurationDTO homeStatusBarConfigurationDTO, string teamId, string userAadId);

        /// <summary>
        /// Gets the home status bar configuration details.
        /// </summary>
        /// <param name="teamId">The team Id oh which configuration details to get.</param>
        /// <returns>The home status bar configuration details.</returns>
        Task<HomeStatusBarConfigurationDTO> GetHomeStatusBarConfigurationAsync(string teamId);
    }
}
