// <copyright file="CommentsHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Mappers;

    /// <summary>
    /// The helper methods related to research project's comments.
    /// </summary>
    public class CommentsHelper : ICommentsHelper
    {
        /// <summary>
        /// The instance of comments entity model repository.
        /// </summary>
        private readonly ICommentsRepository commentsRepository;

        /// <summary>
        /// The instance of comments mapper.
        /// </summary>
        private readonly ICommentsMapper commentsMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsHelper"/> class.
        /// </summary>
        /// <param name="commentsRepository">The instance of the <see cref="CommentsRepository"/> class.</param>
        /// <param name="commentsMapper">The instance of the <see cref="CommentsMapper"/> class.</param>
        public CommentsHelper(
            ICommentsRepository commentsRepository,
            ICommentsMapper commentsMapper)
        {
            this.commentsRepository = commentsRepository;
            this.commentsMapper = commentsMapper;
        }

        /// <inheritdoc/>
        public async Task<CommentsEntity> AddCommentAsync(string resourceTableId, int resourceTypeId, string comment, string userId, string userName)
        {
            var commentEntity = this.commentsMapper.MapForCreateModel(resourceTableId, resourceTypeId, comment, userId, userName);
            var createdComment = await this.commentsRepository.CreateOrUpdateAsync(commentEntity);
            return createdComment;
        }

        /// <inheritdoc/>
        public Task<IEnumerable<CommentsEntity>> GetResourceComments(string resourceTableId, int resourceTypeId)
        {
            return this.commentsRepository.GetCommentsByResourceTypeAsync(resourceTableId, resourceTypeId);
        }
    }
}