// <copyright file="IAdaptiveCardService.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Services.AdaptiveCard
{
    using Microsoft.Bot.Schema;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods for creating adaptive cards.
    /// </summary>
    public interface IAdaptiveCardService
    {
        /// <summary>
        /// Get welcome Card.
        /// </summary>
        /// <returns>Requested welcome card attachment.</returns>
        Attachment GetWelcomeCard();

        /// <summary>
        /// Gets new news article request card attachment.
        /// </summary>
        /// <param name="newsArticleRequestDetails">The newly created news article request details.</param>
        /// <param name="createdByName">The name of user who created request.</param>
        /// <returns>The new news article request adaptive card attachment.</returns>
        Attachment GetNewNewsArticleRequestCard(NewsEntity newsArticleRequestDetails, string createdByName = null);

        /// <summary>
        /// Gets new COI request card attachment.
        /// </summary>
        /// <param name="coiRequestDetails">The newly created COI request details.</param>
        /// <param name="createdByName">The name of user whi created the request.</param>
        /// <returns>The new COI request adaptive card attachment.</returns>
        Attachment GetNewCoiRequestCard(CommunityOfInterestEntity coiRequestDetails, string createdByName = null);
    }
}