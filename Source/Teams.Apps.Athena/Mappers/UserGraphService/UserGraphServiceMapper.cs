// <copyright file="UserGraphServiceMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Microsoft.Graph;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The mapper methods for user graph service.
    /// </summary>
    public class UserGraphServiceMapper : IUserGraphServiceMapper
    {
        /// <inheritdoc/>
        public UserDetails MapToViewModel(User user)
        {
            user = user ?? throw new ArgumentNullException(nameof(user));

            return new UserDetails
            {
                Id = user.Id,
                FirstName = user.GivenName,
                Surname = user.Surname,
                DisplayName = user.DisplayName,
                Mail = user.Mail,
                MobilePhone = user.MobilePhone,
                UserPrincipalName = user.UserPrincipalName,
            };
        }

        /// <inheritdoc/>
        public UserDetails MapToUserDetailsViewModel(UserEntity user, string profilePhoto)
        {
            user = user ?? throw new ArgumentNullException(nameof(user));

            return new UserDetails
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                Surname = user.LastName,
                DisplayName = user.UserDisplayName,
                Mail = user.EmailAddress,
                ProfileImage = profilePhoto,
            };
        }
    }
}
