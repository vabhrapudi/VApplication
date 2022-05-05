// <copyright file="KeywordsController.cs" company="NPS Foundation">
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
    ///  Keywords controller is responsible to expose API endpoints for performing search operation on keywords entity.
    /// </summary>
    [Route("api/keywords")]
    [ApiController]
    [Authorize]
    public class KeywordsController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for keywords.
        /// </summary>
        private readonly IKeywordsHelper keywordsHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeywordsController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="keywordsHelper">The instance of keywords helper which helps in managing operations on keywords entity.</param>
        public KeywordsController(
            ILogger<KeywordsController> logger,
            TelemetryClient telemetryClient,
            IKeywordsHelper keywordsHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.keywordsHelper = keywordsHelper;
        }

        /// <summary>
        /// Gets the keywords.
        /// </summary>
        /// <param name="searchQuery">Input string from user.</param>
        /// <returns>Returns keywords details.</returns>
        [HttpGet]
        public async Task<IActionResult> GetKeywordsAsync(string searchQuery)
        {
            this.RecordEvent("GetKeywordsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "searchString", searchQuery },
            });

            try
            {
                var getKeywordsDetails = await this.keywordsHelper.GetKeywordsAsync(searchQuery);

                if (getKeywordsDetails == null)
                {
                    this.RecordEvent("GetKeywordsAsync", RequestType.Failed);
                    return this.NotFound("keywords not found.");
                }

                this.RecordEvent("GetKeywordsAsync", RequestType.Succeeded);
                return this.Ok(getKeywordsDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetKeywordsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching keywords.");
                throw;
            }
        }

        /// <summary>
        /// Gets the keywords of COI team.
        /// </summary>
        /// <param name="teamId">Team Id.</param>
        /// <returns>Returns keywords details.</returns>
        [HttpGet("coiTeamKeywords")]
        public async Task<IActionResult> GetCoiTeamKeywordsAsync(string teamId)
        {
            this.RecordEvent("GetCoiTeamKeywordsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "TeamId", teamId },
            });

            try
            {
                var getKeywords = await this.keywordsHelper.GetCoiTeamKeywordsAsync(teamId);

                if (getKeywords == null)
                {
                    this.RecordEvent("GetCoiTeamKeywordsAsync", RequestType.Failed);
                    return this.NotFound("keywords not found.");
                }

                this.RecordEvent("GetCoiTeamKeywordsAsync", RequestType.Succeeded);
                return this.Ok(getKeywords);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetCoiTeamKeywordsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching keywords.");
                throw;
            }
        }

        /// <summary>
        /// Get keywords as per keyword Ids.
        /// </summary>
        /// <param name="keywordIds">Collections of keyword Ids.</param>
        /// <returns>List of Keywords.</returns>
        [HttpPost("keywordIds")]
        public async Task<IActionResult> GetKeywordsByKeywordIdsAsync([FromBody] IEnumerable<int> keywordIds)
        {
            this.RecordEvent("GetKeywordsByKeywordIdsAsync", RequestType.Initiated);

            try
            {
                var keywords = await this.keywordsHelper.GetKeywordsByKeywordIdsAsync(keywordIds);

                this.RecordEvent("GetKeywordsByKeywordIdsAsync", RequestType.Succeeded);

                return this.Ok(keywords);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetKeywordsByKeywordIdsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting keywords.");
                throw;
            }
        }

        /// <summary>
        /// Gets the all keywords.
        /// </summary>
        /// <returns>Returns all keywords from blob storage.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllKeywordsAsync()
        {
            this.RecordEvent("GetAllKeywordsAsync", RequestType.Initiated);

            try
            {
                var keywords = await this.keywordsHelper.GetAllKeywordsAsync();

                if (keywords == null)
                {
                    this.RecordEvent("GetAllKeywordsAsync", RequestType.Failed);
                    return this.NotFound("Keywords not found.");
                }

                this.RecordEvent("GetAllKeywordsAsync", RequestType.Succeeded);
                return this.Ok(keywords);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetAllKeywordsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching all keywords.");
                throw;
            }
        }
    }
}
