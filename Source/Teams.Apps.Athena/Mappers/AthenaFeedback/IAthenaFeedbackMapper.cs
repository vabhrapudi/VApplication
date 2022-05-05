// <copyright file="IAthenaFeedbackMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related of athena feedbacks.
    /// </summary>
    public interface IAthenaFeedbackMapper
    {
        /// <summary>
        /// maps athena feedback create DTO to athena feedback entity.
        /// </summary>
        /// <param name="athenaFeedbackCreateDTO">Feedback details submitted by user.</param>
        /// <param name="userAadObjectId">Loggin in user's unique object identifier.</param>
        /// <returns>The feedback entity model.</returns>
        AthenaFeedbackEntity MapForCreateModel(AthenaFeedbackCreateDTO athenaFeedbackCreateDTO, string userAadObjectId);

        /// <summary>
        /// Maps athena feedback entity model to athena feedback view model.
        /// </summary>
        /// <param name="athenaFeedbackEntity">The athena feedback entity model.</param>
        /// <param name="userDetails">The user details.</param>
        /// <returns>The athena feedback entity view model.</returns>
        AthenaFeedbackViewDTO MapForViewModel(AthenaFeedbackEntity athenaFeedbackEntity, UserDetails userDetails);
    }
}
