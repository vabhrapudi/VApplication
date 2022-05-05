// <copyright file="ResearchProjectMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using System.Linq;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related research projects entity model mappings.
    /// </summary>
    public class ResearchProjectMapper : IResearchProjectMapper
    {
        private const string KeywordsSeparator = " ";

        /// <inheritdoc/>
        public ResearchProjectDTO MapForViewModel(ResearchProjectEntity researchProjectEntity)
        {
            researchProjectEntity = researchProjectEntity ?? throw new ArgumentNullException(nameof(researchProjectEntity));

            return new ResearchProjectDTO
            {
                TableId = researchProjectEntity.TableId,
                ResearchProjectId = researchProjectEntity.ResearchProjectId,
                NodeTypeId = researchProjectEntity.NodeTypeId,
                Status = researchProjectEntity.Status,
                Title = researchProjectEntity.Title,
                Abstract = researchProjectEntity.Abstract,
                Keywords = string.IsNullOrWhiteSpace(researchProjectEntity.Keywords) ? Array.Empty<int>() : Array.ConvertAll(researchProjectEntity.Keywords.Split(KeywordsSeparator), int.Parse),
                SumOfRatings = researchProjectEntity.SumOfRatings,
                NumberOfRatings = researchProjectEntity.NumberOfRatings,
                DateStarted = researchProjectEntity.DateStarted,
                Files = researchProjectEntity.Files,
                LastUpdate = researchProjectEntity.LastUpdate,
                Advisors = researchProjectEntity.Advisors,
                Authors = researchProjectEntity.Authors,
                AuthorsOrg = researchProjectEntity.AuthorsOrg,
                DateCompleted = researchProjectEntity.DateCompleted,
                DegreeLevel = researchProjectEntity.DegreeLevel,
                DegreeProgram = researchProjectEntity.DegreeProgram,
                DegreeTitles = researchProjectEntity.DegreeTitles,
                OriginatingRequest = researchProjectEntity.OriginatingRequest,
                Publisher = researchProjectEntity.Publisher,
                Recognition = researchProjectEntity.Recognition,
                ResearchDept = researchProjectEntity.ResearchDept,
                ReviewerNotes = researchProjectEntity.ReviewerNotes,
                SecondReaders = researchProjectEntity.SecondReaders,
                StatusDescription = researchProjectEntity.StatusDescription,
                UseRights = researchProjectEntity.UseRights,
                Priority = researchProjectEntity.Priority,
                Importance = researchProjectEntity.Importance,
            };
        }

        /// <inheritdoc/>
        public ResearchProjectEntity MapForCreateModel(ResearchProjectCreateDTO researchProjectCreateDTO)
        {
            researchProjectCreateDTO = researchProjectCreateDTO ?? throw new ArgumentNullException(nameof(researchProjectCreateDTO));

            return new ResearchProjectEntity
            {
                TableId = Guid.NewGuid().ToString(),
                ResearchProjectId = -1,
                Title = researchProjectCreateDTO.Title,
                Abstract = researchProjectCreateDTO.Abstract,
                Keywords = researchProjectCreateDTO.KeywordsJson == null ? null : string.Join(KeywordsSeparator, researchProjectCreateDTO.KeywordsJson.Select(x => x.KeywordId)),
                LastUpdate = DateTime.UtcNow,
                DateStarted = DateTime.UtcNow,
                DateCompleted = DateTime.UtcNow,
                AverageRating = "0",
            };
        }
    }
}
