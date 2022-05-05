// <copyright file="IAthenaFeedbackSearchService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Feedback search service provider to fetch feedback based on search and filter criteria.
    /// </summary>
    public interface IAthenaFeedbackSearchService
    {
        /// <summary>
        /// Gets athena feedbacks list as per search and filter criteria.
        /// </summary>
        /// <param name="searchParametersDTO">Search parameters for enhanced searching.</param>
        /// <returns>List of feedbacks.</returns>
        Task<IEnumerable<AthenaFeedbackEntity>> GetAthenaFeedbacksAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Run the indexer on demand.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task RunIndexerOnDemandAsync();
    }
}