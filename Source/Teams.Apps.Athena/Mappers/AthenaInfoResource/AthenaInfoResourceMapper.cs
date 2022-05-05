// <copyright file="AthenaInfoResourceMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related Athena info resource entity model mappings.
    /// </summary>
    public class AthenaInfoResourceMapper : IAthenaInfoResourceMapper
    {
        private const string KeywordsSeparator = " ";

        /// <inheritdoc/>
        public AthenaInfoResourceDTO MapForViewModel(AthenaInfoResourceEntity athenaInfoResourceEntity)
        {
            if (athenaInfoResourceEntity == null)
            {
                throw new ArgumentNullException(nameof(athenaInfoResourceEntity));
            }

            return new AthenaInfoResourceDTO
            {
                TableId = athenaInfoResourceEntity.TableId,
                AuthorIds = string.IsNullOrWhiteSpace(athenaInfoResourceEntity.AuthorIds) ? Array.Empty<int>() : Array.ConvertAll(athenaInfoResourceEntity.AuthorIds.Split(KeywordsSeparator), int.Parse),
                Keywords = string.IsNullOrWhiteSpace(athenaInfoResourceEntity.Keywords) ? Array.Empty<int>() : Array.ConvertAll(athenaInfoResourceEntity.Keywords.Split(KeywordsSeparator), int.Parse),
                Authors = athenaInfoResourceEntity.Authors,
                Sponsors = athenaInfoResourceEntity.Sponsors,
                SponsorIds = string.IsNullOrWhiteSpace(athenaInfoResourceEntity.SponsorIds) ? Array.Empty<int>() : Array.ConvertAll(athenaInfoResourceEntity.SponsorIds.Split(KeywordsSeparator), int.Parse),
                SecurityLevel = athenaInfoResourceEntity.SecurityLevel,
                SourceGroup = athenaInfoResourceEntity.SourceGroup,
                SourceOrg = athenaInfoResourceEntity.SourceOrg,
                SubmitterId = athenaInfoResourceEntity.SubmitterId,
                IsPartOfSeries = athenaInfoResourceEntity.IsPartOfSeries,
                ResearchSourceId = athenaInfoResourceEntity.ResearchSourceId,
                AvgUserRating = athenaInfoResourceEntity.AvgUserRating,
                Collection = athenaInfoResourceEntity.Collection,
                Description = athenaInfoResourceEntity.Description,
                DocId = athenaInfoResourceEntity.DocId,
                InfoResourceId = athenaInfoResourceEntity.InfoResourceId,
                PublishedDate = athenaInfoResourceEntity.PublishedDate,
                LastUpdate = athenaInfoResourceEntity.LastUpdate,
                NodeTypeId = athenaInfoResourceEntity.NodeTypeId,
                Provenance = athenaInfoResourceEntity.Provenance,
                Publisher = athenaInfoResourceEntity.Publisher,
                UsageLicensing = athenaInfoResourceEntity.UsageLicensing,
                UserComments = athenaInfoResourceEntity.UserComments,
                Title = athenaInfoResourceEntity.Title,
                Website = athenaInfoResourceEntity.Website,
            };
        }
    }
}
