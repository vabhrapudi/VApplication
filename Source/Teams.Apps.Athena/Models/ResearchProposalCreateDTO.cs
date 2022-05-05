// <copyright file="ResearchProposalCreateDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents the research proposal create DTO.
    /// </summary>
    public class ResearchProposalCreateDTO
    {
        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        [Required]
        public IEnumerable<KeywordDTO> KeywordsJson { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the details of research proposal.
        /// </summary>
        [Required]
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the budget.
        /// </summary>
        [Required]
        public string Budget { get; set; }

        /// <summary>
        /// Gets or sets the potential funding.
        /// </summary>
        [Required]
        public string PotentialFunding { get; set; }

        /// <summary>
        /// Gets or sets security level.
        /// </summary>
        [Required]
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets start date.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets priority.
        /// </summary>
        [Required]
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets topic type.
        /// </summary>
        [Required]
        public string TopicType { get; set; }

        /// <summary>
        /// Gets or sets the focus question 1.
        /// </summary>
        [Required]
        public string FocusQuestion1 { get; set; }

        /// <summary>
        /// Gets or sets the focus question 2.
        /// </summary>
        [Required]
        public string FocusQuestion2 { get; set; }

        /// <summary>
        /// Gets or sets the focus question 3.
        /// </summary>
        [Required]
        public string FocusQuestion3 { get; set; }

        /// <summary>
        /// Gets or sets the objectives.
        /// </summary>
        [Required]
        public string Objectives { get; set; }

        /// <summary>
        /// Gets or sets the plan.
        /// </summary>
        [Required]
        public string Plan { get; set; }

        /// <summary>
        /// Gets or sets the deliverables.
        /// </summary>
        [Required]
        public string Deliverables { get; set; }

        /// <summary>
        /// Gets or sets the research completion time.
        /// </summary>
        [Required]
        public string CompletionTime { get; set; }

        /// <summary>
        /// Gets or sets the endorsements.
        /// </summary>
        [Required]
        public string Endorsements { get; set; }
    }
}
