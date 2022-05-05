// <copyright file="StorageSettings.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models.Configuration
{
    using Microsoft.Teams.Athena.Models;

    /// <summary>
    /// A class which helps to provide storage settings.
    /// </summary>
    public class StorageSettings : BotSettings
    {
        /// <summary>
        /// Gets or sets storage connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
