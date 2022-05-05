// <copyright file="ResearchProposalsRepository.cs" company="NPS Foundation">
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
    /// The repository for managing table operations related to research proposals.
    /// </summary>
    public class ResearchProposalsRepository : BaseRepository<ResearchProposalEntity>, IResearchProposalsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchProposalsRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public ResearchProposalsRepository(
            ILogger<ResearchProposalsRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 ResearchProposalsTableMetadata.TableName,
                 ResearchProposalsTableMetadata.PartitionKey,
                 repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<ResearchProposalEntity> GetResearchProposalAsync(string title)
        {
            var researchProposalTitleFilter = TableQuery.GenerateFilterCondition(
                        nameof(ResearchProposalEntity.Title),
                        QueryComparisons.Equal,
                        title);
            var researchProposal = await this.GetWithFilterAsync(researchProposalTitleFilter);
            return researchProposal.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<ResearchProposalEntity> GetResearchProposalByIdAsync(int researchProposalId)
        {
            var researchProposalIdFilter = TableQuery.GenerateFilterConditionForInt(
                        nameof(ResearchProposalEntity.ResearchProposalId),
                        QueryComparisons.Equal,
                        researchProposalId);
            var researchProposal = await this.GetWithFilterAsync(researchProposalIdFilter);
            return researchProposal.FirstOrDefault();
        }
    }
}