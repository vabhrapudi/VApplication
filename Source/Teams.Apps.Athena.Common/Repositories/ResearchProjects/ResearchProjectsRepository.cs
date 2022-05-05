// <copyright file="ResearchProjectsRepository.cs" company="NPS Foundation">
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
    /// The repository for managing table operations related to research projects.
    /// </summary>
    public class ResearchProjectsRepository : BaseRepository<ResearchProjectEntity>, IResearchProjectsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchProjectsRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public ResearchProjectsRepository(
            ILogger<ResearchProjectsRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 ResearchProjectsTableMetadata.TableName,
                 ResearchProjectsTableMetadata.PartitionKey,
                 repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<ResearchProjectEntity> GetMatchingResearchProjectAsync(string title)
        {
            var researchProjectTitleFilter = TableQuery.GenerateFilterCondition(
                        nameof(ResearchProjectEntity.Title),
                        QueryComparisons.Equal,
                        title);
            var researchProject = await this.GetWithFilterAsync(researchProjectTitleFilter);
            return researchProject.FirstOrDefault();
        }

        /// <inheritdoc/>
        public async Task<ResearchProjectEntity> GetResearchProjectByProjectIdAsync(int researchProjectId)
        {
            var researchProjectIdFilter = TableQuery.GenerateFilterConditionForInt(
                        nameof(ResearchProjectEntity.ResearchProjectId),
                        QueryComparisons.Equal,
                        researchProjectId);
            var researchProject = await this.GetWithFilterAsync(researchProjectIdFilter);
            return researchProject.FirstOrDefault();
        }
    }
}
