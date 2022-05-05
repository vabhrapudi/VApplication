// <copyright file="SearchParametersDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// This class is responsible to store the search parameters.
    /// </summary>
    public class SearchParametersDTO
    {
        /// <summary>
        /// Gets or sets scope of the search.
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// Gets or sets news sort by filter.
        /// </summary>
        public int SortByFilter { get; set; }

        /// <summary>
        /// Gets or sets the COI column to be sorted.
        /// </summary>
        public CoiSortColumn CoiSortColumn { get; set; }

        /// <summary>
        /// Gets or sets the COI column to be sorted.
        /// </summary>
        public NewsArticleSortColumn NewsArticleSortColumn { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        public SortOrder SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the search fields.
        /// </summary>
#pragma warning disable CA2227 // Need to insert data.
        public IList<string> SearchFields { get; set; }
#pragma warning restore CA2227 // Need to insert data.

        /// <summary>
        /// Gets or sets the filter query.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets page count for which post needs to be fetched.
        /// </summary>
        public int? PageCount { get; set; } = null;

        /// <summary>
        /// Gets or sets number of search results to skip.
        /// </summary>
        public int? SkipRecords { get; set; } = null;

        /// <summary>
        /// Gets or sets top count of search results to get.
        /// </summary>
        public int? TopRecordsCount { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether the service returns all records.
        /// </summary>
        public bool IsGetAllRecords { get; set; }

        /// <summary>
        /// Gets or sets the list of OData $orderby expressions by which to sort the results.
        /// </summary>
#pragma warning disable CA2227 // Need this collection to add order by expressions.
        public IList<string> OrderBy { get; set; }
#pragma warning restore CA2227 // Need this collection to add order by expressions.
    }
}