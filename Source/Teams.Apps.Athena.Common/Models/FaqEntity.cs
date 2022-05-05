// <copyright file="FaqEntity.cs" company="NPS Foundation">
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
    /// Represents FAQ entity.
    /// </summary>
    public class FaqEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the question Id.
        /// </summary>
        [Key]
        public string QuestionId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = FaqTableMetadata.FaqPartition;
            }
        }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        [Required]
        [IsSearchable]
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who raised the question.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who updated the FAQ details.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when question is created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when question updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
