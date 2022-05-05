// <copyright file="IUserGraphServiceMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Microsoft.Graph;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to user graph service.
    /// </summary>
    public interface IUserGraphServiceMapper
    {
        /// <summary>
        /// Maps a graph user model to view model.
        /// </summary>
        /// <param name="user">The graph user details.</param>
        /// <returns>The view model.</returns>
        UserDetails MapToViewModel(User user);

        /// <summary>
        /// Maps a user entity model to user details view model.
        /// </summary>
        /// <param name="user">The user entity.</param>
        /// <param name="profilePhoto">The profile photo.</param>
        /// <returns>The view model.</returns>
        UserDetails MapToUserDetailsViewModel(UserEntity user, string profilePhoto);
    }
}
