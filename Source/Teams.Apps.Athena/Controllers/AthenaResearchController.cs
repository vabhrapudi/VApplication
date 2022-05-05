// <copyright file="AthenaResearchController.cs" company="NPS Foundation">
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
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes API endpoints related to athena research.
    /// </summary>
    [Route("api/athena-research")]
    [ApiController]
    [Authorize]
    public class AthenaResearchController : BaseController
    {
        /// <summary>
        /// The instance of <see cref="ILogger"/>.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The instance of <see cref="AthenaResearchImportanceBlobRepository"/> class.
        /// </summary>
        private readonly IAthenaResearchImportanceBlobRepository athenaResearchImportanceBlobRepository;

        /// <summary>
        /// The instance of <see cref="AthenaResearchPriorityBlobRepository"/> class.
        /// </summary>
        private readonly IAthenaResearchPriorityBlobRepository athenaResearchPriorityBlobRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaResearchController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="athenaResearchImportanceBlobRepository">The instance of <see cref="AthenaResearchImportanceBlobRepository"/> class.</param>
        /// <param name="athenaResearchPriorityBlobRepository">The instance of <see cref="AthenaResearchPriorityBlobRepository"/> class.</param>
        public AthenaResearchController(
            ILogger<AthenaResearchController> logger,
            TelemetryClient telemetryClient,
            IAthenaResearchImportanceBlobRepository athenaResearchImportanceBlobRepository,
            IAthenaResearchPriorityBlobRepository athenaResearchPriorityBlobRepository)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.athenaResearchImportanceBlobRepository = athenaResearchImportanceBlobRepository;
            this.athenaResearchPriorityBlobRepository = athenaResearchPriorityBlobRepository;
        }

        /// <summary>
        /// Gets the athena research importance.
        /// </summary>
        /// <returns>The athena research importance.</returns>
        [HttpGet("importance")]
        public async Task<IActionResult> GetAthenaResearchImportanceAsync()
        {
            this.RecordEvent("GetAthenaResearchImportanceAsync", RequestType.Initiated);

            try
            {
                var response = await this.athenaResearchImportanceBlobRepository.GetBlobJsonFileContentAsync(AthenaResearchImportanceBlobMetadata.FileName);

                if (response == null)
                {
                    this.RecordEvent("GetAthenaResearchImportanceAsync", RequestType.Failed);
                    return this.NotFound("Athena research importance not found.");
                }

                this.RecordEvent("GetAthenaResearchImportanceAsync", RequestType.Succeeded);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetAthenaResearchImportanceAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting athena research importance.");
                throw;
            }
        }

        /// <summary>
        /// Gets the athena research priorities.
        /// </summary>
        /// <returns>The athena research priorities.</returns>
        [HttpGet("priorities")]
        public async Task<IActionResult> GetAthenaResearchPrioritiesAsync()
        {
            this.RecordEvent("GetAthenaResearchPrioritiesAsync", RequestType.Initiated);

            try
            {
                var response = await this.athenaResearchPriorityBlobRepository.GetBlobJsonFileContentAsync(AthenaResearchPriorityBlobMetadata.FileName);

                if (response == null)
                {
                    this.RecordEvent("GetAthenaResearchPrioritiesAsync", RequestType.Failed);
                    return this.NotFound("Athena research priorities not found.");
                }

                this.RecordEvent("GetAthenaResearchPrioritiesAsync", RequestType.Succeeded);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetAthenaResearchPrioritiesAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting athena research priorities.");
                throw;
            }
        }
    }
}
