// <copyright file="SponsorRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the sponsor data stored in the table storage.
    /// </summary>
    public class SponsorRepository : BaseRepository<SponsorEntity>, ISponsorRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SponsorRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public SponsorRepository(
            ILogger<SponsorRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: SponsorTableMetadata.TableName,
                  defaultPartitionKey: SponsorTableMetadata.SponsorPartitionKey,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<SponsorEntity> GetSponsorByIdAsync(int sponsorId)
        {
            var sponsorFilter = TableQuery.GenerateFilterConditionForInt(
                        nameof(SponsorEntity.SponsorId),
                        QueryComparisons.Equal,
                        sponsorId);
            var sponsor = await this.GetWithFilterAsync(sponsorFilter);
            return (SponsorEntity)sponsor;
        }
    }
}
