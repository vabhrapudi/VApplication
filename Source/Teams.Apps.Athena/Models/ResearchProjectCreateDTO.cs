// <copyright file="ResearchProjectCreateDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents the research project create DTO.
    /// </summary>
    public class ResearchProjectCreateDTO
    {
        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        public IEnumerable<KeywordDTO> KeywordsJson { get; set; }

        /// <summary>
        /// Gets or sets title of research project.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the abstract of research project.
        /// </summary>
        public string Abstract { get; set; }
    }
}