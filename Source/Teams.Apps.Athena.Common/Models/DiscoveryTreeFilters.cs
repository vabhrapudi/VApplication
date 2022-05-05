// <copyright file="DiscoveryTreeFilters.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes the filters of discovery tree.
    /// </summary>
    public class DiscoveryTreeFilters
    {
        /// <summary>
        /// Gets or sets the news filters.
        /// </summary>
        public IEnumerable<DiscoveryTreeFilterItems> NewsFilters { get; set; }

        /// <summary>
        /// Gets or sets the organization filters.
        /// </summary>
        public IEnumerable<DiscoveryTreeFilterItems> OrganizationFilters { get; set; }

        /// <summary>
        /// Gets or sets the status filters.
        /// </summary>
        public IEnumerable<DiscoveryTreeFilterItems> StatusFilters { get; set; }

        /// <summary>
        /// Gets or sets the coi filters.
        /// </summary>
        public IEnumerable<DiscoveryTreeFilterItems> CoiFilters { get; set; }

        /// <summary>
        /// Gets or sets the research resource filters.
        /// </summary>
        public IEnumerable<DiscoveryTreeFilterItems> ResearchResourceFilters { get; set; }

        /// <summary>
        /// Gets or sets the research personnel filters.
        /// </summary>
        public IEnumerable<DiscoveryTreeFilterItems> PersonnelFilters { get; set; }
    }
}
