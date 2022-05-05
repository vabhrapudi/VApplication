// <copyright file="SponsorsSearchServiceMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    /// <summary>
    /// The metadata for sponsors search service.
    /// </summary>
    public static class SponsorsSearchServiceMetadata
    {
        /// <summary>
        /// Index name for the sponsors search service.
        /// </summary>
        public const string IndexName = "sponsors-index";

        /// <summary>
        /// Indexer name for the sponsors search service.
        /// </summary>
        public const string IndexerName = "sponsors-indexer";

        /// <summary>
        /// Sponsors search service data source name.
        /// </summary>
        public const string DataSourceName = "sponsors-storage";
    }
}
