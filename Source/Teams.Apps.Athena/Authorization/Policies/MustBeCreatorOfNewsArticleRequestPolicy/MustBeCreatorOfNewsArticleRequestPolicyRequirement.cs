// <copyright file="MustBeCreatorOfNewsArticleRequestPolicyRequirement.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authorization.Policies
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// This authorization class implements marker interface <see cref="IAuthorizationRequirement"/>
    /// to check whether a user is creator of news article request that is to be accessed.
    /// </summary>
    public class MustBeCreatorOfNewsArticleRequestPolicyRequirement : IAuthorizationRequirement
    {
    }
}
