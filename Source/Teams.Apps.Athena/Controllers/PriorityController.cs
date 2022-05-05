// <copyright file="PriorityController.cs" company="NPS Foundation">
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
    using Teams.Apps.Athena.Authorization;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Constants;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Priority controller is responsible to expose API endpoints related priority.
    /// </summary>
    [Route("api/priorities")]
    [ApiController]
    public class PriorityController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Holds the instance of <see cref="PriorityHelper"/> class.
        /// </summary>
        private readonly IPriorityHelper priorityHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="priorityHelper">The instance of <see cref="PriorityHelper"/> class.</param>
        public PriorityController(
            ILogger<PriorityController> logger,
            TelemetryClient telemetryClient,
            IPriorityHelper priorityHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.priorityHelper = priorityHelper;
        }

        /// <summary>
        /// Gets the priority by its Id.
        /// </summary>
        /// <param name="priorityId">The priority Id.</param>
        /// <returns>The priority details</returns>
        [HttpGet("{priorityId}")]
        [Authorize(AuthorizationPolicyNames.MustBeAdminPolicy)]
        public async Task<IActionResult> GetPriorityById(Guid priorityId)
        {
            this.RecordEvent("GetPriorityById", RequestType.Initiated);

            try
            {
                var priorityDetails = await this.priorityHelper.GetPriorityByIdAsync(priorityId.ToString());

                if (priorityDetails == null)
                {
                    this.RecordEvent("GetPriorityById", RequestType.Failed);
                    return this.NotFound();
                }

                this.RecordEvent("GetPriorityById", RequestType.Succeeded);

                return this.Ok(priorityDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetPriorityById", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting priority details.");
                throw;
            }
        }

        /// <summary>
        /// Gets all the priorities.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        public async Task<IActionResult> GetPrioritiesAsync()
        {
            this.RecordEvent("GetPrioritiesAsync", RequestType.Initiated);

            try
            {
                var priorityDetails = await this.priorityHelper.GetPrioritiesAsync();

                this.RecordEvent("GetPrioritiesAsync", RequestType.Succeeded);

                return this.Ok(priorityDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetPrioritiesAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting priorities.");
                throw;
            }
        }

        /// <summary>
        /// Creates a new priority.
        /// </summary>
        /// <param name="priorityDTO">The details of priority to be created.</param>
        /// <returns>Returns priority details that was created.</returns>
        [HttpPost]
        [Authorize(AuthorizationPolicyNames.MustBeAdminPolicy)]
        public async Task<IActionResult> CreatePriorityAsync([FromBody] PriorityDTO priorityDTO)
        {
            this.RecordEvent("CreatePriorityAsync", RequestType.Initiated);

            try
            {
                var createdriorityDetails = await this.priorityHelper.CreatePriorityAsync(priorityDTO, this.UserAadId);

                if (createdriorityDetails == null)
                {
                    this.RecordEvent("CreatePriorityAsync", RequestType.Failed);
                    return this.BadRequest($"Maximum {Constants.MaxNumberOfPriorities} priorities are allowed to created under a priority type.");
                }

                this.RecordEvent("CreatePriorityAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created, createdriorityDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("CreatePriorityAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while creating priority.");
                throw;
            }
        }

        /// <summary>
        /// Updates the priority.
        /// </summary>
        /// <param name="priorityDTO">The details of priority to be updated.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPatch]
        [Authorize(AuthorizationPolicyNames.MustBeAdminPolicy)]
        public async Task<IActionResult> UpdatePriorityAsync([FromBody] PriorityDTO priorityDTO)
        {
            this.RecordEvent("UpdatePriorityAsync", RequestType.Initiated);

            try
            {
                var updatedPriorityDetails = await this.priorityHelper.UpdatePriorityAsync(priorityDTO, this.UserAadId);

                if (updatedPriorityDetails == null)
                {
                    this.RecordEvent("UpdatePriorityAsync", RequestType.Failed);
                    return this.NotFound($"The priority was not found or Maximum {Constants.MaxNumberOfPriorities} priorities are allowed to create under a priority type.");
                }

                this.RecordEvent("UpdatePriorityAsync", RequestType.Succeeded);
                return this.Ok(updatedPriorityDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("UpdatePriorityAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while updating priority.");
                throw;
            }
        }

        /// <summary>
        /// Gets the priority types.
        /// </summary>
        /// <returns>The collections of priority types title and Id.</returns>
        [HttpGet("types")]
        public async Task<IActionResult> GetPriorityTypeData()
        {
            this.RecordEvent("GetPriorityTypeData", RequestType.Initiated);

            try
            {
                var priorityTypeData = await this.priorityHelper.GetPriorityTypesAsync();

                if (priorityTypeData == null)
                {
                    this.RecordEvent("GetPriorityTypeData", RequestType.Failed);
                    return this.NotFound();
                }

                this.RecordEvent("GetPriorityTypeData", RequestType.Succeeded);

                return this.Ok(priorityTypeData);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetPriorityTypeData", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting priority type data.");
                throw;
            }
        }

        /// <summary>
        /// Deletes the priorities.
        /// </summary>
        /// <param name="priorityIds">The collection of priority Id's to be deleted.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete]
        [Authorize(AuthorizationPolicyNames.MustBeAdminPolicy)]
        public async Task<IActionResult> DeletePrioritiesAsync([FromBody] IEnumerable<Guid> priorityIds)
        {
            this.RecordEvent("DeletePrioritiesAsync", RequestType.Initiated);

            if (priorityIds.IsNullOrEmpty())
            {
                this.RecordEvent("DeletePrioritiesAsync", RequestType.Failed);
                return this.BadRequest("The priority Ids are required.");
            }

            try
            {
                await this.priorityHelper.DeletePrioritiesAsync(priorityIds);

                this.RecordEvent("DeletePrioritiesAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("DeletePrioritiesAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while deleting priorities.");
                throw;
            }
        }

        /// <summary>
        /// Gets the priorities insights.
        /// </summary>
        /// <param name="prioritiesInsightsDto">The filter data.</param>
        /// <returns>The priorities insights data.</returns>
        [HttpPost("insights")]
        public async Task<IActionResult> GetPrioritiesInsightsAsync([FromBody] PrioritiesInsightsDto prioritiesInsightsDto)
        {
            this.RecordEvent("GetPrioritiesInsightsAsync", RequestType.Initiated);

            if (prioritiesInsightsDto == null)
            {
                this.RecordEvent("GetPrioritiesInsightsAsync", RequestType.Failed);
                return this.BadRequest("The priority details are required.");
            }

            try
            {
                var insightsData = await this.priorityHelper.GetPrioritiesInsightsAsync(prioritiesInsightsDto.PriorityIds, prioritiesInsightsDto.KeywordIdsFilter);

                this.RecordEvent("GetPrioritiesInsightsAsync", RequestType.Succeeded);

                return this.Ok(insightsData);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetPrioritiesInsightsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting priorities insights.");
                throw;
            }
        }
    }
}
