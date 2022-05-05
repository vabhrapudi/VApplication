// <copyright file="QuickAccessController.cs" company="NPS Foundation">
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
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes API endpoints related to quick access list.
    /// </summary>
    [Route("api/quick-access")]
    [ApiController]
    [Authorize]
    public class QuickAccessController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for quick access list.
        /// </summary>
        private IQuickAccessHelper quickAccessHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuickAccessController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="quickAccessHelper">The instance of <see cref="QuickAccessHelper"/> class.</param>
        public QuickAccessController(
            ILogger<QuickAccessController> logger,
            TelemetryClient telemetryClient,
            IQuickAccessHelper quickAccessHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.quickAccessHelper = quickAccessHelper;
        }

        /// <summary>
        /// Gets the list of quick access items.
        /// </summary>
        /// <returns>The quick access list.</returns>
        [HttpGet]
        public async Task<IActionResult> GetQuickAccessListAsync()
        {
            this.RecordEvent("GetQuickAccessListAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "UserId", this.UserAadId },
            });

            try
            {
                var quickAccessList = await this.quickAccessHelper.GetQuickAccessListAsync(this.UserAadId);

                this.RecordEvent("GetQuickAccessListAsync", RequestType.Succeeded);

                return this.Ok(quickAccessList);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetQuickAccessListAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting quick access list.");

                throw;
            }
        }

        /// <summary>
        /// Adds quick access item.
        /// </summary>
        /// <param name="quickAccessItem">The quick access item.</param>
        /// <returns>The created comment.</returns>
        [HttpPost]
        public async Task<IActionResult> AddQuickAccessItemAsync(QuickAccessItemCreateDTO quickAccessItem)
        {
            this.RecordEvent("AddQuickAccessItemAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "UserId", this.UserAadId },
            });

            try
            {
                var response = await this.quickAccessHelper.AddQuickAccessItemAsync(quickAccessItem, this.UserAadId);

                if (response == null)
                {
                    this.RecordEvent("AddQuickAccessItemAsync", RequestType.Failed);
                    return this.Conflict();
                }

                this.RecordEvent("AddQuickAccessItemAsync", RequestType.Succeeded);

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                this.RecordEvent("AddQuickAccessItemAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while adding quick access item.");
                throw;
            }
        }

        /// <summary>
        /// Deletes the quick access item.
        /// </summary>
        /// <param name="quickAccessItemId">The quick access item Id.</param>
        /// <returns>Returns task indicating operation result.</returns>
        [HttpDelete("{quickAccessItemId}")]
        public async Task<IActionResult> DeleteQuickAccessItemAsync(Guid quickAccessItemId)
        {
            this.RecordEvent("DeleteQuickAccessItemAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "UserId", this.UserAadId },
            });

            if (quickAccessItemId == Guid.Empty)
            {
                this.RecordEvent("DeleteQuickAccessItemAsync", RequestType.Failed);
                this.logger.LogError("Empty quick access item Id value was provided.");

                return this.BadRequest("The valid Guid must be provided.");
            }

            try
            {
                await this.quickAccessHelper.DeleteQuickAccessItemAsync(quickAccessItemId.ToString());

                this.RecordEvent("DeleteQuickAccessItemAsync", RequestType.Succeeded);

                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("DeleteQuickAccessItemAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while deleting quick access item.");
                throw;
            }
        }
    }
}