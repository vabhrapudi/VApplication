// <copyright file="HomeStatusBarConfigurationHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The helper methods related to home status bar configuration.
    /// </summary>
    public class HomeStatusBarConfigurationHelper : IHomeStatusBarConfigurationHelper
    {
        private readonly IHomeStatusBarConfigurationRepository homeStatusBarConfigurationRepository;
        private readonly IHomeStatusBarConfigurationMapper homeStatusBarConfigurationMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeStatusBarConfigurationHelper"/> class.
        /// </summary>
        /// <param name="homeStatusBarConfigurationRepository">The instance of <see cref="HomeStatusBarConfigurationRepository"/> class.</param>
        /// <param name="homeStatusBarConfigurationMapper">The instance of <see cref="HomeStatusBarConfigurationMapper"/> class.</param>
        public HomeStatusBarConfigurationHelper(
            IHomeStatusBarConfigurationRepository homeStatusBarConfigurationRepository,
            IHomeStatusBarConfigurationMapper homeStatusBarConfigurationMapper)
        {
            this.homeStatusBarConfigurationRepository = homeStatusBarConfigurationRepository;
            this.homeStatusBarConfigurationMapper = homeStatusBarConfigurationMapper;
        }

        /// <inheritdoc/>
        public async Task<HomeStatusBarConfigurationDTO> CreateHomeStatusBarConfigurationAsync(HomeStatusBarConfigurationDTO homeStatusBarConfigurationDTO, string teamId, string userAadId)
        {
            homeStatusBarConfigurationDTO = homeStatusBarConfigurationDTO ?? throw new ArgumentNullException(nameof(homeStatusBarConfigurationDTO));

            var homeStatusBarConfigurationDetails = this.homeStatusBarConfigurationMapper.MapForCreateModel(homeStatusBarConfigurationDTO, teamId, userAadId);

            var createdHomeStatusBarConfiguration = await this.homeStatusBarConfigurationRepository.CreateOrUpdateAsync(homeStatusBarConfigurationDetails);

            return this.homeStatusBarConfigurationMapper.MapForViewModel(createdHomeStatusBarConfiguration);
        }

        /// <inheritdoc/>
        public async Task<HomeStatusBarConfigurationDTO> UpdateHomeStatusBarConfigurationAsync(HomeStatusBarConfigurationDTO homeStatusBarConfigurationDTO, string teamId, string userAadId)
        {
            homeStatusBarConfigurationDTO = homeStatusBarConfigurationDTO ?? throw new ArgumentNullException(nameof(homeStatusBarConfigurationDTO));

            var existingHomeStatusBarConfigurationDetails = await this.homeStatusBarConfigurationRepository.GetAsync(HomeStatusBarConfigurationTableMetadata.PartitionKey, teamId);

            if (existingHomeStatusBarConfigurationDetails == null)
            {
                return null;
            }

            this.homeStatusBarConfigurationMapper.MapForUpdateModel(homeStatusBarConfigurationDTO, existingHomeStatusBarConfigurationDetails, userAadId);

            var updatedHomeStatusBarConfiguration = await this.homeStatusBarConfigurationRepository.CreateOrUpdateAsync(existingHomeStatusBarConfigurationDetails);

            return this.homeStatusBarConfigurationMapper.MapForViewModel(updatedHomeStatusBarConfiguration);
        }

        /// <inheritdoc/>
        public async Task<HomeStatusBarConfigurationDTO> GetHomeStatusBarConfigurationAsync(string teamId)
        {
            var homeStatusBarConfigurationDetails = await this.homeStatusBarConfigurationRepository.GetAsync(HomeStatusBarConfigurationTableMetadata.PartitionKey, teamId);

            if (homeStatusBarConfigurationDetails == null)
            {
                return null;
            }

            return this.homeStatusBarConfigurationMapper.MapForViewModel(homeStatusBarConfigurationDetails);
        }
    }
}
