// <copyright file="CoiTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// The metadata of community of interest (COI) table.
    /// </summary>
    public static class CoiTableMetadata
    {
        /// <summary>
        /// The COI table name where all COI details get stored.
        /// </summary>
        public const string TableName = "CommunityOfInterest";

        /// <summary>
        /// The COI table partition key.
        /// </summary>
        public const string PartitionKey = "COI";
    }
}
