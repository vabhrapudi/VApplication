// <copyright file="CoiRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Manages table operations on COI table.
    /// </summary>
    public class CoiRepository : BaseRepository<CommunityOfInterestEntity>, ICoiRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoiRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">The options used to create repository.</param>
        public CoiRepository(
            ILogger<CoiRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                  logger,
                  repositoryOptions.Value.StorageAccountConnectionString,
                  CoiTableMetadata.TableName,
                  CoiTableMetadata.PartitionKey,
                  repositoryOptions.Value.EnsureTableExists)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CommunityOfInterestEntity>> GetActiveCoiRequestsAsync(Guid userAadId)
        {
            var requestCreatedByUserFilterCondition = TableQuery.GenerateFilterCondition("CreatedByObjectId", QueryComparisons.Equal, userAadId.ToString());
            var requestNotDeletedFilterCondition = TableQuery.GenerateFilterConditionForBool("IsDeleted", QueryComparisons.Equal, false);

            var filter = TableQuery.CombineFilters(requestCreatedByUserFilterCondition, TableOperators.And, requestNotDeletedFilterCondition);

            return await this.GetWithFilterAsync(filter);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CommunityOfInterestEntity>> GetActiveCoiRequestsAsync(IEnumerable<string> coiRequestIds, Guid userAadId)
        {
            var requestCreatedByUserFilterCondition = TableQuery.GenerateFilterCondition("CreatedByObjectId", QueryComparisons.Equal, userAadId.ToString());
            var requestNotDeletedFilterCondition = TableQuery.GenerateFilterConditionForBool("IsDeleted", QueryComparisons.Equal, false);
            var coiRequestIdsFilterCondition = this.GetRowKeysFilter(coiRequestIds);

            var createdByAndIsDeletedFilter = TableQuery.CombineFilters(requestCreatedByUserFilterCondition, TableOperators.And, requestNotDeletedFilterCondition);

            var filter = TableQuery.CombineFilters(createdByAndIsDeletedFilter, TableOperators.And, coiRequestIdsFilterCondition);

            return await this.GetWithFilterAsync(filter);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CommunityOfInterestEntity>> GetActiveCoiRequestsAsync(string name)
        {
            var requestNameFilterCondition = TableQuery.GenerateFilterCondition("CoiName", QueryComparisons.Equal, name.Trim());
            var requestNotDeletedFilterCondition = TableQuery.GenerateFilterConditionForBool("IsDeleted", QueryComparisons.Equal, false);

            var filter = TableQuery.CombineFilters(requestNameFilterCondition, TableOperators.And, requestNotDeletedFilterCondition);

            return await this.GetWithFilterAsync(filter);
        }

        /// <inheritdoc/>
        public string GetCoiFilterAsync(string teamId)
        {
            List<string> teamIdList = new List<string>();
            teamIdList.Add(teamId);
            var coiFilter = this.GetCustomColumnFilter(teamIdList, nameof(CommunityOfInterestEntity.TeamId));
            return coiFilter;
        }

        /// <inheritdoc/>
        public async Task<CommunityOfInterestEntity> GetCommunityByIdAsync(int coiId)
        {
            var coiIdFilter = TableQuery.GenerateFilterConditionForInt(
                       nameof(CommunityOfInterestEntity.CoiId),
                       QueryComparisons.Equal,
                       coiId);
            var communityOfInterests = await this.GetWithFilterAsync(coiIdFilter);
            return communityOfInterests.FirstOrDefault();
        }
    }
}