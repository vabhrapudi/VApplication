// <copyright file="QuickAccessHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Mappers;

    /// <summary>
    /// The helper methods related to quick access.
    /// </summary>
    public class QuickAccessHelper : IQuickAccessHelper
    {
        /// <summary>
        /// The instance of quick access entity model repository.
        /// </summary>
        private readonly IQuickAccessRepository quickAccessRepository;

        /// <summary>
        /// The instance of quick access mapper.
        /// </summary>
        private readonly IQuickAccessMapper quickAccessMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickAccessHelper"/> class.
        /// </summary>
        /// <param name="quickAccessRepository">The instance of the <see cref="QuickAccessRepository"/> class.</param>
        /// <param name="quickAccessMapper">The instance of the <see cref="QuickAccessMapper"/> class.</param>
        public QuickAccessHelper(
            IQuickAccessRepository quickAccessRepository,
            IQuickAccessMapper quickAccessMapper)
        {
            this.quickAccessRepository = quickAccessRepository;
            this.quickAccessMapper = quickAccessMapper;
        }

        /// <inheritdoc/>
        public async Task<QuickAccessEntity> AddQuickAccessItemAsync(QuickAccessItemCreateDTO quickAccessItem, string userId)
        {
            quickAccessItem = quickAccessItem ?? throw new ArgumentNullException(nameof(quickAccessItem));

            var existingEntity = await this.quickAccessRepository.GetQuickAccessItemByTaxonomyIdAsync(quickAccessItem.TaxonomyId, userId);
            if (existingEntity != null)
            {
                return null;
            }

            var quickAccessEntity = this.quickAccessMapper.MapForCreateModel(quickAccessItem, userId);
            return await this.quickAccessRepository.CreateOrUpdateAsync(quickAccessEntity);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<QuickAccessEntity>> GetQuickAccessListAsync(string userId)
        {
            return this.quickAccessRepository.GetQuickAccessListByUserIdAsync(userId);
        }

        /// <inheritdoc/>
        public async Task DeleteQuickAccessItemAsync(string quickAccessItemId)
        {
            var quickAccessEntity = await this.quickAccessRepository.GetAsync(QuickAccessTableMetadata.PartitionKey, quickAccessItemId);
            if (quickAccessEntity != null)
            {
                await this.quickAccessRepository.DeleteAsync(quickAccessEntity);
            }
        }
    }
}
