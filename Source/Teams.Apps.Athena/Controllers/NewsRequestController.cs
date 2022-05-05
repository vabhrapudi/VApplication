// <copyright file="NewsRequestController.cs" company="NPS Foundation">
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
    /// Exposes API endpoint related news article requests.
    /// </summary>
    [Route("api/news/requests")]
    [ApiController]
    [Authorize]
    public class NewsRequestController : BaseController
    {
        private readonly ILogger logger;

        private readonly INewsRequestHelper newsRequestHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsRequestController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> object which logs errors and information.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="newsRequestHelper">The instance of <see cref="INewsRequestHelper"/>.</param>
        public NewsRequestController(
            ILogger<NewsRequestController> logger,
            TelemetryClient telemetryClient,
            INewsRequestHelper newsRequestHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.newsRequestHelper = newsRequestHelper;
        }

        /// <summary>
        /// Creates a new news article request.
        /// </summary>
        /// <param name="newsArticleRequestDetails">The details of news article request to be created.</param>
        /// <returns>The newly created news article request details.</returns>
        [HttpPost]
        [Authorize(AuthorizationPolicyNames.MustBeUser)]
        public async Task<IActionResult> CreateNewsArticleRequestAsync([FromBody] NewsEntityDTO newsArticleRequestDetails)
        {
            this.RecordEvent("HTTP POST- CreateNewsArticleRequestAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "title", newsArticleRequestDetails?.Title },
            });

            if (newsArticleRequestDetails == null)
            {
                this.logger.LogError("The news article request details weren't provided.");
                this.RecordEvent("HTTP POST- CreateNewsArticleRequestAsync", RequestType.Failed);

                return this.BadRequest();
            }

            try
            {
                var newsArticleRequest = await this.newsRequestHelper.CreateNewsArticleRequestAsync(newsArticleRequestDetails, Guid.Parse(this.UserAadId), this.Upn, this.UserName);

                if (newsArticleRequest == null)
                {
                    this.logger.LogError("Failed to create news article request. Possible reason is the request with same " +
                        "title already exists.");

                    this.RecordEvent("HTTP POST- CreateNewsArticleRequestAsync", RequestType.Failed);

                    return this.Conflict("The news article request with same title already exists.");
                }

                this.RecordEvent("HTTP POST- CreateNewsArticleRequestAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created, newsArticleRequest);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while creating new news article request.");
                this.RecordEvent("HTTP POST- CreateNewsArticleRequestAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the active news articles requests created by logged-in user.
        /// </summary>
        /// <param name="searchText">The search text which will search by news article title or keywords.</param>
        /// <param name="pageNumber">The page number of which requests to be fetched.</param>
        /// <param name="statusFilterValues">The values to filter requests by status.</param>
        /// <param name="sortColumn">The sort column of type <see cref="NewsArticleSortColumn"/>.</param>
        /// <param name="sortOrder">The sort order of type <see cref="SortOrder"/>.</param>
        /// <returns>The collection of news article requests.</returns>
        [HttpPost("me")]
        [Authorize(AuthorizationPolicyNames.MustBeUser)]
        public async Task<IActionResult> GetActiveNewsArticleRequestsAsync(string searchText, int pageNumber, [FromBody] IEnumerable<int> statusFilterValues, int sortColumn = (int)NewsArticleSortColumn.CreatedAt, int sortOrder = (int)SortOrder.Ascending)
        {
            this.RecordEvent("HTTP GET- GetActiveNewsArticleRequestsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "userAADId", this.UserAadId },
            });

            if (pageNumber < 0)
            {
                this.RecordEvent("HTTP GET- GetActiveNewsArticleRequestsAsync", RequestType.Failed);
                this.logger.LogError("Invalid page number value received.");

                return this.BadRequest("Invalid page number value.");
            }

            if (!Enum.IsDefined(typeof(NewsArticleSortColumn), sortColumn))
            {
                this.logger.LogError("Invalid sort column value provided.");
                this.RecordEvent("HTTP GET- GetActiveNewsArticleRequestsAsync", RequestType.Failed);

                return this.BadRequest("Invalid sort column value provided.");
            }

            if (!Enum.IsDefined(typeof(SortOrder), sortOrder))
            {
                this.logger.LogError("Invalid sort order value provided.");
                this.RecordEvent("HTTP GET- GetActiveNewsArticleRequestsAsync", RequestType.Failed);

                return this.BadRequest("Invalid sort order value provided.");
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
                var newsArticleRequests = await this.newsRequestHelper.GetActiveNewsArticlesAsync(searchText, pageNumber, (NewsArticleSortColumn)sortColumn, (SortOrder)sortOrder, statusFilterValues, Guid.Parse(this.UserAadId));

                this.RecordEvent("HTTP GET- GetActiveNewsArticleRequestsAsync", RequestType.Succeeded);

                return this.Ok(newsArticleRequests);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting news article requests.");
                this.RecordEvent("HTTP GET- GetActiveNewsArticleRequestsAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the active news requests created by logged-in user.
        /// </summary>
        /// <param name="searchText">The search text to be searched in news name or keywords.</param>
        /// <param name="pageNumber">The page number for which news requests to be retrieved.</param>
        /// <param name="statusFilterValues">Selected status filter.</param>
        /// <param name="sortColumn">The column to be sorted.</param>
        /// <param name="sortOrder">The order in which requests to be sorted.</param>
        /// <returns>The collection of news requests.</returns>
        [HttpPost("all/pending")]
        public async Task<IActionResult> GetPendingForApprovalNewsRequestsAsync(string searchText, int pageNumber, [FromBody] List<int> statusFilterValues, int sortColumn = (int)NewsArticleSortColumn.Status, int sortOrder = (int)SortOrder.Ascending)
        {
            this.RecordEvent("HTTP GET- GetPendingForApprovalNewsRequestsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "userAADId", this.UserAadId },
            });

            if (pageNumber < 0)
            {
                this.RecordEvent("HTTP GET- GetPendingForApprovalNewsRequestsAsync", RequestType.Failed);
                this.logger.LogError("Invalid page number value received.");

                return this.BadRequest("Invalid page number value.");
            }

            if (!Enum.IsDefined(typeof(SortColumn), sortColumn))
            {
                this.RecordEvent("HTTP GET- GetPendingForApprovalNewsRequestsAsync", RequestType.Failed);
                this.logger.LogError("Invalid CoiSortColumn value received.");

                return this.BadRequest("Invalid CoiSortColumn value.");
            }

            if (!Enum.IsDefined(typeof(SortOrder), sortOrder))
            {
                this.RecordEvent("HTTP GET- GetPendingForApprovalNewsRequestsAsync", RequestType.Failed);
                this.logger.LogError("Invalid SortOrder value received.");

                return this.BadRequest("Invalid SortOrder value received.");
            }

            try
            {
                var pendingNewsRequests = await this.newsRequestHelper.GetPendingForApprovalNewsArticlesAsync(searchText, pageNumber, (NewsArticleSortColumn)sortColumn, (SortOrder)sortOrder, statusFilterValues);

                this.RecordEvent("HTTP GET- GetPendingForApprovalNewsRequestsAsync", RequestType.Succeeded);

                return this.Ok(pendingNewsRequests);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting active COI requests.");
                this.RecordEvent("HTTP GET- GetPendingForApprovalNewsRequestsAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the news article request details which is not deleted.
        /// </summary>
        /// <param name="tableId">The table Id of news article request to get.</param>
        /// <returns>The details of news article request. If request was not found or deleted, the API returns
        /// Not Found status code.</returns>
        [HttpGet("me/{tableId}")]
        [Authorize(AuthorizationPolicyNames.MustBeCreatorOfNewsArticleRequestPolicy)]
        public async Task<IActionResult> GetNewsArticleRequestAsync(Guid tableId)
        {
            this.RecordEvent("HTTP GET- GetNewsArticleRequestAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "userAADId", this.UserAadId },
                { "newsArticleTableId", tableId.ToString() },
            });

            if (tableId == Guid.Empty)
            {
                this.logger.LogError("Empty Guid value was provided.");
                this.RecordEvent("HTTP GET- GetNewsArticleRequestAsync", RequestType.Failed);

                return this.BadRequest("Invalid news article table Id was provided.");
            }

            try
            {
                var newsArticleRequest = await this.newsRequestHelper.GetNewsArticleRequestAsync(tableId);

                this.RecordEvent("HTTP GET- GetNewsArticleRequestAsync", RequestType.Succeeded);

                if (newsArticleRequest == null)
                {
                    return this.NotFound();
                }

                return this.Ok(newsArticleRequest);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting news article request.");
                this.RecordEvent("HTTP GET- GetNewsArticleRequestAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Deletes the news article requests in batch.
        /// </summary>
        /// <param name="newsArticleTableIds">The collection of news article requests Ids to be deleted.</param>
        /// <returns>A task representing news article delete operation.</returns>
        [HttpPatch("me/delete")]
        [Authorize(AuthorizationPolicyNames.MustBeUser)]
        public async Task<IActionResult> DeleteNewsArticleRequestsAsync([FromBody] IEnumerable<string> newsArticleTableIds)
        {
            this.RecordEvent("HTTP DELETE- DeleteNewsArticleRequestsAsync", RequestType.Initiated);

            if (newsArticleTableIds.IsNullOrEmpty())
            {
                this.logger.LogError("The news article table Ids weren't provided.");
                this.RecordEvent("HTTP DELETE- DeleteNewsArticleRequestsAsync", RequestType.Failed);

                return this.BadRequest();
            }

            try
            {
                await this.newsRequestHelper.DeleteNewsArticleRequestsAsync(newsArticleTableIds, Guid.Parse(this.UserAadId));

                this.RecordEvent("HTTP DELETE- DeleteNewsArticleRequestsAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while deleting news article requests.");
                this.RecordEvent("HTTP DELETE- DeleteNewsArticleRequestsAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Creates a new draft news article request.
        /// </summary>
        /// <param name="draftNewsArticleRequest">The draft news article details.</param>
        /// <returns>The created draft news article details.</returns>
        [HttpPost("me/draft")]
        [Authorize(AuthorizationPolicyNames.MustBeUser)]
        public async Task<IActionResult> CreateDraftNewsArticleRequestAsync([FromBody] DraftNewsEntityDTO draftNewsArticleRequest)
        {
            this.RecordEvent("HTTP POST- CreateDraftNewsArticleRequestAsync", RequestType.Initiated);

            try
            {
                var newsArticleRequest = await this.newsRequestHelper.CreateDraftNewsArticleRequestAsync(draftNewsArticleRequest, Guid.Parse(this.UserAadId), this.Upn);

                if (newsArticleRequest == null)
                {
                    this.logger.LogError("Failed to draft news article request. Possible reason is the request with same " +
                        "title already exists.");

                    this.RecordEvent("HTTP POST- CreateDraftNewsArticleRequestAsync", RequestType.Failed);

                    return this.Conflict("The news article request with same title already exists.");
                }

                this.RecordEvent("HTTP POST- CreateDraftNewsArticleRequestAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created, newsArticleRequest);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while creating new draft news article request.");
                this.RecordEvent("HTTP POST- CreateDraftNewsArticleRequestAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Submits a drafted news article request.
        /// </summary>
        /// <param name="tableId">The draft news table Id to be submitted.</param>
        /// <param name="draftNewsArticleRequest">The draft news article details.</param>
        /// <returns>A news article details.</returns>
        [HttpPatch("me/draft/submit/{tableId}")]
        [Authorize(AuthorizationPolicyNames.MustBeCreatorOfNewsArticleRequestPolicy)]
        public async Task<IActionResult> SubmitDraftNewsArticleRequestAsync(Guid tableId, [FromBody] NewsEntityDTO draftNewsArticleRequest)
        {
            this.RecordEvent("HTTP PATCH- SubmitDraftNewsArticleRequestAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "tableId", tableId.ToString() },
            });

            if (tableId == Guid.Empty)
            {
                this.logger.LogError("Invalid news table Id received to submit a draft news article request.");
                this.RecordEvent("HTTP PATCH- SubmitDraftNewsArticleRequestAsync", RequestType.Failed);

                return this.BadRequest("A valid news table Id is required to submit a draft news article request.");
            }

#pragma warning disable CA1062 // Null check is validated at model level.
            draftNewsArticleRequest.TableId = tableId.ToString();
#pragma warning restore CA1062 // Null check is validated at model level.

            try
            {
                var newsArticleRequest = await this.newsRequestHelper.SubmitDraftNewsArticleRequestAsync(draftNewsArticleRequest, Guid.Parse(this.UserAadId), this.Upn, this.UserName);

                if (newsArticleRequest == null)
                {
                    this.logger.LogError("The news article was not found or the request is not in draft state or the request with same title already exists.");
                    this.RecordEvent("HTTP PATCH- SubmitDraftNewsArticleRequestAsync", RequestType.Failed);

                    return this.Conflict("Unable to submit a draft news article request. The possible reason is that the request with same name already exists or the request status is not Draft or the request wasn't found.");
                }

                this.RecordEvent("HTTP PATCH- SubmitDraftNewsArticleRequestAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created, newsArticleRequest);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while submitting draft news article request.");
                this.RecordEvent("HTTP PATCH- SubmitDraftNewsArticleRequestAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Updates a drafted news article request.
        /// </summary>
        /// <param name="tableId">The table Id of news article to be updated.</param>
        /// <param name="draftNewsArticleRequest">The draft news article details.</param>
        /// <returns>The updated draft news article details.</returns>
        [HttpPatch("me/draft/update/{tableId}")]
        [Authorize(AuthorizationPolicyNames.MustBeCreatorOfNewsArticleRequestPolicy)]
        public async Task<IActionResult> UpdateDraftNewsArticleRequestAsync(Guid tableId, [FromBody] DraftNewsEntityDTO draftNewsArticleRequest)
        {
            this.RecordEvent("HTTP PATCH- UpdateDraftNewsArticleRequestAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "tableId", tableId.ToString() },
            });

            if (tableId == Guid.Empty)
            {
                this.logger.LogError("Invalid news table Id received to update a draft news article request.");
                this.RecordEvent("HTTP PATCH- UpdateDraftNewsArticleRequestAsync", RequestType.Failed);

                return this.BadRequest("A valid news table Id is required to update a draft news article request.");
            }

            // If external link is provided then validate that the value is an URL.
#pragma warning disable CA1062 // Null check is validated at model level using data annotations.
            if (!draftNewsArticleRequest.ExternalLink.IsNullOrEmpty()
#pragma warning restore CA1062 // Null check is validated at model level using data annotations.
                && !Uri.TryCreate(draftNewsArticleRequest.ExternalLink, UriKind.Absolute, out Uri externalLinkResult))
            {
                this.logger.LogError("The external link has value but received invalid URL.");
                this.RecordEvent("HTTP PATCH- UpdateDraftNewsArticleRequestAsync", RequestType.Failed);

                return this.BadRequest("A valid external link should be provided.");
            }

            // If image URL is provided then validate that the value is an URL.
            if (!draftNewsArticleRequest.ImageUrl.IsNullOrEmpty()
                && !Uri.TryCreate(draftNewsArticleRequest.ImageUrl, UriKind.Absolute, out Uri imageUrlResult))
            {
                this.logger.LogError("The image URL has value but received invalid URL.");
                this.RecordEvent("HTTP PATCH- UpdateDraftNewsArticleRequestAsync", RequestType.Failed);

                return this.BadRequest("A valid image URL should be provided.");
            }

            draftNewsArticleRequest.TableId = tableId.ToString();

            try
            {
                var updatedDraftNewsArticleRequest = await this.newsRequestHelper.UpdateDraftNewsArticleRequestAsync(draftNewsArticleRequest, Guid.Parse(this.UserAadId), this.Upn);

                if (updatedDraftNewsArticleRequest == null)
                {
                    this.logger.LogError("The news article was not found or the request is not in draft state or the request with same title already exists.");
                    this.RecordEvent("HTTP PATCH- UpdateDraftNewsArticleRequestAsync", RequestType.Failed);

                    return this.Conflict("Unable to update the draft news article. Possibly, the request was not found or the request is not in draft state or the request with same title already exists.");
                }

                this.RecordEvent("HTTP PATCH- UpdateDraftNewsArticleRequestAsync", RequestType.Succeeded);

                return this.Ok(updatedDraftNewsArticleRequest);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while updating a draft news article request.");
                this.RecordEvent("HTTP PATCH- UpdateDraftNewsArticleRequestAsync", RequestType.Failed);
                throw;
            }
        }
    }
}
