// <copyright file="UserSettingController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// User setting controller is responsible to expose API endpoints for performing CRUD operation on user entity.
    /// </summary>
    [Route("api/usersetting")]
    [ApiController]
    [Authorize]
    public class UserSettingController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The instance of <see cref="UserSettingsHelper"/> class.
        /// </summary>
        private readonly IUserSettingsHelper userSettingsHelper;

        /// <summary>
        /// The instance of <see cref="UserPersistentDataHelper"/> class.
        /// </summary>
        private readonly IUserPersistentDataHelper userPersistentDataHelper;

        /// <summary>
        /// Provides the helper methods for job title.
        /// </summary>
        private readonly IJobTitleHelper jobTitleHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettingController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="userSettingsHelper">The instance of user settings helper which helps in managing operations on user entity.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="userPersistentDataHelper">The instance of <see cref="UserPersistentDataHelper"/> class.</param>
        /// <param name="jobTitleHelper">The instance of job title helper which helps in managing operations on job titles.</param>
        public UserSettingController(
            ILogger<UserSettingController> logger,
            IUserSettingsHelper userSettingsHelper,
            TelemetryClient telemetryClient,
            IUserPersistentDataHelper userPersistentDataHelper,
            IJobTitleHelper jobTitleHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.userSettingsHelper = userSettingsHelper;
            this.userPersistentDataHelper = userPersistentDataHelper;
            this.jobTitleHelper = jobTitleHelper;
        }

        /// <summary>
        /// Get user settings.
        /// </summary>
        /// <returns>Returns user details.</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserSettingAsync()
        {
            var userAadId = this.UserAadId;
            this.RecordEvent("Get user- The HTTP GET call to get user details has been initiated.", RequestType.Initiated, new Dictionary<string, string>
            {
                { "User", userAadId },
            });

            if (string.IsNullOrEmpty(userAadId))
            {
                this.RecordEvent("Get user- The HTTP GET call to get user details has failed.", RequestType.Failed);
                return this.BadRequest("Invalid user Id.");
            }

            try
            {
                var userDetails = await this.userSettingsHelper.GetUserByIdAsync(userAadId);

                if (userDetails == null)
                {
                    this.RecordEvent("Get uer- The HTTP GET call to get user details has failed.", RequestType.Failed);
                    return this.NotFound("User not found.");
                }

                this.RecordEvent("Get user- The HTTP GET call to get user details has succeeded.", RequestType.Succeeded);
                return this.Ok(userDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("Get user- The HTTP GET call to get user details has failed.", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching user details.");
                throw;
            }
        }

        /// <summary>
        /// Gets a user details by email address.
        /// </summary>
        /// <param name="emailAddress">The email address of user.</param>
        /// <returns>Returns user details.</returns>
        [HttpGet("{emailAddress}")]
        public async Task<IActionResult> GetUserDetailsByEmailAddressAsync(string emailAddress)
        {
            this.RecordEvent("Get user- The HTTP GET call to get user details has been initiated.", RequestType.Initiated, new Dictionary<string, string>
            {
                { "EmailAddress", emailAddress },
            });

            if (string.IsNullOrEmpty(emailAddress))
            {
                this.RecordEvent("Get user- The HTTP GET call to get user details has failed.", RequestType.Failed);
                return this.BadRequest("Invalid email address.");
            }

            try
            {
                var userDetails = await this.userSettingsHelper.GetUserDetailsByEmailAdressAsync(emailAddress);

                if (userDetails == null)
                {
                    this.RecordEvent("Get uer- The HTTP GET call to get user details has failed.", RequestType.Failed);
                    return this.NotFound("User not found.");
                }

                this.RecordEvent("Get user- The HTTP GET call to get user details has succeeded.", RequestType.Succeeded);
                return this.Ok(userDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("Get user- The HTTP GET call to get user details has failed.", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching user details.");
                throw;
            }
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="userDetails">The details of user to be created.</param>
        /// <returns>Returns user details that was created.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(UserSettingsCreateDTO userDetails)
        {
            this.RecordEvent("Create user- The HTTP POST call has initiated.", RequestType.Initiated);
            if (userDetails == null)
            {
                this.logger.LogError("User detail is null.");
                this.RecordEvent("Create User- The HTTP POST call has failed.", RequestType.Failed);
                return this.BadRequest();
            }

            try
            {
                var createResult = await this.userSettingsHelper.CreateUserAsync(userDetails, this.UserAadId);

                if (createResult == null)
                {
                    this.RecordEvent("Create user- The HTTP POST call has failed.", RequestType.Failed);
                    return this.StatusCode((int)HttpStatusCode.InternalServerError);
                }

                this.RecordEvent("Create user- The HTTP POST call has succeeded.", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created, createResult);
            }
            catch (Exception ex)
            {
                this.RecordEvent("Create user- The HTTP POST call has failed.", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while creating user.");
                throw;
            }
        }

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <param name="userDetails">The details of user to be updated.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPatch]
        public async Task<IActionResult> UpdateUserAsync(UserSettingsUpdateDTO userDetails)
        {
            this.RecordEvent("Update user- The HTTP PATCH call has initiated.", RequestType.Initiated, new Dictionary<string, string>
            {
#pragma warning disable CA1062 // Validated arguments at model level.
                { "TableId", userDetails.TableId.ToString() },
#pragma warning restore CA1062 // Validated arguments at model level.
            });

            if (userDetails.TableId.IsEmptyOrInvalidGuid())
            {
                this.RecordEvent("Update user- The HTTP PATCH call has failed.", RequestType.Failed);
                return this.BadRequest("The user table Id is invalid.");
            }

            try
            {
                var userExistingData = await this.userSettingsHelper.GetUserItemByIdAsync(userDetails.TableId);
                if (string.IsNullOrEmpty(userExistingData.UserId))
                {
                    userExistingData.UserId = this.UserAadId;
                }

                var updateResult = await this.userSettingsHelper.UpdateUserAsync(userDetails, userExistingData);

                if (updateResult == null)
                {
                    this.RecordEvent("Update user- The HTTP PATCH call has failed.", RequestType.Failed);
                    return this.StatusCode((int)HttpStatusCode.InternalServerError, new ErrorResponse { Message = "Error occurred while updating user." });
                }

                this.RecordEvent("Update user- The HTTP PATCH call has succeeded.", RequestType.Succeeded);
                return this.Ok(updateResult);
            }
            catch (Exception ex)
            {
                this.RecordEvent("Update user- The HTTP PATCH call has failed.", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while updating user.");
                throw;
            }
        }

        /// <summary>
        /// Saves the discovery tree persistent data.
        /// </summary>
        /// <param name="discoveryTreePersistentData">The data to be saved.</param>
        /// <returns>Returns user persistent data.</returns>
        [HttpPost("discovery-tree/persistent-data/save")]
        public async Task<IActionResult> SaveDiscoveryTreePersistentDataAsync([FromBody] DiscoveryTreePersistentData discoveryTreePersistentData)
        {
            this.RecordEvent("SaveDiscoveryTreePersistentDataAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "UserAadId", this.UserAadId },
            });

            if (discoveryTreePersistentData == null)
            {
                this.logger.LogError("User persistent data is null.");
                this.RecordEvent("SaveDiscoveryTreePersistentDataAsync", RequestType.Failed);
                return this.BadRequest(discoveryTreePersistentData);
            }

            try
            {
                var userPersistentData = await this.userPersistentDataHelper.SaveDiscoveryTreeUserPersistentDataAsync(discoveryTreePersistentData, this.UserAadId);
                if (userPersistentData == null)
                {
                    this.RecordEvent("SaveDiscoveryTreePersistentDataAsync", RequestType.Failed);
                    return this.StatusCode((int)HttpStatusCode.InternalServerError);
                }

                this.RecordEvent("SaveDiscoveryTreePersistentDataAsync", RequestType.Succeeded);
                return this.Ok(userPersistentData);
            }
            catch (Exception ex)
            {
                this.RecordEvent("SaveDiscoveryTreePersistentDataAsync.", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while saving discovery tree persistent data.");
                throw;
            }
        }

        /// <summary>
        /// Gets user persistent data.
        /// </summary>
        /// <returns>Returns user persistent data.</returns>
        [HttpGet("persistent-data")]
        public async Task<IActionResult> GetUserPersistentDataAsync()
        {
            this.RecordEvent("GetUserPersistentDataAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "UserAadId", this.UserAadId },
            });

            try
            {
                var userPersistentData = await this.userPersistentDataHelper.GetUserPersistentDataAsync(this.UserAadId);
                return this.Ok(userPersistentData);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetUserPersistentDataAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting user persistent data.");
                throw;
            }
        }

        /// <summary>
        /// Gets user persistent data.
        /// </summary>
        /// <returns>Returns user persistent data.</returns>
        [HttpGet("active-users/count")]
        public async Task<IActionResult> GetActiveAthenaUsersCountAsync()
        {
            this.RecordEvent("GetActiveAthenaUsersCountAsync", RequestType.Initiated);

            try
            {
                var athenaUsers = await this.userSettingsHelper.GetAthenaUsersAsync();

                this.RecordEvent("GetActiveAthenaUsersCountAsync", RequestType.Succeeded);

                return this.Ok(athenaUsers.Count());
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetActiveAthenaUsersCountAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting active users count.");
                throw;
            }
        }

        /// <summary>
        /// Gets the job titles.
        /// </summary>
        /// <returns>Returns job titles.</returns>
        [HttpGet("job-types")]
        public async Task<IActionResult> GetJobTitlesAsync()
        {
            this.RecordEvent("GetJobTitlesAsync", RequestType.Initiated);

            try
            {
                var jobTitles = await this.jobTitleHelper.GetJobTitlesAsync();

                if (jobTitles == null)
                {
                    this.RecordEvent("GetJobTitlesAsync", RequestType.Failed);
                    return this.NotFound("Job titles not found.");
                }

                this.RecordEvent("GetJobTitlesAsync", RequestType.Succeeded);
                return this.Ok(jobTitles);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetJobTitlesAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching job titles.");
                throw;
            }
        }
    }
}