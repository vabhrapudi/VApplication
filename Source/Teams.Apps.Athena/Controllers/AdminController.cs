// <copyright file="AdminController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Authorization;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Request approval controller is responsible to expose API endpoints for performing request operation on coi/news request.
    /// </summary>
    [Route("api/admin")]
    [ApiController]
    [Authorize]
    public class AdminController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The instance of coi/news entity model repository.
        /// </summary>
        private readonly IAdminHelper adminHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="adminHelper">The instance of request helper which helps in managing approve and reject request.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        public AdminController(
            ILogger<AdminController> logger,
            IAdminHelper adminHelper,
            TelemetryClient telemetryClient)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.adminHelper = adminHelper;
        }

        /// <summary>
        /// Get COI request by id.
        /// </summary>
        /// <param name="requestId">Coi request id.</param>
        /// <returns>Returns coi request details.</returns>
        [HttpGet("coiRequest/{requestId}")]
        public async Task<IActionResult> GetCoiRequestByIdAsync(string requestId)
        {
            this.RecordEvent("Get COI request details for approval- The HTTP GET call to get COI request details for approval has been initiated.", RequestType.Initiated, new Dictionary<string, string>
            {
                { "RequestId", requestId },
            });

            if (string.IsNullOrEmpty(requestId))
            {
                this.RecordEvent("Get COI request details for approval- The HTTP GET call to get COI request details for approval has failed.", RequestType.Failed);
                return this.BadRequest("Invalid request Id.");
            }

            try
            {
                var requestDetails = await this.adminHelper.GetCoiRequestByIdAsync(requestId);

                if (requestDetails == null)
                {
                    this.RecordEvent("Get COI request details for approval- The HTTP GET call to get COI request details for approval has failed.", RequestType.Failed);
                    return this.NotFound("Request not found.");
                }

                this.RecordEvent("Get COI request details for approval- The HTTP GET call to get COI request details for approval has succeeded.", RequestType.Succeeded);
                return this.Ok(requestDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("Get COI request details for approval- The HTTP GET call to get COI request details has failed.", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching request details.");
                throw;
            }
        }

        /// <summary>
        /// Get News request by id.
        /// </summary>
        /// <param name="requestId">News request id.</param>
        /// <returns>Returns news request details.</returns>
        [HttpGet("newsRequest/{requestId}")]
        public async Task<IActionResult> GetNewsRequestByIdAsync(string requestId)
        {
            this.RecordEvent("Get News request details for approval- The HTTP GET call to get news request details for approval has been initiated.", RequestType.Initiated, new Dictionary<string, string>
            {
                { "RequestId", requestId },
            });

            if (string.IsNullOrEmpty(requestId))
            {
                this.RecordEvent("Get News request details for approval- The HTTP GET call to get news request details for approval has failed.", RequestType.Failed);
                return this.BadRequest("Invalid request Id.");
            }

            try
            {
                var requestDetails = await this.adminHelper.GetNewsRequestByIdAsync(requestId);

                if (requestDetails == null)
                {
                    this.RecordEvent("Get News request details for approval- The HTTP GET call to get news request details for approval has failed.", RequestType.Failed);
                    return this.NotFound("Request not found.");
                }

                this.RecordEvent("Get News request details for approval- The HTTP GET call to get news request details for approval has succeeded.", RequestType.Succeeded);
                return this.Ok(requestDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("Get News request details for approval- The HTTP GET call to get news request details has failed.", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching request details.");
                throw;
            }
        }

        /// <summary>
        /// Approve COI requests.
        /// </summary>
        /// <param name="coiRequestIds">The COI request Ids to be approved.</param>
        /// <returns>Returns HTTP status 200 on successful operation.</returns>
        [HttpPost("requests/coi/approve")]
        [Authorize(AuthorizationPolicyNames.MustBeAdminPolicy)]
        public async Task<IActionResult> ApproveCoiRequestsAsync([FromBody] IEnumerable<Guid> coiRequestIds)
        {
            this.RecordEvent("ApproveCoiRequestsAsync", RequestType.Initiated);

            var collectionHasInvalidRequestId = coiRequestIds != null
                && coiRequestIds.Any(coiRequestId => coiRequestId == Guid.Empty);

            if (collectionHasInvalidRequestId)
            {
                this.logger.LogError("Some of request Ids values are invalid.");
                this.RecordEvent("ApproveCoiRequestsAsync", RequestType.Failed);
                return this.BadRequest("Some of request Ids values are invalid.");
            }

            try
            {
                var coiRequestsApprovedSuccessfully = await this.adminHelper.ApproveOrRejectCoiRequestsAsync(coiRequestIds, true, null);

                if (coiRequestsApprovedSuccessfully)
                {
                    return this.Ok(true);
                }

                return this.Ok(false);
            }
            catch (Exception ex)
            {
                this.RecordEvent("ApproveCoiRequestsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while approving COI requests.");
                throw;
            }
        }

        /// <summary>
        /// Reject COI requests.
        /// </summary>
        /// <param name="rejectReason">The reject reason comments.</param>
        /// <param name="coiRequestIds">The COI request Ids to be rejected.</param>
        /// <returns>Returns HTTP status 200 on successful operation.</returns>
        [HttpPost("requests/coi/reject")]
        [Authorize(AuthorizationPolicyNames.MustBeAdminPolicy)]
        public async Task<IActionResult> RejectCoiRequestsAsync(string rejectReason, [FromBody] IEnumerable<Guid> coiRequestIds)
        {
            this.RecordEvent("RejectCoiRequestsAsync", RequestType.Initiated);

            var collectionHasInvalidRequestId = coiRequestIds != null
                && coiRequestIds.Any(coiRequestId => coiRequestId == Guid.Empty);

            if (collectionHasInvalidRequestId)
            {
                this.logger.LogError("Some of request Ids values are invalid.");
                this.RecordEvent("RejectCoiRequestsAsync", RequestType.Failed);
                return this.BadRequest("Some of request Ids values are invalid.");
            }

            if (string.IsNullOrWhiteSpace(rejectReason))
            {
                this.logger.LogError("Reject reason was not provided.");
                this.RecordEvent("RejectCoiRequestsAsync", RequestType.Failed);
                return this.BadRequest("Reject reason was not provided.");
            }

            try
            {
                var coiRequestsRejectedSuccessfully = await this.adminHelper.ApproveOrRejectCoiRequestsAsync(coiRequestIds, false, rejectReason);

                if (coiRequestsRejectedSuccessfully)
                {
                    return this.Ok(true);
                }

                return this.Ok(false);
            }
            catch (Exception ex)
            {
                this.RecordEvent("RejectCoiRequestsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while rejecting COI requests.");
                throw;
            }
        }

        /// <summary>
        /// Approve news article requests.
        /// </summary>
        /// <param name="newsArticleRequestIds">The news article request Ids to be approved.</param>
        /// <param name="makeNewsArticleImportant">Whether to make news article important or not.</param>
        /// <returns>Returns HTTP status 200 on successful operation.</returns>
        [HttpPost("requests/news/approve")]
        [Authorize(AuthorizationPolicyNames.MustBeAdminPolicy)]
        public async Task<IActionResult> ApproveNewsArticleRequestsAsync([FromBody] IEnumerable<Guid> newsArticleRequestIds, bool? makeNewsArticleImportant = null)
        {
            this.RecordEvent("ApproveNewsArticleRequestsAsync", RequestType.Initiated);

            var collectionHasInvalidRequestId = newsArticleRequestIds != null
                && newsArticleRequestIds.Any(newsArticleRequestId => newsArticleRequestId == Guid.Empty);

            if (collectionHasInvalidRequestId)
            {
                this.logger.LogError("Some of request Ids values are invalid.");
                this.RecordEvent("ApproveNewsArticleRequestsAsync", RequestType.Failed);
                return this.BadRequest("Some of request Ids values are invalid.");
            }

            try
            {
                var newsArticleRequestsApprovedSuccessfully = await this.adminHelper
                    .ApproveOrRejectNewsArticleRequestsAsync(newsArticleRequestIds, true, null, makeNewsArticleImportant);

                if (newsArticleRequestsApprovedSuccessfully)
                {
                    return this.Ok(true);
                }

                return this.Ok(false);
            }
            catch (Exception ex)
            {
                this.RecordEvent("ApproveNewsArticleRequestsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while approving news article requests.");
                throw;
            }
        }

        /// <summary>
        /// Reject news article requests.
        /// </summary>
        /// <param name="rejectReason">The reject reason comments.</param>
        /// <param name="newsArticleRequestIds">The news article request Ids to be rejected.</param>
        /// <returns>Returns HTTP status 200 on successful operation.</returns>
        [HttpPost("requests/news/reject")]
        [Authorize(AuthorizationPolicyNames.MustBeAdminPolicy)]
        public async Task<IActionResult> RejectNewsArticleRequestsAsync(string rejectReason, [FromBody] IEnumerable<Guid> newsArticleRequestIds)
        {
            this.RecordEvent("RejectNewsArticleRequestsAsync", RequestType.Initiated);

            var collectionHasInvalidRequestId = newsArticleRequestIds != null
                && newsArticleRequestIds.Any(newsArticleRequestId => newsArticleRequestId == Guid.Empty);

            if (collectionHasInvalidRequestId)
            {
                this.logger.LogError("Some of request Ids values are invalid.");
                this.RecordEvent("RejectNewsArticleRequestsAsync", RequestType.Failed);
                return this.BadRequest("Some of request Ids values are invalid.");
            }

            if (string.IsNullOrWhiteSpace(rejectReason))
            {
                this.logger.LogError("Reject reason was not provided.");
                this.RecordEvent("RejectNewsArticleRequestsAsync", RequestType.Failed);
                return this.BadRequest("Reject reason was not provided.");
            }

            try
            {
                var newsArticleRequestsRejectedSuccessfully = await this.adminHelper
                    .ApproveOrRejectNewsArticleRequestsAsync(newsArticleRequestIds, false, rejectReason);

                if (newsArticleRequestsRejectedSuccessfully)
                {
                    return this.Ok(true);
                }

                return this.Ok(false);
            }
            catch (Exception ex)
            {
                this.RecordEvent("RejectNewsArticleRequestsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while rejecting news article requests.");
                throw;
            }
        }
    }
}