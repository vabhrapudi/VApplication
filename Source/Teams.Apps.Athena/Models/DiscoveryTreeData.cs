// <copyright file="DiscoveryTreeData.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a discovery tree data element.
    /// </summary>
    public class DiscoveryTreeData
    {
        /// <summary>
        /// Gets or sets the research projects.
        /// </summary>
        public IEnumerable<ResearchProjectDTO> ResearchProjects { get; set; }

        /// <summary>
        /// Gets or sets the research requests.
        /// </summary>
        public IEnumerable<ResearchRequestViewDTO> ResearchRequests { get; set; }

        /// <summary>
        /// Gets or sets the Athena sponsors.
        /// </summary>
        public IEnumerable<SponsorDTO> Sponsors { get; set; }

        /// <summary>
        /// Gets or sets the Athena partners.
        /// </summary>
        public IEnumerable<PartnerDTO> Partners { get; set; }

        /// <summary>
        /// Gets or sets the Athena events.
        /// </summary>
        public IEnumerable<EventDTO> Events { get; set; }

        /// <summary>
        /// Gets or sets the research proposals.
        /// </summary>
        public IEnumerable<ResearchProposalViewDTO> ResearchProposals { get; set; }

        /// <summary>
        /// Gets or sets the community of interests.
        /// </summary>
        public IEnumerable<CoiEntityDTO> Cois { get; set; }

        /// <summary>
        /// Gets or sets the news articles.
        /// </summary>
        public IEnumerable<NewsEntityDTO> NewsArticles { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public IEnumerable<UserSettingsViewDTO> Users { get; set; }

        /// <summary>
        /// Gets or sets the Athena info resources.
        /// </summary>
        public IEnumerable<AthenaInfoResourceDTO> AthenaInfoResources { get; set; }

        /// <summary>
        /// Gets or sets the Athena tools.
        /// </summary>
        public IEnumerable<AthenaToolDTO> AthenaTools { get; set; }
    }
}
