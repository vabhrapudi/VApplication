// <copyright file="SecurityLevels.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Represents a security level items.
    /// </summary>
    public class SecurityLevels
    {
        /// <summary>
        /// Gets or sets the security Id.
        /// </summary>
        public int SecurityId { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets notes.
        /// </summary>
        public string Notes { get; set; }
    }
}