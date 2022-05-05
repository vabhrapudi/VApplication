// <copyright file="BotSettings.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Microsoft.Teams.Athena.Models
{
    /// <summary>
    /// A class which helps to provide Bot settings for application.
    /// </summary>
    public class BotSettings
    {
        /// <summary>
        /// Gets or sets application tenant id.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets application base Uri which helps in generating customer token.
        /// </summary>
        public string AppBaseUri { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft app Id for the bot.
        /// </summary>
        public string MicrosoftAppId { get; set; }

        /// <summary>
        /// Gets or sets the Microsoft app password for the bot.
        /// </summary>
        public string MicrosoftAppPassword { get; set; }

        /// <summary>
        /// Gets or sets the number of news need to render per page.
        /// </summary>
        public int NewsPageSize { get; set; }

        /// <summary>
        /// Gets or sets the duration for which card will me maintained in cache.
        /// </summary>
        public int CardCacheDurationInHour { get; set; }

        /// <summary>
        /// Gets or sets the Athena admin team Id.
        /// </summary>
        public string AdminTeamId { get; set; }

        /// <summary>
        /// Gets or sets the duration in days for which AAD user details to be stored in memory cache.
        /// </summary>
        public int AadUserDetailsCacheDurationInDays { get; set; }

        /// <summary>
        /// Gets or sets the manifest Id of personal scope Athena app.
        /// </summary>
        public string UserManifestId { get; set; }

        /// <summary>
        /// Gets or sets the duration in minutes for which admin details to be stored in memory.
        /// </summary>
        public int AdminDetailsCacheDurationInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the duration in minutes for which keywords to be stored in memory.
        /// </summary>
        public int KeywordsCacheDurationInHours { get; set; }
    }
}