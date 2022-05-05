// <copyright file="ResourceFeedbackRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories.ResourceFeedback
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Repository of the team data stored in the table storage.
    /// </summary>
    public class ResourceFeedbackRepository : BaseRepository<ResourceFeedback>, IResourceFeedbackRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceFeedbackRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public ResourceFeedbackRepository(
            ILogger<ResourceFeedbackRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  storageAccountConnectionString: repositoryOptions.Value.StorageAccountConnectionString,
                  tableName: ResourceFeedbackTableNames.TableName,
                  defaultPartitionKey: ResourceFeedbackTableNames.ResourceFeedbackPartition,
                  ensureTableExists: repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <summary>
        /// Fetch resource feedback based on resource type.
        /// </summary>
        /// <param name="resourceType">Resource type Id.</param>
        /// <param name="resourceIds">List of resource Ids</param>
        /// <param name="userObjectId">User object Id.</param>
        /// <returns>List of matching resource feedback entities.</returns>
        public async Task<IEnumerable<ResourceFeedback>> GetFeedbackByResourceTypeAsync(int resourceType, IEnumerable<string> resourceIds, string userObjectId)
        {
            var resourceFilter = this.GetCustomColumnFilter(resourceIds, "ResourceId");
            var resourceTypeFilter = TableQuery.GenerateFilterConditionForInt(
                "ResourceTypeId", QueryComparisons.Equal, resourceType);
            var createdByFilter = TableQuery.GenerateFilterCondition(
                "CreatedBy", QueryComparisons.Equal, userObjectId);
            var finalFilter = TableQuery.CombineFilters(resourceTypeFilter, TableOperators.And, TableQuery.CombineFilters(createdByFilter, TableOperators.And, resourceFilter));

            return await this.GetWithFilterAsync(finalFilter);
        }
    }
}