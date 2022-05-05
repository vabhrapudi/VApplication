// <copyright file="NewsFilterParametersDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// This class is responsible to store the news filter parameters.
    /// </summary>
    public class NewsFilterParametersDTO
    {
        /// <summary>
        /// Gets or sets the array of keywords.
        /// </summary>
        public IEnumerable<KeywordEntity> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the array of news types.
        /// </summary>
        public IEnumerable<NodeType> NewsTypes { get; set; }
    }
}
