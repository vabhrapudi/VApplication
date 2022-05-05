// <copyright file="ResearchProjectController.cs" company="NPS Foundation">
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
    /// Exposes API endpoints related to research projects.
    /// </summary>
    [Route("api/researchProjects")]
    [ApiController]
    [Authorize]
    public class ResearchProjectController : BaseController
    {
        private readonly ILogger logger;

        private IResearchProjectHelper researchProjectHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResearchProjectController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="researchProjectHelper">The instance of <see cref="IResearchProjectHelper"/> class.</param>
        public ResearchProjectController(
            ILogger<ResearchProjectController> logger,
            TelemetryClient telemetryClient,
            IResearchProjectHelper researchProjectHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.researchProjectHelper = researchProjectHelper;
        }

        /// <summary>
        /// Create a new research project.
        /// </summary>
        /// <param name="researchProjectCreateDTO">The details of research project to be created.</param>
        /// <returns>Returns created status code.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateResearchProjectAsync([FromBody] ResearchProjectCreateDTO researchProjectCreateDTO)
        {
            this.RecordEvent("CreateResearchProjectAsync", RequestType.Initiated);

            try
            {
                var researchProject = await this.researchProjectHelper.CreateResearchProjectAsync(researchProjectCreateDTO);

                if (researchProject == null)
                {
                    this.RecordEvent("CreateResearchProjectAsync", RequestType.Failed);
                    return this.Conflict("Unable to create research project. The possible reason is that " + " the research project with same title already exists.");
                }

                this.RecordEvent("CreateResearchProjectAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                this.RecordEvent("CreateResearchProjectAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while creating research project.");
                throw;
            }
        }

        /// <summary>
        /// Gets a research project by table Id.
        /// </summary>
        /// <param name="researchProjectTableId">Unique research project Table Id.</param>
        /// <returns>Returns research project details.</returns>
        [HttpGet("{researchProjectTableId}")]
        public async Task<IActionResult> GetResearchProjectByIdAsync(string researchProjectTableId)
        {
            this.RecordEvent("GetResearchProjectByIdAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "researchProjectTableId", researchProjectTableId },
            });

            if (researchProjectTableId == null)
            {
                this.logger.LogError("Research project Id is null or invalid.");
                this.RecordEvent("GetResearchProjectByIdAsync", RequestType.Failed);
                return this.BadRequest("Invalid research project Id.");
            }

            try
            {
                var researchProjectDetails = await this.researchProjectHelper.GetResearchProjectByIdAsync(researchProjectTableId, this.UserAadId);

                if (researchProjectDetails == null)
                {
                    this.RecordEvent("GetResearchProjectByIdAsync", RequestType.Failed);
                    return this.NotFound("Research project not found.");
                }

                this.RecordEvent("GetResearchProjectByIdAsync", RequestType.Succeeded);
                return this.Ok(researchProjectDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetResearchProjectByIdAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching research project.");
                throw;
            }
        }

        /// <summary>
        /// Stores rating of user for a research project.
        /// </summary>
        /// <param name="researchProjectTableId">The research project table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>Returns HTTP status code OK on successful operation.</returns>
        [HttpPost("rate/{researchProjectTableId}/{rating}")]
        public async Task<IActionResult> RateResearchProjectAsync(string researchProjectTableId, int rating)
        {
            this.RecordEvent("RateResearchProjectAsync", RequestType.Initiated);

            if (string.IsNullOrEmpty(researchProjectTableId))
            {
                this.RecordEvent("RateResearchProjectAsync", RequestType.Failed);
                this.logger.LogError("Empty research project table Id value was provided.");
                return this.BadRequest("The valid research project table Id must be provided.");
            }

            try
            {
                await this.researchProjectHelper.RateResearchProjectAsync(researchProjectTableId, rating, this.UserAadId);

                this.RecordEvent("RateResearchProjectAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("RateResearchProjectAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while ratings research project.");
                throw;
            }
        }
    }
}