// <copyright file="DraftNewsEntityDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Represents the details of news article exposed to end-user.
    /// </summary>
    public class DraftNewsEntityDTO
    {
        /// <summary>
        /// Gets or sets news table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets news Id.
        /// </summary>
        public int NewsId { get; set; }

        /// <summary>
        /// Gets or sets the news title.
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage = "The request title should not exceed length of 100 characters.")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets abstract of a news.
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the news description.
        /// </summary>
        [MaxLength(300, ErrorMessage = "The request description should not exceed length of 300 characters.")]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the URL to external news article.
        /// </summary>
        [MaxLength(300, ErrorMessage = "The external link should not exceed length of 300 characters.")]
        public string ExternalLink { get; set; }

        /// <summary>
        /// Gets or sets the URL to news image.
        /// </summary>
        [MaxLength(300, ErrorMessage = "The image URL should not exceed length of 300 characters.")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the news in string representation of JSON array.
        /// </summary>
        public IEnumerable<KeywordDTO> KeywordsJson { get; set; }

        /// <summary>
        /// Gets or sets the news article request status of type <see cref="NewsArticleRequestStatus"/>.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether news article is important.
        /// </summary>
        public bool IsImportant { get; set; }

        /// <summary>
        /// Gets or sets the rating of news article.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the news creation date and time.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
