// <copyright file="FaqTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// FAQ data table names.
    /// </summary>
    public static class FaqTableMetadata
    {
        /// <summary>
        /// Table name for the FAQ data table.
        /// </summary>
        public const string TableName = "FaqEntity";

        /// <summary>
        /// FAQ data partition key name.
        /// </summary>
        public const string FaqPartition = "FAQ";
    }
}