// <copyright file="AthenaToolsTablemetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Describes the Athena tools table meta data.
    /// </summary>
    public static class AthenaToolsTableMetadata
    {
        /// <summary>
        /// The table name to store Athena tools details.
        /// </summary>
        public const string TableName = "AthenaToolsEntity";

        /// <summary>
        /// The Athena tools table partition key name.
        /// </summary>
        public const string PartitionKey = "AthenaTool";
    }
}
