// <copyright file="UsersSearchServiceMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    /// <summary>
    /// The metadata for users search service.
    /// </summary>
    public static class UsersSearchServiceMetadata
    {
        /// <summary>
        /// Index name for the users search service.
        /// </summary>
        public const string IndexName = "users-index";

        /// <summary>
        /// Indexer name for the users search service.
        /// </summary>
        public const string IndexerName = "users-indexer";

        /// <summary>
        /// Users search service data source name.
        /// </summary>
        public const string DataSourceName = "users-storage";
    }
}
