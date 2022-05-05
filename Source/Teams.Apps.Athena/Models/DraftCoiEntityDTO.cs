// <copyright file="DraftCoiEntityDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Represents the draft COI data that to be exposed to end-user.
    /// </summary>
    public class DraftCoiEntityDTO
    {
        private const int CoiNameMaxLength = 100;

        private const int CoiDescriptionMaxLength = 300;

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
        [Required(ErrorMessage = "The COI name is required.")]
        [MaxLength(CoiNameMaxLength, ErrorMessage = "The COI name should not exceed the length of 100 characters.")]
        public string CoiName { get; set; }

        /// <summary>
        /// Gets or sets COI team description.
        /// </summary>
        [MaxLength(CoiDescriptionMaxLength, ErrorMessage = "The COI description should not exceed the length of 300 characters.")]
        public string CoiDescription { get; set; }

        /// <summary>
        /// Gets or sets the string representation of JSON array of string which consists of keywords
        /// associated with the COI.
        /// </summary>
        public IEnumerable<KeywordDTO> KeywordsJson { get; set; }

        /// <summary>
        /// Gets or sets the COI team type. The type contains value of type <see cref="CoiTeamType"/>.
        /// </summary>
        [EnumDataType(typeof(CoiTeamType), ErrorMessage = "Invalid COI type provided.")]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets news article request status of type <see cref="CoiRequestStatus"/>.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether COI team has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets date time of Group creation.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
