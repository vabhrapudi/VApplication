// <copyright file="DiscoveryTreeFilterItems.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a filter items.
    /// </summary>
    public class DiscoveryTreeFilterItems
    {
        /// <summary>
        /// Gets or sets the filter Id.
        /// </summary>
        public int FilterId { get; set; }

        /// <summary>
        /// Gets or sets the parentId.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether default on is true or false.
        /// </summary>
        public dynamic DefaultOn { get; set; }

        /// <summary>
        /// Gets or sets the tool bar icon.
        /// </summary>
        public string ToolbarIcon { get; set; }

        /// <summary>
        /// Gets or sets the db entity.
        /// </summary>
        [Required]
        public string DbEntity { get; set; }

        /// <summary>
        /// Gets or sets the db field.
        /// </summary>
        [Required]
        public string DbField { get; set; }

        /// <summary>
        /// Gets or sets the db value.
        /// </summary>
        [Required]
        public IEnumerable<int> DbValue { get; set; }

        /// <summary>
        /// Gets or sets the db comparison.
        /// </summary>
        public string DbComparison { get; set; }

        /// <summary>
        /// Gets or sets the type of 'DBField' field.
        /// </summary>
        [Required]
        public string DbFieldType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enabled is true or false.
        /// </summary>
        public dynamic Enabled { get; set; }
    }
}
