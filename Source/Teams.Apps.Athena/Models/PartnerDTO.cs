// <copyright file="PartnerDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an Athena partner view model.
    /// </summary>
    public class PartnerDTO
    {
        /// <summary>
        /// Gets or sets unique table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the partner Id.
        /// </summary>
        public int PartnerId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id of an element.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the partner's security level.
        /// </summary>
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets partner's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets partner's organization.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets partner's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets partner's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets partners's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets partners's phone.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets partners's alternate contact info.
        /// </summary>
        public string OtherContactInfo { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the element.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets partner's project.
        /// </summary>
        public string Projects { get; set; }

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
