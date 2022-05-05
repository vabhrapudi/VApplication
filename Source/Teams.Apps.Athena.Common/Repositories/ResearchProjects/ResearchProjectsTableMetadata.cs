// <copyright file="ResearchProjectsTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Describes the research projects entity metadata.
    /// </summary>
    public static class ResearchProjectsTableMetadata
    {
        /// <summary>
        /// The table name to store research project details.
        /// </summary>
        public const string TableName = "ResearchProjectsEntity";

        /// <summary>
        /// The research projects table partition key name.
        /// </summary>
        public const string PartitionKey = "ResearchProject";
    }
}
