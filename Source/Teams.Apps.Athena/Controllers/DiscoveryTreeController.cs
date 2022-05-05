// <copyright file="DiscoveryTreeController.cs" company="NPS Foundation">
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
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes API endpoints related to discovery tree taxonomy.
    /// </summary>
    [Route("api/discovery-tree")]
    [ApiController]
    [Authorize]
    public class DiscoveryTreeController : BaseController
    {
        private readonly ILogger logger;

        private IDiscoveryTreeHelper discoveryTreeHelper;

        private IDiscoveryTreeTaxonomyBlobRepository discoveryTreeTaxonomyBlobRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryTreeController"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="ILogger"/>.</param>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        /// <param name="discoveryTreeHelper">The instance of <see cref="DiscoveryTreeHelper"/> class.</param>
        /// <param name="discoveryTreeTaxonomyBlobRepository">The instance of <see cref="DiscoveryTreeTaxonomyBlobRepository"/> class.</param>
        public DiscoveryTreeController(
            ILogger<DiscoveryTreeController> logger,
            TelemetryClient telemetryClient,
            IDiscoveryTreeHelper discoveryTreeHelper,
            IDiscoveryTreeTaxonomyBlobRepository discoveryTreeTaxonomyBlobRepository)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.discoveryTreeHelper = discoveryTreeHelper;
            this.discoveryTreeTaxonomyBlobRepository = discoveryTreeTaxonomyBlobRepository;
        }

        /// <summary>
        /// Gets the discovery tree taxonomy.
        /// </summary>
        /// <returns>The discovery tree taxonomy.</returns>
        [HttpGet("taxonomy")]
        public async Task<IActionResult> GetDiscoveryTreeTaxonomyAsync()
        {
            this.RecordEvent("GetDiscoveryTreeTaxonomyAsync", RequestType.Initiated);

            try
            {
                // var taxonomy = await this.discoveryTreeTaxonomySearchService.GetDiscoveryTreeTaxonomy(null, 0);
                var taxonomy = await this.discoveryTreeTaxonomyBlobRepository.GetBlobJsonFileContentAsync(DiscoveryTreeTaxonomyBlobMetadata.FileName);

                if (taxonomy == null)
                {
                    this.RecordEvent("GetDiscoveryTreeTaxonomyAsync", RequestType.Failed);
                    return this.NotFound("Discovery tree taxonomy not found.");
                }

                this.RecordEvent("GetDiscoveryTreeTaxonomyAsync", RequestType.Succeeded);

                return this.Ok(taxonomy);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetDiscoveryTreeTaxonomyAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting discovery tree taxonomy.");
                throw;
            }
        }

        /// <summary>
        /// Gets the discovery tree taxonomy.
        /// </summary>
        /// <param name="keywords">The collection of keywords.</param>
        /// <returns>The discovery tree taxonomy.</returns>
        [HttpPost("node-data")]
        public async Task<IActionResult> GetDiscoveryTreeNodeDataAsync([FromBody] IEnumerable<int> keywords)
        {
            this.RecordEvent("GetDiscoveryTreeNodeDataAsync", RequestType.Initiated);

            try
            {
                var nodeData = await this.discoveryTreeHelper.GetDiscoveryTreeNodeData(keywords);

                this.RecordEvent("GetDiscoveryTreeNodeDataAsync", RequestType.Succeeded);

                return this.Ok(nodeData);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetDiscoveryTreeNodeDataAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting discovery tree node data.");
                throw;
            }
        }

        /// <summary>
        /// Gets the discovery tree node type.
        /// </summary>
        /// <returns>The discovery tree node type.</returns>
        [HttpGet("node-type")]
        public async Task<IActionResult> GetDiscoveryTreeNodeTypeAsync()
        {
            this.RecordEvent("GetDiscoveryTreeNodeDataAsync", RequestType.Initiated);

            try
            {
                var nodeType = await this.discoveryTreeHelper.GetDiscoveryTreeNodeTypeAsync();

                if (nodeType == null)
                {
                    this.RecordEvent("GetDiscoveryTreeNodeDataAsync", RequestType.Failed);
                    return this.NotFound("Discovery tree node types not found.");
                }

                this.RecordEvent("GetDiscoveryTreeNodeDataAsync", RequestType.Succeeded);

                return this.Ok(nodeType);
            }
            catch (Exception ex)
            {
                this.RecordEvent("nodeType", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting discovery tree node type.");
                throw;
            }
        }

        /// <summary>
        /// Gets the discovery tree filters.
        /// </summary>
        /// <returns>The discovery tree filters.</returns>
        [HttpGet("filters")]
        public async Task<IActionResult> GetDiscoveryTreeFiltersAsync()
        {
            this.RecordEvent("GetDiscoveryTreeFiltersAsync", RequestType.Initiated);

            try
            {
                var filters = await this.discoveryTreeHelper.GetDiscoveryTreeFilters();

                if (filters == null)
                {
                    this.RecordEvent("GetDiscoveryTreeFiltersAsync", RequestType.Failed);
                    return this.NotFound("Discovery tree filters not found.");
                }

                this.RecordEvent("GetDiscoveryTreeFiltersAsync", RequestType.Succeeded);

                return this.Ok(filters);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetDiscoveryTreeFiltersAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting discovery tree filters.");
                throw;
            }
        }

        /// <summary>
        /// Gets the users by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The collection of users.</returns>
        [HttpPost("users")]
        public async Task<IActionResult> GetUsersByKeywordIds(IEnumerable<int> keywordIds)
        {
            this.RecordEvent("GetInterestedUsersAsync", RequestType.Initiated);

            try
            {
                var users = await this.discoveryTreeHelper.GetUsersByKeywordIds(keywordIds);

                this.RecordEvent("GetInterestedUsersAsync", RequestType.Succeeded);

                return this.Ok(users);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetInterestedUsersAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while getting interested users.");
                throw;
            }
        }

        /// <summary>
        /// Follows a resource.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword ids.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPatch("keywords")]
        public async Task<IActionResult> FollowResourceAsync(IEnumerable<int> keywordIds)
        {
            this.RecordEvent("FollowResourceAsync", RequestType.Initiated);

            if (keywordIds.IsNullOrEmpty())
            {
                this.RecordEvent("FollowResourceAsync", RequestType.Failed);
                return this.BadRequest("The keyword Ids are required in order to follow the resource.");
            }

            try
            {
                var updatedUserEntity = await this.discoveryTreeHelper.FollowResourceAsync(keywordIds, this.UserAadId);

                if (updatedUserEntity == null)
                {
                    this.RecordEvent("FollowResourceAsync", RequestType.Failed);
                    return this.NotFound("Failed to update user keywords.");
                }

                this.RecordEvent("FollowResourceAsync", RequestType.Succeeded);
                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("FollowResourceAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while updating keywords.");
                throw;
            }
        }

        /// <summary>
        /// Find or filters the discovery tree resources.
        /// </summary>
        /// <param name="searchAndFilterOptions">The search and filter options.</param>
        /// <returns>>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost("search-filter")]
        public async Task<IActionResult> FindOrFilterDiscoveryTreeResourcesAsync([FromBody] DiscoveryTreeSearchAndFilter searchAndFilterOptions)
        {
            this.RecordEvent("FindOrFilterDiscoveryTreeResourcesAsync", RequestType.Initiated);

            try
            {
                var resources = await this.discoveryTreeHelper.FindOrFilterDiscoveryTreeResourcesAsync(searchAndFilterOptions?.SearchStrings, searchAndFilterOptions?.SearchKeywords, searchAndFilterOptions?.SelectedFilters);

                this.RecordEvent("FindOrFilterDiscoveryTreeResourcesAsync", RequestType.Succeeded);

                return this.Ok(resources);
            }
            catch (Exception ex)
            {
                this.RecordEvent("FindOrFilterDiscoveryTreeResourcesAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while searching and filtering discovery tree resources.");
                throw;
            }
        }
    }
}