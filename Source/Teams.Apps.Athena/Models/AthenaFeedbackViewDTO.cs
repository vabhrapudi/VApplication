// <copyright file="AthenaFeedbackViewDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;

    /// <summary>
    /// Represents an athena feedback view model.
    /// </summary>
    public class AthenaFeedbackViewDTO
    {
        /// <summary>
        /// Gets or sets the feedback Id.
        /// </summary>
        public string FeedbackId { get; set; }

        /// <summary>
        /// Gets or sets the feedback level.
        /// </summary>
        public int Feedback { get; set; }

        /// <summary>
        /// Gets or sets the detailed feedback.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the date and time on which feedback was submitted.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the user details who submitted the feedback.
        /// </summary>
        public UserDetails CreatedBy { get; set; }
    }
}
