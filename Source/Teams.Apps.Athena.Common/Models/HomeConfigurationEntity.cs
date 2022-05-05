// <copyright file="HomeConfigurationEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Represents the home configuration entity.
    /// </summary>
    public class HomeConfigurationEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets unique article Id.
        /// </summary>
        [Key]
        public string ArticleId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the team Id.
        /// </summary>
        public string TeamId
        {
            get
            {
                return this.PartitionKey;
            }

            set
            {
                this.PartitionKey = value;
            }
        }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets image URL.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who created the article.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who updated the article.
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which article was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which article was updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
