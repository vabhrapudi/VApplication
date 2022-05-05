// <copyright file="ICoiHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes helper methods related to COI requests.
    /// </summary>
    public interface ICoiHelper
    {
        /// <summary>
        /// Gets all COI requests created by user which are not deleted.
        /// </summary>
        /// <param name="searchText">The search text to be searched in COI name or keywords.</param>
        /// <param name="pageNumber">The page number for which COI requests to be retrieved.</param>
        /// <param name="sortColumn">The column to be sorted.</param>
        /// <param name="sortOrder">The order in which requests to be sorted.</param>
        /// <param name="statusFilterValues">The status filter values.</param>
        /// <param name="userAadId">The user AAD Id of which requests to get.</param>
        /// <returns>The collection of COI requests.</returns>
        Task<IEnumerable<CoiEntityDTO>> GetActiveCoiRequestsAsync(string searchText, int pageNumber, CoiSortColumn sortColumn, SortOrder sortOrder, IEnumerable<int> statusFilterValues, Guid userAadId);

        /// <summary>
        /// Gets an active COI request by Id.
        /// </summary>
        /// <param name="coiRequestId">The COI request Id to get.</param>
        /// <returns>The COI request details.</returns>
        Task<CoiEntityDTO> GetCoiRequestAsync(Guid coiRequestId);

        /// <summary>
        /// Gets all COI requests created by user which are approved.
        /// </summary>
        /// <param name="keywords">The status filter values.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IEnumerable<CoiEntityDTO>> GetApprovedCoiRequestAsync(IEnumerable<KeywordEntity> keywords);

        /// <summary>
        /// Gets the approved COIs based on keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids of which COI to get.</param>
        /// <returns>The collection of COIs.</returns>
        Task<IEnumerable<CoiEntityDTO>> GetApprovedCoiRequestsByKeywordIdsAsync(IEnumerable<int> keywordIds);

        /// <summary>
        /// Gets the COI requests.
        /// </summary>
        /// <param name="searchParametersDTO">Advanced search paramters.</param>
        /// <returns>The collection of COI requests.</returns>
        Task<IEnumerable<CoiEntityDTO>> GetCoiRequestsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Gets a COI by table Id.
        /// </summary>
        /// <param name="coiTableId">The COI table Id of the research project to fetch.</param>
        /// <param name="userAadObjectId">The user aad object Id.</param>
        /// <returns>Returns COI entity details.</returns>
        Task<CoiEntityDTO> GetCoiByTableIdAsync(string coiTableId, string userAadObjectId);

        /// <summary>
        /// Stores rating of user for a COI.
        /// </summary>
        /// <param name="coiTableId">The COI table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="userAadObjectId">User Id who submitted rating.</param>
        /// <returns>Returns task indicating operation result.</returns>
        Task RateCoiAsync(string coiTableId, int rating, string userAadObjectId);

        /// <summary>
        /// Gets the COI request details by team Id.
        /// </summary>
        /// <param name="teamId">The team Id of which COI details to get.</param>
        /// <returns>The COI details.</returns>
        Task<CoiEntityDTO> GetCoiDetailsAsync(Guid teamId);

        /// <summary>
        /// Gets the COIs.
        /// </summary>
        /// <param name="keywords">The collection of keywords.</param>
        /// <param name="fromDate">The date and time from which COIs to get.</param>
        /// <param name="count">The number of COIs to get.</param>
        /// <returns>The collection of COIs.</returns>
        Task<IEnumerable<CoiEntityDTO>> GetApprovedCoiRequestsAsync(IEnumerable<int> keywords, DateTime fromDate, int? count);

        /// <summary>
        /// Gets the approved COI requests from repository which are created using Athena app.
        /// </summary>
        /// <returns>The collection of COI requests.</returns>
        Task<IEnumerable<CoiEntityDTO>> GetApprovedCoiRequestsCreatedInAthenaAppAsync();
    }
}
