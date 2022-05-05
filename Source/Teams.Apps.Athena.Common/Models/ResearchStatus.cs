// <copyright file="ResearchStatus.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Represents the research status.
    /// </summary>
    public enum ResearchStatus
    {
        /// <summary>
        /// Represents that the new research is proposed.
        /// </summary>
        Proposed,

        /// <summary>
        /// Represents that the research is in-progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// Represents that the research was completed.
        /// </summary>
        Completed,
    }
}
