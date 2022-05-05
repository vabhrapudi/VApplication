// <copyright file="AthenaIngestionRepository.cs" company="NPS Foundation">
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
    /// The repository for managing table operations related to Athena ingestion.
    /// </summary>
    public class AthenaIngestionRepository : BaseRepository<AthenaIngestionEntity>, IAthenaIngestionRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaIngestionRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public AthenaIngestionRepository(
            ILogger<AthenaIngestionRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaIngestionTableMetadata.TableName,
                 AthenaIngestionTableMetadata.PartitionKey,
                 repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<AthenaIngestionEntity> GetAthenaIngestionByEntityNameAsync(string entityName)
        {
            var entityNameFilter = TableQuery.GenerateFilterCondition(
                        nameof(AthenaIngestionEntity.DbEntity),
                        QueryComparisons.Equal,
                        entityName);
            var athenaIngestion = await this.GetWithFilterAsync(entityNameFilter);
            return athenaIngestion.FirstOrDefault();
        }
    }
}
