// <copyright file="QuickAccessTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// The metadata of quick access table.
    /// </summary>
    public static class QuickAccessTableMetadata
    {
        /// <summary>
        /// The quick access table name where all quick access items get stored.
        /// </summary>
        public const string TableName = "QuickAccessEntity";

        /// <summary>
        /// The quick access table partition key.
        /// </summary>
        public const string PartitionKey = "QuickAccessItem";
    }
}
