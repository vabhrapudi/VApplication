// <copyright file="DailyBriefingHomeArticleDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;

    /// <summary>
    /// Describes the daily briefing home article details.
    /// </summary>
    public class DailyBriefingHomeArticleDTO
    {
        /// <summary>
        /// Gets or sets the resource Id.
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the last updated date and time.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the article URL.
        /// </summary>
        public string ArticleUrl { get; set; }

        /// <summary>
        /// Gets or sets the node type Id of resource.
        /// </summary>
        public int NodeTypeId { get; set; }
    }
}
