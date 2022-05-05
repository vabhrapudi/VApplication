// <copyright file="UserDetails.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    /// <summary>
    /// Represents an user's details.
    /// </summary>
    public class UserDetails
    {
        /// <summary>
        /// Gets or sets the user AAD Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the given name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the mail Id.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Gets or sets the mobile phone number.
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// Gets or sets the UPN.
        /// </summary>
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// Gets or sets the profile image of user.
        /// </summary>
        public string ProfileImage { get; set; }
    }
}
