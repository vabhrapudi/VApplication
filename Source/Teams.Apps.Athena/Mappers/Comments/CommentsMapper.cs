// <copyright file="CommentsMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// A model class that contains methods related to comment entity model mappings.
    /// </summary>
    public class CommentsMapper : ICommentsMapper
    {
        /// <inheritdoc/>
        public CommentsEntity MapForCreateModel(string resourceTableId, int resourceTypeId, string comment, string userId, string userName)
        {
            return new CommentsEntity
            {
                CommentId = Guid.NewGuid().ToString(),
                ResourceTableId = resourceTableId,
                ResourceTypeId = resourceTypeId,
                Comment = comment,
                UserId = userId,
                UserName = userName,
            };
        }
    }
}