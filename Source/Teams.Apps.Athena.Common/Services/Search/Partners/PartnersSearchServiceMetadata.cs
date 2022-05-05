// <copyright file="PartnersSearchServiceMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    /// <summary>
    /// The metadata for Athena partners search service.
    /// </summary>
    public static class PartnersSearchServiceMetadata
    {
        /// <summary>
        /// Index name for the Athena partners search service.
        /// </summary>
        public const string IndexName = "partners-index";

        /// <summary>
        /// Indexer name for Athena partners search service.
        /// </summary>
        public const string IndexerName = "partners-indexer";

        /// <summary>
        /// Athena partners search service data source name.
        /// </summary>
        public const string DataSourceName = "partners-storage";
    }
}
