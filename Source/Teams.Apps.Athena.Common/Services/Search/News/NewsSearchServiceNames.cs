// <copyright file="NewsSearchServiceNames.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search.News
{
    /// <summary>
    /// News data table names.
    /// </summary>
    public static class NewsSearchServiceNames
    {
        /// <summary>
        /// Index name for the News search service.
        /// </summary>
        public static readonly string IndexName = "news-index";

        /// <summary>
        /// Indexer name for the News search service.
        /// </summary>
        public static readonly string IndexerName = "news-indexer";

        /// <summary>
        /// News search service data source name.
        /// </summary>
        public static readonly string DataSourceName = "news-storage";
    }
}