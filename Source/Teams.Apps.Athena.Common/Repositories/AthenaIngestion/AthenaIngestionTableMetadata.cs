// <copyright file="AthenaIngestionTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Describes the Athena ingestion entity metadata.
    /// </summary>
    public static class AthenaIngestionTableMetadata
    {
        /// <summary>
        /// The table name to store Athena ingestion entity details.
        /// </summary>
        public const string TableName = "AthenaIngestionEntity";

        /// <summary>
        /// The Athena ingestion table partition key name.
        /// </summary>
        public const string PartitionKey = "AthenaIngestion";
    }
}
