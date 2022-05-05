// <copyright file="HomeConfigurationsTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Describes the home tab configurations entity metadata.
    /// </summary>
    public static class HomeConfigurationsTableMetadata
    {
        /// <summary>
        /// The table name to store home configurations details.
        /// </summary>
        public const string TableName = "HomeConfigurationsEntity";

        /// <summary>
        /// The home configurations table partition key name.
        /// </summary>
        public const string PartitionKey = "HomeConfigurations";
    }
}
