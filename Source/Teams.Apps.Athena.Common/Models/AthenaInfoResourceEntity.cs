// <copyright file="AthenaInfoResourceEntity.cs" company="NPS Foundation">
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
    /// Represents the info resource entity.
    /// </summary>
    public class AthenaInfoResourceEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the info resource table Id.
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
                this.PartitionKey = AthenaInfoResourcesTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the Info resource Id.
        /// </summary>
        [IsFilterable]
        public int InfoResourceId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        [IsFilterable]
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        [IsFilterable]
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the space separated string of keywords Ids.
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
        /// Gets or sets the last updated date and time.
        /// </summary>
        [IsFilterable]
        [IsSortable]
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets title of info resource.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets authors.
        /// </summary>
        [IsFilterable]
        public string Authors { get; set; }

        /// <summary>
        /// Gets or sets the author Ids.
        /// </summary>
        [IsFilterable]
        public string AuthorIds { get; set; }

        /// <summary>
        /// Gets or sets the sponsors.
        /// </summary>
        [IsFilterable]
        public string Sponsors { get; set; }

        /// <summary>
        /// Gets or sets the sponsor Ids.
        /// </summary>
        [IsFilterable]
        public string SponsorIds { get; set; }

        /// <summary>
        /// Gets or sets the info resource published date.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        [IsFilterable]
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the provenance.
        /// </summary>
        [IsFilterable]
        public string Provenance { get; set; }

        /// <summary>
        /// Gets or sets the submitter Id.
        /// </summary>
        [IsFilterable]
        public int SubmitterId { get; set; }

        /// <summary>
        /// Gets or sets the collection.
        /// </summary>
        [IsFilterable]
        public string Collection { get; set; }

        /// <summary>
        /// Gets or sets the research resource Id.
        /// </summary>
        [IsFilterable]
        public int ResearchSourceId { get; set; }

        /// <summary>
        /// Gets or sets the source organization.
        /// </summary>
        [IsFilterable]
        public string SourceOrg { get; set; }

        /// <summary>
        /// Gets or sets the source group.
        /// </summary>
        [IsFilterable]
        public string SourceGroup { get; set; }

        /// <summary>
        /// Gets or sets the is part series.
        /// </summary>
        [IsFilterable]
        public string IsPartOfSeries { get; set; }

        /// <summary>
        /// Gets or sets the URL to website.
        /// </summary>
        [IsFilterable]
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the Doc Id.
        /// </summary>
        [IsFilterable]
        public string DocId { get; set; }

        /// <summary>
        /// Gets or sets the user licensing.
        /// </summary>
        [IsFilterable]
        public string UsageLicensing { get; set; }

        /// <summary>
        /// Gets or sets the user comments.
        /// </summary>
        [IsFilterable]
        public string UserComments { get; set; }

        /// <summary>
        /// Gets or sets average rating for resource.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public int AvgUserRating { get; set; }
    }
}
