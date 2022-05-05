// <copyright file="CommentsController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes API endpoints related to research project's comments.
    /// </summary>
    [Route("api/comments")]
    [ApiController]
    [Authorize]
    public class CommentsController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for comments.
        /// </summary>
        private ICommentsHelper commentsHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="commentsHelper">The instance of <see cref="ICommentsHelper"/> class.</param>
        public CommentsController(
            ILogger<CommentsController> logger,
            TelemetryClient telemetryClient,
            ICommentsHelper commentsHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.commentsHelper = commentsHelper;
        }

        /// <summary>
        /// Gets the comments of resource.
        /// </summary>
        /// <param name="resourceTableId">Resource table Id.</param>
        /// <param name="resourceTypeId">Resource type Id.</param>
        /// <returns>The comments.</returns>
        [HttpGet]
        public async Task<IActionResult> GetResourceComments(string resourceTableId, int resourceTypeId)
        {
            this.RecordEvent("GetResourceComments", RequestType.Initiated);

            if (string.IsNullOrEmpty(resourceTableId))
            {
                this.RecordEvent("GetResourceComments", RequestType.Failed);
                this.logger.LogError("Empty resource table Id value was provided.");

                return this.BadRequest("The valid resource table Id must be provided.");
            }

            try
            {
                var comments = await this.commentsHelper.GetResourceComments(resourceTableId, resourceTypeId);

                this.RecordEvent("GetResourceComments", RequestType.Succeeded);

                return this.Ok(comments);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetResourceComments", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting comments of resource.");

                throw;
            }
        }

        /// <summary>
        /// Add comment for resource.
        /// </summary>
        /// <param name="resourceTableId">Resource table Id.</param>
        /// <param name="resourceTypeId">Resource type Id.</param>
        /// <param name="comment">The comment.</param>
        /// <returns>The created comment.</returns>
        [HttpPost]
        public async Task<IActionResult> AddCommentAsync(string resourceTableId, int resourceTypeId, string comment)
        {
            this.RecordEvent("AddCommentAsync", RequestType.Initiated);

            if (string.IsNullOrEmpty(resourceTableId))
            {
                this.RecordEvent("AddCommentAsync", RequestType.Failed);
                this.logger.LogError("Empty resource table Id value was provided.");

                return this.BadRequest("The valid resource table Id must be provided.");
            }

            if (string.IsNullOrEmpty(comment))
            {
                this.RecordEvent("AddCommentAsync", RequestType.Failed);
                this.logger.LogError("Invalid comment.");

                return this.BadRequest("The comment is invalid.");
            }

            try
            {
                var createdComment = await this.commentsHelper.AddCommentAsync(resourceTableId, resourceTypeId, comment, this.UserAadId, this.UserName);

                this.RecordEvent("AddCommentAsync", RequestType.Succeeded);

                return this.Ok(createdComment);
            }
            catch (Exception ex)
            {
                this.RecordEvent("AddCommentAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while adding comment.");
                throw;
            }
        }
    }
}