// <copyright file="CommentsTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// The metadata of comments table.
    /// </summary>
    public static class CommentsTableMetadata
    {
        /// <summary>
        /// The comment table name where all comments get stored.
        /// </summary>
        public const string TableName = "CommentsEntity";

        /// <summary>
        /// The comment table partition key.
        /// </summary>
        public const string PartitionKey = "Comment";
    }
}