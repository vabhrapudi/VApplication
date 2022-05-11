// <copyright file="AthenaFeedbackSortByItems.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models.Enums
{
    /// <summary>
    /// Represents 0 for recent, 1 for category and 2 for feedback type for feedback.
    /// </summary>
    public enum AthenaFeedbackSortByItems
    {
        /// <summary>
        /// Represents default sorting of feedback by most recent first.
        /// </summary>
        Recent,

        /// <summary>
        /// Represents sorting of feedback by category.
        /// </summary>
        Category,

        /// <summary>
        /// Represents sorting of feedback by feedback type.
        /// </summary>
        FeeedbackType,
    }
}