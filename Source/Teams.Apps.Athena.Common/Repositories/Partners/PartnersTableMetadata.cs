// <copyright file="PartnersTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Describes the partners entity metadata.
    /// </summary>
    public static class PartnersTableMetadata
    {
        /// <summary>
        /// The table name to store partners details.
        /// </summary>
        public const string TableName = "PartnersEntity";

        /// <summary>
        /// The partners table partition key name.
        /// </summary>
        public const string PartitionKey = "Partners";
    }
}
