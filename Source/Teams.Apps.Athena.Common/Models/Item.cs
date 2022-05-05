// <copyright file="Item.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Represents item.
    /// </summary>
    public class Item : TableEntity
    {
        /// <summary>
        /// Gets or sets item id.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets item type.
        /// </summary>
        public int ItemType { get; set; }
    }
}
