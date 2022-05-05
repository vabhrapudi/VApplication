// <copyright file="ResearchProposalsSearchServiceMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    /// <summary>
    /// The metadata for research proposals search service.
    /// </summary>
    public static class ResearchProposalsSearchServiceMetadata
    {
        /// <summary>
        /// Index name for the research proposals search service.
        /// </summary>
        public const string IndexName = "research-proposals-index";

        /// <summary>
        /// Indexer name for the research proposals search service.
        /// </summary>
        public const string IndexerName = "research-proposals-indexer";

        /// <summary>
        /// Research proposals search service data source name.
        /// </summary>
        public const string DataSourceName = "research-proposals-storage";
    }
}
