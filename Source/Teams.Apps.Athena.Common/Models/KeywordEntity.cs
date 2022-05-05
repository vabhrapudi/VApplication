// <copyright file="KeywordEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Search;

    /// <summary>
    /// Represents the keywords entity.
    /// </summary>
    public class KeywordEntity
    {
        /// <summary>
        /// Gets or sets the keyword Id.
        /// </summary>
        [Key]
        [IsSortable]
        [IsFilterable]
        public string KeywordId { get; set; }

        /// <summary>
        /// Gets or sets the keyword title.
        /// </summary>
        [IsSearchable]
        [IsSortable]
        [IsFilterable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the keyword synonyms.
        /// </summary>
        [IsSearchable]
        [IsSortable]
        public string Synonyms { get; set; }

        /// <summary>
        /// Gets or sets the parent node ID.
        /// </summary>
        [IsSortable]
        public string ParentNode { get; set; }
    }
}
