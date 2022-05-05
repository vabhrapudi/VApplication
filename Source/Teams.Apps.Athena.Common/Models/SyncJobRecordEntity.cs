// <copyright file="SyncJobRecordEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents a sync record entity.
    /// </summary>
    public class SyncJobRecordEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the news table Id.
        /// </summary>
        [Key]
        public string SyncJob
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = SyncJobRecordTableMetadata.SyncRecordPartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether last function run is successful.
        /// </summary>
        public bool IsLastRunSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the last run date and time.
        /// </summary>
        public DateTime LastRunAt { get; set; }
    }
}
