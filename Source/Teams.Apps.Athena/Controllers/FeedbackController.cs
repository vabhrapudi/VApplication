// <copyright file="FeedbackController.cs" company="NPS Foundation">
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
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Handles requests related to feedback.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Instance of feedbackhelper service for saving feedback.
        /// </summary>
        private readonly IFeedbackHelper feedbackHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="feedbackHelper">Instance of feedbackhelper service for saving feedback.</param>
        public FeedbackController(
            TelemetryClient telemetryClient,
            ILogger<FeedbackController> logger,
            IFeedbackHelper feedbackHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.feedbackHelper = feedbackHelper;
        }

        /// <summary>
        /// Saves user feedback related to Athena.
        /// </summary>
        /// <param name="feedback">Feedback given by user.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("athena")]
        public async Task<IActionResult> SaveAthenaFeedbackAsync([FromBody] AthenaFeedbackCreateDTO feedback)
        {
            this.RecordEvent("HTTP POST- SaveAthenaFeedbackAsync", RequestType.Initiated);

            try
            {
                await this.feedbackHelper.SaveAthenaFeedback(feedback, this.UserAadId);

                return this.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while saving athena feedback.");
                this.RecordEvent("HTTP POST- SaveAthenaFeedbackAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets user feedbacks related to Athena.
        /// </summary>
        /// <param name="pageNumber">Page number for which feedbacks needs to be fetched.</param>
        /// <param name="feedbackFilterValues">The values to filter feedbacks by feedback types.</param>
        /// <returns>List of feedbacks.</returns>
        [HttpPost("athenaFeedbacks")]
        public async Task<IActionResult> GetAthenaFeedbacksAsync(int pageNumber, [FromBody] IEnumerable<int> feedbackFilterValues)
        {
            this.RecordEvent("HTTP GET- GetAthenaFeedbacksAsync", RequestType.Initiated);

            if (pageNumber < 0)
            {
                this.RecordEvent("HTTP GET- GetAthenaFeedbacksAsync", RequestType.Failed);
                this.logger.LogError("Invalid page number value received.");

                return this.BadRequest("Invalid page number value.");
            }

            if (!feedbackFilterValues.IsNullOrEmpty())
            {
                var feedbackHasInvalidType = feedbackFilterValues
                    .Any(status => !Enum.IsDefined(typeof(AthenaFeedbackValues), status));

                if (feedbackHasInvalidType)
                {
                    this.RecordEvent("HTTP GET- GetAthenaFeedbacksAsync", RequestType.Failed);
                    this.logger.LogError("Some of the feedback filter values are invalid.");

                    return this.BadRequest("Some of the feedback filter values are invalid.");
                }
            }

            try
            {
                var feedbacks = await this.feedbackHelper.GetAthenaFeedbacksAsync(pageNumber, feedbackFilterValues);

                if (feedbacks == null)
                {
                    this.RecordEvent("HTTP GET- GetAthenaFeedbacksAsync", RequestType.Failed);
                    return this.NotFound();
                }

                this.RecordEvent("HTTP GET- GetAthenaFeedbacksAsync", RequestType.Succeeded);

                return this.Ok(feedbacks);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while fetching athena feedbacks.");
                this.RecordEvent("HTTP GET- GetAthenaFeedbacksAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets user feedback details by feedback Id.
        /// </summary>
        /// <param name="feedbackId">The feedback Id.</param>
        /// <returns>Feedback details.</returns>
        [HttpGet("athena/{feedbackId}")]
        public async Task<IActionResult> GetAthenaFeedbackDetailsAsync(Guid feedbackId)
        {
            this.RecordEvent("HTTP GET- GetAthenaFeedbackDetailsAsync", RequestType.Initiated);

            if (feedbackId == Guid.Empty)
            {
                this.logger.LogError("Feedback Id is null or invalid.");
                this.RecordEvent("HTTP GET- GetAthenaFeedbackDetailsAsync", RequestType.Failed);
                return this.BadRequest("Invalid feedback Id.");
            }

            try
            {
                var feedbacks = await this.feedbackHelper.GetAthenaFeedbackDetailsAsync(feedbackId.ToString());

                if (feedbacks == null)
                {
                    this.RecordEvent("HTTP GET- GetAthenaFeedbackDetailsAsync", RequestType.Failed);
                    return this.NotFound();
                }

                this.RecordEvent("HTTP GET- GetAthenaFeedbackDetailsAsync", RequestType.Succeeded);

                return this.Ok(feedbacks);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while fetching athena feedback details.");
                this.RecordEvent("HTTP GET- GetAthenaFeedbackDetailsAsync", RequestType.Failed);
                throw;
            }
        }
    }
}
