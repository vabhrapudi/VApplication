// <copyright file="EventDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describes the Athena event view model.
    /// </summary>
    public class EventDTO
    {
        /// <summary>
        /// Gets or sets unique table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the event Id.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the event's security level.
        /// </summary>
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the event.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the date and time when event details get updated.
        /// </summary>
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the date of event.
        /// </summary>
        public DateTime? DateOfEvent { get; set; }

        /// <summary>
        /// Gets or sets the event title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the location of event.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// Gets or sets the other contact information.
        /// </summary>
        public string OtherContactInfo { get; set; }

        /// <summary>
        /// Gets or sets sum of ratings given by users.
        /// </summary>
        public int SumOfRatings { get; set; }

        /// <summary>
        /// Gets or sets number of end-users who submitted the rating.
        /// </summary>
        public int NumberOfRatings { get; set; }

        /// <summary>
        /// Gets or sets rating of a research proposal given by user.
        /// </summary>
        public decimal UserRating { get; set; }
    }
}
