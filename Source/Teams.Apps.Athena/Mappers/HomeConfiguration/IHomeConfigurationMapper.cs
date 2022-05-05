// <copyright file="IHomeConfigurationMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to home tab configuration.
    /// </summary>
    public interface IHomeConfigurationMapper
    {
        /// <summary>
        /// Maps home tab configuration entity model to home tab configuration view model.
        /// </summary>
        /// <param name="homeTabConfigurationEntity">The home tab configuration entity model.</param>
        /// <returns>The home tab configuration view model.</returns>
        HomeConfigurationArticleDTO MapForViewModel(HomeConfigurationEntity homeTabConfigurationEntity);

        /// <summary>
        /// Maps home tab configuration view model to home tab configuration entity model.
        /// </summary>
        /// <param name="homeConfigurationArticleDTO">The home tab configuration article DTO model.</param>
        /// <param name="teamId">The team Id.</param>
        /// <param name="userAadId">The user AAD Id who is creating the article.</param>
        /// <returns>The home tab configuration entity model.</returns>
        HomeConfigurationEntity MapForCreateModel(HomeConfigurationArticleDTO homeConfigurationArticleDTO, string teamId, string userAadId);

        /// <summary>
        /// Maps home tab configuration view model to home tab configuration entity model.
        /// </summary>
        /// <param name="homeConfigurationArticleDTO">The home tab configuration view model.</param>
        /// <param name="homeTabConfigurationEntity">The home tab configuration entity model.</param>
        /// <param name="teamId">The team Id.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <returns>The home tab configurations entity model.</returns>
        HomeConfigurationEntity MapForUpdateModel(HomeConfigurationArticleDTO homeConfigurationArticleDTO, HomeConfigurationEntity homeTabConfigurationEntity, string teamId, string userAadId);
    }
}
