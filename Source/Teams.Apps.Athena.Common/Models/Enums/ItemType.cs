// <copyright file="ItemType.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models.Enums
{
    /// <summary>
    /// Enum representation for item types.
    /// </summary>
    public enum Itemtype
    {
        /// <summary>
        /// Indicates invalid item.
        /// </summary>
        None,

        /// <summary>
        /// Represents news item.
        /// </summary>
        News,

        /// <summary>
        /// Represents COI item.
        /// </summary>
        COI,

        /// <summary>
        /// Represents research request item.
        /// </summary>
        ResearchRequest,

        /// <summary>
        /// Represents research project item.
        /// </summary>
        ResearchProject,

        /// <summary>
        /// Represents research paper item.
        /// </summary>
        ResearchPaper,

        /// <summary>
        /// Represents user.
        /// </summary>
        User,

        /// <summary>
        /// Represents research proposal item.
        /// </summary>
        ResearchProposal,

        /// <summary>
        /// Represents event item.
        /// </summary>
        Event,

        /// <summary>
        /// Represents partner item.
        /// </summary>
        Partner,

        /// <summary>
        /// Represents sponsor item.
        /// </summary>
        Sponsor,
    }
}
