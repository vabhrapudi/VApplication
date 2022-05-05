// <copyright file="PriorityHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Constants;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide helper method associated with priority entity operation.
    /// </summary>
    public class PriorityHelper : IPriorityHelper
    {
        private const int AllFilterDbValue = -1;

        private readonly IAthenaPrioritiesRepository athenaPrioritiesRepository;
        private readonly IPriorityMapper priorityMapper;
        private readonly IAthenaPriorityTypesBlobRepository athenaPriorityTypesBlobRepository;
        private readonly IFilterQueryHelper filterQueryHelper;
        private readonly IDiscoveryTreeHelper discoveryTreeHelper;
        private readonly IResearchProjectHelper researchProjectHelper;
        private readonly IResearchRequestHelper researchRequestHelper;
        private readonly IResearchProposalHelper researchProposalHelper;
        private readonly IDiscoveryTreeFiltersBlobRepository discoveryTreeFiltersBlobRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityHelper"/> class.
        /// </summary>
        /// <param name="athenaPrioritiesRepository">The instance of <see cref="AthenaPrioritiesRepository"/> class.</param>
        /// <param name="priorityMapper">The instance of <see cref="PriorityMapper"/> class.</param>
        /// <param name="athenaPriorityTypesBlobRepository">The instance of <see cref="AthenaPriorityTypesBlobRepository"/> class.</param>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="discoveryTreeHelper">The instance of <see cref="discoveryTreeHelper"/> class.</param>
        /// <param name="researchProjectHelper">The instance of <see cref="ResearchProjectHelper"/> class.</param>
        /// <param name="researchRequestHelper">The instance of <see cref="ResearchRequestHelper"/> class.</param>
        /// <param name="researchProposalHelper">The instance of <see cref="ResearchProposalHelper"/> class.</param>
        /// <param name="discoveryTreeFiltersBlobRepository">The instance of <see cref="DiscoveryTreeFiltersBlobRepository"/> class.</param>
        public PriorityHelper(
            IAthenaPrioritiesRepository athenaPrioritiesRepository,
            IPriorityMapper priorityMapper,
            IAthenaPriorityTypesBlobRepository athenaPriorityTypesBlobRepository,
            IFilterQueryHelper filterQueryHelper,
            IDiscoveryTreeHelper discoveryTreeHelper,
            IResearchProjectHelper researchProjectHelper,
            IResearchRequestHelper researchRequestHelper,
            IResearchProposalHelper researchProposalHelper,
            IDiscoveryTreeFiltersBlobRepository discoveryTreeFiltersBlobRepository)
        {
            this.athenaPrioritiesRepository = athenaPrioritiesRepository;
            this.priorityMapper = priorityMapper;
            this.athenaPriorityTypesBlobRepository = athenaPriorityTypesBlobRepository;
            this.filterQueryHelper = filterQueryHelper;
            this.discoveryTreeHelper = discoveryTreeHelper;
            this.researchProjectHelper = researchProjectHelper;
            this.researchRequestHelper = researchRequestHelper;
            this.researchProposalHelper = researchProposalHelper;
            this.discoveryTreeFiltersBlobRepository = discoveryTreeFiltersBlobRepository;
        }

        /// <inheritdoc/>
        public async Task<PriorityDTO> CreatePriorityAsync(PriorityDTO priorityDTO, string userAadId)
        {
            priorityDTO = priorityDTO ?? throw new ArgumentNullException(nameof(priorityDTO));

            var existingPriorities = await this.athenaPrioritiesRepository.GetAllAsync();
            var existingPrioritiesOfSameType = existingPriorities.Where(x => x.Type == priorityDTO.Type);

            // Checking if the number of priorities under a priority type exceeds the max number.
            if (existingPrioritiesOfSameType.Count() >= Constants.MaxNumberOfPriorities)
            {
                return null;
            }

            var priorityToCreate = this.priorityMapper.MapForCreateModel(priorityDTO, userAadId);
            var updatedPriority = await this.athenaPrioritiesRepository.CreateOrUpdateAsync(priorityToCreate);

            return this.priorityMapper.MapForViewModel(updatedPriority);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PriorityDTO>> GetPrioritiesAsync()
        {
            var priorities = await this.athenaPrioritiesRepository.GetAllAsync();
            return priorities
                .OrderBy(x => x.CreatedAt)
                .Select(x => this.priorityMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<PriorityDTO> GetPriorityByIdAsync(string priorityId)
        {
            priorityId = priorityId ?? throw new ArgumentNullException(nameof(priorityId));
            var priority = await this.athenaPrioritiesRepository.GetAsync(AthenaPrioritiesTableMetadata.PartitionKey, priorityId);
            if (priority == null)
            {
                return null;
            }

            return this.priorityMapper.MapForViewModel(priority);
        }

        /// <inheritdoc/>
        public async Task<PriorityDTO> UpdatePriorityAsync(PriorityDTO priorityDTO, string userAadId)
        {
            priorityDTO = priorityDTO ?? throw new ArgumentNullException(nameof(priorityDTO));

            var existingPriority = await this.athenaPrioritiesRepository.GetAsync(AthenaPrioritiesTableMetadata.PartitionKey, priorityDTO.Id);
            if (existingPriority == null)
            {
                return null;
            }

            var existingPriorities = await this.athenaPrioritiesRepository.GetAllAsync();
            var existingPrioritiesOfSameType = existingPriorities.Where(x => x.Type == priorityDTO.Type);

            // Checking if the number of priorities under a priority type exceeds the max number.
            if (existingPrioritiesOfSameType.Count() >= Constants.MaxNumberOfPriorities && priorityDTO.Type != existingPriority.Type)
            {
                return null;
            }

            var priotityToUpdate = this.priorityMapper.MapForUpdateModel(priorityDTO, existingPriority, userAadId);
            var updatedPriority = await this.athenaPrioritiesRepository.CreateOrUpdateAsync(priotityToUpdate);

            return this.priorityMapper.MapForViewModel(updatedPriority);
        }

        /// <inheritdoc/>
        public async Task DeletePrioritiesAsync(IEnumerable<Guid> priorityIds)
        {
            var priorityIdsFilter = this.athenaPrioritiesRepository.GetRowKeysFilter(priorityIds.Select(x => x.ToString()));

            var priorities = await this.athenaPrioritiesRepository.GetWithFilterAsync(priorityIdsFilter);

            await this.athenaPrioritiesRepository.BatchDeleteAsync(priorities);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PriorityType>> GetPriorityTypesAsync()
        {
            return await this.athenaPriorityTypesBlobRepository.GetBlobJsonFileContentAsync(AthenaPriorityTypesBlobMetadata.FileName);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PriorityInsight>> GetPrioritiesInsightsAsync(IEnumerable<Guid> priorityIds, IEnumerable<int> keywordIds)
        {
            if (priorityIds.IsNullOrEmpty() || keywordIds.IsNullOrEmpty())
            {
                return Enumerable.Empty<PriorityInsight>();
            }

            var filters = await this.discoveryTreeFiltersBlobRepository.GetBlobJsonFileContentAsync(DiscoveryTreeFiltersBlobMetadata.FileName);

            // Get status filters except 'All' filter.
            filters = filters.Where(x => (x.FilterId == Constants.ProposedResearchFilterId || x.FilterId == Constants.InProgressResearchFilterId || x.FilterId == Constants.CompletedResearchFilterId)
                && !x.DbValue.IsNullOrEmpty()
                && x.DbValue.First() != AllFilterDbValue);

            if (filters.IsNullOrEmpty())
            {
                return Enumerable.Empty<PriorityInsight>();
            }

            var nodeTypes = await this.discoveryTreeHelper.GetDiscoveryTreeNodeTypeAsync();

            var dictionary = new Dictionary<string, Dictionary<string, List<string>>>();
            var filterTypesByFile = new Dictionary<string, List<string>>();

            foreach (var filter in filters)
            {
                this.PrepareDictionary(filter, dictionary, FileNames.AthenaResearchProjects, nodeTypes, filterTypesByFile);
                this.PrepareDictionary(filter, dictionary, FileNames.AthenaResearchRequests, nodeTypes, filterTypesByFile);
                this.PrepareDictionary(filter, dictionary, FileNames.AthenaResearchProposals, nodeTypes, filterTypesByFile);
            }

            var prioritiesFilter = this.athenaPrioritiesRepository.GetRowKeysFilter(priorityIds.Select(x => x.ToString()));

            var priorities = await this.athenaPrioritiesRepository.GetWithFilterAsync(prioritiesFilter);
            priorities = priorities.OrderBy(x => x.CreatedAt);

            var prioritiesData = new List<PriorityInsight>();

            var searchParametersDto = new SearchParametersDTO
            {
                IsGetAllRecords = true,
            };

            foreach (var priority in priorities)
            {
                var insights = new PriorityInsight
                {
                    Title = priority.Title,
                };

                if (string.IsNullOrEmpty(priority.Keywords))
                {
                    prioritiesData.Add(insights);
                    continue;
                }

                var keywords = Array.ConvertAll(priority.Keywords.Split(" "), int.Parse);
                keywords = keywords.Intersect(keywordIds).ToArray();

                if (keywords.IsNullOrEmpty())
                {
                    prioritiesData.Add(insights);
                    continue;
                }

                foreach (var filter in filters)
                {
                    var totalCount = 0;

                    var athenaResearchProjectsQuery = this.GetQuery(dictionary, FileNames.AthenaResearchProjects, filter.Title);

                    if (!athenaResearchProjectsQuery.IsNullOrEmpty())
                    {
                        var keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(ResearchProjectEntity.Keywords), keywords);
                        searchParametersDto.Filter = this.filterQueryHelper.CombineFilters(keywordsFilter, athenaResearchProjectsQuery, TableOperators.And);

                        var researchProjects = await this.researchProjectHelper.GetResearchProjectsAsync(searchParametersDto);
                        totalCount += researchProjects.Count();
                    }

                    var athenaResearchRequestsQuery = this.GetQuery(dictionary, FileNames.AthenaResearchRequests, filter.Title);

                    if (!athenaResearchRequestsQuery.IsNullOrEmpty())
                    {
                        var keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(ResearchRequestEntity.Keywords), keywords);
                        searchParametersDto.Filter = this.filterQueryHelper.CombineFilters(keywordsFilter, athenaResearchRequestsQuery, TableOperators.And);

                        var researchRequests = await this.researchRequestHelper.GetResearchRequestsAsync(searchParametersDto);
                        totalCount += researchRequests.Count();
                    }

                    var athenaResearchProposalsQuery = this.GetQuery(dictionary, FileNames.AthenaResearchProposals, filter.Title);

                    if (!athenaResearchProposalsQuery.IsNullOrEmpty())
                    {
                        var keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(ResearchProposalEntity.Keywords), keywords);
                        searchParametersDto.Filter = this.filterQueryHelper.CombineFilters(keywordsFilter, athenaResearchProposalsQuery, TableOperators.And);

                        var researchProposals = await this.researchProposalHelper.GetResearchProposalsAsync(searchParametersDto);
                        totalCount += researchProposals.Count();
                    }

                    if (filter.FilterId == Constants.ProposedResearchFilterId)
                    {
                        insights.Proposed += totalCount;
                    }
                    else if (filter.FilterId == Constants.InProgressResearchFilterId)
                    {
                        insights.Current += totalCount;
                    }
                    else if (filter.FilterId == Constants.CompletedResearchFilterId)
                    {
                        insights.Completed += totalCount;
                    }
                }

                prioritiesData.Add(insights);
            }

            return prioritiesData;
        }

        private void PrepareDictionary(DiscoveryTreeFilterItems filter, Dictionary<string, Dictionary<string, List<string>>> dictionary, string resultType, IEnumerable<NodeType> nodeTypes, Dictionary<string, List<string>> filterTypesByFile)
        {
            var entityFilters = filter.DbEntity.Split(",").Any(y => y.Trim() == resultType);

            if (!entityFilters)
            {
                return;
            }

            var resultList = this.discoveryTreeHelper.GetFilterList(new[] { filter }, resultType, nodeTypes, filterTypesByFile);

            if (dictionary.ContainsKey(filter.Title))
            {
                if (dictionary[filter.Title].ContainsKey(resultType))
                {
                    dictionary[filter.Title][resultType].AddRange(resultList);
                }
                else
                {
                    dictionary[filter.Title].Add(resultType, resultList);
                }
            }
            else
            {
                dictionary.Add(filter.Title, new Dictionary<string, List<string>> { { resultType, resultList } });
            }
        }

        private string GetQuery(Dictionary<string, Dictionary<string, List<string>>> dictionary, string resultType, string status)
        {
            if (!dictionary.ContainsKey(status) || !dictionary[status].ContainsKey(resultType))
            {
                return null;
            }

            var queries = dictionary[status][resultType];

            if (queries.IsNullOrEmpty())
            {
                return null;
            }

            return "(" + string.Join($" {TableOperators.Or} ", queries) + ")";
        }
    }
}
