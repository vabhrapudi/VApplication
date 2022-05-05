// <copyright file="StudyDepartmentEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Teams.Apps.Athena.Common.Repositories.StudyDepartment;

    /// <summary>
    /// Represents the study department entity.
    /// </summary>
    public class StudyDepartmentEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the Study department Id.
        /// </summary>
        [Key]
        public string StudyDepartmenId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = StudyDepartmentTableNames.StudyDepartmentPartition;
            }
        }

        /// <summary>
        /// Gets or sets the Study department title.
        /// </summary>
        [Required]
        public string StudyDepartmentTitle { get; set; }
    }
}