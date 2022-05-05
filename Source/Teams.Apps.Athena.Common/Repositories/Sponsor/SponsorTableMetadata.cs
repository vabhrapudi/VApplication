// <copyright file="SponsorTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Represents the metadata for sponsor details table.
    /// </summary>
    public static class SponsorTableMetadata
    {
        /// <summary>
        /// Table name for the sponsor data table.
        /// </summary>
        public const string TableName = "SponsorsEntity";

        /// <summary>
        /// Sponsors data partition key name.
        /// </summary>
        public const string SponsorPartitionKey = "Sponsor";
    }
}
