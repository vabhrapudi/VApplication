// <copyright file="AthenaInfoResourcesHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The helper methods related to Athena info resources.
    /// </summary>
    public class AthenaInfoResourcesHelper : IAthenaInfoResourcesHelper
    {
        /// <summary>
        /// Holds the instance of <see cref="AthenaInfoResourcesSearchService"/> class.
        /// </summary>
        private readonly IAthenaInfoResourcesSearchService athenaInfoResourcesSearchService;

        /// <summary>
        /// Holds the instance of <see cref="AthenaInfoResourceMapper"/> class.
        /// </summary>
        private readonly IAthenaInfoResourceMapper athenaInfoResourceMapper;

        /// <summary>
        /// Holds the instance of <see cref="FilterQueryHelper"/> class.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaInfoResourcesHelper"/> class.
        /// </summary>
        /// <param name="athenaInfoResourcesSearchService">The instance of <see cref="AthenaInfoResourcesSearchService"/> class.</param>
        /// <param name="athenaInfoResourceMapper">The instance of <see cref="AthenaInfoResourceMapper"/> class.</param>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        public AthenaInfoResourcesHelper (
            IAthenaInfoResourcesSearchService athenaInfoResourcesSearchService,
            IAthenaInfoResourceMapper athenaInfoResourceMapper,
            IFilterQueryHelper filterQueryHelper)
        {
            this.athenaInfoResourcesSearchService = athenaInfoResourcesSearchService;
            this.athenaInfoResourceMapper = athenaInfoResourceMapper;
            this.filterQueryHelper = filterQueryHelper;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AthenaInfoResourceDTO>> GetAthenaInfoResourcesAsync(SearchParametersDTO searchParametersDTO)
        {
            var athenaInfoResources = await this.athenaInfoResourcesSearchService.GetAthenaInfoResourcesAsync(searchParametersDTO);
            return athenaInfoResources.Select(x => this.athenaInfoResourceMapper.MapForViewModel(x));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AthenaInfoResourceDTO>> GetAthenaInfoResourcesAsync(IEnumerable<int> keywords, DateTime fromDate, int? count)
        {
            var dateFilter = this.filterQueryHelper.GetFilterConditionForDate(nameof(AthenaInfoResourceEntity.LastUpdate), QueryComparisons.GreaterThanOrEqual, new[] { fromDate.ToZuluTimeFormatWithStartOfDay() });
            var keywordsFilter = this.filterQueryHelper.GetFilterConditionForExactStringMatch(nameof(AthenaInfoResourceEntity.Keywords), keywords);

            var filter = this.filterQueryHelper.CombineFilters(dateFilter, keywordsFilter, TableOperators.And);

            var searchParametersDto = new SearchParametersDTO
            {
                TopRecordsCount = count,
                Filter = filter,
                OrderBy = new List<string> { $"{nameof(AthenaInfoResourceEntity.LastUpdate)} desc" },
            };

            var infoResources = await this.athenaInfoResourcesSearchService.GetAthenaInfoResourcesAsync(searchParametersDto);
            return infoResources.Select(x => this.athenaInfoResourceMapper.MapForViewModel(x));
        }
    }
}
