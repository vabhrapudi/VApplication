// <copyright file="ICommentsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for comments repository.
    /// </summary>
    public interface ICommentsRepository : IRepository<CommentsEntity>
    {
        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <param name="resourceTableId">The resource table Id.</param>
        /// <param name="resourceTypeId">The resource type Id.</param>
        /// <returns>The collection of comments.</returns>
        Task<IEnumerable<CommentsEntity>> GetCommentsByResourceTypeAsync(string resourceTableId, int resourceTypeId);
    }
}