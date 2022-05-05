// <copyright file="StudyDepartmentController.cs" company="NPS Foundation">
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
    /// Study department controller is responsible to expose API endpoints for performing search operation on study department entity.
    /// </summary>
    [Route("api/studyDepartment")]
    [ApiController]
    [Authorize]
    public class StudyDepartmentController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for study department.
        /// </summary>
        private readonly IStudyDepartmentHelper studyDepartmentHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudyDepartmentController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="studyDepartmentHelper">The instance of study department helper which helps in managing operations on study department entity.</param>
        public StudyDepartmentController(
            ILogger<StudyDepartmentController> logger,
            TelemetryClient telemetryClient,
            IStudyDepartmentHelper studyDepartmentHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.studyDepartmentHelper = studyDepartmentHelper;
        }

        /// <summary>
        /// Gets the study departments.
        /// </summary>
        /// <returns>Returns study departmens.</returns>
        [HttpGet]
        public async Task<IActionResult> GetStudyDepartmentsAsync()
        {
            this.RecordEvent("GetStudyDepartmentsAsync", RequestType.Initiated);

            try
            {
                var studyDepartments = await this.studyDepartmentHelper.GetStudyDepartmentsAsync();

                if (studyDepartments == null)
                {
                    this.RecordEvent("GetStudyDepartmentsAsync", RequestType.Failed);
                    return this.NotFound("Study department not found.");
                }

                this.RecordEvent("GetStudyDepartmentsAsync", RequestType.Succeeded);
                return this.Ok(studyDepartments);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetStudyDepartmentsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching study departments.");
                throw;
            }
        }
    }
}