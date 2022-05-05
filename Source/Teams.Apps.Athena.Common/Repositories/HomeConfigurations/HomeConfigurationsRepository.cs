// <copyright file="HomeConfigurationsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The repository for managing table operations related to home configurations.
    /// </summary>
    public class HomeConfigurationsRepository : BaseRepository<HomeConfigurationEntity>, IHomeConfigurationsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeConfigurationsRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public HomeConfigurationsRepository(
            ILogger<HomeConfigurationsRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 HomeConfigurationsTableMetadata.TableName,
                 HomeConfigurationsTableMetadata.PartitionKey,
                 repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}
