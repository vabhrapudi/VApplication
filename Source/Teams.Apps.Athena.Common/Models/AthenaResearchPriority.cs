// <copyright file="AthenaResearchPriority.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Represents a athena research priority.
    /// </summary>
    public class AthenaResearchPriority
    {
        /// <summary>
        /// Gets or sets the priority Id.
        /// </summary>
        public int PriorityId { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }
    }
}
