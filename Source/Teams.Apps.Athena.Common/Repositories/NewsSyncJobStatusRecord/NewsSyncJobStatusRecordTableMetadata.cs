// <copyright file="NewsSyncJobStatusRecordTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// News sync status record data table names.
    /// </summary>
    public static class NewsSyncJobStatusRecordTableMetadata
    {
        /// <summary>
        /// Table name for the sync job status record table.
        /// </summary>
        public const string TableName = "NewsSyncJobStatusRecordEntity";

        /// <summary>
        /// News sync job status record data partition key name.
        /// </summary>
        public const string NewsSyncStatusRecordPartitionKey = "NewsSyncJob";
    }
}