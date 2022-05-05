// <copyright file="WelcomeCard.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    /// <summary>
    /// Represents a welcome card information.
    /// </summary>
    public class WelcomeCard
    {
        /// <summary>
        /// Gets or sets welcome card title.
        /// </summary>
        public string WelcomeCardTitle { get; set; }

        /// <summary>
        /// Gets or sets welcome card description line1.
        /// </summary>
        public string WelcomeCardContentLine1 { get; set; }

        /// <summary>
        /// Gets or sets welcome card description point 1.
        /// </summary>
        public string WelcomeCardContentPoint1 { get; set; }

        /// <summary>
        /// Gets or sets welcome card description point 2.
        /// </summary>
        public string WelcomeCardContentPoint2 { get; set; }

        /// <summary>
        /// Gets or sets welcome card description point 3.
        /// </summary>
        public string WelcomeCardContentPoint3 { get; set; }

        /// <summary>
        /// Gets or sets welcome card description line 2.
        /// </summary>
        public string WelcomeCardContentLine2 { get; set; }

        /// <summary>
        /// Gets or sets settings button text.
        /// </summary>
        public string SettingsButtonText { get; set; }

        /// <summary>
        /// Gets or sets settings url.
        /// </summary>
        public string SettingsButtonUrl { get; set; }
    }
}