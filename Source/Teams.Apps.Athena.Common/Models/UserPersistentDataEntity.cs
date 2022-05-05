// <copyright file="UserPersistentDataEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents an user persistent data entity.
    /// </summary>
    public class UserPersistentDataEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the user AAD Id.
        /// </summary>
        [Key]
        public string UserId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = UserPersistentDataTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the saved data for discovery tree.
        /// </summary>
        public string DiscoveryTreeData { get; set; }
    }
}
