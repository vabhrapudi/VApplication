// <copyright file="OrganizationController.cs" company="NPS Foundation">
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
    /// Organization controller is responsible to expose API endpoints for performing search operation on organization entity.
    /// </summary>
    [Route("api/organization")]
    [ApiController]
    [Authorize]
    public class OrganizationController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for organization.
        /// </summary>
        private readonly IOrganizationHelper organizationHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrganizationController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="organizationHelper">The instance of organization helper which helps in managing operations on organization entity.</param>
        public OrganizationController(
            ILogger<OrganizationController> logger,
            TelemetryClient telemetryClient,
            IOrganizationHelper organizationHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.organizationHelper = organizationHelper;
        }

        /// <summary>
        /// Gets the organizations.
        /// </summary>
        /// <returns>Returns organizations.</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrganizationsAsync()
        {
            this.RecordEvent("GetOrganizationsAsync", RequestType.Initiated);

            try
            {
                var organizations = await this.organizationHelper.GetOrganizationsAsync();

                if (organizations == null)
                {
                    this.RecordEvent("GetOrganizationsAsync", RequestType.Failed);
                    return this.NotFound("Organization not found.");
                }

                this.RecordEvent("GetOrganizationsAsync", RequestType.Succeeded);
                return this.Ok(organizations);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetOrganizationsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching organizations.");
                throw;
            }
        }
    }
}