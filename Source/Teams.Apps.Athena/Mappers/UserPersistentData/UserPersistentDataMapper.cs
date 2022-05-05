// <copyright file="UserPersistentDataMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Newtonsoft.Json;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The mapper methods related to user persistent data.
    /// </summary>
    public class UserPersistentDataMapper : IUserPersistentDataMapper
    {
        /// <inheritdoc/>
        public UserPersistentDataDTO MapForViewUserPersistentDataModel(UserPersistentDataEntity userPersistentDataEntity)
        {
            userPersistentDataEntity = userPersistentDataEntity ?? throw new ArgumentNullException(nameof(userPersistentDataEntity));

            return new UserPersistentDataDTO
            {
                DiscoveryTreePersistentData = JsonConvert.DeserializeObject<DiscoveryTreePersistentData>(userPersistentDataEntity.DiscoveryTreeData),
            };
        }
    }
}
