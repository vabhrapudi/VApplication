// <copyright file="ResearchProposalsTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Describes the metadata related to research proposals entity.
    /// </summary>
    public static class ResearchProposalsTableMetadata
    {
        /// <summary>
        /// The table name to store research proposals details.
        /// </summary>
        public const string TableName = "ResearchProposalsEntity";

        /// <summary>
        /// The research proposals table partition key name.
        /// </summary>
        public const string PartitionKey = "ResearchProposal";
    }
}
