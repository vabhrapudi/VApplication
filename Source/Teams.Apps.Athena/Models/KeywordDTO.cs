// <copyright file="KeywordDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Search;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a view model for keywords entity.
    /// </summary>
    public class KeywordDTO
    {
        /// <summary>
        /// Gets or sets the keyword Id.
        /// </summary>
        [Key]
        public string KeywordId { get; set; }

        /// <summary>
        /// Gets or sets the keyword name.
        /// </summary>
        [Required]
        [IsSearchable]
        [IsSortable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the synonyms.
        /// </summary>
        public string Synonyms { get; set; }

        /// <summary>
        /// Gets or sets parent node.
        /// </summary>
        public string ParentNode { get; set; }
    }
}
