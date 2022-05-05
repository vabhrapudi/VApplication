// <copyright file="DiscoveryTreeTaxonomyElement.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Search;

    /// <summary>
    /// Represents a element in discovery tree taxonomy.
    /// </summary>
    public class DiscoveryTreeTaxonomyElement
    {
        /// <summary>
        /// Gets or sets the indentifier of an element.
        /// </summary>
        [Key]
        public int TaxonomyId { get; set; }

        /// <summary>
        /// Gets or sets the parent Id of an element.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent Id of an element.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the element name.
        /// </summary>
        [IsSearchable]
        [IsSortable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the element.
        /// </summary>
        [IsSearchable]
        public IEnumerable<int> Keywords { get; set; }
    }
}
