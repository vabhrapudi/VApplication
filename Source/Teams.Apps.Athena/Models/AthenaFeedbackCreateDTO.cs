// <copyright file="AthenaFeedbackCreateDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.ComponentModel.DataAnnotations;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Represents Athena feedback DTO for saving a new feedback.
    /// </summary>
    public class AthenaFeedbackCreateDTO
    {
        /// <summary>
        /// Gets or sets the detailed feedback.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the feedback.
        /// </summary>
        [Required]
        [Range(0, 2)]
        public AthenaFeedbackValues Feedback { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        [Range(0, 6)]
        public AthenaFeebackCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the feedback type.
        /// </summary>
        [Range(0, 3)]
        public AthenaFeedbackType Type { get; set; }
    }
}