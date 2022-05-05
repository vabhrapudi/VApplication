// <copyright file="ResearchRequestController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes API endpoints related to research requests.
    /// </summary>
    [Route("api/research-requests")]
    [ApiController]
    [Authorize]
    public class ResearchRequestController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for research request.
        /// </summary>
        private IResearchRequestHelper researchRequestHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchRequestController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="researchRequestHelper">The instance of <see cref="ResearchRequestHelper"/> class.</param>
        public ResearchRequestController(
            ILogger<ResearchRequestController> logger,
            TelemetryClient telemetryClient,
            IResearchRequestHelper researchRequestHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.researchRequestHelper = researchRequestHelper;
        }

        /// <summary>
        /// Gets a research request by table Id.
        /// </summary>
        /// <param name="researchRequestTableId">Unique research request Table Id.</param>
        /// <returns>Returns research request details.</returns>
        [HttpGet("{researchRequestTableId}")]
        public async Task<IActionResult> GetResearchRequestByTableIdAsync(Guid researchRequestTableId)
        {
            this.RecordEvent("GetResearchRequestByIdAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "researchProposalTableId", researchRequestTableId.ToString() },
            });

            if (researchRequestTableId == Guid.Empty)
            {
                this.logger.LogError("Research request Id is null or invalid.");
                this.RecordEvent("GetResearchRequestByIdAsync", RequestType.Failed);
                return this.BadRequest("Invalid research request Id.");
            }

            try
            {
                var researchProposalDetails = await this.researchRequestHelper.GetResearchRequestByTableIdAsync(researchRequestTableId.ToString(), this.UserAadId);

                if (researchProposalDetails == null)
                {
                    this.RecordEvent("GetResearchRequestByIdAsync", RequestType.Failed);
                    return this.NotFound("Research request not found.");
                }

                this.RecordEvent("GetResearchRequestByIdAsync", RequestType.Succeeded);
                return this.Ok(researchProposalDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetResearchRequestByIdAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching research request.");
                throw;
            }
        }

        /// <summary>
        /// Stores rating of user for a research request.
        /// </summary>
        /// <param name="researchRequestTableId">The research request table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>Returns HTTP status code OK on successful operation.</returns>
        [HttpPost("rate/{researchRequestTableId}/{rating}")]
        public async Task<IActionResult> RateResearchRequestAsync(Guid researchRequestTableId, int rating)
        {
            this.RecordEvent("RateResearchRequestAsync", RequestType.Initiated);

            if (researchRequestTableId == Guid.Empty)
            {
                this.RecordEvent("RateResearchRequestAsync", RequestType.Failed);
                this.logger.LogError("Empty research request table Id value was provided.");
                return this.BadRequest("The valid research request table Id must be provided.");
            }

            try
            {
                await this.researchRequestHelper.RateResearchRequestAsync(researchRequestTableId.ToString(), rating, this.UserAadId);

                this.RecordEvent("RateResearchRequestAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("RateResearchRequestAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while rating research request.");
                throw;
            }
        }
    }
}
