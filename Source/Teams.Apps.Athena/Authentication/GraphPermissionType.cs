// <copyright file="GraphPermissionType.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authentication
{
    /// <summary>
    /// Microsoft Graph permission types.
    /// </summary>
    public enum GraphPermissionType
    {
        /// <summary>
        /// This represents application permission of Microsoft Graph.
        /// </summary>
        Application,

        /// <summary>
        /// This represents delegate permission of Microsoft Graph.
        /// </summary>
        UserDelegated,
    }
}