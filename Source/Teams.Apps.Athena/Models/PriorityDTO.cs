// <copyright file="PriorityDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Describes the priority view model.
    /// </summary>
    public class PriorityDTO
    {
        /// <summary>
        /// Gets or sets the priority Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets priority title.
        /// </summary>
        [Required]
        [MaxLength(75)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets priority type.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of keywords Ids.
        /// </summary>
        [Required]
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the insights data.
        /// </summary>
#pragma warning disable CA2227 // Need to insert data.
        public Dictionary<string, int> Insights { get; set; }
#pragma warning restore CA2227 // Need to insert data.
    }
}