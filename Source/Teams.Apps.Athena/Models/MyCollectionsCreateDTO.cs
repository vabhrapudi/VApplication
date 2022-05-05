// <copyright file="MyCollectionsCreateDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Holds the details of a my collection create entity.
    /// </summary>
    public class MyCollectionsCreateDTO
    {
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
    }
}