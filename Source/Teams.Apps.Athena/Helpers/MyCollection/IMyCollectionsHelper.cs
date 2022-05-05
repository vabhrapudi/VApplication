// <copyright file="IMyCollectionsHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing my collection entity.
    /// </summary>
    public interface IMyCollectionsHelper
    {
        /// <summary>
        /// Gets a my collection items by Id.
        /// </summary>
        /// <param name="collectionId">The collection Id of the collection to fetch.</param>
        /// <returns>Returns list of my collection entity item details.</returns>
        Task<IEnumerable<MyCollectionsItemsViewDTO>> GetCollectionItemsByIdAsync(string collectionId);

        /// <summary>
        /// Gets a my collection by Id.
        /// </summary>
        /// <param name="collectionId">The collection Id of the collection to fetch.</param>
        /// <returns>Returns my collection entity details.</returns>
        Task<MyCollectionsViewDTO> GetCollectionByIdAsync(string collectionId);

        /// <summary>
        /// Gets a my collection details by Id.
        /// </summary>
        /// <param name="collectionId">Get collection id of my collection.</param>
        /// <returns>Returns my collection view DTO details.</returns>
        Task<MyCollectionsEntity> GetSingleCollectionsByIdAsync(string collectionId);

        /// <summary>
        /// Creates a new my collection entity.
        /// </summary>
        /// <param name="myCollectionsCreateDTO">The user details.</param>
        /// <param name="userId">The user Id.</param>
        /// <returns>Returns my collection view details</returns>
        Task<MyCollectionsViewDTO> CreateCollectionAsync(MyCollectionsCreateDTO myCollectionsCreateDTO, string userId);

        /// <summary>
        /// Updates my collection entity.
        /// </summary>
        /// <param name="myCollectionsUpdateDTO">The my collection details that need to be updated.</param>
        /// <param name="myCollectionsEntity">Existing my collection details</param>
        /// <returns>Return my collection updated details.</returns>
        Task<MyCollectionsViewDTO> UpdateCollectionAsync(MyCollectionsUpdateDTO myCollectionsUpdateDTO, MyCollectionsEntity myCollectionsEntity);

        /// <summary>
        /// Deletes collection by collection id.
        /// </summary>
        /// <param name="collectionId"> Collection Id of my collection to be deleted</param>
        /// <returns> returns bool value</returns>
        Task<bool> DeleteCollectionAsync(string collectionId);

        /// <summary>
        /// Add items in collection.
        /// </summary>
        /// <param name="collectionId"> Collection Id of my collection where item has to be added.</param>
        /// <param name="items">List of items.</param>
        /// <returns> Returns true if items are added successfully, else false.</returns>
        Task<bool> AddItemsAsync(string collectionId, List<Item> items);

        /// <summary>
        /// Checks the number of collections per user.
        /// </summary>
        /// <param name="userId"> The user Id</param>
        /// <returns> Returns true if collections are under limit, else return false.</returns>
        Task<bool> IsCollectionsUnderLimit(string userId);

        /// <summary>
        /// Gets a my collection details.
        /// </summary>
        /// <param name="userId"> The user Id</param>
        /// <returns>Returns list of collection entity.</returns>
        Task<IEnumerable<MyCollectionsViewDTO>> GetAllCollectionsAsync(string userId);
    }
}
