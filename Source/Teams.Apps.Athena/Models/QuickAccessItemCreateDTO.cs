// <copyright file="QuickAccessItemCreateDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a element in discovery tree quick access create entity.
    /// </summary>
    public class QuickAccessItemCreateDTO
    {
        /// <summary>
        /// Gets or sets the indentifier of an element.
        /// </summary>
        [Required]
        public string TaxonomyId { get; set; }

        /// <summary>
        /// Gets or sets the parent Id of an element.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent Id of an element.
        /// </summary>
        public int NodeTypeId { get; set; }
    }
}
