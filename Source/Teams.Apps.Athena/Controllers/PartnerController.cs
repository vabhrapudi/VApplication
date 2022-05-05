// <copyright file="PartnerController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes API endpoints related to partner.
    /// </summary>
    [Route("api/partners")]
    [ApiController]
    [Authorize]
    public class PartnerController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for partner.
        /// </summary>
        private readonly IPartnerHelper partnerHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartnerController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="partnerHelper">The instance of <see cref="PartnerHelper"/> class.</param>
        public PartnerController(
            ILogger<PartnerController> logger,
            TelemetryClient telemetryClient,
            IPartnerHelper partnerHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.partnerHelper = partnerHelper;
        }

        /// <summary>
        /// Gets a partner by table Id.
        /// </summary>
        /// <param name="partnerTableId">Unique partner Table Id.</param>
        /// <returns>Returns partner details.</returns>
        [HttpGet("{partnerTableId}")]
        public async Task<IActionResult> GetPartnerByTableIdAsync(Guid partnerTableId)
        {
            this.RecordEvent("GetPartnerByIdAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "partnerTableId", partnerTableId.ToString() },
            });

            if (partnerTableId == Guid.Empty)
            {
                this.logger.LogError("Partner Id is null or invalid.");
                this.RecordEvent("GetPartnerByIdAsync", RequestType.Failed);
                return this.BadRequest("Invalid partner Id.");
            }

            try
            {
                var partnerDetails = await this.partnerHelper.GetPartnerByTableIdAsync(partnerTableId.ToString(), this.UserAadId);

                if (partnerDetails == null)
                {
                    this.RecordEvent("GetPartnerByIdAsync", RequestType.Failed);
                    return this.NotFound("Partner not found.");
                }

                this.RecordEvent("GetPartnerByIdAsync", RequestType.Succeeded);
                return this.Ok(partnerDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetPartnerByIdAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching partner.");
                throw;
            }
        }

        /// <summary>
        /// Stores rating of user for a partner.
        /// </summary>
        /// <param name="partnerTableId">The partner table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>Returns HTTP status code OK on successful operation.</returns>
        [HttpPost("rate/{partnerTableId}/{rating}")]
        public async Task<IActionResult> RatePartnerAsync(Guid partnerTableId, int rating)
        {
            this.RecordEvent("RatePartnerAsync", RequestType.Initiated);

            if (partnerTableId == Guid.Empty)
            {
                this.RecordEvent("RatePartnerAsync", RequestType.Failed);
                this.logger.LogError("Empty partner table Id value was provided.");
                return this.BadRequest("The valid partner table Id must be provided.");
            }

            try
            {
                await this.partnerHelper.RatePartnerAsync(partnerTableId.ToString(), rating, this.UserAadId);

                this.RecordEvent("RatePartnerAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("RatePartnerAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while rating partner.");
                throw;
            }
        }
    }
}