// <copyright file="ICoiRequestHelper.cs" company="NPS Foundation">
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
    public interface ICoiRequestHelper
    {
        /// <summary>
        /// Creates a new COI request.
        /// </summary>
        /// <param name="coiRequestDetails">The details of COI request to be created.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <param name="upn">The logged-in user's user principle name (UPN).</param>
        /// <param name="userName">The logged in user's name</param>
        /// <returns>The details of newly created COI request.</returns>
        Task<CoiEntityDTO> CreateCoiRequestAsync(CoiEntityDTO coiRequestDetails, Guid userAadId, string upn, string userName);

        /// <summary>
        /// Creates a new draft COI request.
        /// </summary>
        /// <param name="draftCoiRequestDetails">The draft COI details.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <param name="upn">The logged-in user's user principle name (UPN).</param>
        /// <returns>The details of newly created draft COI request.</returns>
        Task<CoiEntityDTO> CreateDraftCoiRequestAsync(DraftCoiEntityDTO draftCoiRequestDetails, Guid userAadId, string upn);

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
        /// Deletes COI requests in batch.
        /// </summary>
        /// <param name="coiRequestsIds">The collection of COI requests Ids to be deleted.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <returns>A task indicating requests delete operation.</returns>
        Task DeleteCoiRequestsAsync(IEnumerable<string> coiRequestsIds, Guid userAadId);

        /// <summary>
        /// Gets an active COI request by Id.
        /// </summary>
        /// <param name="coiRequestId">The COI request Id to get.</param>
        /// <returns>The COI request details.</returns>
        Task<CoiEntityDTO> GetCoiRequestAsync(Guid coiRequestId);

        /// <summary>
        /// Submits a draft COI request.
        /// </summary>
        /// <param name="draftCoiRequest">The draft COI details to be submitted.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <param name="upn">The logged-in user's user principle name (UPN).</param>
        /// <param name="userName">The logged-in user's name.</param>
        /// <returns>The submitted COI request details.</returns>
        Task<CoiEntityDTO> SubmitDraftCoiRequestAsync(CoiEntityDTO draftCoiRequest, Guid userAadId, string upn, string userName);

        /// <summary>
        /// Updates a draft COI request.
        /// </summary>
        /// <param name="draftCoiRequestDetails">The draft COI details to be updated.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <param name="upn">The logged-in user's user principle name (UPN).</param>
        /// <returns>The updated draft COI request details.</returns>
        Task<CoiEntityDTO> UpdateDraftCoiRequestAsync(DraftCoiEntityDTO draftCoiRequestDetails, Guid userAadId, string upn);

        /// <summary>
        /// Gets all COI requests created by user which are approved.
        /// </summary>
        /// <param name="keywords">The status filter values.</param>
        /// <param name="fetchPublicOnly">If true then only public approved COI requests will be fetched else all.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IEnumerable<CoiEntityDTO>> GetApprovedCoiRequestAsync(IEnumerable<KeywordEntity> keywords, bool fetchPublicOnly);

        /// <summary>
        /// Searches for COI requests pending for admin approval.
        /// </summary>
        /// <param name="searchText">The search text to be searched in COI name or keywords.</param>
        /// <param name="pageNumber">The page number for which COI requests to be retrieved.</param>
        /// <param name="sortColumn">The column to be sorted.</param>
        /// <param name="sortOrder">The order in which requests to be sorted.</param>
        /// <param name="selectedStatusFilter">List of selected status filters.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<IEnumerable<CoiRequestViewDTO>> GetCoiRequestsPendingForApprovalAsync(string searchText, int pageNumber, CoiSortColumn sortColumn, SortOrder sortOrder, List<int> selectedStatusFilter);
    }
}
