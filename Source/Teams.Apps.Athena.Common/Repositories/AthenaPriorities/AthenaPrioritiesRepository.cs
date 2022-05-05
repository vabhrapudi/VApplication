// <copyright file="AthenaPrioritiesRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The repository for managing table operations related to Athena priorities.
    /// </summary>
    public class AthenaPrioritiesRepository : BaseRepository<PriorityEntity>, IAthenaPrioritiesRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaPrioritiesRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public AthenaPrioritiesRepository(
             ILogger<AthenaPrioritiesRepository> logger,
             IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaPrioritiesTableMetadata.TableName,
                 AthenaPrioritiesTableMetadata.PartitionKey,
                 repositoryOptions.Value.EnsureTableExists)
        {
        }
    }
}
