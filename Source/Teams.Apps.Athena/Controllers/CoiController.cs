// <copyright file="CoiController.cs" company="NPS Foundation">
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
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes API endpoints related to COI.
    /// </summary>
    [Route("api/cois")]
    [ApiController]
    [Authorize]
    public class CoiController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The instance of <see cref="CoiHelper"/> class.
        /// </summary>
        private readonly ICoiHelper coiHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoiController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="coiHelper">The instance of <see cref="CoiHelper"/> class.</param>
        public CoiController(
            ILogger<CoiController> logger,
            TelemetryClient telemetryClient,
            ICoiHelper coiHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.coiHelper = coiHelper;
        }

        /// <summary>
        /// Gets a COI by table Id.
        /// </summary>
        /// <param name="coiTableId">Unique COI table Id.</param>
        /// <returns>Returns COI details.</returns>
        [HttpGet("{coiTableId}")]
        public async Task<IActionResult> GetCoiByTableIdAsync(Guid coiTableId)
        {
            this.RecordEvent("GetCoiByIdAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "coiTableId", coiTableId.ToString() },
            });

            if (coiTableId == Guid.Empty)
            {
                this.logger.LogError("COI Id is null or invalid.");
                this.RecordEvent("GetCoiByIdAsync", RequestType.Failed);
                return this.BadRequest("Invalid COI Id.");
            }

            try
            {
                var coiDetails = await this.coiHelper.GetCoiByTableIdAsync(coiTableId.ToString(), this.UserAadId);

                if (coiDetails == null)
                {
                    this.RecordEvent("GetCoiByIdAsync", RequestType.Failed);
                    return this.NotFound("COI not found.");
                }

                this.RecordEvent("GetCoiByIdAsync", RequestType.Succeeded);
                return this.Ok(coiDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetCoiByIdAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching COI.");
                throw;
            }
        }

        /// <summary>
        /// Stores rating of user for a COI.
        /// </summary>
        /// <param name="coiTableId">The COI table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>Returns HTTP status code OK on successful operation.</returns>
        [HttpPost("rate/{coiTableId}/{rating}")]
        public async Task<IActionResult> RateCoiAsync(Guid coiTableId, int rating)
        {
            this.RecordEvent("RateCoiAsync", RequestType.Initiated);

            if (coiTableId == Guid.Empty)
            {
                this.RecordEvent("RateCoiAsync", RequestType.Failed);
                this.logger.LogError("Empty COI table Id value was provided.");
                return this.BadRequest("The valid COI table Id must be provided.");
            }

            try
            {
                await this.coiHelper.RateCoiAsync(coiTableId.ToString(), rating, this.UserAadId);

                this.RecordEvent("RateCoiAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("RateCoiAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while rating coi.");
                throw;
            }
        }

        /// <summary>
        /// Gets the total number of approved COIs created in Athena.
        /// </summary>
        /// <returns>The total number of COIs created in Athena.</returns>
        [HttpGet("approved/count")]
        public async Task<IActionResult> GetTotalAthenaCoisCountAsync()
        {
            this.RecordEvent("GetTotalAthenaCoisCountAsync", RequestType.Initiated);

            try
            {
                var approvedCoisInAthena = await this.coiHelper.GetApprovedCoiRequestsCreatedInAthenaAppAsync();

                this.RecordEvent("GetTotalAthenaCoisCountAsync", RequestType.Succeeded);

                return this.Ok(approvedCoisInAthena.Count());
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetTotalAthenaCoisCountAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting total COIs count.");
                throw;
            }
        }
    }
}
