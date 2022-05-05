// <copyright file="NewsArticleSortColumn.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Represents the news article columns that can be sorted.
    /// </summary>
    public enum NewsArticleSortColumn
    {
        /// <summary>
        /// Represents the created at column.
        /// </summary>
        CreatedAt,

        /// <summary>
        /// Represents the news article title column.
        /// </summary>
        Title,

        /// <summary>
        /// Represents the news article request status column.
        /// </summary>
        Status,

        /// <summary>
        /// Represents the news article request no column.
        /// </summary>
        None,
    }
}
