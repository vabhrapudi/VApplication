// <copyright file="GraduationDegree.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories.GraduationDegree;

    /// <summary>
    /// Represents the graduation degree entity.
    /// </summary>
    public class GraduationDegree : TableEntity
    {
        /// <summary>
        /// Gets or sets the graduation degree Id.
        /// </summary>
        [Key]
        public string GraduationDegreeId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = GraduationDegreeTableNames.GraduationDegreePartition;
            }
        }

        /// <summary>
        /// Gets or sets the graduation degree title.
        /// </summary>
        [Required]
        public string GraduationDegreeTitle { get; set; }
    }
}