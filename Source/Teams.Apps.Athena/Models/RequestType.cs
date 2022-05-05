// <copyright file="RequestType.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    /// <summary>
    /// Specifies the API request status.
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// Indicates that request initiated.
        /// </summary>
        Initiated,

        /// <summary>
        /// Indicates that request succeeded.
        /// </summary>
        Succeeded,

        /// <summary>
        /// Indicates that request failed.
        /// </summary>
        Failed,
    }
}
