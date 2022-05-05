// <copyright file="ResearchProjectsSearchServiceMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    /// <summary>
    /// The metadata for research projects search service.
    /// </summary>
    public static class ResearchProjectsSearchServiceMetadata
    {
        /// <summary>
        /// Index name for the research projects search service.
        /// </summary>
        public const string IndexName = "research-projects-index";

        /// <summary>
        /// Indexer name for the research projects search service.
        /// </summary>
        public const string IndexerName = "research-projects-indexer";

        /// <summary>
        /// Research projects search service data source name.
        /// </summary>
        public const string DataSourceName = "research-projects-storage";
    }
}
