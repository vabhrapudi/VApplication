// <copyright file="Constants.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common
{
    /// <summary>
    /// Constant values that are used in multiple files.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Azure search service Index name for NewsEntity table.
        /// </summary>
        public const string NewsIndex = "news-index";

        /// <summary>
        /// Default rating value for News.
        /// </summary>
        public const int DefaultRating = 0;

        /// <summary>
        /// The OID claim type.
        /// </summary>
        public const string OidClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
    }
}
