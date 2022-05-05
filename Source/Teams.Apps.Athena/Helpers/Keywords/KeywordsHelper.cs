// <copyright file="KeywordsHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Athena.Models;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Services.Keywords;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing keywords entity.
    /// </summary>
    public class KeywordsHelper : IKeywordsHelper
    {
        private const int KeywordsSearchApiRecordsCountPerPage = 40;
        private const string KeywordsMemoryCacheKey = "keywords";
        private const char KeywordIdsSeparator = ' ';

        /// <summary>
        /// The instance of keywords search services to access search service.
        /// </summary>
        private readonly IKeywordsSearchService keywordsSearchServices;

        /// <summary>
        /// The instance of COI repository.
        /// </summary>
        private readonly ICoiRepository coiRepository;

        /// <summary>
        /// The instance of <see cref="FilterQueryHelper"/> class.
        /// </summary>
        private readonly IFilterQueryHelper filterQueryHelper;

        /// <summary>
        /// The instance of <see cref="KeywordsBlobRepository"/> class.
        /// </summary>
        private readonly IKeywordsBlobRepository keywordsBlobRepository;

        /// <summary>
        /// The instance of <see cref="KeywordMapper"/> class.
        /// </summary>
        private readonly IKeywordMapper keywordMapper;

        /// <summary>
        /// The instance of <see cref="MemoryCache"/> class.
        /// </summary>
        private readonly IMemoryCache memoryCache;

        private readonly IOptions<BotSettings> botOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeywordsHelper"/> class.
        /// </summary>
        /// <param name="keywordsSearchServices">The instance of keywords search services.</param>
        /// <param name="coiRepository">The instance of COI repository.</param>
        /// <param name="filterQueryHelper">The instance of <see cref="FilterQueryHelper"/> class.</param>
        /// <param name="keywordsBlobRepository">The instance of <see cref="KeywordsBlobRepository"/> class.</param>
        /// <param name="keywordMapper">The instance of <see cref="KeywordMapper"/> class.</param>
        /// <param name="memoryCache">Memory cache instance for caching authorization result.</param>
        /// <param name="botOptions">The options for application configuration.</param>
        public KeywordsHelper(
            IKeywordsSearchService keywordsSearchServices,
            ICoiRepository coiRepository,
            IFilterQueryHelper filterQueryHelper,
            IKeywordsBlobRepository keywordsBlobRepository,
            IKeywordMapper keywordMapper,
            IMemoryCache memoryCache,
            IOptions<BotSettings> botOptions)
        {
            this.keywordsSearchServices = keywordsSearchServices;
            this.coiRepository = coiRepository;
            this.filterQueryHelper = filterQueryHelper;
            this.keywordsBlobRepository = keywordsBlobRepository;
            this.keywordMapper = keywordMapper;
            this.memoryCache = memoryCache;
            this.botOptions = botOptions;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<int>> GetCoiTeamKeywordsAsync(string teamId)
        {
            var filterQuery = this.coiRepository.GetCoiFilterAsync(teamId);
            var coiRequest = await this.coiRepository.GetWithFilterAsync(filterQuery);
            return string.IsNullOrWhiteSpace(coiRequest.First().Keywords) ? Array.Empty<int>() : Array.ConvertAll(coiRequest.First().Keywords.Split(KeywordIdsSeparator), int.Parse);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<KeywordEntity>> GetKeywordsAsync(string searchQuery)
        {
            var searchParametersDto = new SearchParametersDTO
            {
                SearchString = searchQuery.EscapeSpecialCharacters(),
                TopRecordsCount = KeywordsSearchApiRecordsCountPerPage,
            };

            return await this.keywordsSearchServices.GetKeywordsAsync(searchParametersDto);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<KeywordEntity>> GetKeywordsByKeywordIdsAsync(IEnumerable<int> keywordIds)
        {
            var keywords = Array.ConvertAll(keywordIds.Distinct().ToArray(), element => $"{element}");
            var keywordsList = new List<KeywordEntity>();

            var keywordsBatches = keywords.Split(400);

            foreach (var keywordsBatch in keywordsBatches)
            {
                var keywordsFilterQuery = this.filterQueryHelper.GetFilterCondition(nameof(KeywordEntity.KeywordId), keywordsBatch);

                SearchParametersDTO searchParametersDTO = new SearchParametersDTO
                {
                    Filter = keywordsFilterQuery,
                };

                var keywordsData = await this.keywordsSearchServices.GetKeywordsAsync(searchParametersDTO);

                if (keywordsData.Any())
                {
                    keywordsList.AddRange(keywordsData);
                }
            }

            return keywordsList;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<KeywordDTO>> GetAllKeywordsAsync()
        {
            bool isValueAvailableInCache = this.memoryCache.TryGetValue(KeywordsMemoryCacheKey, out IEnumerable<KeywordDTO> keywordsInMemory);

            if (isValueAvailableInCache && !keywordsInMemory.IsNullOrEmpty())
            {
                return keywordsInMemory;
            }

            var keywords = await this.keywordsBlobRepository.GetBlobJsonFileContentAsync(KeywordsBlobRepositoryMetadata.FileName);
            var keywordsViewModels = keywords?.Select(x => this.keywordMapper.MapForViewModel(x));

            this.memoryCache.Set(KeywordsMemoryCacheKey, keywordsViewModels, TimeSpan.FromHours(this.botOptions.Value.KeywordsCacheDurationInHours));

            return keywordsViewModels;
        }
    }
}
