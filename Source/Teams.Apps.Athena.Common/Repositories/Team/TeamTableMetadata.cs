// <copyright file="TeamTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Represents the metadata for team table.
    /// </summary>
    public static class TeamTableMetadata
    {
        /// <summary>
        /// Table name for the team data table.
        /// </summary>
        public const string TableName = "TeamEntity";

        /// <summary>
        /// Team data partition key name.
        /// </summary>
        public const string TeamPartitionKey = "Team";
    }
}