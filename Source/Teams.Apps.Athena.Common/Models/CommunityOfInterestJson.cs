// <copyright file="CommunityOfInterestJson.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents CommunityOfInterest Json.
    /// </summary>
    public class CommunityOfInterestJson
    {
        /// <summary>
        /// Gets or sets unique table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the community Id.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int CoiId { get; set; }

        /// <summary>
        /// Gets or sets the parent Id of an element.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the community's security level.
        /// </summary>
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets community's title.
        /// </summary>
        public string CoiName { get; set; }

        /// <summary>
        /// Gets or sets community's description.
        /// </summary>
        public string CoiDescription { get; set; }

        /// <summary>
        /// Gets or sets champion Id.
        /// </summary>
        public string ChampionIds { get; set; }

        /// <summary>
        /// Gets or sets community's contact Id.
        /// </summary>
        public string ContactId { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the element.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the KeywordsText.
        /// </summary>
        public string KeywordsText { get; set; }

        /// <summary>
        /// Gets or sets the list of community members.
        /// </summary>
        public string CommunityMemberList { get; set; }

        /// <summary>
        /// Gets or sets the list of community members.
        /// </summary>
        public int NumberOfMembers { get; set; }

        /// <summary>
        /// Gets or sets the community founded date and time.
        /// </summary>
        public DateTime DateFounded { get; set; }

        /// <summary>
        /// Gets or sets the last updated date and time.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the sponsor Ids.
        /// </summary>
        public IEnumerable<int> SponsorIds { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        public string WebSite { get; set; }
    }
}
