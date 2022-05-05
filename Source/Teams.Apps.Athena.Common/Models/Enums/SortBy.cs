// <copyright file="SortBy.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models.Enums
{
    /// <summary>
    /// Represents 0 for recent, 1 for significance and 2 for rating high to low for news.
    /// </summary>
    public enum SortBy
    {
        /// <summary>
        /// Represents default sorting of news by most recent first.
        /// </summary>
        Recent,

        /// <summary>
        /// Represents sorting of news by its significance.
        /// </summary>
        Significance,

        /// <summary>
        /// Represents sorting of news by highly rated news.
        /// </summary>
        RatingHighToLow,
    }
}
