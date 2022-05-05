// <copyright file="IMyCollectionsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for collection data repository.
    /// </summary>
    public interface IMyCollectionsRepository : IRepository<MyCollectionsEntity>
    {
        /// <summary>
        /// Gets all collections of particular user.
        /// </summary>
        /// <param name="userAadId">The user AAD Id of which news articles to get.</param>
        /// <returns>The collection of my collections.</returns>
        Task<IEnumerable<MyCollectionsEntity>> GetCollectionsByUserIdAsync(string userAadId);

        /// <summary>
        /// Validates whether a collection with same name already exists.
        /// </summary>
        /// <param name="name">The collection name.</param>
        /// <returns>Returns true if the collection with same name already exists. Else returns false.</returns>
        Task<MyCollectionsEntity> GetMyCollectionAsync(string name);

        /// <summary>
        /// Validates whether a collection with same name already exists where id is not equal to current id.
        /// </summary>
        /// <param name="name">The collection name.</param>
        /// <param name="collectionId">The collection id</param>
        /// <returns>Returns true if the collection with same name already exists. Else returns false.</returns>
        Task<MyCollectionsEntity> GetMyCollectionAsync(string name, string collectionId);
    }
}