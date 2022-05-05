// <copyright file="NewsRequestViewDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Holds the details of news requests.
    /// </summary>
    public class NewsRequestViewDTO
    {
        /// <summary>
        /// Gets or sets the news table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the news Id.
        /// </summary>
        public int NewsId { get; set; }

        /// <summary>
        /// Gets or sets the news title.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets abstract of a news.
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the news body content.
        /// </summary>
        [Required]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the URL to external news article.
        /// </summary>
        [Url]
        public string ExternalLink { get; set; }

        /// <summary>
        /// Gets or sets the URL to news image.
        /// </summary>
        [Url]
        public string ImageURL { get; set; }

        /// <summary>
        /// Gets or sets the news creation date and time.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets user Id of user who created news request.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets approved status.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets status of news request.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether request is marked as important.
        /// </summary>
        public bool IsImportant { get; set; }
    }
}