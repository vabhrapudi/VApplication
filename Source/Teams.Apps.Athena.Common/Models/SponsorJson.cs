// <copyright file="SponsorJson.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an sponsor Json.
    /// </summary>
    public class SponsorJson
    {
        /// <summary>
        /// Gets or sets unique table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the sponsor Id.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public int SponsorId { get; set; }

        /// <summary>
        /// Gets or sets the parent Id of an element.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the sponsor's security level.
        /// </summary>
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets sponsor's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets sponsor's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets sponsor's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets sponsor's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets sponsor's organization.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets sponsor's service.
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets sponsor's phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets sponsor's alternate contact info.
        /// </summary>
        public string OtherContactInfo { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the element.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets keyword text separated by semicolon character.
        /// </summary>
        public string KeywordsText { get; set; }

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
