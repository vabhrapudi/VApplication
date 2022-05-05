// <copyright file="NewsSyncJobStatusRecordEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents a news sync status record entity.
    /// </summary>
    public class NewsSyncJobStatusRecordEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the news sync job Id.
        /// </summary>
        [Key]
        public string NewsSyncJobId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = NewsSyncJobStatusRecordTableMetadata.NewsSyncStatusRecordPartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether last function run is successful.
        /// </summary>
        public bool HasSucceeded { get; set; }

        /// <summary>
        /// Gets or sets the news sync job run date and time.
        /// </summary>
        public DateTime NewsSyncJobRanAt { get; set; }

        /// <summary>
        /// Gets or sets the reason of failure.
        /// </summary>
        public string ReasonForFailure { get; set; }
    }
}
