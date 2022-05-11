// <copyright file="NewsController.cs" company="NPS Foundation">
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
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Models.Enums;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// News controller is responsible to expose API endpoints to add and fetch news details.
    /// </summary>
    [Route("api/news")]
    [ApiController]
    [Authorize]
    public class NewsController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The instance of <see cref="NewsHelper"/> class.
        /// </summary>
        private readonly INewsHelper newsHelper;

        /// <summary>
        /// The instance of <see cref="AthenaNewsSourcesBlobRepository"/> class.
        /// </summary>
        private readonly IAthenaNewsSourcesBlobRepository athenaNewsSourcesBlobRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="newsHelper">The instance of <see cref="NewsHelper"/> class.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="athenaNewsSourcesBlobRepository">The instance of <see cref="AthenaNewsSourcesBlobRepository"/> class.</param>
        public NewsController(
            ILogger<NewsController> logger,
            INewsHelper newsHelper,
            TelemetryClient telemetryClient,
            IAthenaNewsSourcesBlobRepository athenaNewsSourcesBlobRepository)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.newsHelper = newsHelper;
            this.athenaNewsSourcesBlobRepository = athenaNewsSourcesBlobRepository;
        }

        /// <summary>
        /// Gets news items.
        /// </summary>
        /// <param name="searchString">Search string.</param>
        /// <param name="newsFilters">Holds the filter parameters for news.</param>
        /// <param name="pageCount">>Page count for which post needs to be fetched.</param>
        /// <param name="sortBy">0 for recent, 1 for significance and 2 for rating high to low for news. Refer <see cref="SortBy"/> for values.</param>
        /// <returns>Returns news details that was created.</returns>
        [HttpPost("search")]
        public async Task<IActionResult> GetNewsAsync(string searchString, [FromBody] NewsFilterParametersDTO newsFilters, int pageCount, int sortBy)
        {
            this.RecordEvent("GetNewsAsync", RequestType.Initiated);

            try
            {
                var newsEntity = await this.newsHelper.GetNewsAsync(searchString, pageCount, sortBy, newsFilters, this.UserAadId);

                if (newsEntity == null)
                {
                    this.RecordEvent("GetNewsAsync", RequestType.Failed);
                    return this.NotFound();
                }

                this.RecordEvent("GetNewsAsync", RequestType.Succeeded);

                return this.Ok(newsEntity);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetNewsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while gettings news articles.");
                throw;
            }
        }

        /// <summary>
        /// Gets news item by news table Id.
        /// </summary>
        /// <param name="tableId">News table Id.</param>
        /// <returns>Returns news details.</returns>
        [HttpGet("{tableId}")]
        public async Task<IActionResult> GetNewsByTableIdAsync(string tableId)
        {
            this.RecordEvent("GetNewsByTableIdAsync", RequestType.Initiated);

            try
            {
                var newsEntity = await this.newsHelper.GetNewsByTableIdAsync(tableId, this.UserAadId);

                if (newsEntity == null)
                {
                    this.RecordEvent("GetNewsByTableIdAsync", RequestType.Failed);
                    return this.NotFound();
                }

                this.RecordEvent("GetNewsByTableIdAsync", RequestType.Succeeded);

                return this.Ok(newsEntity);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetNewsByTableIdAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while gettings news articles.");
                throw;
            }
        }

        /// <summary>
        /// Gets coi news items.
        /// </summary>
        /// <param name="teamId">The COI team Id of which news articles to get.</param>
        /// <param name="searchString">Search string.</param>
        /// <param name="newsFilters">Holds the filter parameters for news.</param>
        /// <param name="pageCount">>Page count for which post needs to be fetched.</param>
        /// <param name="sortBy">0 for recent, 1 for significance and 2 for rating high to low for news. Refer <see cref="SortBy"/> for values.</param>
        /// <returns>Returns news details that was created.</returns>
        [HttpPost("coiNewsSearch")]
        public async Task<IActionResult> GetCoiNewsAsync(string teamId, string searchString, [FromBody] NewsFilterParametersDTO newsFilters, int pageCount, int sortBy)
        {
            this.RecordEvent("GetCoiNewsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { nameof(teamId), teamId },
            });

            if (string.IsNullOrWhiteSpace(teamId))
            {
                this.RecordEvent("GetCoiNewsAsync", RequestType.Failed);
                this.logger.LogError("Empty team Id value was provided.");
                return this.BadRequest("The valid team Id must be provided.");
            }

            try
            {
                var newsEntity = await this.newsHelper.GetCoiNewsAsync(teamId, searchString, pageCount, sortBy, newsFilters, this.UserAadId);

                if (newsEntity == null)
                {
                    this.RecordEvent("GetCoiNewsAsync", RequestType.Failed);
                    return this.NotFound();
                }

                this.RecordEvent("GetCoiNewsAsync", RequestType.Succeeded);

                return this.Ok(newsEntity);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetCoiNewsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while gettings COI news.");
                throw;
            }
        }

        /// <summary>
        /// Stores rating of user for a news item.
        /// </summary>
        /// <param name="newsId">The news Id for which rating to be submitted.</param>
        /// <param name="rating">The array of keywords.</param>
        /// <returns>Returns HTTP status code OK on successful operation.</returns>
        [HttpPost("rate/{newsId}/{rating}")]
        public async Task<IActionResult> RateNewsAsync(Guid newsId, int rating)
        {
            this.RecordEvent("RateNewsAsync", RequestType.Initiated);

            if (newsId == Guid.Empty)
            {
                this.RecordEvent("RateNewsAsync", RequestType.Failed);
                this.logger.LogError("Empty news Id value was provided.");
                return this.BadRequest("The valid news Id must be provided.");
            }

            try
            {
                await this.newsHelper.RateNewsAsync(newsId.ToString(), rating, this.UserAadId);

                this.RecordEvent("RateNewsAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("RateNewsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while ratings news article.");
                throw;
            }
        }

        /// <summary>
        /// Gets the athena news sources.
        /// </summary>
        /// <returns>The athena news sources.</returns>
        [HttpGet("sources")]
        public async Task<IActionResult> GetAthenaNewsSourcesAsync()
        {
            this.RecordEvent("GetAthenaNewsSourcesAsync", RequestType.Initiated);

            try
            {
                var response = await this.athenaNewsSourcesBlobRepository.GetBlobJsonFileContentAsync(AthenaNewsSourcesBlobMetadata.FileName);

                if (response == null)
                {
                    this.RecordEvent("GetAthenaNewsSourcesAsync", RequestType.Failed);
                    return this.NotFound("Athena news sources not found.");
                }

                this.RecordEvent("GetAthenaNewsSourcesAsync", RequestType.Succeeded);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetAthenaNewsSourcesAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting athena news sources.");
                throw;
            }
        }

        /// <summary>
        /// Gets the news node type.
        /// </summary>
        /// <returns>Returns collection of news node type.</returns>
        [HttpGet("node-types")]
        public async Task<IActionResult> GetNodeTypesForNewsAsync()
        {
            this.RecordEvent("HTTP GET- GetNodeTypesForNewsAsync", RequestType.Initiated);

            try
            {
                var newsNodeTypes = await this.newsHelper.GetNodeTypesForNewsAsync();

                this.RecordEvent("HTTP GET- GetNodeTypesForNewsAsync", RequestType.Succeeded);

                if (newsNodeTypes == null)
                {
                    return this.NotFound();
                }

                return this.Ok(newsNodeTypes);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting news node types.");
                this.RecordEvent("HTTP GET- GetNodeTypesForNewsAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Gets the news keyword Ids.
        /// </summary>
        /// <returns>Returns collection of news keyword Ids.</returns>
        [HttpGet("keywordIds")]
        public async Task<IActionResult> GetNewsKeywordIdsAsync()
        {
            this.RecordEvent("HTTP GET- GetNewsKeywordIdsAsync", RequestType.Initiated);

            try
            {
                var newsKeywordIds = await this.newsHelper.GetNewsKeywordIdsAsync();

                this.RecordEvent("HTTP GET- GetNewsKeywordIdsAsync", RequestType.Succeeded);

                if (newsKeywordIds == null)
                {
                    return this.NotFound();
                }

                return this.Ok(newsKeywordIds);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting news keyword Ids.");
                this.RecordEvent("HTTP GET- GetNewsKeywordIdsAsync", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Updates the news article.
        /// </summary>
        /// <param name="tableId">The table Id of news article.</param>
        /// <param name="isImportant">Indicates if the news aricle is important.</param>
        /// <returns>Returns the updated news article.</returns>
        [HttpPatch("update/{tableId}/{isImportant}")]
        public async Task<IActionResult> UpdateNewsAsync(Guid tableId, bool isImportant)
        {
            this.RecordEvent("UpdateNewsAsync", RequestType.Initiated);

            try
            {
                var response = await this.newsHelper.UpdateNewsAsync(tableId.ToString(), isImportant);

                if (response == null)
                {
                    this.RecordEvent("UpdateNewsAsync", RequestType.Failed);
                    return this.NotFound();
                }

                this.RecordEvent("UpdateNewsAsync", RequestType.Succeeded);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                this.RecordEvent("UpdateNewsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while updating news article.");
                throw;
            }
        }
    }
}