// <copyright file="Conversation.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;

    /// <summary>
    /// Represents the conversation between user and Bot.
    /// </summary>
    public class Conversation
    {
        /// <summary>
        /// Gets or sets the user AAD Id.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets Id of conversation between user and Bot.
        /// </summary>
        public string ConversationId { get; set; }

        /// <summary>
        /// Gets or sets service URL.
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets date and time on which Bot was installed for user.
        /// </summary>
        public DateTime BotInstalledOn { get; set; }
    }
}
