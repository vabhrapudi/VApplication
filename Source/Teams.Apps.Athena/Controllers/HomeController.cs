// <copyright file="HomeController.cs" company="NPS Foundation">
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
    using Teams.Apps.Athena.Authorization;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes the API endpoints related home tab.
    /// </summary>
    [Route("api/home")]
    [ApiController]
    [Authorize(AuthorizationPolicyNames.MustBeTeamMemberPolicy)]
    public class HomeController : BaseController
    {
        private readonly ILogger logger;
        private readonly IHomeHelper homeHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="homeHelper">The instance of <see cref="HomeHelper"/> class.</param>
        public HomeController(
            ILogger<HomeController> logger,
            TelemetryClient telemetryClient,
            IHomeHelper homeHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.homeHelper = homeHelper;
        }

        /// <summary>
        /// Gets the status bar details of Athena central team.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>The status bar details.</returns>
        [HttpGet("{teamId}/statusbar")]
        public async Task<IActionResult> GetStatusBarDetailsForCentralTeamAsync(Guid teamId)
        {
            this.RecordEvent("GetStatusBarDetailsForCentralTeamAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            if (teamId == Guid.Empty)
            {
                this.RecordEvent("GetStatusBarDetailsForCentralTeamAsync", RequestType.Failed);
                return this.BadRequest("Invalid team Id was provided.");
            }

            try
            {
                var statusBarDetails = await this.homeHelper.GetActiveHomeStatusBarDetailsForCentralTeamAsync();

                if (statusBarDetails == null)
                {
                    this.RecordEvent("GetStatusBarDetailsForCentralTeamAsync", RequestType.Succeeded);
                    return this.NoContent();
                }

                this.RecordEvent("GetStatusBarDetailsForCentralTeamAsync", RequestType.Succeeded);
                return this.Ok(statusBarDetails);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to get home status bar details for Athena central team.");
                this.RecordEvent("GetStatusBarDetailsForCentralTeamAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the status bar details of a COI team.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>The status bar details.</returns>
        [HttpGet("coi/{teamId}/statusbar")]
        public async Task<IActionResult> GetStatusBarDetailsForCoiTeamAsync(Guid teamId)
        {
            this.RecordEvent("GetStatusBarDetailsForCoiTeamAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            if (teamId == Guid.Empty)
            {
                this.RecordEvent("GetStatusBarDetailsForCoiTeamAsync", RequestType.Failed);
                return this.BadRequest("Invalid team Id was provided.");
            }

            try
            {
                var statusBarDetails = await this.homeHelper.GetActiveHomeStatusBarDetailsAsync(teamId);

                if (statusBarDetails == null)
                {
                    this.RecordEvent("GetStatusBarDetailsForCoiTeamAsync", RequestType.Succeeded);
                    return this.NoContent();
                }

                this.RecordEvent("GetStatusBarDetailsForCoiTeamAsync", RequestType.Succeeded);
                return this.Ok(statusBarDetails);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to get home status bar details for COI team.");
                this.RecordEvent("GetStatusBarDetailsForCoiTeamAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the 'New to Athena' section articles configured for a COI team.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>The new to Athena articles.</returns>
        [HttpGet("coi/{teamId}/new-articles")]
        public async Task<IActionResult> GetNewToAthenaArticlesForCoiTeamAsync(Guid teamId)
        {
            this.RecordEvent("GetNewToAthenaArticlesForCoiTeamAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            if (teamId == Guid.Empty)
            {
                this.RecordEvent("GetNewToAthenaArticlesForCoiTeamAsync", RequestType.Failed);
                return this.BadRequest("Invalid team Id was provided.");
            }

            try
            {
                var articles = await this.homeHelper.GetNewToAthenaArticlesAsync(teamId);

                this.RecordEvent("GetNewToAthenaArticlesForCoiTeamAsync", RequestType.Succeeded);
                return this.Ok(articles);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to get new to Athena articles for COI team.");
                this.RecordEvent("GetNewToAthenaArticlesForCoiTeamAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the 'New to Athena' section articles configured for Athena central team.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>The new to Athena articles.</returns>
        [HttpGet("{teamId}/new-articles")]
        public async Task<IActionResult> GetNewToAthenaArticlesForCentralTeamAsync(Guid teamId)
        {
            this.RecordEvent("GetNewToAthenaArticlesForCentralTeamAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            if (teamId == Guid.Empty)
            {
                this.RecordEvent("GetNewToAthenaArticlesForCentralTeamAsync", RequestType.Failed);
                return this.BadRequest("Invalid team Id was provided.");
            }

            try
            {
                var articles = await this.homeHelper.GetNewToAthenaArticlesForCentralTeamAsync();

                this.RecordEvent("GetNewToAthenaArticlesForCentralTeamAsync", RequestType.Succeeded);
                return this.Ok(articles);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to get new to Athena articles for Athena central team.");
                this.RecordEvent("GetNewToAthenaArticlesForCentralTeamAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the daily briefing articles of an user for Athena central team.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>The daily briefing articles of Athena central team.</returns>
        [HttpGet("{teamId}/briefing-articles")]
        public async Task<IActionResult> GetDailyBriefingArticlesOfUserForCentralTeamAsync(Guid teamId)
        {
            this.RecordEvent("GetDailyBriefingArticlesOfUserForCentralTeamAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            if (teamId == Guid.Empty)
            {
                this.RecordEvent("GetDailyBriefingArticlesOfUserForCentralTeamAsync", RequestType.Failed);
                return this.BadRequest("Invalid team Id was provided.");
            }

            try
            {
                var articles = await this.homeHelper.GetDailyBriefingArticlesOfUserForCentralTeamAsync(this.UserAadId);

                this.RecordEvent("GetDailyBriefingArticlesOfUserForCentralTeamAsync", RequestType.Succeeded);
                return this.Ok(articles);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to get daily briefing articles of user for central team.");
                this.RecordEvent("GetDailyBriefingArticlesOfUserForCentralTeamAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the daily briefing articles of an user for COI team.
        /// </summary>
        /// <param name="teamId">The COI team Id.</param>
        /// <returns>The daily briefing articles of COI team.</returns>
        [HttpGet("coi/{teamId}/briefing-articles")]
        public async Task<IActionResult> GetDailyBriefingArticlesOfUserForCoiTeamAsync(Guid teamId)
        {
            this.RecordEvent("GetDailyBriefingArticlesOfUserForCoiTeamAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            if (teamId == Guid.Empty)
            {
                this.RecordEvent("GetDailyBriefingArticlesOfUserForCoiTeamAsync", RequestType.Failed);
                return this.BadRequest("Invalid team Id was provided.");
            }

            try
            {
                var articles = await this.homeHelper.GetDailyBriefingArticlesOfUserForCoiTeamAsync(teamId, this.UserAadId);

                this.RecordEvent("GetDailyBriefingArticlesOfUserForCoiTeamAsync", RequestType.Succeeded);
                return this.Ok(articles);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Failed to get daily briefing articles of user for COI team.");
                this.RecordEvent("GetDailyBriefingArticlesOfUserForCoiTeamAsync", RequestType.Failed);
                throw;
            }
        }
    }
}
