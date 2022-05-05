// <copyright file="MyCollectionsUpdateDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Holds the details of a my collection entity to be updated.
    /// </summary>
    public class MyCollectionsUpdateDTO
    {
        /// <summary>
        /// Gets or sets the user AAD Id.
        /// </summary>
        [Required]
        public string CollectionId { get; set; }

        /// <summary>
        /// Gets or sets collection name.
        /// </summary>
        [Required]
        [MaxLength(75)]
        public string Name { get; set; }

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
        /// Gets or sets items.
        /// </summary>
        public IEnumerable<Item> Items { get; set; }
    }
}