// <copyright file="UserBotConversationRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The repository for managing user-Bot conversation details.
    /// </summary>
    public class UserBotConversationRepository : BaseRepository<UserBotConversationEntity>, IUserBotConversationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserBotConversationRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create the repository.</param>
        public UserBotConversationRepository(
            ILogger<UserBotConversationRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: UserBotConversationTableMetadata.TableName,
                  defaultPartitionKey: UserBotConversationTableMetadata.PartitionKey,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}
