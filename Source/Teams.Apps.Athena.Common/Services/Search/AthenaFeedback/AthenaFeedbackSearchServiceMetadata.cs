// <copyright file="AthenaFeedbackSearchServiceMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    /// <summary>
    /// The metadata for Athena feedback search service.
    /// </summary>
    public static class AthenaFeedbackSearchServiceMetadata
    {
        /// <summary>
        /// Index name for the Athena feedback search service.
        /// </summary>
        public const string IndexName = "athena-feedback-index";

        /// <summary>
        /// Indexer name for Athena feedback search service.
        /// </summary>
        public const string IndexerName = "athena-feedback-indexer";

        /// <summary>
        /// Athena feedback search service data source name.
        /// </summary>
        public const string DataSourceName = "athena-feedback-storage";
    }
}