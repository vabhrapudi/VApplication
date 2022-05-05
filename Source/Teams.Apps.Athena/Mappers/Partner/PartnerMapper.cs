// <copyright file="PartnerMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related partners entity model mappings.
    /// </summary>
    public class PartnerMapper : IPartnerMapper
    {
        private const string KeywordsSeparator = " ";

        /// <inheritdoc/>
        public PartnerDTO MapForViewModel(PartnerEntity partnerEntityModel)
        {
            if (partnerEntityModel == null)
            {
                throw new ArgumentNullException(nameof(partnerEntityModel));
            }

            return new PartnerDTO
            {
                TableId = partnerEntityModel.TableId,
                SecurityLevel = partnerEntityModel.SecurityLevel,
                Keywords = string.IsNullOrWhiteSpace(partnerEntityModel.Keywords) ? Array.Empty<int>() : Array.ConvertAll(partnerEntityModel.Keywords.Split(KeywordsSeparator), int.Parse),
                Description = partnerEntityModel.Description,
                PartnerId = partnerEntityModel.PartnerId,
                FirstName = partnerEntityModel.FirstName,
                LastName = partnerEntityModel.LastName,
                Title = partnerEntityModel.Title,
                NodeTypeId = partnerEntityModel.NodeTypeId,
                Phone = partnerEntityModel.Phone,
                OtherContactInfo = partnerEntityModel.OtherContactInfo,
                Organization = partnerEntityModel.Organization,
                Projects = partnerEntityModel.Projects,
                NumberOfRatings = partnerEntityModel.NumberOfRatings,
                SumOfRatings = partnerEntityModel.SumOfRatings,
            };
        }
    }
}
