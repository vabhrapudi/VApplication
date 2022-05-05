// <copyright file="AthenaEventMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related Athena event entity model mappings.
    /// </summary>
    public class AthenaEventMapper : IAthenaEventMapper
    {
        private const string KeywordsSeparator = " ";

        /// <inheritdoc/>
        public EventDTO MapForViewModel(EventEntity eventEntity)
        {
            eventEntity = eventEntity ?? throw new ArgumentNullException(nameof(eventEntity));

            return new EventDTO
            {
                TableId = eventEntity.TableId,
                EventId = eventEntity.EventId,
                NodeTypeId = eventEntity.NodeTypeId,
                SecurityLevel = eventEntity.SecurityLevel,
                Keywords = string.IsNullOrWhiteSpace(eventEntity.Keywords) ? Array.Empty<int>() : Array.ConvertAll(eventEntity.Keywords.Split(KeywordsSeparator), int.Parse),
                LastUpdate = eventEntity.LastUpdate,
                DateOfEvent = eventEntity.DateOfEvent,
                Title = eventEntity.Title,
                Description = eventEntity.Description,
                Organization = eventEntity.Organization,
                Location = eventEntity.Location,
                WebSite = eventEntity.WebSite,
                OtherContactInfo = eventEntity.OtherContactInfo,
                NumberOfRatings = eventEntity.NumberOfRatings,
                SumOfRatings = eventEntity.SumOfRatings,
            };
        }
    }
}
