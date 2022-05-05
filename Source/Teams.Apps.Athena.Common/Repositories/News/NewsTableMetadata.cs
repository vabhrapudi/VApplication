// <copyright file="NewsTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// News data table names.
    /// </summary>
    public static class NewsTableMetadata
    {
        /// <summary>
        /// Table name for the news data table.
        /// </summary>
        public const string TableName = "NewsEntity";

        /// <summary>
        /// News data partition key name.
        /// </summary>
        public const string NewsPartitionKey = "News";
    }
}