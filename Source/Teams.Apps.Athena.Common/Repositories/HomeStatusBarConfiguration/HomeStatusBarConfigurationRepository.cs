// <copyright file="HomeStatusBarConfigurationRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The repository for managing table operations related to home status bar configuration.
    /// </summary>
    public class HomeStatusBarConfigurationRepository : BaseRepository<HomeStatusBarConfigurationEntity>, IHomeStatusBarConfigurationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeStatusBarConfigurationRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public HomeStatusBarConfigurationRepository(
            ILogger<HomeStatusBarConfigurationRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  repositoryOptions.Value.StorageAccountConnectionString,
                  HomeStatusBarConfigurationTableMetadata.TableName,
                  HomeStatusBarConfigurationTableMetadata.PartitionKey,
                  repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}
