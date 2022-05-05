// <copyright file="HomeConfigurationMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related home configuration entity model mappings.
    /// </summary>
    public class HomeConfigurationMapper : IHomeConfigurationMapper
    {
        /// <inheritdoc/>
        public HomeConfigurationArticleDTO MapForViewModel(HomeConfigurationEntity homeTabConfigurationEntity)
        {
            homeTabConfigurationEntity = homeTabConfigurationEntity ?? throw new ArgumentNullException(nameof(homeTabConfigurationEntity));

            return new HomeConfigurationArticleDTO
            {
                ArticleId = homeTabConfigurationEntity.ArticleId,
                Title = homeTabConfigurationEntity.Title,
                Description = homeTabConfigurationEntity.Description,
                ImageUrl = homeTabConfigurationEntity.ImageUrl,
            };
        }

        /// <inheritdoc/>
        public HomeConfigurationEntity MapForCreateModel(HomeConfigurationArticleDTO homeConfigurationArticleDTO, string teamId, string userAadId)
        {
            if (homeConfigurationArticleDTO == null)
            {
                throw new ArgumentNullException(nameof(homeConfigurationArticleDTO));
            }

            return new HomeConfigurationEntity
            {
                ArticleId = Guid.NewGuid().ToString(),
                TeamId = teamId,
                Title = homeConfigurationArticleDTO.Title.Trim(),
                Description = homeConfigurationArticleDTO.Description.Trim(),
                ImageUrl = homeConfigurationArticleDTO.ImageUrl,
                CreatedBy = userAadId,
                UpdatedBy = userAadId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
        }

        /// <inheritdoc/>
        public HomeConfigurationEntity MapForUpdateModel(HomeConfigurationArticleDTO homeConfigurationArticleDTO, HomeConfigurationEntity homeTabConfigurationEntity, string teamId, string userAadId)
        {
            homeConfigurationArticleDTO = homeConfigurationArticleDTO ?? throw new ArgumentNullException(nameof(homeConfigurationArticleDTO));
            homeTabConfigurationEntity = homeTabConfigurationEntity ?? throw new ArgumentNullException(nameof(homeTabConfigurationEntity));

            homeTabConfigurationEntity.Title = homeConfigurationArticleDTO.Title;
            homeTabConfigurationEntity.Description = homeConfigurationArticleDTO.Description;
            homeTabConfigurationEntity.ImageUrl = homeConfigurationArticleDTO.ImageUrl;
            homeTabConfigurationEntity.UpdatedBy = userAadId;
            homeTabConfigurationEntity.UpdatedAt = DateTime.UtcNow;

            return homeTabConfigurationEntity;
        }
    }
}
