// <copyright file="MyCollectionsViewDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Holds the details of a my collection entity for view.
    /// </summary>
    public class MyCollectionsViewDTO
    {
        /// <summary>
        /// Gets or sets the user AAD Id.
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// Gets or sets collection name.
        /// </summary>
        [MaxLength(75)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets image url.
        /// </summary>
        [MaxLength(300)]
        public string ImageURL { get; set; }
    }
}