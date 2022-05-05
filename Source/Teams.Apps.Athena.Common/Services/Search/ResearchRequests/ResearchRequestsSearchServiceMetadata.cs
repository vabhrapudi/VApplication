// <copyright file="ResearchRequestsSearchServiceMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    /// <summary>
    /// The metadata for research requests search service.
    /// </summary>
    public static class ResearchRequestsSearchServiceMetadata
    {
        /// <summary>
        /// Index name for the research requests search service.
        /// </summary>
        public const string IndexName = "research-requests-index";

        /// <summary>
        /// Indexer name for the research requests search service.
        /// </summary>
        public const string IndexerName = "research-requests-indexer";

        /// <summary>
        /// Research requests search service data source name.
        /// </summary>
        public const string DataSourceName = "research-requests-storage";
    }
}
