// <copyright file="ResearchRequestJson.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the research requests Json.
    /// </summary>
    public class ResearchRequestJson
    {
        /// <summary>
        /// Gets or sets unique table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets research request Id.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int ResearchRequestId { get; set; }

        /// <summary>
        /// Gets or sets research request title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the sponsor Id.
        /// </summary>
        public IEnumerable<int> SponsorIds { get; set; }

        /// <summary>
        /// Gets or sets the keyword string.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the KeywordsText.
        /// </summary>
        public string KeywordsText { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the topic Id.
        /// </summary>
        public string TopicId { get; set; }

        /// <summary>
        /// Gets or sets the topic type.
        /// </summary>
        public string TopicType { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the last updated date and time.
        /// </summary>
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the produced project Id.
        /// </summary>
        public string ProducedProjectIds { get; set; }

        /// <summary>
        /// Gets or sets the focus question 1.
        /// </summary>
        public string FocusQuestion1 { get; set; }

        /// <summary>
        /// Gets or sets the focus question 2.
        /// </summary>
        public string FocusQuestion2 { get; set; }

        /// <summary>
        /// Gets or sets the focus question 3.
        /// </summary>
        public string FocusQuestion3 { get; set; }

        /// <summary>
        /// Gets or sets the start date and time.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the completion time.
        /// </summary>
        public string CompletionTime { get; set; }

        /// <summary>
        /// Gets or sets the potential funding.
        /// </summary>
        public string PotentialFunding { get; set; }

        /// <summary>
        /// Gets or sets the desired curriculum 1.
        /// </summary>
        public string DesiredCurriculum1 { get; set; }

        /// <summary>
        /// Gets or sets the desired curriculum 2.
        /// </summary>
        public string DesiredCurriculum2 { get; set; }

        /// <summary>
        /// Gets or sets the desired curriculum 3.
        /// </summary>
        public string DesiredCurriculum3 { get; set; }

        /// <summary>
        /// Gets or sets the desired curriculum 4.
        /// </summary>
        public string DesiredCurriculum4 { get; set; }

        /// <summary>
        /// Gets or sets the desired curriculum 5.
        /// </summary>
        public string DesiredCurriculum5 { get; set; }

        /// <summary>
        /// Gets or sets the erb trb org.
        /// </summary>
        public string ErbTrbOrg { get; set; }

        /// <summary>
        /// Gets or sets the importance.
        /// </summary>
        public int Importance { get; set; }

        /// <summary>
        /// Gets or sets the endorsements.
        /// </summary>
        public string Endorsements { get; set; }

        /// <summary>
        /// Gets or sets research source Id.
        /// </summary>
        public int ResearchSourceId { get; set; }

        /// <summary>
        /// Gets or sets repository Id.
        /// </summary>
        public int RepositoryId { get; set; }

        /// <summary>
        /// Gets or sets sponsors.
        /// </summary>
        public string Sponsors { get; set; }

        /// <summary>
        /// Gets or sets research topic Id.
        /// </summary>
        public int ResearchTopicId { get; set; }

        /// <summary>
        /// Gets or sets research estimate Id.
        /// </summary>
        public int ResearchEstimateId { get; set; }

        /// <summary>
        /// Gets or sets focus question 4.
        /// </summary>
        public string FocusQuestion4 { get; set; }

        /// <summary>
        /// Gets or sets focus question 5.
        /// </summary>
        public string FocusQuestion5 { get; set; }

        /// <summary>
        /// Gets or sets the status of research request.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the fiscal year.
        /// </summary>
        public int FiscalYear { get; set; }

        /// <summary>
        /// Gets or sets topic notes.
        /// </summary>
        public string TopicNotes { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Iref title.
        /// </summary>
        public string IrefTitle { get; set; }

        /// <summary>
        /// Gets or sets the completion date.
        /// </summary>
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        /// Gets or sets contributing students count.
        /// </summary>
        public int ContributingStudentsCount { get; set; }

        /// <summary>
        /// Gets or sets notes by user.
        /// </summary>
        public string NotesByUser { get; set; }

        /// <summary>
        /// Gets or sets average user rating.
        /// </summary>
        public int AvgUserRating { get; set; }
    }
}