// <copyright file="CoiTeamType.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Represents the COI team types.
    /// </summary>
    public enum CoiTeamType
    {
        /// <summary>
        /// No team type specified.
        /// </summary>
        None,

        /// <summary>
        /// Anyone can see the team, but only the owner can add users to the team.
        /// </summary>
        Private,

        /// <summary>
        /// Anyone can join the team.
        /// </summary>
        Public,
    }
}
