// <copyright file="ResearchPaper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Azure.Search;

    /// <summary>
    /// Represents a research paper entity.
    /// </summary>
    public class ResearchPaper : TableEntity
    {
        private const string ResearchPapersEntityPartitionKey = "ResearchPapers";

        /// <summary>
        /// Gets the partition key.
        /// </summary>
        public new string PartitionKey
        {
            get { return ResearchPapersEntityPartitionKey; }
            private set { value = ResearchPapersEntityPartitionKey; }
        }

        /// <summary>
        /// Gets or sets the research Id.
        /// </summary>
        [Key]
        public string ResearchId
        {
            get { return this.RowKey; }
            set { this.RowKey = value; }
        }

        /// <summary>
        /// Gets or sets the name of source institution for research.
        /// </summary>
        [IsFilterable]
        [IsFacetable]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the title of research.
        /// </summary>
        [Required]
        [IsSearchable]
        [IsSortable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the abstract of research article.
        /// </summary>
        [IsSearchable]
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the status of a research.
        /// </summary>
        [IsFilterable]
        [IsFacetable]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the string representation of JSON array of name/email of authors.
        /// </summary>
        [IsSearchable]
        public string Authors { get; set; }

        /// <summary>
        /// Gets or sets the string representation of JSON array of name/email of advisors.
        /// </summary>
        public string Advisors { get; set; }

        /// <summary>
        /// Gets or sets the string representation of JSON array of name/email of second readers.
        /// </summary>
        public string SecondReaders { get; set; }

        /// <summary>
        /// Gets or sets the string representation of JSON array of name/email of sponsors.
        /// </summary>
        public string Sponsors { get; set; }

        /// <summary>
        /// Gets or sets the name of organization to which research belongs to.
        /// </summary>
        public string AuthorOrganization { get; set; }

        /// <summary>
        /// Gets or sets the subject's organization.
        /// </summary>
        public string ResearchOrganization { get; set; }

        /// <summary>
        /// Gets or sets the department of research.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        [IsFacetable]
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets the date and time in UTC when the research has been started.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the date and time in UTC when the research was completed.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public DateTime CompletedDate { get; set; }

        /// <summary>
        /// Gets or sets the geography of research.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        [IsFacetable]
        public string Geography { get; set; }

        /// <summary>
        /// Gets or sets the security classification of the research.
        /// </summary>
        [IsFilterable]
        public string Classification { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        [Url]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id who created the research paper.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which research paper is created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the URL of external research.
        /// </summary>
        [Url]
        public string ExternalLink { get; set; }

        /// <summary>
        /// Gets or sets the string representation of JSON array of keywords associated with the research paper.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Keywords { get; set; }
    }
}
