// <copyright file="DiscoveryTreePersistentData.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the persistent data for discovery tree.
    /// </summary>
    public class DiscoveryTreePersistentData
    {
        /// <summary>
        /// Gets or sets the selected filter Ids.
        /// </summary>
        public IEnumerable<int> SelectedFilterIds { get; set; }

        /// <summary>
        /// Gets or sets the Ids selected for 'Configure filter'.
        /// </summary>
        public IEnumerable<int> SelectedConfigureFilterIds { get; set; }
    }
}
