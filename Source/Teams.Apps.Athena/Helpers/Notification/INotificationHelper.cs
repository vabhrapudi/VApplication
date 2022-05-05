// <copyright file="INotificationHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Threading.Tasks;
    using Microsoft.Bot.Schema;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes the operations related to Bot notifications.
    /// </summary>
    public interface INotificationHelper
    {
        /// <summary>
        /// Sends the notification in personal scope.
        /// </summary>
        /// <param name="userBotConversation">The user-bot conversation details.</param>
        /// <param name="card">The card to be send to user.</param>
        /// <param name="isUpdateCard">Indicated whether to update the card.</param>
        /// <param name="activityId">The activity Id if card needs to be updated.</param>
        /// <returns>An asynchronous task.</returns>
        Task<string> SendNotificationInPersonalScopeAsync(Conversation userBotConversation, Attachment card, bool isUpdateCard = false, string activityId = null);

        /// <summary>
        /// Sends the notification in team scope.
        /// </summary>
        /// <param name="teamDetails">The Microsoft Teams team details to which notification to send.</param>
        /// <param name="card">The card to be send.</param>
        /// <param name="isUpdateCard">Indicated whether to update the card.</param>
        /// <param name="activityId">The activity Id if card needs to be updated.</param>
        /// <returns>An asynchronous task.</returns>
        Task<string> SendNotificationInTeamScopeAsync(TeamEntity teamDetails, Attachment card, bool isUpdateCard = false, string activityId = null);
    }
}
