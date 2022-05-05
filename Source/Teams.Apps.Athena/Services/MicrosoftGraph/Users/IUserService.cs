// <copyright file="IUserService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Services.MicrosoftGraph
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Graph;

    /// <summary>
    /// Exposes methods related Microsoft Graph.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets the details of Azure active directory users by making Graph call if user details does not available in memory cache.
        /// </summary>
        /// <param name="userAADIds">The AAD Ids of users.</param>
        /// <returns>An asynchronous task representing get users operation.</returns>
        Task<IEnumerable<User>> GetUsersAsync(IEnumerable<string> userAADIds);

        /// <summary>
        /// Gets the base64 string of AAD user's profile photo.
        /// </summary>
        /// <param name="userAADId">The AAD user Id.</param>
        /// <returns>A asynchronous task representing get user's profile photo operation.</returns>
        Task<string> GetUserProfilePhotoAsync(string userAADId);
    }
}