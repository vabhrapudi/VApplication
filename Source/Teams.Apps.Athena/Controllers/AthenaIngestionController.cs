// <copyright file="AthenaIngestionController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes API endpoints related to athena ingestion.
    /// </summary>
    [Route("api/athenaIngestion")]
    [ApiController]
    [Authorize]
    public class AthenaIngestionController : BaseController
    {
        /// <summary>
        /// The instance of <see cref="ILogger"/>.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for Athena Ingestion.
        /// </summary>
        private readonly IAthenaIngestionHelper athenaIngestionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaIngestionController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="athenaIngestionHelper">The instance of <see cref="AthenaIngestionHelper"/></param>
        public AthenaIngestionController(
            ILogger<AthenaIngestionController> logger,
            TelemetryClient telemetryClient,
            IAthenaIngestionHelper athenaIngestionHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.athenaIngestionHelper = athenaIngestionHelper;
        }

        /// <summary>
        /// Inserting or Updating data into table.
        /// </summary>
        /// <param name="entityName">Entity name that need to be updated.</param>
        /// <param name="path"> path of the  content to update</param>
        /// <returns>Returns HTTP status code OK on successful operation.</returns>
        [HttpPost("upsertEntity")]
        public async Task<IActionResult> AddUpdateEntityAsync(string entityName, string path)
        {
            Uri url = Uri.TryCreate(path, UriKind.Absolute, out url) ? url : null;
            if (string.IsNullOrEmpty(entityName))
            {
                this.RecordEvent("AddUpdateEntityAsync" + " " + entityName, RequestType.Failed);
                this.logger.LogError("No entity name provided");
                return this.BadRequest("No entity name provided");
            }
            else if (string.IsNullOrEmpty(path))
            {
                this.RecordEvent("AddUpdateEntityAsync" + " " + entityName, RequestType.Failed);
                this.logger.LogError("No path provided");
                return this.BadRequest("No path provided");
            }
            else if (!this.CheckUrlStatus(url))
            {
                this.RecordEvent("AddUpdateEntityAsync" + " " + entityName, RequestType.Failed);
                this.logger.LogError("Not a valid path");
                return this.BadRequest("Not a valid path");
            }
            else if (!path.ToUpperInvariant().EndsWith(".JSON", (StringComparison)2))
            {
                this.RecordEvent("AddUpdateEntityAsync" + " " + entityName, RequestType.Failed);
                this.logger.LogError("Not a valid path");
                return this.BadRequest("Not a valid path, provide json file");
            }

            try
            {
                await this.athenaIngestionHelper.AddUpdateEntity(entityName, path);
                return this.Ok($"Updated {entityName} Entity of file {path}. ");
            }
            catch (Exception ex)
            {
                this.RecordEvent("AddUpdateEntityAsync" + " " + entityName, RequestType.Failed);
                this.logger.LogError("Not a valid path");
                throw new Exception("Error while Adding or Updating Entity");
            }
        }

        /// <summary>
        /// Gets the Athena ingestion details.
        /// </summary>
        /// <returns>The Athena ingestion entity.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAthenaIngestionDetailsAsync()
        {
            this.RecordEvent("GetAthenaIngestionDetailsAsync", RequestType.Initiated);
            try
            {
                var athenaIngestionDetails = await this.athenaIngestionHelper.GetAthenaIngestionDetailsAsync();
                if (athenaIngestionDetails == null)
                {
                    this.RecordEvent("GetAthenaIngestionDetailsAsync", RequestType.Failed);
                    return this.NotFound("Athena ingestion details not found.");
                }

                this.RecordEvent("GetAthenaIngestionDetailsAsync", RequestType.Succeeded);
                return this.Ok(athenaIngestionDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetAthenaIngestionDetailsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching Athena ingestion details.");
                throw;
            }
        }

        /// <summary>
        /// Checks if Url exists
        /// </summary>
        /// <param name="url">url </param>
        /// <returns>boolean</returns>
        protected bool CheckUrlStatus(Uri url)
        {
            try
            {
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "HEAD";
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
