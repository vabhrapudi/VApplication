// <copyright file="ResearchRequestEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Azure.Search;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents the research request entity.
    /// </summary>
    public class ResearchRequestEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets unique table Id.
        /// </summary>
        [Key]
        public string TableId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = ResearchRequestsTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets research request Id.
        /// </summary>
        [IsFilterable]
        public int ResearchRequestId { get; set; }

        /// <summary>
        /// Gets or sets research request title.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        [IsFilterable]
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the sponsor Ids.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string SponsorIds { get; set; }

        /// <summary>
        /// Gets or sets the keyword string.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets keyword text separated by semicolon character.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string KeywordsText { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [IsFilterable]
        [IsSearchable]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        [IsFilterable]
        [IsSearchable]
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        [IsFilterable]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the topic Id.
        /// </summary>
        [IsFilterable]
        public string TopicId { get; set; }

        /// <summary>
        /// Gets or sets the topic type.
        /// </summary>
        [IsFilterable]
        public string TopicType { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        [IsFilterable]
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the last updated date and time.
        /// </summary>
        [IsFilterable]
        [IsSortable]
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the produced project Id.
        /// </summary>
        [IsFilterable]
        public string ProducedProjectIds { get; set; }

        /// <summary>
        /// Gets or sets the focus question 1.
        /// </summary>
        [IsFilterable]
        public string FocusQuestion1 { get; set; }

        /// <summary>
        /// Gets or sets the focus question 2.
        /// </summary>
        [IsFilterable]
        public string FocusQuestion2 { get; set; }

        /// <summary>
        /// Gets or sets the focus question 3.
        /// </summary>
        [IsFilterable]
        public string FocusQuestion3 { get; set; }

        /// <summary>
        /// Gets or sets the start date and time.
        /// </summary>
        [IsFilterable]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the completion time.
        /// </summary>
        [IsFilterable]
        public string CompletionTime { get; set; }

        /// <summary>
        /// Gets or sets the endorsements.
        /// </summary>
        [IsFilterable]
        public string Endorsements { get; set; }

        /// <summary>
        /// Gets or sets the potential funding.
        /// </summary>
        [IsFilterable]
        public string PotentialFunding { get; set; }

        /// <summary>
        /// Gets or sets the desired curriculum 1.
        /// </summary>
        [IsFilterable]
        public string DesiredCurriculum1 { get; set; }

        /// <summary>
        /// Gets or sets the desired curriculum 2.
        /// </summary>
        [IsFilterable]
        public string DesiredCurriculum2 { get; set; }

        /// <summary>
        /// Gets or sets the desired curriculum 3.
        /// </summary>
        [IsFilterable]
        public string DesiredCurriculum3 { get; set; }

        /// <summary>
        /// Gets or sets the desired curriculum 4.
        /// </summary>
        [IsFilterable]
        public string DesiredCurriculum4 { get; set; }

        /// <summary>
        /// Gets or sets the desired curriculum 5.
        /// </summary>
        [IsFilterable]
        public string DesiredCurriculum5 { get; set; }

        /// <summary>
        /// Gets or sets the erb trb org.
        /// </summary>
        [IsFilterable]
        public string ErbTrbOrg { get; set; }

        /// <summary>
        /// Gets or sets the importance.
        /// </summary>
        [IsFilterable]
        public int Importance { get; set; }

        /// <summary>
        /// Gets or sets the sum of ratings.
        /// </summary>
        [IsFilterable]
        public int SumOfRatings { get; set; }

        /// <summary>
        /// Gets or sets the number of ratings.
        /// </summary>
        [IsFilterable]
        public int NumberOfRatings { get; set; }

        /// <summary>
        /// Gets or sets average rating for research request.
        /// </summary>
        [IsFilterable]
        public string AverageRating { get; set; }

        /// <summary>
        /// Gets or sets research source Id.
        /// </summary>
        [IsFilterable]
        public int ResearchSourceId { get; set; }

        /// <summary>
        /// Gets or sets repository Id.
        /// </summary>
        [IsFilterable]
        public int RepositoryId { get; set; }

        /// <summary>
        /// Gets or sets sponsors.
        /// </summary>
        [IsFilterable]
        public string Sponsors { get; set; }

        /// <summary>
        /// Gets or sets research topic Id.
        /// </summary>
        [IsFilterable]
        public int ResearchTopicId { get; set; }

        /// <summary>
        /// Gets or sets research estimate Id.
        /// </summary>
        [IsFilterable]
        public int ResearchEstimateId { get; set; }

        /// <summary>
        /// Gets or sets focus question 4.
        /// </summary>
        [IsFilterable]
        public string FocusQuestion4 { get; set; }

        /// <summary>
        /// Gets or sets focus question 5.
        /// </summary>
        [IsFilterable]
        public string FocusQuestion5 { get; set; }

        /// <summary>
        /// Gets or sets the status of research request.
        /// </summary>
        [IsFilterable]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the fiscal year.
        /// </summary>
        [IsFilterable]
        public int FiscalYear { get; set; }

        /// <summary>
        /// Gets or sets topic notes.
        /// </summary>
        [IsFilterable]
        public string TopicNotes { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        [IsFilterable]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Iref title.
        /// </summary>
        [IsFilterable]
        public string IrefTitle { get; set; }

        /// <summary>
        /// Gets or sets the completion date.
        /// </summary>
        [IsFilterable]
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        /// Gets or sets contributing students count.
        /// </summary>
        [IsFilterable]
        public int ContributingStudentsCount { get; set; }

        /// <summary>
        /// Gets or sets notes by user.
        /// </summary>
        [IsFilterable]
        public string NotesByUser { get; set; }

        /// <summary>
        /// Gets or sets average user rating.
        /// </summary>
        [IsFilterable]
        public int AvgUserRating { get; set; }
    }
}