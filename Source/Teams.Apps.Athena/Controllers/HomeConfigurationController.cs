// <copyright file="HomeConfigurationController.cs" company="NPS Foundation">
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
    /// Home tab configuration controller is responsible to expose API endpoints related home configurations.
    /// </summary>
    [Route("api/home/configuration")]
    [ApiController]
    [Authorize(AuthorizationPolicyNames.MustBeTeamOwnerPolicy)]
    public class HomeConfigurationController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Holds the instance of <see cref="HomeConfigurationHelper"/> class.
        /// </summary>
        private readonly IHomeConfigurationHelper homeConfigurationHelper;

        /// <summary>
        /// Holds the instance of <see cref="HomeStatusBarConfigurationHelper"/> class.
        /// </summary>
        private readonly IHomeStatusBarConfigurationHelper homeStatusBarConfigurationHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeConfigurationController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="homeConfigurationHelper">The instance of <see cref="HomeConfigurationHelper"/> class.</param>
        /// <param name="homeStatusBarConfigurationHelper">The instance of <see cref="HomeStatusBarConfigurationHelper"/> class.</param>
        public HomeConfigurationController(
            ILogger<HomeConfigurationController> logger,
            TelemetryClient telemetryClient,
            IHomeConfigurationHelper homeConfigurationHelper,
            IHomeStatusBarConfigurationHelper homeStatusBarConfigurationHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.homeConfigurationHelper = homeConfigurationHelper;
            this.homeStatusBarConfigurationHelper = homeStatusBarConfigurationHelper;
        }

        /// <summary>
        /// Get home configuration.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="articleId">The article Id.</param>
        /// <returns>Returns home configuration details.</returns>
        [HttpGet("{teamId}/{articleId}")]
        public async Task<IActionResult> GetHomeConfigurationByArticleIdAsync(Guid teamId, Guid articleId)
        {
            this.RecordEvent("GetHomeConfigurationByArticleIdAsync", RequestType.Initiated);

            try
            {
                var homeConfiguration = await this.homeConfigurationHelper.GetHomeConfigurationByArticleIdAsync(teamId.ToString(), articleId.ToString());

                if (homeConfiguration == null)
                {
                    this.RecordEvent("GetHomeConfigurationByArticleIdAsync", RequestType.Failed);
                    return this.NotFound();
                }

                this.RecordEvent("GetHomeConfigurationByArticleIdAsync", RequestType.Succeeded);

                return this.Ok(homeConfiguration);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetHomeConfigurationByArticleIdAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting home configuration.");
                throw;
            }
        }

        /// <summary>
        /// Creates a new home configuration article.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="homeConfigurationArticleDetails">The details of home configuration article to be created.</param>
        /// <returns>Returns home configuration details that was created.</returns>
        [HttpPost("{teamId}")]
        public async Task<IActionResult> CreateHomeConfigurationArticleAsync(Guid teamId, [FromBody] HomeConfigurationArticleDTO homeConfigurationArticleDetails)
        {
            this.RecordEvent("CreateHomeConfigurationArticleAsync", RequestType.Initiated, new Dictionary<string, string>
            {
#pragma warning disable CA1062 // Validated arguments at model level.
                { "teamId", teamId.ToString() },
#pragma warning restore CA1062 // Validated arguments at model level.
            });

            if (teamId == Guid.Empty)
            {
                this.RecordEvent("CreateHomeConfigurationArticleAsync", RequestType.Failed);
                return this.BadRequest("The valid team Id must be provided.");
            }

            try
            {
                var articleDetails = await this.homeConfigurationHelper.CreateHomeConfigurationArticleAsync(teamId.ToString(), homeConfigurationArticleDetails, this.UserAadId);

                if (articleDetails == null)
                {
                    this.RecordEvent("CreateHomeConfigurationArticleAsync", RequestType.Failed);
                    return this.BadRequest($"Maximum {Constants.MaxNumberOfArticlesCanBeConfigured} articles are allowed to configure per team.");
                }

                this.RecordEvent("CreateHomeConfigurationArticleAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created, articleDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("CreateHomeConfigurationArticleAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while creating home configuration.");
                throw;
            }
        }

        /// <summary>
        /// Updates a home configuration article.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="homeConfigurationArticleDetails">The details of article to be updated.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPatch("{teamId}")]
        public async Task<IActionResult> UpdateHomeConfigurationArticleAsync(Guid teamId, [FromBody] HomeConfigurationArticleDTO homeConfigurationArticleDetails)
        {
            this.RecordEvent("UpdateHomeConfigurationArticleAsync", RequestType.Initiated, new Dictionary<string, string>
            {
#pragma warning disable CA1062 // Validated arguments at model level.
                { "articleId", homeConfigurationArticleDetails.ArticleId },
                { "teamId", teamId.ToString() },
#pragma warning restore CA1062 // Validated arguments at model level.
            });

            if (homeConfigurationArticleDetails.ArticleId.IsEmptyOrInvalidGuid())
            {
                this.RecordEvent("UpdateHomeConfigurationArticleAsync", RequestType.Failed);
                return this.BadRequest("The home confuguration article Id is invalid.");
            }

            try
            {
                var updatedArticleDetails = await this.homeConfigurationHelper.UpdateHomeConfigurationArticleAsync(teamId.ToString(), homeConfigurationArticleDetails, this.UserAadId);

                if (updatedArticleDetails == null)
                {
                    this.RecordEvent("UpdateHomeConfigurationArticleAsync", RequestType.Failed);
                    return this.NotFound("The home configuration article was not found for provided team Id.");
                }

                this.RecordEvent("UpdateHomeConfigurationArticleAsync", RequestType.Succeeded);
                return this.Ok(updatedArticleDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("UpdateHomeConfigurationArticleAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while updating home configuration article.");
                throw;
            }
        }

        /// <summary>
        /// Gets the home configuration articles of a team.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetHomeConfigurationArticlesAsync(Guid teamId)
        {
            this.RecordEvent("GetHomeConfigurationArticlesAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            try
            {
                var homeConfigurationArticles = await this.homeConfigurationHelper.GetHomeConfigurationArticlesAsync(teamId);

                this.RecordEvent("GetHomeConfigurationArticlesAsync", RequestType.Succeeded);

                return this.Ok(homeConfigurationArticles);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetHomeConfigurationArticlesAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting home configuration articles of a team.");
                throw;
            }
        }

        /// <summary>
        /// Deletes the home configuration articles.
        /// </summary>
        /// <param name="teamId">The team Id of which articles to be deleted.</param>
        /// <param name="homeConfigurationArticleIds">The collection of article Ids to delete.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("{teamId}")]
        public async Task<IActionResult> DeleteHomeConfigurationArticlesAsync(Guid teamId, [FromBody] IEnumerable<Guid> homeConfigurationArticleIds)
        {
            this.RecordEvent("DeleteHomeConfigurationArticlesAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            if (homeConfigurationArticleIds.IsNullOrEmpty())
            {
                this.RecordEvent("DeleteHomeConfigurationArticlesAsync", RequestType.Failed);
                return this.BadRequest("The article Ids are required.");
            }

            try
            {
                await this.homeConfigurationHelper.DeleteHomeConfigurationArticlesAsync(teamId, homeConfigurationArticleIds);

                this.RecordEvent("DeleteHomeConfigurationArticlesAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("DeleteHomeConfigurationArticlesAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while deleting home configuration articles.");
                throw;
            }
        }

        /// <summary>
        /// Creates the home status bar configuration.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="homeStatusBarConfigurationDTO">The home status bar configuration details.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("statusbar/{teamId}")]
        public async Task<IActionResult> CreateHomeStatusBarConfigurationAsync(Guid teamId, [FromBody] HomeStatusBarConfigurationDTO homeStatusBarConfigurationDTO)
        {
            this.RecordEvent("CreateHomeStatusBarConfigurationAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            if (teamId == Guid.Empty)
            {
                this.RecordEvent("CreateHomeStatusBarConfigurationAsync", RequestType.Failed);
                return this.BadRequest("The valid team Id must be required.");
            }

            try
            {
                var homeStatusBarConfigurationDetails = await this.homeStatusBarConfigurationHelper.CreateHomeStatusBarConfigurationAsync(homeStatusBarConfigurationDTO, teamId.ToString(), this.UserAadId);

                this.RecordEvent("CreateHomeStatusBarConfigurationAsync", RequestType.Succeeded);
                return this.StatusCode((int)HttpStatusCode.Created, homeStatusBarConfigurationDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("CreateHomeStatusBarConfigurationAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while creating home status bar configuration.");
                throw;
            }
        }

        /// <summary>
        /// Updates the home status bar configuration.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <param name="homeStatusBarConfigurationDTO">The home status bar configuration details.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPatch("statusbar/{teamId}")]
        public async Task<IActionResult> UpdateHomeStatusBarConfigurationAsync(Guid teamId, [FromBody] HomeStatusBarConfigurationDTO homeStatusBarConfigurationDTO)
        {
            this.RecordEvent("UpdateHomeStatusBarConfigurationAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            if (teamId == Guid.Empty)
            {
                this.RecordEvent("UpdateHomeStatusBarConfigurationAsync", RequestType.Failed);
                return this.BadRequest("The valid team Id must be required.");
            }

            try
            {
                var homeStatusBarConfigurationDetails = await this.homeStatusBarConfigurationHelper.UpdateHomeStatusBarConfigurationAsync(homeStatusBarConfigurationDTO, teamId.ToString(), this.UserAadId);

                if (homeStatusBarConfigurationDetails == null)
                {
                    this.RecordEvent("UpdateHomeStatusBarConfigurationAsync", RequestType.Failed);
                    return this.NotFound("The requested resource was not found.");
                }

                this.RecordEvent("UpdateHomeStatusBarConfigurationAsync", RequestType.Succeeded);
                return this.Ok(homeStatusBarConfigurationDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("UpdateHomeStatusBarConfigurationAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while updating home status bar configuration.");
                throw;
            }
        }

        /// <summary>
        /// Gets the home status bar configuration.
        /// </summary>
        /// <param name="teamId">The team Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet("statusbar/{teamId}")]
        public async Task<IActionResult> GetHomeStatusBarConfigurationAsync(Guid teamId)
        {
            this.RecordEvent("GetHomeStatusBarConfigurationAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "teamId", teamId.ToString() },
            });

            if (teamId == Guid.Empty)
            {
                this.RecordEvent("GetHomeStatusBarConfigurationAsync", RequestType.Failed);
                return this.BadRequest("The valid team Id must be required.");
            }

            try
            {
                var homeStatusBarConfigurationDetails = await this.homeStatusBarConfigurationHelper.GetHomeStatusBarConfigurationAsync(teamId.ToString());

                if (homeStatusBarConfigurationDetails == null)
                {
                    this.RecordEvent("GetHomeStatusBarConfigurationAsync", RequestType.Failed);
                    return this.NotFound("The requested resource was not found.");
                }

                this.RecordEvent("GetHomeStatusBarConfigurationAsync", RequestType.Succeeded);
                return this.Ok(homeStatusBarConfigurationDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetHomeStatusBarConfigurationAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting home status bar configuration.");
                throw;
            }
        }
    }
}
