// <copyright file="MyCollectionsItemsViewDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;

    /// <summary>
    /// Holds the details of a my collection entity for view.
    /// </summary>
    public class MyCollectionsItemsViewDTO
    {
        /// <summary>
        /// Gets or sets the collection Id.
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// Gets or sets the collection name.
        /// </summary>
        public string CollectionItemName { get; set; }

        /// <summary>
        /// Gets or sets the collection item type.
        /// </summary>
        public int CollectionItemType { get; set; }

        /// <summary>
        /// Gets or sets collection item id.
        /// </summary>
        public string CollectionItemId { get; set; }

        /// <summary>
        /// Gets or sets created by.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets created on.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets category.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets name of the user who have created the collection Item.
        /// </summary>
        public string CreatedByName { get; set; }

        /// <summary>
        /// Gets or sets profile photo of the user who have created the collection Item.
        /// </summary>
        public string CreatedByProfilePhoto { get; set; }

        /// <summary>
        /// Gets or sets the external link.
        /// </summary>
        public string ExternalLink { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public string Source { get; set; }
    }
}