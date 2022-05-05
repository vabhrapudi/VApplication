// <copyright file="UserController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes endpoints for Microsoft graph APIs related to users.
    /// </summary>
    [Route("api/graph/users")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger<UserController> logger;

        /// <summary>
        /// The instance of user graph service helper.
        /// </summary>
        private readonly IUserGraphServiceHelper userGraphServiceHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="userGraphServiceHelper">The instance of <see cref="UserGraphServiceHelper"/>.</param>
        public UserController(
            ILogger<UserController> logger,
            TelemetryClient telemetryClient,
            IUserGraphServiceHelper userGraphServiceHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.userGraphServiceHelper = userGraphServiceHelper;
        }

        /// <summary>
        /// Gets the logged-in user details.
        /// </summary>
        /// <returns>The logged-in user details.</returns>
        [HttpGet("me")]
        public async Task<IActionResult> GetLoggedInUserDetailsAsync()
        {
            this.RecordEvent("GetLoggedInUserDetailsAsync", RequestType.Initiated);

            if (this.UserAadId.IsEmptyOrInvalidGuid())
            {
                this.RecordEvent("GetLoggedInUserDetailsAsync", RequestType.Failed);
                this.logger.LogError("Invalid logged-in user Id was encountered.");

                return this.BadRequest("The user AAD Id is invalid.");
            }

            try
            {
                var users = await this.userGraphServiceHelper.GetUsersAsync(new[] { this.UserAadId });

                this.RecordEvent("GetLoggedInUserDetailsAsync", RequestType.Succeeded);

                return this.Ok(users.FirstOrDefault());
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetLoggedInUserDetailsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting logged-in user details.");

                throw;
            }
        }

        /// <summary>
        /// Gets the logged-in user details.
        /// </summary>
        /// <param name="userIds">List of user Ids.</param>
        /// <returns>The logged-in user details.</returns>
        [HttpPost("details")]
        public async Task<IActionResult> GetUserDetailsAsync([FromBody] List<string> userIds)
        {
            this.RecordEvent("GetUserDetailsAsync", RequestType.Initiated);

            if (userIds.IsNullOrEmpty())
            {
                this.RecordEvent("GetUserDetailsAsync", RequestType.Failed);
                this.logger.LogError("Invalid list of user Ids.");

                return this.BadRequest("The list of user Ids is invalid.");
            }

            try
            {
                var users = await this.userGraphServiceHelper.GetUsersAsync(userIds);

                this.RecordEvent("GetLoggedInUserDetailsAsync", RequestType.Succeeded);

                return this.Ok(users);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetLoggedInUserDetailsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting logged-in user details.");

                throw;
            }
        }
    }
}