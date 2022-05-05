// <copyright file="HomeStatusBarConfigurationEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Describes the home status bar configuration.
    /// </summary>
    public class HomeStatusBarConfigurationEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the configuration Id.
        /// </summary>
        [Key]
        public string TeamId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = HomeStatusBarConfigurationTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the link label.
        /// </summary>
        public string LinkLabel { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the configuration is active on home tab.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who updated the configuration details.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when configuration details were updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
