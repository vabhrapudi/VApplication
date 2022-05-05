// <copyright file="CommentsEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents an comments entity.
    /// </summary>
    public class CommentsEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the user AAD Id.
        /// </summary>
        [Key]
        public string CommentId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = CommentsTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the resource table Id.
        /// </summary>
        public string ResourceTableId { get; set; }

        /// <summary>
        /// Gets or sets the resource type Id.
        /// </summary>
        public int ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the resource comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user Id.
        /// </summary>
        public string UserId { get; set; }
    }
}