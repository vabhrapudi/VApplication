// <copyright file="AthenaEventsSearchServiceMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    /// <summary>
    /// The metadata for Athena events search service.
    /// </summary>
    public static class AthenaEventsSearchServiceMetadata
    {
        /// <summary>
        /// Index name for the Athena events search service.
        /// </summary>
        public const string IndexName = "athena-events-index";

        /// <summary>
        /// Indexer name for Athena events search service.
        /// </summary>
        public const string IndexerName = "athena-events-indexer";

        /// <summary>
        /// Athena events search service data source name.
        /// </summary>
        public const string DataSourceName = "athena-events-storage";
    }
}
