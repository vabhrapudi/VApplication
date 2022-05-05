// <copyright file="QuickAccessEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents a element in discovery tree quick access list.
    /// </summary>
    public class QuickAccessEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the quick access item id.
        /// </summary>
        [Key]
        public string QuickAccessItemId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = QuickAccessTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the indentifier of an element.
        /// </summary>
        public string TaxonomyId { get; set; }

        /// <summary>
        /// Gets or sets the parent Id of an element.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent Id of an element.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the user Id.
        /// </summary>
        public string UserId { get; set; }
    }
}