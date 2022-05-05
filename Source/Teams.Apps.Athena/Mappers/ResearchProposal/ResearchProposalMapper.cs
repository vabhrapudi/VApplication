// <copyright file="ResearchProposalMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using System.Linq;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Constants;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related research proposals entity model mappings.
    /// </summary>
    public class ResearchProposalMapper : IResearchProposalMapper
    {
        private const string KeywordsSeparator = " ";
        private const string RelatedRequestIdsSeparator = " ";

        /// <inheritdoc/>
        public ResearchProposalEntity MapForCreateModel(ResearchProposalCreateDTO researchProposalCreateDTO, string userAadId)
        {
            researchProposalCreateDTO = researchProposalCreateDTO ?? throw new ArgumentNullException(nameof(researchProposalCreateDTO));

            return new ResearchProposalEntity
            {
                TableId = Guid.NewGuid().ToString(),
                ResearchProposalId = Constants.SourceAthena,
                NodeTypeId = 8,
                LastUpdate = DateTime.UtcNow,
                Title = researchProposalCreateDTO.Title,
                Description = researchProposalCreateDTO.Description,
                Details = researchProposalCreateDTO.Details,
                Budget = researchProposalCreateDTO.Budget,
                PotentialFunding = researchProposalCreateDTO.PotentialFunding,
                SecurityLevel = researchProposalCreateDTO.SecurityLevel,
                StartDate = researchProposalCreateDTO.StartDate,
                Keywords = researchProposalCreateDTO.KeywordsJson == null ? null : string.Join(KeywordsSeparator, researchProposalCreateDTO.KeywordsJson.Select(keyword => keyword.KeywordId)),
                Objectives = researchProposalCreateDTO.Objectives,
                TopicType = researchProposalCreateDTO.TopicType,
                FocusQuestion1 = researchProposalCreateDTO.FocusQuestion1,
                FocusQuestion2 = researchProposalCreateDTO.FocusQuestion2,
                FocusQuestion3 = researchProposalCreateDTO.FocusQuestion3,
                CompletionTime = researchProposalCreateDTO.CompletionTime,
                Plan = researchProposalCreateDTO.Plan,
                Endorsements = researchProposalCreateDTO.Endorsements,
                Deliverables = researchProposalCreateDTO.Deliverables,
                RelatedRequestIds = "0",
                Status = ResearchProposalStatusTypes.Open,
                AverageRating = "0",
                UserId = userAadId,
                Priority = researchProposalCreateDTO.Priority,
            };
        }

        /// <inheritdoc/>
        public ResearchProposalViewDTO MapForViewModel(ResearchProposalEntity researchProposalEntity)
        {
            researchProposalEntity = researchProposalEntity ?? throw new ArgumentNullException(nameof(researchProposalEntity));

            return new ResearchProposalViewDTO
            {
                TableId = researchProposalEntity.TableId,
                ResearchProposalId = researchProposalEntity.ResearchProposalId,
                NodeTypeId = researchProposalEntity.NodeTypeId,
                Keywords = string.IsNullOrWhiteSpace(researchProposalEntity.Keywords) ? Array.Empty<int>() : Array.ConvertAll(researchProposalEntity.Keywords.Split(KeywordsSeparator), int.Parse),
                LastUpdate = researchProposalEntity.LastUpdate,
                Title = researchProposalEntity.Title,
                Status = researchProposalEntity.Status,
                Details = researchProposalEntity.Details,
                Priority = researchProposalEntity.Priority,
                TopicType = researchProposalEntity.TopicType,
                Objectives = researchProposalEntity.Objectives,
                Plan = researchProposalEntity.Plan,
                Deliverables = researchProposalEntity.Deliverables,
                Budget = researchProposalEntity.Budget,
                StartDate = researchProposalEntity.StartDate,
                CompletionTime = researchProposalEntity.CompletionTime,
                Endorsements = researchProposalEntity.Endorsements,
                PotentialFunding = researchProposalEntity.PotentialFunding,
                Description = researchProposalEntity.Description,
                FocusQuestion1 = researchProposalEntity.FocusQuestion1,
                FocusQuestion2 = researchProposalEntity.FocusQuestion2,
                FocusQuestion3 = researchProposalEntity.FocusQuestion3,
                SecurityLevel = researchProposalEntity.SecurityLevel,
                RelatedRequestIds = string.IsNullOrWhiteSpace(researchProposalEntity.RelatedRequestIds) ? Array.Empty<int>() : Array.ConvertAll(researchProposalEntity.RelatedRequestIds.Split(RelatedRequestIdsSeparator), int.Parse),
                SubmitterId = researchProposalEntity.SubmitterId,
                SumOfRatings = researchProposalEntity.SumOfRatings,
                NumberOfRatings = researchProposalEntity.NumberOfRatings,
                UserId = researchProposalEntity.UserId,
            };
        }
    }
}
