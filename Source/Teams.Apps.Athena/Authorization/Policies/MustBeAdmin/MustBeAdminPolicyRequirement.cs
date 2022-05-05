// <copyright file="MustBeAdminPolicyRequirement.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authorization.Policies
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// This authorization class implements marker interface <see cref="IAuthorizationRequirement"/>
    /// to check whether an user is admin.
    /// </summary>
    public class MustBeAdminPolicyRequirement : IAuthorizationRequirement
    {
    }
}