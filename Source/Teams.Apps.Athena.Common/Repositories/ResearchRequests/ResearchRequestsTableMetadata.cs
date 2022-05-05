// <copyright file="ResearchRequestsTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Describes the research requests entity metadata.
    /// </summary>
    public static class ResearchRequestsTableMetadata
    {
        /// <summary>
        /// The table name to store research project details.
        /// </summary>
        public const string TableName = "ResearchRequestsEntity";

        /// <summary>
        /// The research projects table partition key name.
        /// </summary>
        public const string PartitionKey = "ResearchRequests";
    }
}
