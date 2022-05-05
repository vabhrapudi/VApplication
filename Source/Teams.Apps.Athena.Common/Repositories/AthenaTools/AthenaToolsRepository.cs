// <copyright file="AthenaToolsRepository.cs" company="NPS Foundation">
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
    /// The repository for managing table operations related to Athena tools.
    /// </summary>
    public class AthenaToolsRepository : BaseRepository<AthenaToolEntity>, IAthenaToolsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaToolsRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public AthenaToolsRepository(
            ILogger<AthenaToolsRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base
                 (
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaToolsTableMetadata.TableName,
                 AthenaToolsTableMetadata.PartitionKey,
                 repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<AthenaToolEntity> GetAthenaToolByIdAsync(int toolId)
        {
            var toolIdFilter = TableQuery.GenerateFilterConditionForInt(
                        nameof(AthenaToolEntity.ToolId),
                        QueryComparisons.Equal,
                        toolId);
            var tool = await this.GetWithFilterAsync(toolIdFilter);
            return tool.FirstOrDefault();
        }
    }
}
