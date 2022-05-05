// <copyright file="SecurityLevelController.cs" company="NPS Foundation">
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
    ///  Keywords controller is responsible to expose API endpoints for performing search operation on security level entity.
    /// </summary>
    [Route("api/security-levels")]
    [ApiController]
    [Authorize]
    public class SecurityLevelController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for security level.
        /// </summary>
        private readonly ISecurityLevelHelper securityLevelHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityLevelController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="securityLevelHelper">The instance of <see cref="SecurityLevelHelper"/>.</param>
        public SecurityLevelController(
        ILogger<SecurityLevelController> logger,
        TelemetryClient telemetryClient,
        ISecurityLevelHelper securityLevelHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.securityLevelHelper = securityLevelHelper;
        }

        /// <summary>
        /// Gets the security levels.
        /// </summary>
        /// <returns>Returns ok status if collections of security levels are successfully fetched./returns>
        [HttpGet]
        public async Task<IActionResult> GetSecurityLevelsAsync()
        {
            this.RecordEvent("GetSecurityLevelsAsync", RequestType.Initiated);

            try
            {
                var getSecuritylevels = await this.securityLevelHelper.GetSecurityLevelsAsync();

                if (getSecuritylevels == null)
                {
                    this.RecordEvent("GetSecurityLevelsAsync", RequestType.Failed);
                    return this.NotFound("security levels not found.");
                }

                this.RecordEvent("GetSecurityLevelsAsync", RequestType.Succeeded);
                return this.Ok(getSecuritylevels);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetSecurityLevelsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching security levels.");
                throw;
            }
        }
    }
}