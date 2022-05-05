// <copyright file="DiscoveryTreeSearchAndFilter.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the discovery tree search and filter options.
    /// </summary>
    public class DiscoveryTreeSearchAndFilter
    {
        /// <summary>
        /// Gets or sets the search strings.
        /// </summary>
        public IEnumerable<string> SearchStrings { get; set; }

        /// <summary>
        /// Gets or sets the search keywords.
        /// </summary>
        public IEnumerable<int> SearchKeywords { get; set; }

        /// <summary>
        /// Gets or sets the selected filters.
        /// </summary>
        public IEnumerable<DiscoveryTreeSelectedFilter> SelectedFilters { get; set; }
    }
}
