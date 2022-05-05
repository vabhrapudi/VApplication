// <copyright file="IUserSettingsMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes methods that manages user entity model mappings.
    /// </summary>
    public interface IUserSettingsMapper
    {
        /// <summary>
        /// Gets user entity model to be inserted in database
        /// </summary>
        /// <param name="userSettingsCreateModel">user entity create model.</param>
        /// <param name="userId">Logged in user id.</param>
        /// <returns>Returns a user entity model.</returns>
        UserEntity MapForCreateModel(UserSettingsCreateDTO userSettingsCreateModel, string userId);

        /// <summary>
        /// Gets user entity model to be inserted in database
        /// </summary>
        /// <param name="userDetails">user details.</param>
        /// <returns>Returns a user entity model.</returns>
        UserEntity MapForCreateModel(UserDetails userDetails);

        /// <summary>
        /// Gets user entity model to be inserted in database
        /// </summary>
        /// <param name="userSettingsUpdateModel">user entity update model.</param>
        /// <param name="userEntity">user entity model.</param>
        /// <returns>Returns a user entity model.</returns>
        UserEntity MapForUpdateModel(UserSettingsUpdateDTO userSettingsUpdateModel, UserEntity userEntity);

        /// <summary>
        /// Gets user entity model sent to be as api response.
        /// </summary>
        /// <param name="userEntity">user entity model.</param>
        /// <returns>Returns a user entity model.</returns>
        UserSettingsViewDTO MapForViewModel(UserEntity userEntity);
    }
}