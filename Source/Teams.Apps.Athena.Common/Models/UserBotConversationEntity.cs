// <copyright file="UserBotConversationEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents a user-Bot conversation entity.
    /// </summary>
    public class UserBotConversationEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the user AAD Id.
        /// </summary>
        [Key]
        public string UserId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = UserBotConversationTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the date and time on which Bot has installed.
        /// </summary>
        public DateTime BotInstalledOn { get; set; }

        /// <summary>
        /// Gets or sets service url for bot.
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets conversation id for bot.
        /// </summary>
        public string ConversationId { get; set; }
    }
}
