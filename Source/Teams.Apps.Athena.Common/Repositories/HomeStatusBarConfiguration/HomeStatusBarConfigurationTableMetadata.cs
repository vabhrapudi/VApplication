// <copyright file="HomeStatusBarConfigurationTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Describes the home status bar configuration entity metadata.
    /// </summary>
    public static class HomeStatusBarConfigurationTableMetadata
    {
        /// <summary>
        /// The table name to store home status bar configuration details.
        /// </summary>
        public const string TableName = "HomeStatusBarConfigurationsEntity";

        /// <summary>
        /// The home status bar configuration table partition key name.
        /// </summary>
        public const string PartitionKey = "HomeStatusBarConfiguration";
    }
}
