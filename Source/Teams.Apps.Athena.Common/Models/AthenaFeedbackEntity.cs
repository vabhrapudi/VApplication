// <copyright file="AthenaFeedbackEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Azure.Search;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents an athena feedback entity.
    /// </summary>
    public class AthenaFeedbackEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the feedback Id.
        /// </summary>
        [Key]
        public string FeedbackId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = AthenaFeedbackTableNames.AthenaFeedbackPartition;
            }
        }

        /// <summary>
        /// Gets or sets the feedback level.
        /// </summary>
        [IsFilterable]
        public int Feedback { get; set; }

        /// <summary>
        /// Gets or sets the detailed feedback.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which feedback was submitted.
        /// </summary>
        [IsSortable]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who submitted the feedback.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        [IsSortable]
        public int Category { get; set; }

        /// <summary>
        /// Gets or sets the feedback type.
        /// </summary>
        [IsSortable]
        public int Type { get; set; }
    }
}
