// <copyright file="OrganizationEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories.Organization;

    /// <summary>
    /// Represents the organization entity.
    /// </summary>
    public class OrganizationEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the Organization Id.
        /// </summary>
        [Key]
        public string OrganizationId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = OrganizationTableNames.OrganizationPartition;
            }
        }

        /// <summary>
        /// Gets or sets the Organization title.
        /// </summary>
        [Required]
        public string OrganizationTitle { get; set; }
    }
}