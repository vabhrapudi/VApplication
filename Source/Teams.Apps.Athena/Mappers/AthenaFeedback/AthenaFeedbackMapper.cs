// <copyright file="AthenaFeedbackMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides mapper methods for athena feedback model mappings.
    /// </summary>
    public class AthenaFeedbackMapper : IAthenaFeedbackMapper
    {
        /// <inheritdoc/>
        public AthenaFeedbackEntity MapForCreateModel(AthenaFeedbackCreateDTO athenaFeedbackCreateDTO, string userAadObjectId)
        {
            athenaFeedbackCreateDTO = athenaFeedbackCreateDTO ?? throw new ArgumentNullException(nameof(athenaFeedbackCreateDTO));
            if (string.IsNullOrEmpty(userAadObjectId))
            {
                throw new ArgumentNullException(nameof(userAadObjectId));
            }

            return new AthenaFeedbackEntity
            {
                FeedbackId = Guid.NewGuid().ToString(),
                Details = athenaFeedbackCreateDTO.Details,
                Feedback = (int)athenaFeedbackCreateDTO.Feedback,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userAadObjectId,
                Category = (int)athenaFeedbackCreateDTO.Category,
                Type = (int)athenaFeedbackCreateDTO.Type,
            };
        }

        /// <inheritdoc/>
        public AthenaFeedbackViewDTO MapForViewModel(AthenaFeedbackEntity athenaFeedbackEntity, UserDetails userDetails)
        {
            athenaFeedbackEntity = athenaFeedbackEntity ?? throw new ArgumentNullException(nameof(athenaFeedbackEntity));

            return new AthenaFeedbackViewDTO
            {
                FeedbackId = athenaFeedbackEntity.FeedbackId,
                Details = athenaFeedbackEntity.Details,
                Feedback = athenaFeedbackEntity.Feedback,
                CreatedAt = athenaFeedbackEntity.CreatedAt,
                CreatedBy = userDetails,
                Category = athenaFeedbackEntity.Category,
                Type = athenaFeedbackEntity.Type,
            };
        }
    }
}
