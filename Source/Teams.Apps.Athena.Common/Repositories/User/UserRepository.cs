// <copyright file="UserRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the user data stored in the table storage.
    /// </summary>
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public UserRepository(
            ILogger<UserRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: UserTableMetadata.TableName,
                  defaultPartitionKey: UserTableMetadata.UserPartitionKey,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<UserEntity> GetUserDetailsByEmailAddressAsync(string emailAddress)
        {
            var emailAddressFilter = TableQuery.GenerateFilterCondition(
                        nameof(UserEntity.EmailAddress),
                        QueryComparisons.Equal,
                        emailAddress);
            var user = await this.GetWithFilterAsync(emailAddressFilter);
            return user.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<UserEntity> GetUserDetailsByUserIdAsync(string userId)
        {
            var userIdFilter = TableQuery.GenerateFilterCondition(
                        nameof(UserEntity.UserId),
                        QueryComparisons.Equal,
                        userId);
            var user = await this.GetWithFilterAsync(userIdFilter);
            return user.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<UserEntity> GetUserDetailsByExternalUserIdAsync(int externalUserId)
        {
            var externalUserIdFilter = TableQuery.GenerateFilterConditionForInt(
                        nameof(UserEntity.ExternalUserId),
                        QueryComparisons.Equal,
                        externalUserId);
            var user = await this.GetWithFilterAsync(externalUserIdFilter);
            return user.FirstOrDefault();
        }
    }
}