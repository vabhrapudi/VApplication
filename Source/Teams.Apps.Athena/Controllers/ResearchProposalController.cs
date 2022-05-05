// <copyright file="ResearchProposalController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes API endpoints related to research proposal.
    /// </summary>
    [Route("api/research-proposals")]
    [ApiController]
    [Authorize]
    public class ResearchProposalController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The instance of <see cref="ResearchProposalHelper"/> class.
        /// </summary>
        private readonly IResearchProposalHelper researchProposalHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchProposalController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="researchProposalHelper">The instance of <see cref="ResearchProposalHelper"/> class.</param>
        public ResearchProposalController(
            ILogger<ResearchProposalController> logger,
            TelemetryClient telemetryClient,
            IResearchProposalHelper researchProposalHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.researchProposalHelper = researchProposalHelper;
        }

        /// <summary>
        /// Create a new research proposal.
        /// </summary>
        /// <param name="researchProposalCreateDTO">The details of research proposal to be created.</param>
        /// <returns>Returns created status code.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateResearchProposalAsync([FromBody] ResearchProposalCreateDTO researchProposalCreateDTO)
        {
            this.RecordEvent("CreateResearchProposalAsync", RequestType.Initiated);

            try
            {
                var researchProject = await this.researchProposalHelper.CreateResearchProposalAsync(researchProposalCreateDTO, this.UserAadId);

                if (researchProject == null)
                {
                    this.RecordEvent("CreateResearchProposalAsync", RequestType.Failed);
                    return this.Conflict("Unable to create research proposal. The possible reason is that " + " the research proposal with same title already exists.");
                }

                this.RecordEvent("CreateResearchProjectAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                this.RecordEvent("CreateResearchProposalAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while creating research proposal.");
                throw;
            }
        }

        /// <summary>
        /// Gets a research proposal by table Id.
        /// </summary>
        /// <param name="researchProposalTableId">Unique research proposal Table Id.</param>
        /// <returns>Returns research proposal details.</returns>
        [HttpGet("{researchProposalTableId}")]
        public async Task<IActionResult> GetResearchProposalByTableIdAsync(Guid researchProposalTableId)
        {
            this.RecordEvent("GetResearchProposalByIdAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "researchProposalTableId", researchProposalTableId.ToString() },
            });

            if (researchProposalTableId == Guid.Empty)
            {
                this.logger.LogError("Research proposal Id is null or invalid.");
                this.RecordEvent("GetResearchProposalByIdAsync", RequestType.Failed);
                return this.BadRequest("Invalid research proposal Id.");
            }

            try
            {
                var researchProposalDetails = await this.researchProposalHelper.GetResearchProposalByTableIdAsync(researchProposalTableId.ToString(), this.UserAadId);

                if (researchProposalDetails == null)
                {
                    this.RecordEvent("GetResearchProposalByIdAsync", RequestType.Failed);
                    return this.NotFound("Research proposal not found.");
                }

                this.RecordEvent("GetResearchProposalByIdAsync", RequestType.Succeeded);
                return this.Ok(researchProposalDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetResearchProposalByIdAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching research proposal.");
                throw;
            }
        }

        /// <summary>
        /// Stores rating of user for a research proposal.
        /// </summary>
        /// <param name="researchProposalTableId">The research proposal table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>Returns HTTP status code OK on successful operation.</returns>
        [HttpPost("rate/{researchProposalTableId}/{rating}")]
        public async Task<IActionResult> RateResearchProposalAsync(Guid researchProposalTableId, int rating)
        {
            this.RecordEvent("RateResearchProposalAsync", RequestType.Initiated);

            if (researchProposalTableId == Guid.Empty)
            {
                this.RecordEvent("RateResearchProposalAsync", RequestType.Failed);
                this.logger.LogError("Empty research proposal table Id value was provided.");
                return this.BadRequest("The valid research proposal table Id must be provided.");
            }

            try
            {
                await this.researchProposalHelper.RateResearchProposalAsync(researchProposalTableId.ToString(), rating, this.UserAadId);

                this.RecordEvent("RateResearchProposalAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("RateResearchProposalAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while rating research proposal.");
                throw;
            }
        }
    }
}
