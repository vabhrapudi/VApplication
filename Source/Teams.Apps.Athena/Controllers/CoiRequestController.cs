// <copyright file="CoiRequestController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Authorization;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes API endpoints related to COI requests.
    /// </summary>
    [Route("api/coi/requests")]
    [ApiController]
    [Authorize]
    public class CoiRequestController : BaseController
    {
        private readonly ILogger logger;

        private readonly ICoiRequestHelper coiRequestHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoiRequestController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="coiRequestHelper">The instance of <see cref="ICoiRequestHelper"/>.</param>
        public CoiRequestController(
            ILogger<CoiRequestController> logger,
            TelemetryClient telemetryClient,
            ICoiRequestHelper coiRequestHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.coiRequestHelper = coiRequestHelper;
        }

        /// <summary>
        /// Creates a new COI request.
        /// </summary>
        /// <param name="coiRequestDetails">The details of COI request to be created.</param>
        /// <returns>The newly created COI request details.</returns>
        [HttpPost]
        [Authorize(AuthorizationPolicyNames.MustBeUser)]
        public async Task<IActionResult> CreateCoiRequestAsync([FromBody] CoiEntityDTO coiRequestDetails)
        {
            this.RecordEvent("HTTP POST- CreateCoiRequestAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "name", coiRequestDetails?.CoiName },
            });

            if (coiRequestDetails.Type == (int)CoiTeamType.None)
            {
                this.logger.LogError("The COI type was not provided.");
                this.RecordEvent("HTTP POST- CreateCoiRequestAsync", RequestType.Failed);

                return this.BadRequest("Valid COI type must be provided.");
            }

            try
            {
                var coiRequest = await this.coiRequestHelper.CreateCoiRequestAsync(coiRequestDetails, Guid.Parse(this.UserAadId), this.Upn, this.UserName);

                if (coiRequest == null)
                {
                    this.logger.LogError("Failed to create COI request. Possible reason is the request with same " +
                        "name already exists.");

                    this.RecordEvent("HTTP POST- CreateCoiRequestAsync", RequestType.Failed);

                    return this.Conflict("The COI request with same name already exists.");
                }

                this.RecordEvent("HTTP POST- CreateCoiRequestAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created, coiRequest);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while creating new COI request.");
                this.RecordEvent("HTTP POST- CreateCoiRequestAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the active COI requests created by logged-in user.
        /// </summary>
        /// <param name="searchText">The search text to be searched in COI name or keywords.</param>
        /// <param name="pageNumber">The page number for which COI requests to be retrieved.</param>
        /// <param name="statusFilterValues">The values to filter requests by status.</param>
        /// <param name="sortColumn">The column to be sorted.</param>
        /// <param name="sortOrder">The order in which requests to be sorted.</param>
        /// <returns>The collection of COI requests.</returns>
        [HttpPost("me")]
        [Authorize(AuthorizationPolicyNames.MustBeUser)]
        public async Task<IActionResult> GetActiveCoiRequestsAsync(string searchText, int pageNumber, [FromBody] IEnumerable<int> statusFilterValues, int sortColumn = (int)CoiSortColumn.CreatedOn, int sortOrder = (int)SortOrder.Ascending)
        {
            this.RecordEvent("HTTP GET- GetActiveCoiRequestsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "userAADId", this.UserAadId },
            });

            if (pageNumber < 0)
            {
                this.RecordEvent("HTTP GET- GetActiveCoiRequestsAsync", RequestType.Failed);
                this.logger.LogError("Invalid page number value received.");

                return this.BadRequest("Invalid page number value.");
            }

            if (!Enum.IsDefined(typeof(CoiSortColumn), sortColumn))
            {
                this.RecordEvent("HTTP GET- GetActiveCoiRequestsAsync", RequestType.Failed);
                this.logger.LogError("Invalid CoiSortColumn value received.");

                return this.BadRequest("Invalid CoiSortColumn value.");
            }

            if (!Enum.IsDefined(typeof(SortOrder), sortOrder))
            {
                this.RecordEvent("HTTP GET- GetActiveCoiRequestsAsync", RequestType.Failed);
                this.logger.LogError("Invalid SortOrder value received.");

                return this.BadRequest("Invalid SortOrder value received.");
            }

            if (!statusFilterValues.IsNullOrEmpty())
            {
                var statusCollectionHasInvalidStatus = statusFilterValues
                    .Any(status => !Enum.IsDefined(typeof(CoiRequestStatus), status));

                if (statusCollectionHasInvalidStatus)
                {
                    this.RecordEvent("HTTP GET- GetActiveCoiRequestsAsync", RequestType.Failed);
                    this.logger.LogError("Some of the status filter values are invalid.");

                    return this.BadRequest("Some of the status filter values are invalid.");
                }
            }

            try
            {
                var activeCoiRequests = await this.coiRequestHelper.GetActiveCoiRequestsAsync(searchText, pageNumber, (CoiSortColumn)sortColumn, (SortOrder)sortOrder, statusFilterValues, Guid.Parse(this.UserAadId));

                this.RecordEvent("HTTP GET- GetActiveCoiRequestsAsync", RequestType.Succeeded);

                return this.Ok(activeCoiRequests);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting active COI requests.");
                this.RecordEvent("HTTP GET- GetActiveCoiRequestsAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets all COI requests created by user which are approved.
        /// </summary>
        /// <param name="keywords">The status filter values.</param>
        /// <param name="fetchPublicOnly">If true then only public approved COI requests will be fetched else all.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("approved")]
        public async Task<IActionResult> GetApprovedCoiRequestAsync([FromBody] IEnumerable<KeywordEntity> keywords, bool fetchPublicOnly = false)
        {
            this.RecordEvent("HTTP GET- GetActiveCoiRequestsAsync", RequestType.Initiated);

            try
            {
                var approvedPublicCOIs = await this.coiRequestHelper.GetApprovedCoiRequestAsync(keywords, fetchPublicOnly);

                if (approvedPublicCOIs == null)
                {
                    this.RecordEvent("GetKeywordsAsync", RequestType.Failed);
                    return this.NotFound("keywords not found.");
                }

                this.RecordEvent("GetKeywordsAsync", RequestType.Succeeded);
                return this.Ok(approvedPublicCOIs);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetKeywordsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching keywords.");
                throw;
            }
        }

        /// <summary>
        /// Gets the active COI requests created by logged-in user.
        /// </summary>
        /// <param name="searchText">The search text to be searched in COI name or keywords.</param>
        /// <param name="pageNumber">The page number for which COI requests to be retrieved.</param>
        /// <param name="statusFilterValues">Selected filter values for status.</param>
        /// <param name="sortColumn">The column to be sorted.</param>
        /// <param name="sortOrder">The order in which requests to be sorted.</param>
        /// <returns>The collection of COI requests.</returns>
        [HttpPost("all/pending")]
        public async Task<IActionResult> GetAdminApprovalTabCoiRequestsAsync(string searchText, int pageNumber, [FromBody] List<int> statusFilterValues, int sortColumn = (int)SortColumn.Status, int sortOrder = (int)SortOrder.Ascending)
        {
            this.RecordEvent("HTTP GET- GetPendingForApprovalCoiRequestsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "userAADId", this.UserAadId },
            });

            if (pageNumber < 0)
            {
                this.RecordEvent("HTTP GET- GetPendingForApprovalCoiRequestsAsync", RequestType.Failed);
                this.logger.LogError("Invalid page number value received.");

                return this.BadRequest("Invalid page number value.");
            }

            if (!Enum.IsDefined(typeof(SortColumn), sortColumn))
            {
                this.RecordEvent("HTTP GET- GetPendingForApprovalCoiRequestsAsync", RequestType.Failed);
                this.logger.LogError("Invalid CoiSortColumn value received.");

                return this.BadRequest("Invalid CoiSortColumn value.");
            }

            if (!Enum.IsDefined(typeof(SortOrder), sortOrder))
            {
                this.RecordEvent("HTTP GET- GetPendingForApprovalCoiRequestsAsync", RequestType.Failed);
                this.logger.LogError("Invalid SortOrder value received.");

                return this.BadRequest("Invalid SortOrder value received.");
            }

            try
            {
                var pendingCoiRequests = await this.coiRequestHelper.GetCoiRequestsPendingForApprovalAsync(searchText, pageNumber, (CoiSortColumn)sortColumn, (SortOrder)sortOrder, statusFilterValues);

                this.RecordEvent("HTTP GET- GetPendingForApprovalCoiRequestsAsync", RequestType.Succeeded);

                return this.Ok(pendingCoiRequests);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting active COI requests.");
                this.RecordEvent("HTTP GET- GetPendingForApprovalCoiRequestsAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the COI request details which is not deleted.
        /// </summary>
        /// <param name="tableId">The table Id of COI request to get.</param>
        /// <returns>The details of COI request. If request was not found or deleted, the API returns
        /// Not Found status code.</returns>
        [HttpGet("me/{tableId}")]
        [Authorize(AuthorizationPolicyNames.MustBeCreatorOfCoiRequestPolicy)]
        public async Task<IActionResult> GetCoiRequestAsync(Guid tableId)
        {
            this.RecordEvent("HTTP GET- GetCoiRequestAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "userAADId", this.UserAadId },
                { "tableId", tableId.ToString() },
            });

            if (tableId == Guid.Empty)
            {
                this.logger.LogError("Empty Guid value was provided.");
                this.RecordEvent("HTTP GET- GetCoiRequestAsync", RequestType.Failed);

                return this.BadRequest("Invalid COI table Id was provided.");
            }

            try
            {
                var coiRequest = await this.coiRequestHelper.GetCoiRequestAsync(tableId);

                this.RecordEvent("HTTP GET- GetCoiRequestAsync", RequestType.Succeeded);

                if (coiRequest == null)
                {
                    return this.NotFound();
                }

                return this.Ok(coiRequest);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting COI request.");
                this.RecordEvent("HTTP GET- GetCoiRequestAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Deletes the COI requests in batch.
        /// </summary>
        /// <param name="tableIds">The collection of COI table Ids to be deleted.</param>
        /// <returns>A task representing COI delete operation.</returns>
        [HttpPatch("me/delete")]
        [Authorize(AuthorizationPolicyNames.MustBeUser)]
        public async Task<IActionResult> DeleteCoiRequestsAsync([FromBody] IEnumerable<Guid> tableIds)
        {
            this.RecordEvent("HTTP DELETE- DeleteCoiRequestsAsync", RequestType.Initiated);

            if (tableIds.IsNullOrEmpty())
            {
                this.logger.LogError("The COI Table Ids weren't provided.");
                this.RecordEvent("HTTP DELETE- DeleteCoiRequestsAsync", RequestType.Failed);

                return this.BadRequest();
            }

            var tableIdsToBeDeleted = tableIds.Select(tableId => tableId.ToString());

            try
            {
                await this.coiRequestHelper.DeleteCoiRequestsAsync(tableIdsToBeDeleted, Guid.Parse(this.UserAadId));

                this.RecordEvent("HTTP DELETE- DeleteCoiRequestsAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while deleting COI requests.");
                this.RecordEvent("HTTP DELETE- DeleteCoiRequestsAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Creates a new draft COI request.
        /// </summary>
        /// <param name="draftCoiRequest">The draft COI details.</param>
        /// <returns>The created draft COI details.</returns>
        [HttpPost("me/draft")]
        [Authorize(AuthorizationPolicyNames.MustBeUser)]
        public async Task<IActionResult> CreateDraftCoiRequestAsync([FromBody] DraftCoiEntityDTO draftCoiRequest)
        {
            this.RecordEvent("HTTP POST- CreateDraftCoiRequestAsync", RequestType.Initiated);

            try
            {
                var coiRequest = await this.coiRequestHelper.CreateDraftCoiRequestAsync(draftCoiRequest, Guid.Parse(this.UserAadId), this.Upn);

                if (coiRequest == null)
                {
                    this.logger.LogError("Failed to draft COI request. Possible reason is the request with same " +
                        "name already exists.");

                    this.RecordEvent("HTTP POST- CreateDraftCoiRequestAsync", RequestType.Failed);

                    return this.Conflict("The COI request with same name already exists.");
                }

                this.RecordEvent("HTTP POST- CreateDraftCoiRequestAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created, coiRequest);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while creating new draft COI request.");
                this.RecordEvent("HTTP POST- CreateDraftCoiRequestAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Submits a drafted COI request.
        /// </summary>
        /// <param name="tableId">The Id of COI to be submitted.</param>
        /// <param name="draftCoiRequest">The draft COI request details.</param>
        /// <returns>A COI details.</returns>
        [HttpPatch("me/draft/submit/{tableId}")]
        [Authorize(AuthorizationPolicyNames.MustBeCreatorOfCoiRequestPolicy)]
        public async Task<IActionResult> SubmitDraftCoiRequestAsync(Guid tableId, [FromBody] CoiEntityDTO draftCoiRequest)
        {
            this.RecordEvent("HTTP PATCH- SubmitDraftCoiRequestAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "tableId", tableId.ToString() },
            });

            if (tableId == Guid.Empty)
            {
                this.logger.LogError("Invalid COI table Id received to submit a draft COI request.");
                this.RecordEvent("HTTP PATCH- SubmitDraftCoiRequestAsync", RequestType.Failed);

                return this.BadRequest("A valid COI table Id is required to submit a draft COI request.");
            }

#pragma warning disable CA1062 // Null check validated at model level using data annotations.
            if (draftCoiRequest.Type == (int)CoiTeamType.None)
#pragma warning restore CA1062 // Null check validated at model level using data annotations.
            {
                this.logger.LogError("Invalid COI type received to submit a draft COI request.");
                this.RecordEvent("HTTP PATCH- SubmitDraftCoiRequestAsync", RequestType.Failed);

                return this.BadRequest("A valid COI type is required to submit a draft COI request.");
            }

            draftCoiRequest.TableId = tableId.ToString();

            try
            {
                var coiRequest = await this.coiRequestHelper.SubmitDraftCoiRequestAsync(draftCoiRequest, Guid.Parse(this.UserAadId), this.Upn, this.UserName);

                if (coiRequest == null)
                {
                    this.logger.LogError("The COI request was not found or the request is not in draft state or the request with same name already exists.");
                    this.RecordEvent("HTTP PATCH- SubmitDraftCoiRequestAsync", RequestType.Failed);

                    return this.Conflict("Unable to submit a draft COI request. The possible reason is that " +
                        " the COI request with same name already exists or request status is not Draft or the request wasn't found.");
                }

                this.RecordEvent("HTTP PATCH- SubmitDraftCoiRequestAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created, coiRequest);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while submitting draft COI requesT.");
                this.RecordEvent("HTTP PATCH- SubmitDraftCoiRequestAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Updates a drafted COI request.
        /// </summary>
        /// <param name="tableId">The Id of COI to be updated.</param>
        /// <param name="draftCoiRequest">The draft COI details.</param>
        /// <returns>The updated draft COI request details.</returns>
        [HttpPatch("me/draft/update/{tableId}")]
        [Authorize(AuthorizationPolicyNames.MustBeCreatorOfCoiRequestPolicy)]
        public async Task<IActionResult> UpdateDraftCoiRequestAsync(Guid tableId, [FromBody] DraftCoiEntityDTO draftCoiRequest)
        {
            this.RecordEvent("HTTP PATCH- UpdateDraftCoiRequestAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "tableId", tableId.ToString() },
            });

            if (tableId == Guid.Empty)
            {
                this.logger.LogError("Invalid COI table Id received to update a draft COI request.");
                this.RecordEvent("HTTP PATCH- UpdateDraftCoiRequestAsync", RequestType.Failed);

                return this.BadRequest("A valid COI table Id is required to update a draft COI request.");
            }

#pragma warning disable CA1062 // Null check validated at model level using data annotations.
            draftCoiRequest.TableId = tableId.ToString();
#pragma warning restore CA1062 // Null check validated at model level using data annotations.

            try
            {
                var updatedDraftCoiRequest = await this.coiRequestHelper.UpdateDraftCoiRequestAsync(draftCoiRequest, Guid.Parse(this.UserAadId), this.Upn);

                if (updatedDraftCoiRequest == null)
                {
                    this.logger.LogError("The COI request was not found or the request is not in draft state or the request with same name already exists.");
                    this.RecordEvent("HTTP PATCH- UpdateDraftCoiRequestAsync", RequestType.Failed);

                    return this.Conflict("Unable to update COI request as request was not found or the request is not in draft state or the request with same name already exists.");
                }

                this.RecordEvent("HTTP PATCH- UpdateDraftCoiRequestAsync", RequestType.Succeeded);

                return this.Ok(updatedDraftCoiRequest);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while updating a draft COI request.");
                this.RecordEvent("HTTP PATCH- UpdateDraftCoiRequestAsync", RequestType.Failed);
                throw;
            }
        }
    }
}
