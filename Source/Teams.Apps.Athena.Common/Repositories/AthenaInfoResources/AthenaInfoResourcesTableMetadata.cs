// <copyright file="AthenaInfoResourcesTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Describes the info resources entity metadata.
    /// </summary>
    public static class AthenaInfoResourcesTableMetadata
    {
        /// <summary>
        /// The table name to store info resources details.
        /// </summary>
        public const string TableName = "InfoResourcesEntity";

        /// <summary>
        /// The info resources table partition key name.
        /// </summary>
        public const string PartitionKey = "InfoResource";
    }
}
