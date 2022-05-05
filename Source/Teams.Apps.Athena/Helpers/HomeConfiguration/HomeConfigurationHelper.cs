// <copyright file="HomeConfigurationHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Constants;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods associated with home tab configuration entity operations.
    /// </summary>
    public class HomeConfigurationHelper : IHomeConfigurationHelper
    {
        private readonly IHomeConfigurationsRepository homeConfigurationRepository;
        private readonly IHomeConfigurationMapper homeConfigurationMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeConfigurationHelper"/> class.
        /// </summary>
        /// <param name="homeConfigurationMapper">The instance of <see cref="HomeConfigurationMapper"/> class.</param>
        /// <param name="homeConfigurationRepository">The instance of <see cref="HomeConfigurationsRepository"/> class.</param>
        public HomeConfigurationHelper(
            IHomeConfigurationsRepository homeConfigurationRepository,
            IHomeConfigurationMapper homeConfigurationMapper)
        {
            this.homeConfigurationRepository = homeConfigurationRepository;
            this.homeConfigurationMapper = homeConfigurationMapper;
        }

        /// <inheritdoc/>
        public async Task<HomeConfigurationArticleDTO> GetHomeConfigurationByArticleIdAsync(string teamId, string articleId)
        {
            articleId = articleId ?? throw new ArgumentNullException(nameof(articleId));

            var homeConfigurationArticle = await this.homeConfigurationRepository.GetAsync(teamId, articleId);

            return this.homeConfigurationMapper.MapForViewModel(homeConfigurationArticle);
        }

        /// <inheritdoc/>
        public async Task<HomeConfigurationArticleDTO> CreateHomeConfigurationArticleAsync(string teamId, HomeConfigurationArticleDTO homeConfigurationArticleDTO, string userAadId)
        {
            homeConfigurationArticleDTO = homeConfigurationArticleDTO ?? throw new ArgumentNullException(nameof(homeConfigurationArticleDTO));

            var existingHomeConfigurationArticlesOfTeam = await this.homeConfigurationRepository.GetAllAsync(teamId);

            if (existingHomeConfigurationArticlesOfTeam.Count() >= Constants.MaxNumberOfArticlesCanBeConfigured)
            {
                return null;
            }

            var homeConfigurationToCreate = this.homeConfigurationMapper.MapForCreateModel(homeConfigurationArticleDTO, teamId, userAadId);
            var homeConfigurationArticle = await this.homeConfigurationRepository.CreateOrUpdateAsync(homeConfigurationToCreate);

            return this.homeConfigurationMapper.MapForViewModel(homeConfigurationArticle);
        }

        /// <inheritdoc/>
        public async Task<HomeConfigurationArticleDTO> UpdateHomeConfigurationArticleAsync(string teamId, HomeConfigurationArticleDTO homeConfigurationArticleDTO, string userAadId)
        {
            homeConfigurationArticleDTO = homeConfigurationArticleDTO ?? throw new ArgumentNullException(nameof(homeConfigurationArticleDTO));

            var homeConfigurationArticleEntityModel = await this.homeConfigurationRepository.GetAsync(teamId, homeConfigurationArticleDTO.ArticleId);

            if (homeConfigurationArticleEntityModel == null)
            {
                return null;
            }

            var homeConfigurationArticleToUpdate = this.homeConfigurationMapper.MapForUpdateModel(homeConfigurationArticleDTO, homeConfigurationArticleEntityModel, teamId, userAadId);
            var updatedHomeConfigurationArticleDetails = await this.homeConfigurationRepository.CreateOrUpdateAsync(homeConfigurationArticleToUpdate);

            return this.homeConfigurationMapper.MapForViewModel(updatedHomeConfigurationArticleDetails);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<HomeConfigurationArticleDTO>> GetHomeConfigurationArticlesAsync(Guid teamId)
        {
            var homeConfigurationArticles = await this.homeConfigurationRepository.GetAllAsync(teamId.ToString());

            homeConfigurationArticles = homeConfigurationArticles.OrderByDescending(article => article.UpdatedAt);

            return homeConfigurationArticles.Select(x => this.homeConfigurationMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task DeleteHomeConfigurationArticlesAsync(Guid teamId, IEnumerable<Guid> articleIds)
        {
            var rowKeysFilter = this.homeConfigurationRepository.GetRowKeysFilter(articleIds.Select(x => x.ToString()));

            var homeConfigurationArticles = await this.homeConfigurationRepository.GetWithFilterAsync(rowKeysFilter, teamId.ToString());

            await this.homeConfigurationRepository.BatchDeleteAsync(homeConfigurationArticles);
        }
    }
}
