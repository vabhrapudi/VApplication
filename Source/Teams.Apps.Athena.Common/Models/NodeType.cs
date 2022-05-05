// <copyright file="NodeType.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    /// <summary>
    /// Represents a node type items.
    /// </summary>
    public class NodeType
    {
        /// <summary>
        /// Gets or sets the filter Id.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets json file.
        /// </summary>
        public string JsonFile { get; set; }

        /// <summary>
        /// Gets or sets notes.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets date field.
        /// </summary>
        public string DateFieldName { get; set; }

        /// <summary>
        /// Gets or sets icon.
        /// </summary>
        public string Icon { get; set; }
    }
}