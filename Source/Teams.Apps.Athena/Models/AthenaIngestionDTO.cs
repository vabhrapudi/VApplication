// <copyright file="AthenaIngestionDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;

    /// <summary>
    /// Represents the Athena ingestion DTO.
    /// </summary>
    public class AthenaIngestionDTO
    {
        /// <summary>
        /// Gets or sets the db entity.
        /// </summary>
        public string DbEntity { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string Filepath { get; set; }

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
