// <copyright file="TeamEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents the Microsoft Teams details where APP is installed.
    /// </summary>
    public class TeamEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets team Id.
        /// </summary>
        [Key]
        public string TeamId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = TeamTableMetadata.TeamPartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the date and time on which Bot has installed.
        /// </summary>
        public DateTime BotInstalledOn { get; set; }

        /// <summary>
        /// Gets or sets service URL for Bot.
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets conversation Id for Bot.
        /// </summary>
        public string ConversationId { get; set; }

        /// <summary>
        /// Gets or sets the group Id.
        /// </summary>
        public string GroupId { get; set; }
    }
}