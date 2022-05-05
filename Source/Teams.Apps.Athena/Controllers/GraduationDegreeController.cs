// <copyright file="GraduationDegreeController.cs" company="NPS Foundation">
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
    /// Graduation degree controller is responsible to expose API endpoints for performing search operation on graduation degree entity.
    /// </summary>
    [Route("api/graduationDegree")]
    [ApiController]
    [Authorize]
    public class GraduationDegreeController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for graduation degree.
        /// </summary>
        private readonly IGraduationDegreeHelper graduationDegreeHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraduationDegreeController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="graduationDegreeHelper">The instance of graduation degree helper which helps in managing operations on graduation degree entity.</param>
        public GraduationDegreeController(
            ILogger<GraduationDegreeController> logger,
            TelemetryClient telemetryClient,
            IGraduationDegreeHelper graduationDegreeHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.graduationDegreeHelper = graduationDegreeHelper;
        }

        /// <summary>
        /// Gets the graduation degrees.
        /// </summary>
        /// <returns>Returns graduation degrees.</returns>
        [HttpGet]
        public async Task<IActionResult> GetGraduationDegreesAsync()
        {
            this.RecordEvent("GetGraduationDegreesAsync", RequestType.Initiated);

            try
            {
                var graduationDegrees = await this.graduationDegreeHelper.GetGraduationDegreesAsync();

                if (graduationDegrees == null)
                {
                    this.RecordEvent("GetGraduationDegreesAsync", RequestType.Failed);
                    return this.NotFound("Graduation degrees not found.");
                }

                this.RecordEvent("GetGraduationDegreesAsync", RequestType.Succeeded);
                return this.Ok(graduationDegrees);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetGraduationDegreesAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching graduation degrees.");
                throw;
            }
        }
    }
}