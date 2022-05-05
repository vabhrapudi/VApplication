// <copyright file="CoiEntityDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Search;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Represents the COI data that to be exposed to end-user.
    /// </summary>
    public class CoiEntityDTO
    {
        private const int CoiNameMaxLength = 100;

        private const int CoiDescriptionMaxLength = 300;

        /// <summary>
        /// Gets or sets COI table Id.
        /// </summary>
        [Key]
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets COI Id.
        /// </summary>
        public int CoiId { get; set; }

        /// <summary>
        /// Gets or sets COI name.
        /// </summary>
        [Required(ErrorMessage = "The COI name is required.")]
        [MaxLength(CoiNameMaxLength, ErrorMessage = "The COI name should not exceed the length of 100 characters.")]
        public string CoiName { get; set; }

        /// <summary>
        /// Gets or sets COI team description.
        /// </summary>
        [Required(ErrorMessage = "The COI description is required.")]
        [MaxLength(CoiDescriptionMaxLength, ErrorMessage = "The COI description should not exceed the length of 300 characters.")]
        public string CoiDescription { get; set; }

        /// <summary>
        /// Gets or sets the string representation of JSON array of string which consists of keywords
        /// associated with the COI.
        /// </summary>
        [Required(ErrorMessage = "The COI keywords are required.")]
        public IEnumerable<KeywordDTO> KeywordsJson { get; set; }

        /// <summary>
        /// Gets or sets the COI team type. The type contains value of type <see cref="CoiTeamType"/>.
        /// </summary>
        [EnumDataType(typeof(CoiTeamType), ErrorMessage = "Invalid COI type provided.")]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets news article request status of type <see cref="CoiRequestStatus"/>.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether COI team has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets date time of Group creation.
        /// </summary>
        [IsSortable]
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the user Aad Id who created the request.
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the user name who created the request.
        /// </summary>
        public string CreatedByName { get; set; }

        /// <summary>
        /// Gets or sets the keyword Ids.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the team Id.
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// Gets or sets the channel Id.
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        public int SecurityLevel { get; set; }

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

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// Gets or sets last date time on which group was updated.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}
