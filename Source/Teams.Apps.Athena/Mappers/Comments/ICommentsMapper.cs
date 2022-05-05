// <copyright file="ICommentsMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods that manages comments entity model mappings.
    /// </summary>
    public interface ICommentsMapper
    {
        /// <summary>
        /// Gets comment model to be inserted in database.
        /// </summary>
        /// <param name="resourceTableId">Resource table Id.</param>
        /// <param name="resourceTypeId">Resource type id.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="userId">User Id.</param>
        /// <param name="userName">User name.</param>
        /// <returns>Returns a comment entity model.</returns>
        CommentsEntity MapForCreateModel(string resourceTableId, int resourceTypeId, string comment, string userId, string userName);
    }
}