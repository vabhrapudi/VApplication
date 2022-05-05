// <copyright file="MyCollectionsEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents my collection.
    /// </summary>
    public class MyCollectionsEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the user AAD Id.
        /// </summary>
        [Required]
        public string CollectionId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = MyCollectionsTableMetadata.MyCollectionsPartition;
            }
        }

        /// <summary>
        /// Gets or sets user UPN.
        /// </summary>
        public string UserUPN { get; set; }

        /// <summary>
        /// Gets or sets collection name.
        /// </summary>
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets items.
        /// </summary>
        public string Items { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets image url.
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string ImageURL { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who created collection.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time when collection is created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when collection is updated.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
