// <copyright file="ActivityEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Represents the adaptive card activity.
    /// </summary>
    public class ActivityEntity : TableEntity
    {
        private int itemType;
        private string itemId;

        /// <summary>
        /// Gets or sets the activity Id.
        /// </summary>
        public string ActivityId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the user Id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the team Id.
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// Gets or sets the item type.
        /// </summary>
        public int ItemType
        {
            get
            {
                return this.itemType;
            }

            set
            {
                this.itemType = value;
                this.PartitionKey = $"{this.itemId}${value}";
            }
        }

        /// <summary>
        /// Gets or sets the item Id.
        /// </summary>
        public string ItemId
        {
            get
            {
                return this.itemId;
            }

            set
            {
                this.itemId = value;
                this.PartitionKey = $"{value}${this.itemType}";
            }
        }
    }
}
