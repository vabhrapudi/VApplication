// <copyright file="UserTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Represents the metadata for user details table.
    /// </summary>
    public static class UserTableMetadata
    {
        /// <summary>
        /// Table name for the users data table.
        /// </summary>
        public const string TableName = "UserEntity";

        /// <summary>
        /// Users data partition key name.
        /// </summary>
        public const string UserPartitionKey = "User";
    }
}