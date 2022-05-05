// <copyright file="IUserGraphServiceHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes helper methods related to user graph service.
    /// </summary>
    public interface IUserGraphServiceHelper
    {
        /// <summary>
        /// Gets the user details.
        /// </summary>
        /// <param name="userAADIds">The collection of AAD Ids of users.</param>
        /// <returns>A task representing get users operation.</returns>
        Task<IEnumerable<UserDetails>> GetUsersAsync(IEnumerable<string> userAADIds);
    }
}