// <copyright file="PrioritiesInsightsDto.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describes priority insights view model.
    /// </summary>
    public class PrioritiesInsightsDto
    {
        /// <summary>
        /// Gets or sets the priority Ids.
        /// </summary>
        public IEnumerable<Guid> PriorityIds { get; set; }

        /// <summary>
        /// Gets or sets the keyword Ids.
        /// </summary>
        public IEnumerable<int> KeywordIdsFilter { get; set; }
    }
}
