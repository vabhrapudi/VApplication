// <copyright file="ITeamService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Services.MicrosoftGraph
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Graph;

    /// <summary>
    /// Exposes the services related to Microsoft Teams team.
    /// </summary>
    public interface ITeamService
    {
        /// <summary>
        /// Creates a new Microsoft Teams team in standard teamplate.
        /// </summary>
        /// <param name="displayName">The Microsoft Teams team display name.</param>
        /// <param name="description">The description of Microsoft Teams team.</param>
        /// <param name="teamVisibilityType">The team visibility type.</param>
        /// <param name="teamOwnerUserAadId">The team owner's user AAD Id.</param>
        /// <returns>Asynchronous task operation returning newly created Microsoft Teams team details.</returns>
        Task<string> CreateTeamAsync(string displayName, string description, TeamVisibilityType teamVisibilityType, Guid teamOwnerUserAadId);

        /// <summary>
        /// Adds new member to team.
        /// </summary>
        /// <param name="teamId">Team Id of team to which member needs to be added.</param>
        /// <param name="userAadId">User AAD object Id which needs to be added to teams.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task AddMemberToTeamAsync(string teamId, Guid userAadId);

        /// <summary>
        /// Removes member from team.
        /// </summary>
        /// <param name="teamId">Team Id of team to which member needs to be removed.</param>
        /// <param name="userAadId">User AAD object Id which needs to be removed from teams.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task RemoveMemberFromTeamAsync(string teamId, Guid userAadId);

        /// <summary>
        /// Gets the primary channel Id of team.
        /// </summary>
        /// <param name="teamId">Team Id of team to which member needs to be removed.</param>
        /// <returns>Channel Id of team.</returns>
        Task<string> GetPrimaryChannelIdOfTeamAsync(string teamId);

        /// <summary>
        /// Gets the team owners.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>The collection of team owners.</returns>
        Task<IEnumerable<User>> GetTeamOwnersAsync(string teamId);

        /// <summary>
        /// Gets the team members.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>The collection of team members.</returns>
        Task<IEnumerable<AadUserConversationMember>> GetTeamMembersAsync(string teamId);
    }
}