// <copyright file="NewsJsonModel.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an News json Model to map json and table fields.
    /// </summary>
    public class NewsJsonModel
    {
        /// <summary>
        /// Gets or sets the news Id.
        /// </summary>
        public int NewsId { get; set; }

        /// <summary>
        /// Gets or sets the news title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets abstract of a news.
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the news body content.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the URL to external news article.
        /// </summary>
        public string ExternalLink { get; set; }

        /// <summary>
        /// Gets or sets the URL to news image.
        /// </summary>
        public string ImageURL { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the news in string representation of JSON array.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets news article request status of type <see cref="NewsArticleRequestStatus"/>.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether news article is important.
        /// </summary>
        public bool IsImportant { get; set; }

        /// <summary>
        /// Gets or sets average rating for news.
        /// </summary>
        public string AverageRating { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which news article was lastly updated.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the submitter Id.
        /// </summary>
        public int SubmitterID { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        public int NodeTypeID { get; set; }

        /// <summary>
        /// Gets or sets the news source Id.
        /// </summary>
        public int NewsSourceId { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the keyword count.
        /// </summary>
        public int KeywordCount { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which news article was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the news aggregator Id.
        /// </summary>
        public int NewsAggregatorId { get; set; }

        /// <summary>
        /// Gets or sets the news published date.
        /// </summary>
        public DateTime PublishedDate { get; set; }
    }
}
