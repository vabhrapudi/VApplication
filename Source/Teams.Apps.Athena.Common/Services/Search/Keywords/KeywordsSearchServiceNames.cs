// <copyright file="KeywordsSearchServiceNames.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Keywords
{
    /// <summary>
    /// FAQ data table names.
    /// </summary>
    public static class KeywordsSearchServiceNames
    {
        /// <summary>
        /// Index name for the keywords search service.
        /// </summary>
        public static readonly string IndexName = "keywords-index";

        /// <summary>
        /// Indexer name for the keywords search service.
        /// </summary>
        public static readonly string IndexerName = "keywords-indexer";

        /// <summary>
        /// Keywords search service data source name.
        /// </summary>
        public static readonly string DataSourceName = "keywords-storage";

        /// <summary>
        /// Keywords blob container name.
        /// </summary>
        public static readonly string ContainerName = "keywords";
    }
}
