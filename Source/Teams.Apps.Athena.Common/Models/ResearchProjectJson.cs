// <copyright file="ResearchProjectJson.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    ///  Represents ResearchProjectJson.
    /// </summary>
    public class ResearchProjectJson
    {
        /// <summary>
        /// Gets or sets unique table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the research project Id.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int ResearchProjectId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets keyword text separated by semicolon character.
        /// </summary>
        public string KeywordsText { get; set; }

        /// <summary>
        /// Gets or sets the last updated date and time.
        /// </summary>
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets title of research.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the abstract of research article.
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the status description.
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// Gets or sets authors.
        /// </summary>
        public string Authors { get; set; }

        /// <summary>
        /// Gets or sets authors Id's.
        /// </summary>
        public IEnumerable<int> AuthorIds { get; set; }

        /// <summary>
        /// Gets or sets advisor.
        /// </summary>
        public string Advisors { get; set; }

        /// <summary>
        /// Gets or sets advisor Id's.
        /// </summary>
        public IEnumerable<int> AdvisorIds { get; set; }

        /// <summary>
        /// Gets or sets second readers.
        /// </summary>
        public string SecondReaders { get; set; }

        /// <summary>
        /// Gets or sets second readers Id's.
        /// </summary>
        public IEnumerable<int> SecondReadersId { get; set; }

        /// <summary>
        /// Gets or sets the reviewer notes.
        /// </summary>
        public string ReviewerNotes { get; set; }

        /// <summary>
        /// Gets or sets name of research department.
        /// </summary>
        public string ResearchDept { get; set; }

        /// <summary>
        /// Gets or sets research source.
        /// </summary>
        public string ResearchSource { get; set; }

        /// <summary>
        /// Gets or sets files.
        /// </summary>
        public string Files { get; set; }

        /// <summary>
        /// Gets or sets authors organization.
        /// </summary>
        public string AuthorsOrg { get; set; }

        /// <summary>
        /// Gets or sets the degree program.
        /// </summary>
        public string DegreeProgram { get; set; }

        /// <summary>
        /// Gets or sets the degree level.
        /// </summary>
        public string DegreeLevel { get; set; }

        /// <summary>
        /// Gets or sets the degree titles.
        /// </summary>
        public string DegreeTitles { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which research project was started.
        /// </summary>
        public DateTime? DateStarted { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which research project was completed.
        /// </summary>
        public DateTime? DateCompleted { get; set; }

        /// <summary>
        /// Gets or sets the recognition.
        /// </summary>
        public string Recognition { get; set; }

        /// <summary>
        /// Gets or sets the sponsor Ids.
        /// </summary>
        public IEnumerable<int> SponsorIds { get; set; }

        /// <summary>
        /// Gets or sets the originating request.
        /// </summary>
        public string OriginatingRequest { get; set; }

        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the use rights.
        /// </summary>
        public string UseRights { get; set; }

        /// <summary>
        /// Gets or sets the document Id.
        /// </summary>
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
        /// Gets or sets the service type Id.
        /// </summary>
        public int ServiceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the partner Ids.
        /// </summary>
        public IEnumerable<int> PartnerIds { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the importance.
        /// </summary>
        public int Importance { get; set; }

        /// <summary>
        /// Gets or sets repository Id.
        /// </summary>
        public int RepositoryId { get; set; }

        /// <summary>
        /// Gets or sets research source Id.
        /// </summary>
        public int ResearchSourceId { get; set; }

        /// <summary>
        /// Gets or sets the department Ids.
        /// </summary>
        public IEnumerable<int> DepartmentId { get; set; }

        /// <summary>
        /// Gets or sets the graduate program Ids.
        /// </summary>
        public IEnumerable<int> GraduateProgramId { get; set; }
    }
}
