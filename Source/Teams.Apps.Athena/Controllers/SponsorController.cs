// <copyright file="SponsorController.cs" company="NPS Foundation">
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
    /// Exposes API endpoints related to athena sponsors.
    /// </summary>
    [Route("api/sponsors")]
    [ApiController]
    [Authorize]
    public class SponsorController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for sponsor.
        /// </summary>
        private readonly ISponsorHelper sponsorHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SponsorController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="sponsorHelper">The instance of <see cref="SponsorHelper"/> class.</param>
        public SponsorController(
            ILogger<SponsorController> logger,
            TelemetryClient telemetryClient,
            ISponsorHelper sponsorHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.sponsorHelper = sponsorHelper;
        }

        /// <summary>
        /// Gets the sponsors by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The collection of sponsors.</returns>
        [HttpPost]
        public async Task<IActionResult> GetSponsorsByKeywordIds(IEnumerable<int> keywordIds)
        {
            this.RecordEvent("GetInterestedSponsorsAsync", RequestType.Initiated);

            try
            {
                var sponsors = await this.sponsorHelper.GetSponsorsByKeywordsAsync(keywordIds);

                this.RecordEvent("GetInterestedSponsorsAsync", RequestType.Succeeded);

                return this.Ok(sponsors);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetInterestedSponsorsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting interested sponsors.");
                throw;
            }
        }

        /// <summary>
        /// Gets a sponsor by table Id.
        /// </summary>
        /// <param name="sponsorTableId">Unique sponsor Table Id.</param>
        /// <returns>Returns sponsor details.</returns>
        [HttpGet("{sponsorTableId}")]
        public async Task<IActionResult> GetSponsorByTableIdAsync(Guid sponsorTableId)
        {
            this.RecordEvent("GetSponsorByIdAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "sponsorTableId", sponsorTableId.ToString() },
            });

            if (sponsorTableId == Guid.Empty)
            {
                this.logger.LogError("Sponsor Id is null or invalid.");
                this.RecordEvent("GetSponsorByIdAsync", RequestType.Failed);
                return this.BadRequest("Invalid sponsor Id.");
            }

            try
            {
                var sponsorDetails = await this.sponsorHelper.GetSponsorByTableIdAsync(sponsorTableId.ToString(), this.UserAadId);

                if (sponsorDetails == null)
                {
                    this.RecordEvent("GetSponsorByIdAsync", RequestType.Failed);
                    return this.NotFound("Sponsor not found.");
                }

                this.RecordEvent("GetSponsorByIdAsync", RequestType.Succeeded);
                return this.Ok(sponsorDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetSponsorByIdAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching sponsor.");
                throw;
            }
        }

        /// <summary>
        /// Stores rating of user for a sponsor.
        /// </summary>
        /// <param name="sponsorTableId">The sponsor table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>Returns HTTP status code OK on successful operation.</returns>
        [HttpPost("rate/{sponsorTableId}/{rating}")]
        public async Task<IActionResult> RateSponsorAsync(Guid sponsorTableId, int rating)
        {
            this.RecordEvent("RateSponsorAsync", RequestType.Initiated);

            if (sponsorTableId == Guid.Empty)
            {
                this.RecordEvent("RateSponsorAsync", RequestType.Failed);
                this.logger.LogError("Empty sponsor table Id value was provided.");
                return this.BadRequest("The valid sponsor table Id must be provided.");
            }

            try
            {
                await this.sponsorHelper.RateSponsorAsync(sponsorTableId.ToString(), rating, this.UserAadId);

                this.RecordEvent("RateSponsorAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("RateSponsorAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while ratings sponsor.");
                throw;
            }
        }
    }
}