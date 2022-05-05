// <copyright file="DiscoveryTreeSelectedFilter.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Reprsents selected filters in discovery tree.
    /// </summary>
    public class DiscoveryTreeSelectedFilter
    {
        /// <summary>
        /// Gets or sets the type of filter.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the filters for selected filter type.
        /// </summary>
        public IEnumerable<DiscoveryTreeFilterItems> Filters { get; set; }
    }
}
