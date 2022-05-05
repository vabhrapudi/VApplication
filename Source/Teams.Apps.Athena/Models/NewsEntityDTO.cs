// <copyright file="NewsEntityDTO.cs" company="NPS Foundation">
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
    public class NewsEntityDTO
    {
        /// <summary>
        /// Gets or sets news table Id.
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
        [MaxLength(100, ErrorMessage = "The request title should not exceed length of 100 characters.")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets abstract of a news.
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the news description.
        /// </summary>
        [Required]
        [MaxLength(300, ErrorMessage = "The request description should not exceed length of 300 characters.")]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the URL to external news article.
        /// </summary>
        [Required]
        [Url]
        [MaxLength(300, ErrorMessage = "The external link should not exceed length of 300 characters.")]
        public string ExternalLink { get; set; }

        /// <summary>
        /// Gets or sets the URL to news image.
        /// </summary>
        [Required]
        [Url]
        [MaxLength(300, ErrorMessage = "The image URL should not exceed length of 300 characters.")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the news in string representation of JSON array.
        /// </summary>
        [Required]
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
        /// Gets or sets the sum of ratings submitted by end-users.
        /// </summary>
        public int SumOfRatings { get; set; }

        /// <summary>
        /// Gets or sets the number of end-users who submitted the rating.
        /// </summary>
        public int NumberOfRatings { get; set; }

        /// <summary>
        /// Gets or sets the news creation date and time.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the news updation date and time.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the name of user who created request.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets news rating.
        /// </summary>
        public int UserRating { get; set; }

        /// <summary>
        /// Gets or sets the keyword Ids.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the news published date.
        /// </summary>
        public DateTime? PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the news source Id.
        /// </summary>
        public int NewsSourceId { get; set; }
    }
}