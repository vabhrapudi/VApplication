// <copyright file="SpecialtyController.cs" company="NPS Foundation">
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
    /// Specialty controller is responsible to expose API endpoints for performing search operation on specialty entity.
    /// </summary>
    [Route("api/specialty")]
    [ApiController]
    [Authorize]
    public class SpecialtyController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for specialty.
        /// </summary>
        private readonly ISpecialtyHelper specialtyHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialtyController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="specialtyHelper">The instance of specialty helper which helps in managing operations on specialty entity.</param>
        public SpecialtyController(
            ILogger<SpecialtyController> logger,
            TelemetryClient telemetryClient,
            ISpecialtyHelper specialtyHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.specialtyHelper = specialtyHelper;
        }

        /// <summary>
        /// Gets the specialty details.
        /// </summary>
        /// <returns>Returns specialty details.</returns>
        [HttpGet]
        public async Task<IActionResult> GetSpecialtyDetailsAsync()
        {
            this.RecordEvent("GetSpecialtyDetailsAsync", RequestType.Initiated);

            try
            {
                var specialtyDetails = await this.specialtyHelper.GetSpecialtyDetailsAsync();

                if (specialtyDetails == null)
                {
                    this.RecordEvent("GetSpecialtyDetailsAsync", RequestType.Failed);
                    return this.NotFound("specialty not found.");
                }

                this.RecordEvent("GetSpecialtyDetailsAsync", RequestType.Succeeded);
                return this.Ok(specialtyDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetSpecialtyDetailsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching specialty details.");
                throw;
            }
        }
    }
}