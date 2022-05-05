// <copyright file="MyCollectionsController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// My collection controller is responsible to expose API endpoints for performing CRUD operation on my collection entity.
    /// </summary>
    [Route("api/collections")]
    [ApiController]
    [Authorize]
    public class MyCollectionsController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provides the helper methods for my collection.
        /// </summary>
        private readonly IMyCollectionsHelper myCollectionsHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyCollectionsController"/> class.
        /// </summary>
        /// <param name="logger">The ILogger object which logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="myCollectionsHelper">The instance of my collection helper which helps in managing operations on my collection entity.</param>
        public MyCollectionsController(
            ILogger<MyCollectionsController> logger,
            TelemetryClient telemetryClient,
            IMyCollectionsHelper myCollectionsHelper)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.myCollectionsHelper = myCollectionsHelper;
        }

        /// <summary>
        /// Gets a collection items by collection Id.
        /// </summary>
        /// <param name="collectionId">Unique collection Id.</param>
        /// <returns>Returns collection item details.</returns>
        [HttpGet("{collectionId}")]
        public async Task<IActionResult> GetCollectionItemDetailsAsync(Guid collectionId)
        {
            this.RecordEvent("GetCollectionItemDetailsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "collectionId", collectionId.ToString() },
            });

            if (collectionId == Guid.Empty)
            {
                this.logger.LogError("Collections Id is null or invalid.");
                this.RecordEvent("GetCollectionItemDetailsAsync", RequestType.Failed);
                return this.BadRequest("Invalid collection Id.");
            }

            try
            {
                var collectionItemDetails = await this.myCollectionsHelper.GetCollectionItemsByIdAsync(collectionId.ToString());

                if (collectionItemDetails == null)
                {
                    this.RecordEvent("GetCollectionItemDetailsAsync", RequestType.Failed);
                    return this.NotFound("Collection not found.");
                }

                this.RecordEvent("GetCollectionItemDetailsAsync", RequestType.Succeeded);
                return this.Ok(collectionItemDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetCollectionItemDetailsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching collection items details.");
                throw;
            }
        }

        /// <summary>
        /// Gets a collection by collection Id.
        /// </summary>
        /// <param name="collectionId">Unique collection Id.</param>
        /// <returns>Returns collection details.</returns>
        [HttpGet("collection/{collectionId}")]
        public async Task<IActionResult> GetCollectionByIdAsync(Guid collectionId)
        {
            this.RecordEvent("GetCollectionByIdAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { "collectionId", collectionId.ToString() },
            });

            if (collectionId == Guid.Empty)
            {
                this.logger.LogError("Collections Id is null or invalid.");
                this.RecordEvent("GetCollectionByIdAsync", RequestType.Failed);
                return this.BadRequest("Invalid collection Id.");
            }

            try
            {
                var collectionItemDetails = await this.myCollectionsHelper.GetCollectionByIdAsync(collectionId.ToString());

                if (collectionItemDetails == null)
                {
                    this.RecordEvent("GetCollectionByIdAsync", RequestType.Failed);
                    return this.NotFound("Collection not found.");
                }

                this.RecordEvent("GetCollectionByIdAsync", RequestType.Succeeded);
                return this.Ok(collectionItemDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetCollectionByIdAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching collection.");
                throw;
            }
        }

        /// <summary>
        /// Gets a collection by collection Id.
        /// </summary>
        /// <returns>Returns collection details.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCollectionAsync()
        {
            this.RecordEvent("GetAllCollectionAsync", RequestType.Initiated);

            try
            {
                var userId = this.UserAadId;
                var myCollections = await this.myCollectionsHelper.GetAllCollectionsAsync(this.UserAadId);

                if (myCollections == null)
                {
                    this.RecordEvent("GetAllCollectionAsync", RequestType.Failed);
                    return this.NotFound("Collection not found.");
                }

                this.RecordEvent("GetAllCollectionAsync", RequestType.Succeeded);
                return this.Ok(myCollections);
            }
            catch (Exception ex)
            {
                this.RecordEvent("GetAllCollectionAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while fetching all collections.");
                throw;
            }
        }

        /// <summary>
        /// Create a new collection.
        /// </summary>
        /// <param name="myCollectionsCreateDetails">The details of collection to be created.</param>
        /// <returns>Returns collection details that was created.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCollectionAsync([FromBody] MyCollectionsCreateDTO myCollectionsCreateDetails)
        {
            this.RecordEvent("CreateCollectionAsync", RequestType.Initiated);

            var collectionLimit = await this.myCollectionsHelper.IsCollectionsUnderLimit(this.UserAadId);

            if (collectionLimit is false)
            {
                this.logger.LogError("Number of collections has reached its maximum limit.");
                this.RecordEvent("CreateCollectionAsync", RequestType.Failed);
                return this.BadRequest("Number of collections has reached its maximum limit.");
            }

            try
            {
                var createCollectionsDetails = await this.myCollectionsHelper.CreateCollectionAsync(myCollectionsCreateDetails, this.UserAadId);

                if (createCollectionsDetails == null)
                {
                    this.RecordEvent("CreateCollectionAsync", RequestType.Failed);
                    return this.Conflict("Unable to create collection. The possible reason is that " + " the collection with same name already exists.");
                }

                this.RecordEvent("CreateCollectionAsync", RequestType.Succeeded);

                return this.StatusCode((int)HttpStatusCode.Created, createCollectionsDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("CreateCollectionAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while creating collection.");
                throw;
            }
        }

        /// <summary>
        /// Updates collection in my collections.
        /// </summary>
        /// <param name="collectionId">Collection Id of my collection to be updated.</param>
        /// <param name="myCollectionsUpdateDetails">The details of my collection to be updated.</param>
        /// <returns>Returns NoContent HTTP status on successful operation.</returns>
        [HttpPatch("{collectionId}")]
        public async Task<IActionResult> UpdateCollectionsAsync(Guid collectionId, [FromBody] MyCollectionsUpdateDTO myCollectionsUpdateDetails)
        {
            this.RecordEvent("UpdateCollectionsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { nameof(collectionId), collectionId.ToString() },
            });

            if (collectionId == Guid.Empty)
            {
                this.logger.LogError("Collections Id is null or invalid.");
                this.RecordEvent("UpdateCollectionsAsync", RequestType.Failed);
                return this.BadRequest();
            }

            try
            {
                var collectionExistingData = await this.myCollectionsHelper.GetSingleCollectionsByIdAsync(collectionId.ToString());
                var updateCollectionDetails = await this.myCollectionsHelper.UpdateCollectionAsync(myCollectionsUpdateDetails, collectionExistingData);
                if (updateCollectionDetails == null)
                {
                    this.RecordEvent("UpdateCollectionsAsync", RequestType.Failed);
                    return this.Conflict("Unable to update collection. The possible reason is that " + " the collection with same name already exists.");
                }

                this.RecordEvent("UpdateCollectionsAsync", RequestType.Succeeded);
                return this.Ok(updateCollectionDetails);
            }
            catch (Exception ex)
            {
                this.RecordEvent("UpdateCollectionsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while updating collections.");
                throw;
            }
        }

        /// <summary>
        /// Deletes the collection by collection Id.
        /// </summary>
        /// <param name="collectionId"> Collection Id of collection to be deleted</param>
        /// <returns>Returns NoContent HTTP status on successful operation.</returns>
        [HttpDelete("{collectionId}")]
        public async Task<IActionResult> DeleteCollectionsAsync(Guid collectionId)
        {
            this.RecordEvent("DeleteCollectionsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { nameof(collectionId), collectionId.ToString() },
            });

            if (collectionId == Guid.Empty)
            {
                this.logger.LogError("Collections Id is null or invalid.");
                this.RecordEvent("DeleteCollectionsAsync", RequestType.Failed);
                return this.BadRequest();
            }

            try
            {
                var isCollectionDeleted = await this.myCollectionsHelper.DeleteCollectionAsync(collectionId.ToString());
                if (!isCollectionDeleted)
                {
                    this.RecordEvent("DeleteCollectionsAsync", RequestType.Failed);
                    return this.NotFound("collections not found.");
                }

                this.RecordEvent("DeleteCollectionsAsync", RequestType.Succeeded);
                return this.Ok();
            }
            catch (Exception ex)
            {
                this.RecordEvent("DeleteCollectionsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while deleting collections.");
                throw;
            }
        }

        /// <summary>
        /// Create a item in given collection.
        /// </summary>
        /// <param name="item">The details of item to be created.</param>
        /// <param name="collectionId">collection id of collection where item has to add.</param>
        /// <returns>Returns OK when items are added successfully.</returns>
        [HttpPost("{collectionId}")]
        public async Task<IActionResult> AddItemsAsync([FromBody] IEnumerable<Item> item, Guid collectionId)
        {
            this.RecordEvent("AddItemsAsync", RequestType.Initiated, new Dictionary<string, string>
            {
                { nameof(collectionId), collectionId.ToString() },
            });

            if (collectionId == Guid.Empty)
            {
                this.logger.LogError("Collections Id is null or invalid.");
                this.RecordEvent("AddItemsAsync", RequestType.Failed);
                return this.BadRequest();
            }

            if (item.IsNullOrEmpty())
            {
                this.logger.LogError("item is null or invalid.");
                this.RecordEvent("AddItemsAsync", RequestType.Failed);
                return this.BadRequest();
            }

            try
            {
                var isItemsAdded = await this.myCollectionsHelper.AddItemsAsync(collectionId.ToString(), item.ToList());
                if (isItemsAdded)
                {
                    return this.Ok();
                }

                return this.NotFound();
            }
            catch (Exception ex)
            {
                this.RecordEvent("AddItemsAsync", RequestType.Failed);
                this.logger.LogError(ex, "Error occurred while adding items in collections.");
                throw;
            }
        }
    }
}
