// <copyright file="SponsorsMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related sponsors entity model mappings.
    /// </summary>
    public class SponsorsMapper : ISponsorsMapper
    {
        private const string KeywordsSeparator = " ";

        /// <inheritdoc/>
        public SponsorDTO MapForViewModel(SponsorEntity sponsorsEntity)
        {
            if (sponsorsEntity == null)
            {
                throw new ArgumentNullException(nameof(sponsorsEntity));
            }

            return new SponsorDTO
            {
                TableId = sponsorsEntity.TableId,
                SecurityLevel = sponsorsEntity.SecurityLevel,
                Service = sponsorsEntity.Service,
                Keywords = string.IsNullOrWhiteSpace(sponsorsEntity.Keywords) ? Array.Empty<int>() : Array.ConvertAll(sponsorsEntity.Keywords.Split(KeywordsSeparator), int.Parse),
                Description = sponsorsEntity.Description,
                SponsorId = sponsorsEntity.SponsorId,
                FirstName = sponsorsEntity.FirstName,
                LastName = sponsorsEntity.LastName,
                Title = sponsorsEntity.Title,
                NodeTypeId = sponsorsEntity.NodeTypeId,
                Phone = sponsorsEntity.Phone,
                OtherContactInfo = sponsorsEntity.OtherContactInfo,
                Organization = sponsorsEntity.Organization,
                NumberOfRatings = sponsorsEntity.NumberOfRatings,
                SumOfRatings = sponsorsEntity.SumOfRatings,
            };
        }
    }
}
