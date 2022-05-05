// <copyright file="ResearchRequestsRepository.cs" company="NPS Foundation">
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
    /// The repository for managing table operations related to research requests.
    /// </summary>
    public class ResearchRequestsRepository : BaseRepository<ResearchRequestEntity>, IResearchRequestsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchRequestsRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public ResearchRequestsRepository(
            ILogger<ResearchRequestsRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 ResearchRequestsTableMetadata.TableName,
                 ResearchRequestsTableMetadata.PartitionKey,
                 repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<ResearchRequestEntity> GetResearchRequestDetailsByIdAsync(int researchRequestId)
        {
            var researchRequestIdFilter = TableQuery.GenerateFilterConditionForInt(
                        nameof(ResearchRequestEntity.ResearchRequestId),
                        QueryComparisons.Equal,
                        researchRequestId);
            var researchRequest = await this.GetWithFilterAsync(researchRequestIdFilter);
            return researchRequest.FirstOrDefault();
        }
    }
}
