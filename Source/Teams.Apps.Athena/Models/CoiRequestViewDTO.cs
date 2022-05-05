// <copyright file="CoiRequestViewDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Holds the details of coi requests.
    /// </summary>
    public class CoiRequestViewDTO
    {
        /// <summary>
        /// Gets or sets COI table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets COI Id.
        /// </summary>
        public int CoiId { get; set; }

        /// <summary>
        /// Gets or sets COI name.
        /// </summary>
        public string CoiName { get; set; }

        /// <summary>
        /// Gets or sets COI team description.
        /// </summary>
        public string CoiDescription { get; set; }

        /// <summary>
        /// Gets or sets COI team type.
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets COI team deep link.
        /// </summary>
        public string GroupLink { get; set; }

        /// <summary>
        /// Gets or sets COI image link.
        /// </summary>
        public string ImageLink { get; set; }

        /// <summary>
        /// Gets or sets date time of Group creation.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets email address of the end user who created the group.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets AAD Object Id of user who created group.
        /// </summary>
        public string CreatedByObjectId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets approved status.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets coi request status.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the keyword Ids.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }
    }
}