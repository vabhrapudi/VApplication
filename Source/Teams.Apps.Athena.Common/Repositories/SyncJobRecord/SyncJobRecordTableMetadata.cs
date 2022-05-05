// <copyright file="SyncJobRecordTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Sync record data table names.
    /// </summary>
    public static class SyncJobRecordTableMetadata
    {
        /// <summary>
        /// Table name for the sync job record table.
        /// </summary>
        public const string TableName = "SyncJobRecordEntity";

        /// <summary>
        /// Sync job record  data partition key name.
        /// </summary>
        public const string SyncRecordPartitionKey = "SyncJobRecord";
    }
}
