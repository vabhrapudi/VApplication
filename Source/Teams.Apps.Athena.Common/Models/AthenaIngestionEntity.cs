// <copyright file="AthenaIngestionEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents an Athena ingestion entity.
    /// </summary>
    public class AthenaIngestionEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the db entity.
        /// </summary>
        [Key]
        public string DbEntity
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = AthenaIngestionTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        public int Frequency { get; set; }
    }
}
