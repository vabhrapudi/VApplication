// <copyright file="ResearchProposalViewDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describe research proposal entity view model.
    /// </summary>
    public class ResearchProposalViewDTO
    {
        /// <summary>
        /// Gets or sets unique table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the research proposal Id.
        /// </summary>
        public int ResearchProposalId { get; set; }

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
        /// Gets or sets the last updated date and time.
        /// </summary>
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets title of research.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the submitter Id.
        /// </summary>
        public int SubmitterId { get; set; }

        /// <summary>
        /// Gets or sets research details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets priority.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets related request Ids.
        /// </summary>
        public IEnumerable<int> RelatedRequestIds { get; set; }

        /// <summary>
        /// Gets or sets topic type.
        /// </summary>
        public string TopicType { get; set; }

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
        /// Gets or sets the objectives.
        /// </summary>
        public string Objectives { get; set; }

        /// <summary>
        /// Gets or sets the plan.
        /// </summary>
        public string Plan { get; set; }

        /// <summary>
        /// Gets or sets the deliverables.
        /// </summary>
        public string Deliverables { get; set; }

        /// <summary>
        /// Gets or sets the budget.
        /// </summary>
        public string Budget { get; set; }

        /// <summary>
        /// Gets or sets the research start date and time.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the research completion time.
        /// </summary>
        public string CompletionTime { get; set; }

        /// <summary>
        /// Gets or sets the endorsements.
        /// </summary>
        public string Endorsements { get; set; }

        /// <summary>
        /// Gets or sets the potential funding.
        /// </summary>
        public string PotentialFunding { get; set; }

        /// <summary>
        /// Gets or sets the research description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets sum of ratings given by users.
        /// </summary>
        public int SumOfRatings { get; set; }

        /// <summary>
        /// Gets or sets number of users who rated for research proposal.
        /// </summary>
        public int NumberOfRatings { get; set; }

        /// <summary>
        /// Gets or sets rating of a research proposal given by user.
        /// </summary>
        public decimal UserRating { get; set; }

        /// <summary>
        /// Gets or sets name of user whi have submitted the research proposal.
        /// </summary>
        public string SubmittedBy { get; set; }

        /// <summary>
        /// Gets or sets user id of creator.
        /// </summary>
        public string UserId { get; set; }
    }
}
