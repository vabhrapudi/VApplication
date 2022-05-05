// <copyright file="MustBeTeamOwnerPolicyRequirement.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authorization.Policies
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// This authorization class implements marker interface <see cref="IAuthorizationRequirement"/>
    /// to check whether a user is team owner.
    /// </summary>
    public class MustBeTeamOwnerPolicyRequirement : IAuthorizationRequirement
    {
    }
}
