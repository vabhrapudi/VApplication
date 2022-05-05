// <copyright file="NewsSyncJobMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Mappers
{
    using System;
    using System.Linq;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provide methods related news entity model mappings.
    /// </summary>
    public class NewsSyncJobMapper : INewsSyncJobMapper
    {
        private const string KeywordIdSeparator = " ";
        private const int MaximumAllowedNewsBodyLength = 32000;

        /// <inheritdoc/>
        public NewsEntity MapForCreateModel(NewsJsonModel newsJsonModel)
        {
            return new NewsEntity
            {
                TableId = Guid.NewGuid().ToString(),
                NewsAggregatorId = newsJsonModel.NewsAggregatorId,
                NewsId = newsJsonModel.NewsId,
                NewsSourceId = newsJsonModel.NewsSourceId,
                SubmitterId = newsJsonModel.SubmitterID,
                AverageRating = "0",
                Status = (int)NewsArticleRequestStatus.Approved,
                NodeTypeId = newsJsonModel.NodeTypeID,
                SecurityLevel = newsJsonModel.SecurityLevel,
                IsImportant = newsJsonModel.IsImportant,
                Title = newsJsonModel.Title,
                Abstract = newsJsonModel.Abstract,
                Body = newsJsonModel.Body.Length >= MaximumAllowedNewsBodyLength ? newsJsonModel.Body.Substring(0, MaximumAllowedNewsBodyLength) : newsJsonModel.Body,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = newsJsonModel.LastUpdate,
                ExternalLink = newsJsonModel.ExternalLink,
                KeywordCount = newsJsonModel.Keywords.Count(),
                ImageURL = newsJsonModel.ImageURL,
                PublishedDate = newsJsonModel.PublishedDate,
                Keywords = newsJsonModel.Keywords == null ? null : string.Join(KeywordIdSeparator, newsJsonModel.Keywords),
            };
        }

        /// <inheritdoc/>
        public NewsEntity MapForUpdateModel(NewsEntity newsEntity, NewsJsonModel newsJsonModel)
        {
            return new NewsEntity
            {
                TableId = newsEntity.TableId,
                NewsAggregatorId = newsJsonModel.NewsAggregatorId,
                NewsId = newsJsonModel.NewsId,
                NewsSourceId = newsJsonModel.NewsSourceId,
                SubmitterId = newsJsonModel.SubmitterID,
                AverageRating = "0",
                Status = (int)NewsArticleRequestStatus.Approved,
                NodeTypeId = newsJsonModel.NodeTypeID,
                SecurityLevel = newsJsonModel.SecurityLevel,
                IsImportant = newsJsonModel.IsImportant,
                Title = newsJsonModel.Title,
                Abstract = newsJsonModel.Abstract,
                Body = newsJsonModel.Body.Length >= MaximumAllowedNewsBodyLength ? newsJsonModel.Body.Substring(0, MaximumAllowedNewsBodyLength) : newsJsonModel.Body,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = newsJsonModel.LastUpdate,
                ExternalLink = newsJsonModel.ExternalLink,
                KeywordCount = newsJsonModel.Keywords.Count(),
                ImageURL = newsJsonModel.ImageURL,
                PublishedDate = newsJsonModel.PublishedDate,
                Keywords = newsJsonModel.Keywords == null ? null : string.Join(KeywordIdSeparator, newsJsonModel.Keywords),
            };
        }
    }
}
