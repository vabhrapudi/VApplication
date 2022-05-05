// <copyright file="CoiSortColumn.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Represents the sort options for COI.
    /// </summary>
    public enum CoiSortColumn
    {
        /// <summary>
        /// Represents that the COIs will be sorted by creation date.
        /// </summary>
        CreatedOn,

        /// <summary>
        /// Represents that the COIs will be sorted by COI name.
        /// </summary>
        Name,

        /// <summary>
        /// Represents that the COIs will be sorted by COI type.
        /// </summary>
        Type,

        /// <summary>
        /// Represents that the COIs will be sorted by request status.
        /// </summary>
        Status,

        /// <summary>
        /// Represents that the COIs should not be sorted.
        /// </summary>
        None,
    }
}
