// <copyright file="ICommentsHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The helper methods provider for comments.
    /// </summary>
    public interface ICommentsHelper
    {
        /// <summary>
        /// Gets the comments.
        /// </summary>
        /// <param name="resourceTableId">The resource table Id.</param>
        /// <param name="resourceTypeId">The resource type Id.</param>
        /// <returns>The collection of comments.</returns>
        Task<IEnumerable<CommentsEntity>> GetResourceComments(string resourceTableId, int resourceTypeId);

        /// <summary>
        /// Add comments.
        /// </summary>
        /// <param name="resourceTableId">The resource table id.</param>
        /// <param name="resourceTypeId">The resource type Id.</param>
        /// <param name="comment">Comment.</param>
        /// <param name="userId">User Id.</param>
        /// <param name="userName">User name.</param>
        /// <returns>The comment entity.</returns>
        Task<CommentsEntity> AddCommentAsync(string resourceTableId, int resourceTypeId, string comment, string userId, string userName);
    }
}