// <copyright file="PriorityEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents the priority entity.
    /// </summary>
    public class PriorityEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the priority Id.
        /// </summary>
        [Key]
        public string Id
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = AthenaPrioritiesTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets priority title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets priority type.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the space separated string of keyword Ids.
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who created the priority.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who updated the priority.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which priority was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which priority was updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
