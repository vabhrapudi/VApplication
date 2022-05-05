// <copyright file="ResourceFeedback.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories.ResourceFeedback;

    /// <summary>
    ///  Represents the resource feedback entity.
    /// </summary>
    public class ResourceFeedback : TableEntity
    {
        /// <summary>
        /// Gets or sets the  feedback id.
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
                this.PartitionKey = ResourceFeedbackTableNames.ResourceFeedbackPartition;
            }
        }

        /// <summary>
        /// Gets or sets the resource Id.
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the resource type Id.
        /// </summary>
        public int ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the feedback.
        /// </summary>
        public string Feedback { get; set; }

        /// <summary>
        /// Gets or sets the rating from 1 to 5.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the date and time when feedback is submitted by user.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who submitted the feedback.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
