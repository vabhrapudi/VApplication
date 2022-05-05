// <copyright file="AdaptiveCardSubmitActionData.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the adaptive card data when submit action get performed on adaptive card.
    /// </summary>
    public class AdaptiveCardSubmitActionData
    {
        /// <summary>
        /// Gets or sets the Bot command.
        /// </summary>
        [JsonProperty("command")]
        public string BotCommand { get; set; }

        /// <summary>
        /// Gets or sets the news Id.
        /// </summary>
        [JsonProperty("newsTableId")]
        public string NewsTableId { get; set; }

        /// <summary>
        /// Gets or sets the COI Id.
        /// </summary>
        [JsonProperty("coiTableId")]
        public string CoiTableId { get; set; }
    }
}
