// <copyright file="EventEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Azure.Search;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents an events entity.
    /// </summary>
    public class EventEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets unique table Id.
        /// </summary>
        [Key]
        public string TableId
        {
            get
            {
                return this.RowKey;
            }

            set
            {
                this.RowKey = value;
                this.PartitionKey = EventsTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the event Id.
        /// </summary>
        [IsFilterable]
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        [IsFilterable]
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the event's security level.
        /// </summary>
        [IsFilterable]
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the event.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets keyword text separated by semicolon character.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string KeywordsText { get; set; }

        /// <summary>
        /// Gets or sets the date and time when event details get updated.
        /// </summary>
        [IsFilterable]
        [IsSortable]
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the date of event.
        /// </summary>
        [IsFilterable]
        public DateTime? DateOfEvent { get; set; }

        /// <summary>
        /// Gets or sets the event title.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of event.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        [IsFilterable]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the location of event.
        /// </summary>
        [IsFilterable]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        [IsFilterable]
        public string WebSite { get; set; }

        /// <summary>
        /// Gets or sets the other contact information.
        /// </summary>
        [IsFilterable]
        public string OtherContactInfo { get; set; }

        /// <summary>
        /// Gets or sets the sum of ratings submitted by end-users.
        /// </summary>
        public int SumOfRatings { get; set; }

        /// <summary>
        /// Gets or sets the number of end-users who submitted the rating.
        /// </summary>
        public int NumberOfRatings { get; set; }

        /// <summary>
        /// Gets or sets average rating for research proposal.
        /// </summary>
        public string AverageRating { get; set; }
    }
}
