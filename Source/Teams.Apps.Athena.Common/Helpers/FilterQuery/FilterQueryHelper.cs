// <copyright file="FilterQueryHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provides the methods which generates the filter query expressions.
    /// </summary>
    public class FilterQueryHelper : IFilterQueryHelper
    {
        /// <inheritdoc/>
        public string GetActiveCoiRequestsFilterCondition(IEnumerable<int> statusFilterValues, Guid userAadId)
        {
            var createdByFilterQuery = this.GetFilterCondition(nameof(CommunityOfInterestEntity.CreatedByObjectId), new[] { userAadId.ToString() });

            if (statusFilterValues.IsNullOrEmpty())
            {
                return createdByFilterQuery;
            }

            var statusFilterQuery = this.GetFilterCondition(nameof(CommunityOfInterestEntity.Status), statusFilterValues);

            return TableQuery.CombineFilters(statusFilterQuery, TableOperators.And, createdByFilterQuery);
        }

        /// <inheritdoc/>
        public string GetActiveNewsArticleRequestsFilterCondition(IEnumerable<int> statusFilterValues, Guid userAadId)
            {
            var createdByFilterQuery = this.GetFilterCondition(nameof(NewsEntity.CreatedBy), new[] { userAadId.ToString() });

            if (statusFilterValues.IsNullOrEmpty())
            {
                return createdByFilterQuery;
            }

            var statusFilterQuery = this.GetFilterCondition(nameof(NewsEntity.Status), statusFilterValues);

            return TableQuery.CombineFilters(statusFilterQuery, TableOperators.And, createdByFilterQuery);
        }

        /// <inheritdoc/>
        public string GetApprovedCoiRequestsFilterCondition(IEnumerable<string> keywordNames)
        {
            if (keywordNames.IsNullOrEmpty())
            {
                return null;
            }

            var approvedFilterCondition = this.GetFilterCondition(nameof(CommunityOfInterestEntity.Status), new List<int> { (int)CoiRequestStatus.Approved });
            var keywordFilterCondition = this.GetFilterConditionForExactMatch(nameof(CommunityOfInterestEntity.KeywordNames), keywordNames);
            return TableQuery.CombineFilters(approvedFilterCondition, TableOperators.And, keywordFilterCondition);
        }

        /// <inheritdoc/>
        public string GetFilterCondition(string columnName, IEnumerable<string> values)
        {
            var conditions = values
                .Select(value => TableQuery.GenerateFilterCondition(columnName, QueryComparisons.Equal, value));

            return string.Join($" {TableOperators.Or} ", conditions);
        }

        /// <inheritdoc/>
        public string GetFilterCondition(string columnName, IEnumerable<int> values)
        {
            var conditions = values
                .Select(value => TableQuery.GenerateFilterConditionForInt(columnName, QueryComparisons.Equal, value));

            return string.Join($" {TableOperators.Or} ", conditions);
        }

        /// <inheritdoc/>
        public string GetFilterConditionForExactMatch(string column, IEnumerable<string> values)
        {
            values = values.Select(keyword => keyword.EscapeSpecialCharacters());

            var valuesString = string.Join(" ", values);

            return $"search.ismatch('{valuesString}', '{column}')";
        }

        /// <inheritdoc/>
        public string GetFilterConditionForExactStringMatch(string column, IEnumerable<string> values)
        {
            var valuesString = string.Join(" ", values);

            return $"search.ismatch('{valuesString}', '{column}')";
        }

        /// <inheritdoc/>
        public string GetFilterConditionForExactStringMatch(string column, IEnumerable<int> values)
        {
            var valuesString = string.Join(" ", values);

            return $"search.ismatch('{valuesString}', '{column}')";
        }

        /// <inheritdoc/>
        public string GetFilterConditionForExactStringMatch(string column, string value)
        {
            return $"search.ismatch('{value}', '{column}')";
        }

        /// <inheritdoc/>
        public string CombineFilters(string filter1, string filter2, string tableOperator)
        {
            return TableQuery.CombineFilters(filter1, tableOperator, filter2);
        }

        /// <inheritdoc/>
        public string GetFilterConditionToMatchAnyValueInCollection(string expression, IEnumerable<string> values)
        {
            var filterValues = values.Select(value => $"value eq '{value}'");
            return $"{expression}/any(value:{string.Join(" or ", filterValues)})";
        }

        /// <inheritdoc/>
        public string GetFilterConditionForDate(string columnName, string queryComparison, IEnumerable<string> values)
        {
            var conditions = values
                .Select(value => string.Join(" ", columnName, queryComparison, value));

            return string.Join($" {TableOperators.Or} ", conditions);
        }

        /// <inheritdoc/>
        public string CombineFilters(IEnumerable<string> filters, string tableOperator)
        {
            filters = filters.Select(x => $"({x})");
            return string.Join($" {tableOperator} ", filters);
        }

        /// <inheritdoc/>
        public string GetNotEqualFilterCondition(string columnName, string value)
        {
            return TableQuery.GenerateFilterCondition(columnName, QueryComparisons.NotEqual, value);
        }

        /// <inheritdoc/>
        public string GetFilterConditionForExactMatch(IEnumerable<string> values)
        {
            values = values.Select(keyword => keyword.EscapeSpecialCharacters());

            var valuesString = string.Join(" ", values);

            return $"search.ismatch('{valuesString}')";
        }
    }
}
