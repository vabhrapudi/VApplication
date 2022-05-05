// <copyright file="UserPersistentDataHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The helper methods to save user persistent data.
    /// </summary>
    public class UserPersistentDataHelper : IUserPersistentDataHelper
    {
        private readonly IUserPersistentDataMapper userPersistentDataMapper;
        private readonly IUserPersistentDataRepository userPersistentDataRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPersistentDataHelper"/> class.
        /// </summary>
        /// <param name="userPersistentDataRepository">The instance of <see cref="UserPersistentDataRepository"/> class.</param>
        /// <param name="userPersistentDataMapper">The instance of <see cref="UserPersistentDataMapper"/> class.</param>
        public UserPersistentDataHelper(
            IUserPersistentDataRepository userPersistentDataRepository,
            IUserPersistentDataMapper userPersistentDataMapper)
        {
            this.userPersistentDataRepository = userPersistentDataRepository;
            this.userPersistentDataMapper = userPersistentDataMapper;
        }

        /// <inheritdoc/>
        public async Task<UserPersistentDataDTO> SaveDiscoveryTreeUserPersistentDataAsync(DiscoveryTreePersistentData discoveryTreePersistentData, string userId)
        {
            discoveryTreePersistentData = discoveryTreePersistentData ?? throw new ArgumentNullException(nameof(discoveryTreePersistentData));

            var existingUserPersistentData = await this.userPersistentDataRepository.GetAsync(UserPersistentDataTableMetadata.PartitionKey, userId);

            if (existingUserPersistentData == null)
            {
                var userPersistentData = new UserPersistentDataEntity
                {
                    UserId = userId,
                    DiscoveryTreeData = JsonConvert.SerializeObject(discoveryTreePersistentData),
                };

                var createdUserPersistentData = await this.userPersistentDataRepository.CreateOrUpdateAsync(userPersistentData);
                return this.userPersistentDataMapper.MapForViewUserPersistentDataModel(createdUserPersistentData);
            }

            existingUserPersistentData.DiscoveryTreeData = JsonConvert.SerializeObject(discoveryTreePersistentData);
            var updatedUserPersistentData = await this.userPersistentDataRepository.InsertOrMergeAsync(existingUserPersistentData);

            return this.userPersistentDataMapper.MapForViewUserPersistentDataModel(updatedUserPersistentData);
        }

        /// <inheritdoc/>
        public async Task<UserPersistentDataDTO> GetUserPersistentDataAsync(string userId)
        {
            userId = userId ?? throw new ArgumentNullException(nameof(userId));
            var userPersistentData = await this.userPersistentDataRepository.GetAsync(UserPersistentDataTableMetadata.PartitionKey, userId);

            if (userPersistentData == null)
            {
                return null;
            }

            return this.userPersistentDataMapper.MapForViewUserPersistentDataModel(userPersistentData);
        }
    }
}
