// <copyright file="IAdaptiveCardHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes helper methods related to adaptive cards.
    /// </summary>
    public interface IAdaptiveCardHelper
    {
        /// <summary>
        /// Sends welcome card in personal scope.
        /// </summary>
        /// <param name="userBotConversationDetails">The user-Bot conversation details.</param>
        /// <returns>Returns activity Id.</returns>
        Task<string> SendWelcomeCardInPersonalScope(UserBotConversationEntity userBotConversationDetails);

        /// <summary>
        /// Sends the adaptive card for newly created news article request to admin team.
        /// </summary>
        /// <param name="newsArticleDetails">The details of news article request.</param>
        /// <param name="userName">The logged-in user's name.</param>
        /// <param name="isUpdateCard">Indicates whether to update card.</param>
        /// <returns>The task representing send new news article request card operation.</returns>
        Task SendNewNewsArticleRequestCardToAdminTeamAsync(NewsEntity newsArticleDetails, string userName, bool isUpdateCard = false);

        /// <summary>
        /// Sends the adaptive card for newly created COI request to admin team.
        /// </summary>
        /// <param name="coiDetails">The details of COI request.</param>
        /// <param name="userName">The logged-in user's name.</param>
        /// <param name="isUpdateCard">Indicates whether to update card.</param>
        /// <returns>The task representing send card operation.</returns>
        Task SendNewCoiRequestCardToAdminTeamAsync(CommunityOfInterestEntity coiDetails, string userName, bool isUpdateCard = false);

        /// <summary>
        /// Sends the adaptive card for COI request to user who created the request.
        /// </summary>
        /// <param name="coiDetails">The details of COI request.</param>
        /// <param name="isUpdateCard">Indicates whether to update card.</param>
        /// <returns>The task representing send card operation.</returns>
        Task SendCoiRequestCardToCreatorAsync(CommunityOfInterestEntity coiDetails, bool isUpdateCard = false);

        /// <summary>
        /// Sends the adaptive card for news request to user who created the request.
        /// </summary>
        /// <param name="newsDetails">The details of news request.</param>
        /// <param name="isUpdateCard">Indicates whether to update card.</param>
        /// <returns>The task representing send card operation.</returns>
        Task SendNewsRequestCardToCreatorAsync(NewsEntity newsDetails, bool isUpdateCard = false);
    }
}
