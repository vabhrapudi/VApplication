// <copyright file="PriorityInsight.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Represents the priority insights.
    /// </summary>
    public class PriorityInsight
    {
        /// <summary>
        /// Gets or sets the priority title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the proposed projects count.
        /// </summary>
        public int Proposed { get; set; }

        /// <summary>
        /// Gets or sets the current projects count.
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// Gets or sets the completed projects count.
        /// </summary>
        public int Completed { get; set; }
    }
}
