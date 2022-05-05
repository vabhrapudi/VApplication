// <copyright file="MustBeUserPolicyRequirement.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authorization.Policies
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// This authorization class implements marker interface <see cref="IAuthorizationRequirement"/>
    /// to check whether a user is valid end-user who installed end-user APP successfully.
    /// </summary>
    public class MustBeUserPolicyRequirement : IAuthorizationRequirement
    {
    }
}
