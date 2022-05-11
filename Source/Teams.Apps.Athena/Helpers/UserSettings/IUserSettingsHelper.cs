// <copyright file="IUserSettingsHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing user entity.
    /// </summary>
    public interface IUserSettingsHelper
    {
        /// <summary>
        /// Creates a new user entity
        /// </summary>
        /// <param name="userSettingsCreateDTO">The user details.</param>
        /// <param name="userId">Logged in user id.</param>
        /// <returns>Returns user details</returns>
        Task<UserSettingsViewDTO> CreateUserAsync(UserSettingsCreateDTO userSettingsCreateDTO, string userId);

        /// <summary>
        /// Creates a new user entity
        /// </summary>
        /// <param name="userSettingsUpdateDTO">The user details that need to be updated.</param>
        /// <param name="userEntity">Existing user details</param>
        /// <returns>Return true if project is updated, else return false.</returns>
        Task<UserSettingsViewDTO> UpdateUserAsync(UserSettingsUpdateDTO userSettingsUpdateDTO, UserEntity userEntity);

        /// <summary>
        /// Gets a user details by Id.
        /// </summary>
        /// <param name="userAadId">Get user id of logged in user.</param>
        /// <returns>Returns user details</returns>
        Task<UserSettingsViewDTO> GetUserByIdAsync(string userAadId);

        /// <summary>
        /// Gets a user details by table Id.
        /// </summary>
        /// <param name="tableId">Get user table id of user.</param>
        /// <returns>Returns user entity.</returns>
        Task<UserEntity> GetUserItemByIdAsync(string tableId);

        /// <summary>
        /// Gets a user details by email address.
        /// </summary>
        /// <param name="emailAddress">Email address of user.</param>
        /// <returns>Returns user details</returns>
        Task<UserSettingsViewDTO> GetUserDetailsByEmailAdressAsync(string emailAddress);

        /// <summary>
        /// Gets the users by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The collection of users.</returns>
        Task<IEnumerable<UserSettingsViewDTO>> GetUsersByKeywordIds(IEnumerable<int> keywordIds);

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="searchParametersDTO">The advanced search parameters.</param>
        /// <returns>The collection of users.</returns>
        Task<IEnumerable<UserSettingsViewDTO>> GetUsersAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Deletes the user settings for a user.
        /// </summary>
        /// <param name="userAadId">The user AAD Id.</param>
        /// <returns>A task reprsenting delete operation.</returns>
        Task DeleteUserSettingsAsync(string userAadId);

        /// <summary>
        /// Gets the all Athena user who installed Athena app in personal scope.
        /// </summary>
        /// <returns>The collection of Athena users.</returns>
        Task<IEnumerable<UserBotConversationEntity>> GetAthenaUsersAsync();

        /// <summary>
        /// Validates if the logged in user is Admin.
        /// </summary>
        /// <param name="userAadId">The aad Id of the user.</param>
        /// <returns>Returns true if the logged in user is Admin, else return false.</returns>
        Task<bool> ValidateIfUserIsAdmin(string userAadId);
    }
}