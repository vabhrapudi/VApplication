// <copyright file="IFilterQueryHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Provides the helper method which generates the filter query expression.
    /// </summary>
    public interface IFilterQueryHelper
    {
        /// <summary>
        /// Gets the filter expression to get active COI requests based on COI request status.
        /// </summary>
        /// <param name="statusFilterValues">The collection of status of which COI requests to get.</param>
        /// <param name="userAadId">The user AAD Id who created the COI requests.</param>
        /// <returns>The filter expression.</returns>
        string GetActiveCoiRequestsFilterCondition(IEnumerable<int> statusFilterValues, Guid userAadId);

        /// <summary>
        /// Gets or sets the approved COI requests based on keywords.
        /// </summary>
        /// <param name="keywordNames">The keyword names to be filtered.</param>
        /// <returns>The approved COI requests.</returns>
        public string GetApprovedCoiRequestsFilterCondition(IEnumerable<string> keywordNames);

        /// <summary>
        /// Gets the filter expression to get active news article requests based on request status.
        /// </summary>
        /// <param name="statusFilterValues">The collection of status of which COI requests to get.</param>
        /// <param name="userAadId">The user AAD Id who created the COI requests.</param>
        /// <returns>The filter expression.</returns>
        string GetActiveNewsArticleRequestsFilterCondition(IEnumerable<int> statusFilterValues, Guid userAadId);

        /// <summary>
        /// Gets or sets the filter condition.
        /// </summary>
        /// <param name="columnName">The table column name to which filter to be applied.</param>
        /// <param name="values">The collection of values.</param>
        /// <returns>The combined filter string.</returns>
        string GetFilterCondition(string columnName, IEnumerable<string> values);

        /// <summary>
        /// Gets or sets the filter condition.
        /// </summary>
        /// <param name="columnName">The table column name to which filter to be applied.</param>
        /// <param name="values">The collection of values.</param>
        /// <returns>The combined filter string.</returns>
        string GetFilterCondition(string columnName, IEnumerable<int> values);

        /// <summary>
        /// Gets the search match filter condition.
        /// </summary>
        /// <param name="column">The column name.</param>
        /// <param name="values">The values.</param>
        /// <returns>The search match filter condition.</returns>
        public string GetFilterConditionForExactMatch(string column, IEnumerable<string> values);

        /// <summary>
        /// Gets the search match filter condition.
        /// </summary>
        /// <param name="column">The column name.</param>
        /// <param name="values">The values.</param>
        /// <returns>The search match filter condition.</returns>
        public string GetFilterConditionForExactStringMatch(string column, IEnumerable<string> values);

        /// <summary>
        /// Gets the search match filter condition.
        /// </summary>
        /// <param name="column">The column name.</param>
        /// <param name="values">The values.</param>
        /// <returns>The search match filter condition.</returns>
        public string GetFilterConditionForExactStringMatch(string column, IEnumerable<int> values);

        /// <summary>
        /// Create a filter string to match any value in provided collection.
        /// </summary>
        /// <param name="expression">The filter expression.</param>
        /// <param name="values">The collection of values.</param>
        /// <returns>The filter string.</returns>
        public string GetFilterConditionToMatchAnyValueInCollection(string expression, IEnumerable<string> values);

        /// <summary>
        /// Combines the filter conditions.
        /// </summary>
        /// <param name="filter1">First filter.</param>
        /// <param name="filter2">Second filter.</param>
        /// <param name="tableOperator">The table operator.</param>
        /// <returns>The combined filter query.</returns>
        public string CombineFilters(string filter1, string filter2, string tableOperator);

        /// <summary>
        /// Gets the search match filter condition.
        /// </summary>
        /// <param name="column">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>The search match filter condition.</returns>
        public string GetFilterConditionForExactStringMatch(string column, string value);

        /// <summary>
        /// Gets the filter condition for date.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="queryComparison">The query comparison contains value from <see cref="QueryComparisons"/>.</param>
        /// <param name="values">The collection of formatted DateTime values.</param>
        /// <returns>The filter condition for date.</returns>
        public string GetFilterConditionForDate(string columnName, string queryComparison, IEnumerable<string> values);

        /// <summary>
        /// Combines the collection of filter conditions.
        /// </summary>
        /// <param name="filters">The collection of filter conditions.</param>
        /// <param name="tableOperator">The table operator.</param>
        /// <returns>The filter condition.</returns>
        public string CombineFilters(IEnumerable<string> filters, string tableOperator);

        /// <summary>
        /// Gets the not equal (ne) filter condition.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>The not equal filter condition.</returns>
        public string GetNotEqualFilterCondition(string columnName, string value);

        /// <summary>
        /// Gets the search match filter condition.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns>The search match filter condition.</returns>
        public string GetFilterConditionForExactMatch(IEnumerable<string> values);
    }
}
