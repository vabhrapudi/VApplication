// <copyright file="MustBeCollectionsCreatorRequirement.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authorization.Policies
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// This authorization class implements the marker interface
    /// <see cref="IAuthorizationRequirement"/> to check if user has created collections requirement
    /// for accessing resources.
    /// </summary>
    public class MustBeCollectionsCreatorRequirement : IAuthorizationRequirement
    {
    }
}
