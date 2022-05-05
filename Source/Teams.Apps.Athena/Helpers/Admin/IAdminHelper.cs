// <copyright file="IAdminHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing approve and reject request.
    /// </summary>
    public interface IAdminHelper
    {
        /// <summary>
        /// Gets coi request details by Id.
        /// </summary>
        /// <param name="requestId">Get coi request id.</param>
        /// <returns>Returns request details</returns>
        Task<CoiEntityDTO> GetCoiRequestByIdAsync(string requestId);

        /// <summary>
        /// Gets news request details by Id.
        /// </summary>
        /// <param name="requestId">Get coi request id.</param>
        /// <returns>Returns request details</returns>
        Task<NewsEntityDTO> GetNewsRequestByIdAsync(string requestId);

        /// <summary>
        /// Approves or rejects COI requests.
        /// </summary>
        /// <param name="coiRequestIds">The COI request Ids.</param>
        /// <param name="isApprove">True if requests to be approved. False if requests to be rejected.</param>
        /// <param name="rejectComments">The reject comments.</param>
        /// <returns>A asynchronous operation to approve or reject COI requests.</returns>
        Task<bool> ApproveOrRejectCoiRequestsAsync(IEnumerable<Guid> coiRequestIds, bool isApprove, string rejectComments);

        /// <summary>
        /// Approves or rejects news article requests.
        /// </summary>
        /// <param name="newsArticleRequestIds">The news article request Ids.</param>
        /// <param name="isApprove">True if requests to be approved. False if requests to be rejected.</param>
        /// <param name="rejectComments">The reject comments.</param>
        /// <param name="makeNewsArticleImportant">Whether to make news article important.</param>
        /// <returns>A asynchronous operation to approve or reject news article requests.</returns>
        Task<bool> ApproveOrRejectNewsArticleRequestsAsync(IEnumerable<Guid> newsArticleRequestIds, bool isApprove, string rejectComments, bool? makeNewsArticleImportant = null);
    }
}