// <copyright file="ActivityTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Represents the activity table metadata.
    /// </summary>
    public static class ActivityTableMetadata
    {
        /// <summary>
        /// The table name for activity table.
        /// </summary>
        public const string TableName = "ActivityEntity";

        /// <summary>
        /// Gets or sets the default partition key.
        /// </summary>
        public const string DefaultPartitionKey = "Activity";
    }
}
