// <copyright file="IFeedbackHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models.Enums;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Interface for exposing methods related to user feedbacks.
    /// </summary>
    public interface IFeedbackHelper
    {
        /// <summary>
        /// Save user feedback on Athena.
        /// </summary>
        /// <param name="athenaFeedbackCreateDTO">Feedback details submitted by user.</param>
        /// <param name="userAadObjectId">Loggin in user's unique object identifier.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task SaveAthenaFeedback(AthenaFeedbackCreateDTO athenaFeedbackCreateDTO, string userAadObjectId);

        /// <summary>
        /// Gets user feedbacks related to Athena.
        /// </summary>
        /// <param name="pageNumber">Page number for which feedbacks needs to be fetched..</param>
        /// <param name="sortBy">Represents 0 for recent, 1 for category and 2 for feedback type for feedback. Refer <see cref="AthenaFeedbackSortByItems"/> for values.</param>
        /// <param name="feedbackFilterValues">The values to filter feedbacks by feedback types.</param>
        /// <returns>List of feedbacks.</returns>
        Task<IEnumerable<AthenaFeedbackViewDTO>> GetAthenaFeedbacksAsync(int pageNumber, int sortBy, IEnumerable<int> feedbackFilterValues);

        /// <summary>
        /// Gets user feedback details by feedback Id.
        /// </summary>
        /// <param name="feedbackId">The feedback Id.</param>
        /// <returns>Feedback details.</returns>
        Task<AthenaFeedbackViewDTO> GetAthenaFeedbackDetailsAsync(string feedbackId);
    }
}