// <copyright file="HomeConfigurationArticleDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Describes the home tab configuration view model.
    /// </summary>
    public class HomeConfigurationArticleDTO
    {
        /// <summary>
        /// Gets or sets the article Id.
        /// </summary>
        public string ArticleId { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [Required]
        [MaxLength(75)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets image URL.
        /// </summary>
        [Url]
        [Required]
        public string ImageUrl { get; set; }
    }
}
