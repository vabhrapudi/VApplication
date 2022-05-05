// <copyright file="IUserPersistentDataMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to user persistent data.
    /// </summary>
    public interface IUserPersistentDataMapper
    {
        /// <summary>
        /// Gets user persistent data from api.
        /// </summary>
        /// <param name="userPersistentDataEntity">user persistent entity model.</param>
        /// <returns>Returns a user persistent data DTO model.</returns>
        UserPersistentDataDTO MapForViewUserPersistentDataModel(UserPersistentDataEntity userPersistentDataEntity);
    }
}
