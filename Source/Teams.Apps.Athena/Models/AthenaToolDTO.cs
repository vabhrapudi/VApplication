// <copyright file="AthenaToolDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;

    /// <summary>
    ///  Represents an Athena tool view model.
    /// </summary>
    public class AthenaToolDTO
    {
        /// <summary>
        /// Gets or sets table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets tool Id.
        /// </summary>
        public int ToolId { get; set; }

        /// <summary>
        /// Gets or sets node type Id.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets security level.
        /// </summary>
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets keywords.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets submitter Id.
        /// </summary>
        public int SubmitterId { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets manufacturer.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets usage licensing.
        /// </summary>
        public string UsageLicensing { get; set; }

        /// <summary>
        /// Gets or sets user comments.
        /// </summary>
        public string UserComments { get; set; }

        /// <summary>
        /// Gets or sets average user rating.
        /// </summary>
        public int AvgUserRating { get; set; }

        /// <summary>
        /// Gets or sets user ratings.
        /// </summary>
        public IEnumerable<int> UserRatings { get; set; }

        /// <summary>
        /// Gets or sets website.
        /// </summary>
        public string Website { get; set; }
    }
}
