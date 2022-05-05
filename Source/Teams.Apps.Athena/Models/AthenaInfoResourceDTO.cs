// <copyright file="AthenaInfoResourceDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an Athena info resources view model.
    /// </summary>
    public class AthenaInfoResourceDTO
    {
        /// <summary>
        /// Gets or sets table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the Info resource Id.
        /// </summary>
        public int InfoResourceId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the collection of keyword Ids.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the last updated date and time.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets title of info resource.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets authors.
        /// </summary>
        public string Authors { get; set; }

        /// <summary>
        /// Gets or sets the collection author Ids.
        /// </summary>
        public IEnumerable<int> AuthorIds { get; set; }

        /// <summary>
        /// Gets or sets the sponsors.
        /// </summary>
        public string Sponsors { get; set; }

        /// <summary>
        /// Gets or sets the collection sponsor Ids.
        /// </summary>
        public IEnumerable<int> SponsorIds { get; set; }

        /// <summary>
        /// Gets or sets the info resource published date.
        /// </summary>
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Gets or sets the provenance.
        /// </summary>
        public string Provenance { get; set; }

        /// <summary>
        /// Gets or sets the submitter Id.
        /// </summary>
        public int SubmitterId { get; set; }

        /// <summary>
        /// Gets or sets the collection.
        /// </summary>
        public string Collection { get; set; }

        /// <summary>
        /// Gets or sets the research resource Id.
        /// </summary>
        public int ResearchSourceId { get; set; }

        /// <summary>
        /// Gets or sets the source organization.
        /// </summary>
        public string SourceOrg { get; set; }

        /// <summary>
        /// Gets or sets the source group.
        /// </summary>
        public string SourceGroup { get; set; }

        /// <summary>
        /// Gets or sets the is part series.
        /// </summary>
        public string IsPartOfSeries { get; set; }

        /// <summary>
        /// Gets or sets the URL to website.
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets the Doc Id.
        /// </summary>
        public string DocId { get; set; }

        /// <summary>
        /// Gets or sets the user licensing.
        /// </summary>
        public string UsageLicensing { get; set; }

        /// <summary>
        /// Gets or sets the user comments.
        /// </summary>
        public string UserComments { get; set; }

        /// <summary>
        /// Gets or sets average rating for resource.
        /// </summary>
        public int AvgUserRating { get; set; }
    }
}
