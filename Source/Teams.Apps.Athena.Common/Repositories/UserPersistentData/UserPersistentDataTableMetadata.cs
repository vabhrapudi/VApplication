// <copyright file="UserPersistentDataTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Represents the metadata for user persistent data table.
    /// </summary>
    public static class UserPersistentDataTableMetadata
    {
        /// <summary>
        /// Table name for the user persistent data table.
        /// </summary>
        public const string TableName = "UserPersistentDataEntity";

        /// <summary>
        /// User persistent data partition key name.
        /// </summary>
        public const string PartitionKey = "UserPersistentData";
    }
}
