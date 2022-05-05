// <copyright file="EventController.cs" company="NPS Foundation">
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
    /// Exposes API endpoints related to event.
    /// </summary>
    [Route("api/events")]
    [ApiController]
    [Authorize]
    public class EventController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for event.
        /// </summary>
        private IEventHelper eventHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="eventHelper">The instance of <see cref="EventHelper"/> class.</param>
        public EventController(
            ILogger<EventController> logger,
            TelemetryClient telemetryClient,
            IEventHelper eventHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.eventHelper = eventHelper;
        }

        /// <summary>
        /// Gets a event by table Id.
        /// </summary>
        /// <param name="eventTableId">Unique event Table Id.</param>
        /// <returns>Returns event details.</returns>
        [HttpGet("{eventTableId}")]
        public async Task<IActionResult> GetEventByTableIdAsync(Guid eventTableId)
        {
            this.RecordEvent("GetEventByIdAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "eventTableId", eventTableId.ToString() },
            });

            if (eventTableId == Guid.Empty)
            {
                this.logger.LogError("Event Id is null or invalid.");
                this.RecordEvent("GetEventByIdAsync", RequestType.Failed);
                return this.BadRequest("Invalid event Id.");
            }

            try
            {
                var eventDetails = await this.eventHelper.GetEventByTableIdAsync(eventTableId.ToString(), this.UserAadId);

                if (eventDetails == null)
                {
                    this.RecordEvent("GetEventByIdAsync", RequestType.Failed);
                    return this.NotFound("Event not found.");
                }

                this.RecordEvent("GetEventByIdAsync", RequestType.Succeeded);
                return this.Ok(eventDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetEventByIdAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching event.");
                throw;
            }
        }

        /// <summary>
        /// Stores rating of user for a event.
        /// </summary>
        /// <param name="eventTableId">The event table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>Returns HTTP status code OK on successful operation.</returns>
        [HttpPost("rate/{eventTableId}/{rating}")]
        public async Task<IActionResult> RateEventAsync(Guid eventTableId, int rating)
        {
            this.RecordEvent("RateEventAsync", RequestType.Initiated);

            if (eventTableId == Guid.Empty)
            {
                this.RecordEvent("RateEventAsync", RequestType.Failed);
                this.logger.LogError("Empty event table Id value was provided.");
                return this.BadRequest("The valid event table Id must be provided.");
            }

            try
            {
                await this.eventHelper.RateEventAsync(eventTableId.ToString(), rating, this.UserAadId);

                this.RecordEvent("RateEventAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("RateEventAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while rating event.");
                throw;
            }
        }
    }
}
