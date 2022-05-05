// <copyright file="IUserPersistentDataHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes methods related to saving user persistent data.
    /// </summary>
    public interface IUserPersistentDataHelper
    {
        /// <summary>
        /// Gets the user persistent data.
        /// </summary>
        /// <param name="userId">The user AAD Id of logged in user.</param>
        /// <returns>Returns user persistent data.</returns>
        Task<UserPersistentDataDTO> GetUserPersistentDataAsync(string userId);

        /// <summary>
        /// Saves the user persistent data for discovery tree.
        /// </summary>
        /// <param name="discoveryTreePersistentData">The discovery tree persistent data.</param>
        /// <param name="userId">The user Id of logged in user.</param>
        /// <returns>Returns user persistent data.</returns>
        Task<UserPersistentDataDTO> SaveDiscoveryTreeUserPersistentDataAsync(DiscoveryTreePersistentData discoveryTreePersistentData, string userId);
    }
}
