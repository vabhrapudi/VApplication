// <copyright file="ResearchRequestMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related research requests entity model mappings.
    /// </summary>
    public class ResearchRequestMapper : IResearchRequestMapper
    {
        private const string KeywordsSeparator = " ";

        /// <inheritdoc/>
        public ResearchRequestViewDTO MapForViewModel(ResearchRequestEntity researchRequestEntity)
        {
            if (researchRequestEntity == null)
            {
                throw new ArgumentNullException(nameof(researchRequestEntity));
            }

            return new ResearchRequestViewDTO
            {
                TableId = researchRequestEntity.TableId,
                SecurityLevel = researchRequestEntity.SecurityLevel,
                ResearchRequestId = researchRequestEntity.ResearchRequestId,
                Keywords = string.IsNullOrWhiteSpace(researchRequestEntity.Keywords) ? Array.Empty<int>() : Array.ConvertAll(researchRequestEntity.Keywords.Split(KeywordsSeparator), int.Parse),
                Description = researchRequestEntity.Description,
                Title = researchRequestEntity.Title,
                NodeTypeId = researchRequestEntity.NodeTypeId,
                DesiredCurriculum1 = researchRequestEntity.DesiredCurriculum1,
                DesiredCurriculum2 = researchRequestEntity.DesiredCurriculum2,
                DesiredCurriculum3 = researchRequestEntity.DesiredCurriculum3,
                DesiredCurriculum4 = researchRequestEntity.DesiredCurriculum4,
                DesiredCurriculum5 = researchRequestEntity.DesiredCurriculum5,
                CompletionTime = researchRequestEntity.CompletionTime,
                StartDate = researchRequestEntity.StartDate,
                Details = researchRequestEntity.Details,
                Endorsements = researchRequestEntity.Endorsements,
                FocusQuestion1 = researchRequestEntity.FocusQuestion1,
                FocusQuestion2 = researchRequestEntity.FocusQuestion2,
                FocusQuestion3 = researchRequestEntity.FocusQuestion3,
                LastUpdate = researchRequestEntity.LastUpdate,
                PotentialFunding = researchRequestEntity.PotentialFunding,
                TopicType = researchRequestEntity.TopicType,
                ErbTrbOrg = researchRequestEntity.ErbTrbOrg,
                Priority = researchRequestEntity.Priority,
                NumberOfRatings = researchRequestEntity.NumberOfRatings,
                SumOfRatings = researchRequestEntity.SumOfRatings,
                Sponsors = researchRequestEntity.Sponsors,
                CompletionDate = researchRequestEntity.CompletionDate,
                CreateDate = researchRequestEntity.CreateDate,
                ContributingStudentsCount = researchRequestEntity.ContributingStudentsCount,
                TopicNotes = researchRequestEntity.TopicNotes,
                FiscalYear = researchRequestEntity.FiscalYear,
                FocusQuestion4 = researchRequestEntity.FocusQuestion4,
                FocusQuestion5 = researchRequestEntity.FocusQuestion5,
                NotesByUser = researchRequestEntity.NotesByUser,
                Status = researchRequestEntity.Status,
                Importance = researchRequestEntity.Importance,
                IrefTitle = researchRequestEntity.IrefTitle,
            };
        }
    }
}
