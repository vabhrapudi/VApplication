// <copyright file="AthenaNewsKeyword.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a athena news keyword.
    /// </summary>
    public class AthenaNewsKeyword
    {
        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }
    }
}