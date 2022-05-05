// <copyright file="ResearchProjectEntity.cs" company="NPS Foundation">
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
    /// Represents the research projects entity.
    /// </summary>
    public class ResearchProjectEntity : TableEntity
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
                this.PartitionKey = ResearchProjectsTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the research project Id.
        /// </summary>
        [IsFilterable]
        public int ResearchProjectId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        [IsFilterable]
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        [IsFilterable]
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
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
        /// Gets or sets the last updated date and time.
        /// </summary>
        [IsFilterable]
        [IsSortable]
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets title of research.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the abstract of research article.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [IsFilterable]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the status description.
        /// </summary>
        [IsFilterable]
        public string StatusDescription { get; set; }

        /// <summary>
        /// Gets or sets authors.
        /// </summary>
        [IsFilterable]
        [IsSearchable]
        public string Authors { get; set; }

        /// <summary>
        /// Gets or sets author Ids.
        /// </summary>
        [IsFilterable]
        public string AuthorIds { get; set; }

        /// <summary>
        /// Gets or sets advisor.
        /// </summary>
        [IsFilterable]
        [IsSearchable]
        public string Advisors { get; set; }

        /// <summary>
        /// Gets or sets advisor Ids.
        /// </summary>
        [IsFilterable]
        public string AdvisorIds { get; set; }

        /// <summary>
        /// Gets or sets second readers.
        /// </summary>
        [IsFilterable]
        [IsSearchable]
        public string SecondReaders { get; set; }

        /// <summary>
        /// Gets or sets second readers Ids.
        /// </summary>
        [IsFilterable]
        public string SecondReadersId { get; set; }

        /// <summary>
        /// Gets or sets the reviewer notes.
        /// </summary>
        [IsFilterable]
        public string ReviewerNotes { get; set; }

        /// <summary>
        /// Gets or sets the repository Id.
        /// </summary>
        [IsFilterable]
        public int RepositoryId { get; set; }

        /// <summary>
        /// Gets or sets name of research department.
        /// </summary>
        [IsFilterable]
        public string ResearchDept { get; set; }

        /// <summary>
        /// Gets or sets research source.
        /// </summary>
        [IsFilterable]
        public int ResearchSourceId { get; set; }

        /// <summary>
        /// Gets or sets the department Ids.
        /// </summary>
        [IsFilterable]
        public string DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets files.
        /// </summary>
        [IsFilterable]
        public string Files { get; set; }

        /// <summary>
        /// Gets or sets authors organization.
        /// </summary>
        [IsFilterable]
        public string AuthorsOrg { get; set; }

        /// <summary>
        /// Gets or sets the degree program.
        /// </summary>
        [IsFilterable]
        public string DegreeProgram { get; set; }

        /// <summary>
        /// Gets or sets the degree level.
        /// </summary>
        [IsFilterable]
        public string DegreeLevel { get; set; }

        /// <summary>
        /// Gets or sets the degree titles.
        /// </summary>
        [IsFilterable]
        public string DegreeTitles { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which research project was started.
        /// </summary>
        [IsFilterable]
        public DateTime? DateStarted { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which research project was completed.
        /// </summary>
        [IsFilterable]
        public DateTime? DateCompleted { get; set; }

        /// <summary>
        /// Gets or sets the recognition.
        /// </summary>
        [IsFilterable]
        public string Recognition { get; set; }

        /// <summary>
        /// Gets or sets the sponsor Ids.
        /// </summary>
        [IsFilterable]
        [IsSearchable]
        public string SponsorIds { get; set; }

        /// <summary>
        /// Gets or sets the originating request.
        /// </summary>
        [IsFilterable]
        public string OriginatingRequest { get; set; }

        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        [IsFilterable]
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the use rights.
        /// </summary>
        [IsFilterable]
        public string UseRights { get; set; }

        /// <summary>
        /// Gets or sets the document Id.
        /// </summary>
        [IsFilterable]
        public string DocID { get; set; }

        /// <summary>
        /// Gets or sets the sum of ratings submitted by end-users.
        /// </summary>
        public int SumOfRatings { get; set; }

        /// <summary>
        /// Gets or sets the number of end-users who submitted the rating.
        /// </summary>
        public int NumberOfRatings { get; set; }

        /// <summary>
        /// Gets or sets average rating for research project.
        /// </summary>
        public string AverageRating { get; set; }

        /// <summary>
        /// Gets or sets the service type Id.
        /// </summary>
        [IsFilterable]
        public int ServiceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the partner Ids.
        /// </summary>
        [IsFilterable]
        [IsSearchable]
        public string PartnerIds { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        [IsFilterable]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the importance.
        /// </summary>
        [IsFilterable]
        public int Importance { get; set; }

        /// <summary>
        /// Gets or sets the graduate program Ids.
        /// </summary>
        [IsFilterable]
        public string GraduateProgramId { get; set; }
    }
}