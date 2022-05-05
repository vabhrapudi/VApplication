// <copyright file="NewsMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using System.Linq;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related news entity model mappings.
    /// </summary>
    public class NewsMapper : INewsMapper
    {
        private const string KeywordIdsSeparator = " ";

        /// <inheritdoc/>
        public NewsEntity MapForCreateDraftModel(DraftNewsEntityDTO newsViewModel, Guid userAadId, string upn)
        {
            if (newsViewModel == null)
            {
                throw new ArgumentNullException(nameof(newsViewModel));
            }

            return new NewsEntity
            {
                TableId = Guid.NewGuid().ToString(),
                NewsId = newsViewModel.NewsId,
                Title = newsViewModel.Title.Trim(),
                Abstract = newsViewModel.Abstract?.Trim(),
                Body = newsViewModel.Body?.Trim(),
                ExternalLink = newsViewModel.ExternalLink?.Trim(),
                ImageURL = newsViewModel.ImageUrl?.Trim(),
                Status = (int)NewsArticleRequestStatus.Draft,
                IsImportant = newsViewModel.IsImportant,
                AverageRating = "0",
                KeywordNames = newsViewModel.KeywordsJson == null ? null : string.Join(";", newsViewModel.KeywordsJson.Select(keyword => keyword.Title)),
                Keywords = newsViewModel.KeywordsJson == null ? null : string.Join(KeywordIdsSeparator, newsViewModel.KeywordsJson.Select(keyword => keyword.KeywordId)),
                KeywordCount = newsViewModel.KeywordsJson.Count(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userAadId.ToString(),
                PublishedDate = DateTime.UtcNow,
            };
        }

        /// <inheritdoc/>
        public NewsEntity MapForCreateModel(NewsEntityDTO newsViewModel, Guid userAadId, string upn)
        {
            if (newsViewModel == null)
            {
                throw new ArgumentNullException(nameof(newsViewModel));
            }

            return new NewsEntity
            {
                TableId = Guid.NewGuid().ToString(),
                NewsId = newsViewModel.NewsId,
                Title = newsViewModel.Title.Trim(),
                Abstract = newsViewModel.Abstract?.Trim(),
                Body = newsViewModel.Body.Trim(),
                ExternalLink = newsViewModel.ExternalLink.Trim(),
                ImageURL = newsViewModel.ImageUrl.Trim(),
                Status = (int)NewsArticleRequestStatus.Pending,
                IsImportant = newsViewModel.IsImportant,
                AverageRating = "0",
                KeywordNames = string.Join(";", newsViewModel.KeywordsJson.Select(keyword => keyword.Title)),
                Keywords = string.Join(KeywordIdsSeparator, newsViewModel.KeywordsJson.Select(keyword => keyword.KeywordId)),
                KeywordCount = newsViewModel.KeywordsJson.Count(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = userAadId.ToString(),
                PublishedDate = DateTime.UtcNow,
            };
        }

        /// <inheritdoc/>
        public void MapForUpdateModel(NewsEntityDTO newsViewModel, NewsEntity newsEntityModel)
        {
            if (newsViewModel == null)
            {
                throw new ArgumentNullException(nameof(newsViewModel));
            }

            if (newsEntityModel == null)
            {
                throw new ArgumentNullException(nameof(newsEntityModel));
            }

            newsEntityModel.Title = newsViewModel.Title.Trim();
            newsEntityModel.Abstract = newsViewModel.Abstract?.Trim();
            newsEntityModel.Body = newsViewModel.Body.Trim();
            newsEntityModel.ExternalLink = newsViewModel.ExternalLink.Trim();
            newsEntityModel.ImageURL = newsViewModel.ImageUrl.Trim();
            newsEntityModel.KeywordNames = string.Join(";", newsViewModel.KeywordsJson.Select(keyword => keyword.Title));
            newsEntityModel.Keywords = string.Join(KeywordIdsSeparator, newsViewModel.KeywordsJson.Select(keyword => keyword.KeywordId));
            newsEntityModel.KeywordCount = newsViewModel.KeywordsJson.Count();
            newsEntityModel.IsImportant = newsViewModel.IsImportant;
            newsEntityModel.UpdatedAt = DateTime.UtcNow;
            newsEntityModel.PublishedDate = DateTime.UtcNow;
        }

        /// <inheritdoc/>
        public void MapForUpdateModel(DraftNewsEntityDTO draftNewsViewModel, NewsEntity newsEntityModel)
        {
            if (draftNewsViewModel == null)
            {
                throw new ArgumentNullException(nameof(draftNewsViewModel));
            }

            if (newsEntityModel == null)
            {
                throw new ArgumentNullException(nameof(newsEntityModel));
            }

            newsEntityModel.Title = draftNewsViewModel.Title.Trim();
            newsEntityModel.Abstract = draftNewsViewModel.Abstract?.Trim();
            newsEntityModel.Body = draftNewsViewModel.Body?.Trim();
            newsEntityModel.ExternalLink = draftNewsViewModel.ExternalLink?.Trim();
            newsEntityModel.ImageURL = draftNewsViewModel.ImageUrl?.Trim();
            newsEntityModel.KeywordNames = draftNewsViewModel.KeywordsJson == null ? null : string.Join(";", draftNewsViewModel.KeywordsJson.Select(keyword => keyword.Title));
            newsEntityModel.Keywords = draftNewsViewModel.KeywordsJson == null ? null : string.Join(KeywordIdsSeparator, draftNewsViewModel.KeywordsJson.Select(keyword => keyword.KeywordId));
            newsEntityModel.KeywordCount = draftNewsViewModel.KeywordsJson.Count();
            newsEntityModel.IsImportant = draftNewsViewModel.IsImportant;
            newsEntityModel.UpdatedAt = DateTime.UtcNow;
            newsEntityModel.PublishedDate = DateTime.UtcNow;
        }

        /// <inheritdoc/>
        public NewsEntityDTO MapForViewModel(NewsEntity newsEntityModel)
        {
            if (newsEntityModel == null)
            {
                throw new ArgumentNullException(nameof(newsEntityModel));
            }

            return new NewsEntityDTO
            {
                TableId = newsEntityModel.TableId,
                NewsId = newsEntityModel.NewsId,
                Title = newsEntityModel.Title,
                Abstract = newsEntityModel.Abstract,
                Body = newsEntityModel.Body,
                ExternalLink = newsEntityModel.ExternalLink,
                ImageUrl = newsEntityModel.ImageURL,
                Keywords = string.IsNullOrWhiteSpace(newsEntityModel.Keywords) ? Array.Empty<int>() : Array.ConvertAll(newsEntityModel.Keywords.Split(KeywordIdsSeparator), int.Parse),
                IsImportant = newsEntityModel.IsImportant,
                Status = newsEntityModel.Status,
                SumOfRatings = newsEntityModel.SumOfRatings,
                NumberOfRatings = newsEntityModel.NumberOfRatings,
                CreatedAt = newsEntityModel.CreatedAt,
                CreatedBy = newsEntityModel.CreatedBy,
                NodeTypeId = newsEntityModel.NodeTypeId,
                PublishedDate = newsEntityModel.PublishedDate,
                UpdatedAt = newsEntityModel.UpdatedAt,
                NewsSourceId = newsEntityModel.NewsSourceId,
            };
        }

        /// <inheritdoc/>
        public NewsRequestViewDTO MapForApprovalRequestViewModel(NewsEntity newsEntity)
        {
            newsEntity = newsEntity ?? throw new ArgumentNullException(nameof(newsEntity));
            return new NewsRequestViewDTO
            {
                TableId = newsEntity.TableId,
                NewsId = newsEntity.NewsId,
                Title = newsEntity.Title,
                Abstract = newsEntity.Abstract,
                Body = newsEntity.Body,
                ExternalLink = newsEntity.ExternalLink,
                ImageURL = newsEntity.ImageURL,
                CreatedAt = newsEntity.CreatedAt,
                Status = newsEntity.Status,
                CreatedBy = newsEntity.CreatedBy,
                IsImportant = newsEntity.IsImportant,
            };
        }
    }
}