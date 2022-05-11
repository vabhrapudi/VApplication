// <copyright file="AthenaFeedbackType.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Describes the Athena feedback types.
    /// </summary>
    public enum AthenaFeedbackType
    {
        /// <summary>
        /// No feedback type.
        /// </summary>
        None,

        /// <summary>
        /// The type which relates to bugs.
        /// </summary>
        Bug,

        /// <summary>
        /// The type which relates to UI issues.
        /// </summary>
        UIIssue,

        /// <summary>
        /// The type which relates to future feature request.
        /// </summary>
        FutureFeatureRequest,
    }
}
