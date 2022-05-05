// <copyright file="NewsEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Azure.Search;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents a news entity.
    /// </summary>
    public class NewsEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the news table Id.
        /// </summary>
        [Key]
        public string TableId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = NewsTableMetadata.NewsPartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the news Id.
        /// </summary>
        [IsFilterable]
        public int NewsId { get; set; }

        /// <summary>
        /// Gets or sets the news title.
        /// </summary>
        [Required]
        [IsSearchable]
        [IsFilterable]
        [IsSortable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets abstract of a news.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the news body content.
        /// </summary>
        [Required]
        [IsSearchable]
        [IsFilterable]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the URL to external news article.
        /// </summary>
        [Url]
        [IsFilterable]
        public string ExternalLink { get; set; }

        /// <summary>
        /// Gets or sets the URL to news image.
        /// </summary>
        [Url]
        [IsFilterable]
        public string ImageURL { get; set; }

        /// <summary>
        /// Gets or sets news article request status of type <see cref="NewsArticleRequestStatus"/>.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether news article is important.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public bool IsImportant { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether news article is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the sum of ratings submitted by end-users.
        /// </summary>
        [IsSortable]
        public int SumOfRatings { get; set; }

        /// <summary>
        /// Gets or sets the number of end-users who submitted the rating.
        /// </summary>
        public int NumberOfRatings { get; set; }

        /// <summary>
        /// Gets or sets average rating for news.
        /// </summary>
        [IsSortable]
        public string AverageRating { get; set; }

        /// <summary>
        /// Gets or sets the comma separated string of keywords' title.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string KeywordNames { get; set; }

        /// <summary>
        /// Gets or sets the space separated string of keywords Ids.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who created the news article.
        /// </summary>
        [IsFilterable]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the admin comment on approval or rejection.
        /// </summary>
        [IsFilterable]
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets the news creation date and time.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which news article was lastly updated.
        /// </summary>
        [IsFilterable]
        [IsSortable]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the news aggregator Id.
        /// </summary>
        [IsFilterable]
        public int NewsAggregatorId { get; set; }

        /// <summary>
        /// Gets or sets the submitter Id.
        /// </summary>
        [IsFilterable]
        public int SubmitterId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        [IsFilterable]
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the news source Id.
        /// </summary>
        [IsFilterable]
        public int NewsSourceId { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        [IsFilterable]
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the keyword count.
        /// </summary>
        [IsFilterable]
        public int KeywordCount { get; set; }

        /// <summary>
        /// Gets or sets the news published date.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public DateTime? PublishedDate { get; set; }
    }
}