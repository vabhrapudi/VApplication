// <copyright file="NewsArticleRequestStatus.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Specifies the status of news article request.
    /// </summary>
    public enum NewsArticleRequestStatus
    {
        /// <summary>
        /// The request is drafted and partial information is saved.
        /// </summary>
        Draft,

        /// <summary>
        /// Represents that the request is created and submitted for approval.
        /// </summary>
        Pending,

        /// <summary>
        /// Represents that the request is approved.
        /// </summary>
        Approved,

        /// <summary>
        /// Represents that the request is rejected.
        /// </summary>
        Rejected,
    }
}
