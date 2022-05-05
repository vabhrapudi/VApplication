// <copyright file="AdminApprovalDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Holds the coi/news approval request.
    /// </summary>
    public class AdminApprovalDTO
    {
        /// <summary>
        /// Gets or sets request Id.
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
        public List<string> RequestIds { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        /// <summary>
        /// Gets or sets item type.
        /// </summary>
        public int ItemType { get; set; }

        /// <summary>
        /// Gets or sets comment for approval or rejection.
        /// </summary>
        public string Comment { get; set; }
    }
}