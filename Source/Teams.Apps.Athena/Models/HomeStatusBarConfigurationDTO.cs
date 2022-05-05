// <copyright file="HomeStatusBarConfigurationDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Describes the home status bar configuration view model.
    /// </summary>
    public class HomeStatusBarConfigurationDTO
    {
        /// <summary>
        /// Gets or sets the team Id.
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the link label.
        /// </summary>
        [MaxLength(50)]
        public string LinkLabel { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the configuration is active on home tab.
        /// </summary>
        public bool IsActive { get; set; }
    }
}