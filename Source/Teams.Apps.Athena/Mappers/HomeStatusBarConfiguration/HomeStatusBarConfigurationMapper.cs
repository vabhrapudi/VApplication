// <copyright file="HomeStatusBarConfigurationMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The mapper methods related to home status bar configurations.
    /// </summary>
    public class HomeStatusBarConfigurationMapper : IHomeStatusBarConfigurationMapper
    {
        /// <inheritdoc/>
        public HomeStatusBarConfigurationEntity MapForCreateModel(HomeStatusBarConfigurationDTO homeStatusBarConfigurationDTO, string teamId, string userAadId)
        {
            homeStatusBarConfigurationDTO = homeStatusBarConfigurationDTO ?? throw new ArgumentNullException(nameof(homeStatusBarConfigurationDTO));

            return new HomeStatusBarConfigurationEntity
            {
                TeamId = teamId,
                Message = homeStatusBarConfigurationDTO.Message.Trim(),
                LinkLabel = homeStatusBarConfigurationDTO.LinkLabel?.Trim(),
                Url = homeStatusBarConfigurationDTO.Url,
                IsActive = homeStatusBarConfigurationDTO.IsActive,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = userAadId,
            };
        }

        /// <inheritdoc/>
        public void MapForUpdateModel(HomeStatusBarConfigurationDTO homeStatusBarConfigurationDTO, HomeStatusBarConfigurationEntity homeStatusBarConfigurationEntity, string userAadId)
        {
            homeStatusBarConfigurationDTO = homeStatusBarConfigurationDTO ?? throw new ArgumentNullException(nameof(homeStatusBarConfigurationDTO));
            homeStatusBarConfigurationEntity = homeStatusBarConfigurationEntity ?? throw new ArgumentNullException(nameof(homeStatusBarConfigurationEntity));

            homeStatusBarConfigurationEntity.Message = homeStatusBarConfigurationDTO.Message;
            homeStatusBarConfigurationEntity.LinkLabel = homeStatusBarConfigurationDTO.LinkLabel;
            homeStatusBarConfigurationEntity.Url = homeStatusBarConfigurationDTO.Url;
            homeStatusBarConfigurationEntity.IsActive = homeStatusBarConfigurationDTO.IsActive;
            homeStatusBarConfigurationEntity.UpdatedAt = DateTime.UtcNow;
            homeStatusBarConfigurationEntity.UpdatedBy = userAadId;
        }

        /// <inheritdoc/>
        public HomeStatusBarConfigurationDTO MapForViewModel(HomeStatusBarConfigurationEntity homeStatusBarConfigurationEntity)
        {
            homeStatusBarConfigurationEntity = homeStatusBarConfigurationEntity ?? throw new ArgumentNullException(nameof(homeStatusBarConfigurationEntity));

            return new HomeStatusBarConfigurationDTO
            {
                TeamId = homeStatusBarConfigurationEntity.TeamId,
                Message = homeStatusBarConfigurationEntity.Message,
                LinkLabel = homeStatusBarConfigurationEntity.LinkLabel,
                Url = homeStatusBarConfigurationEntity.Url,
                IsActive = homeStatusBarConfigurationEntity.IsActive,
            };
        }
    }
}
