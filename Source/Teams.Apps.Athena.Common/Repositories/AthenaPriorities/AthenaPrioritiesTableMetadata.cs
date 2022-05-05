// <copyright file="AthenaPrioritiesTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Describes the Athena priorities table metadata.
    /// </summary>
    public static class AthenaPrioritiesTableMetadata
    {
        /// <summary>
        /// The table name to store Athena priorities details.
        /// </summary>
        public const string TableName = "AthenaPrioritiesEntity";

        /// <summary>
        /// The Athena priorities table partition key name.
        /// </summary>
        public const string PartitionKey = "AthenaPriority";
    }
}
