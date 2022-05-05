// <copyright file="CollectionItemType.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models.Enums
{
    /// <summary>
    /// Represents 0 for none, 1 for news, 2 for COI, 3 for research request, 4 for research project, 5 for research paper and so on.
    /// </summary>
    public enum CollectionItemType
    {
        /// <summary>
        /// Represents no collection item type.
        /// </summary>
        None,

        /// <summary>
        /// Represents news collection item type.
        /// </summary>
        News,

        /// <summary>
        /// Represents COI collection item type.
        /// </summary>
        COI,

        /// <summary>
        /// Represents research request collection item type.
        /// </summary>
        ResearchRequest,

        /// <summary>
        /// Represents research project collection item type.
        /// </summary>
        ResearchProject,

        /// <summary>
        /// Represents research paper collection item type.
        /// </summary>
        ResearchPaper,

        /// <summary>
        /// Represents user collection item type.
        /// </summary>
        User,

        /// <summary>
        /// Represents research proposal item type.
        /// </summary>
        ResearchProposal,

        /// <summary>
        /// Represents event item type.
        /// </summary>
        Event,

        /// <summary>
        /// Represents partner item type.
        /// </summary>
        Partner,

        /// <summary>
        /// Represents sponsor item type.
        /// </summary>
        Sponsor,

        /// <summary>
        /// Represents source item type.
        /// </summary>
        Source,

        /// <summary>
        /// Represents information item type.
        /// </summary>
        Information,

        /// <summary>
        /// Represents tool item type.
        /// </summary>
        Tool,
    }
}