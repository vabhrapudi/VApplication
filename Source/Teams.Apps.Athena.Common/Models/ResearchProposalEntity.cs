// <copyright file="ResearchProposalEntity.cs" company="NPS Foundation">
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
    /// Describe the details of a research proposal.
    /// </summary>
    public class ResearchProposalEntity : TableEntity
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
                this.PartitionKey = ResearchProposalsTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the research proposal Id.
        /// </summary>
        [IsFilterable]
        public int ResearchProposalId { get; set; }

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
        /// Gets or sets the status.
        /// </summary>
        [IsFilterable]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the submitter Id.
        /// </summary>
        [IsFilterable]
        public int SubmitterId { get; set; }

        /// <summary>
        /// Gets or sets research details.
        /// </summary>
        [IsFilterable]
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets priority.
        /// </summary>
        [IsFilterable]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets related request Ids.
        /// </summary>
        [IsFilterable]
        [IsSearchable]
        public string RelatedRequestIds { get; set; }

        /// <summary>
        /// Gets or sets topic type.
        /// </summary>
        [IsFilterable]
        public string TopicType { get; set; }

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
        /// Gets or sets the objectives.
        /// </summary>
        [IsFilterable]
        public string Objectives { get; set; }

        /// <summary>
        /// Gets or sets the plan.
        /// </summary>
        [IsFilterable]
        public string Plan { get; set; }

        /// <summary>
        /// Gets or sets the deliverables.
        /// </summary>
        [IsFilterable]
        public string Deliverables { get; set; }

        /// <summary>
        /// Gets or sets the budget.
        /// </summary>
        [IsFilterable]
        public string Budget { get; set; }

        /// <summary>
        /// Gets or sets the research start date and time.
        /// </summary>
        [IsFilterable]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the research completion time.
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
        /// Gets or sets the research description.
        /// </summary>
        [IsFilterable]
        [IsSearchable]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the sum of ratings submitted by end-users.
        /// </summary>
        public int SumOfRatings { get; set; }

        /// <summary>
        /// Gets or sets the number of end-users who submitted the rating.
        /// </summary>
        public int NumberOfRatings { get; set; }

        /// <summary>
        /// Gets or sets average rating for research proposal.
        /// </summary>
        public string AverageRating { get; set; }

        /// <summary>
        /// Gets or sets user id of creator.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the average user rating.
        /// </summary>
        [IsFilterable]
        public int AvgUserRating { get; set; }

        /// <summary>
        /// Gets or sets research source Id.
        /// </summary>
        [IsFilterable]
        public int ResearchSourceId { get; set; }
    }
}
