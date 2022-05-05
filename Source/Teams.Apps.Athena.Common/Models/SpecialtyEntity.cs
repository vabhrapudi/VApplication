// <copyright file="SpecialtyEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories.Specialty;

    /// <summary>
    /// Represents the specialty entity.
    /// </summary>
    public class SpecialtyEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the Specialty Id.
        /// </summary>
        [Key]
        public string SpecialtyId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = SpecialtyTableNames.SpecialtyPartition;
            }
        }

        /// <summary>
        /// Gets or sets the Specialty title.
        /// </summary>
        [Required]
        public string SpecialtyTitle { get; set; }
    }
}