// <copyright file="AthenaToolEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Azure.Search;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents the Athena tool entity.
    /// </summary>
    public class AthenaToolEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets table Id.
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
                this.PartitionKey = AthenaToolsTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets tool Id.
        /// </summary>
        [IsFilterable]
        public int ToolId { get; set; }

        /// <summary>
        /// Gets or sets node type Id.
        /// </summary>
        [IsFilterable]
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets security level.
        /// </summary>
        [IsFilterable]
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets keywords.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets keywords.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string KeywordsText { get; set; }

        /// <summary>
        /// Gets or sets submitter Id.
        /// </summary>
        [IsFilterable]
        public int SubmitterId { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets manufacturer.
        /// </summary>
        [IsFilterable]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets usage licensing.
        /// </summary>
        [IsFilterable]
        public string UsageLicensing { get; set; }

        /// <summary>
        /// Gets or sets user comments.
        /// </summary>
        [IsFilterable]
        public string UserComments { get; set; }

        /// <summary>
        /// Gets or sets average user rating.
        /// </summary>
        [IsFilterable]
        public int AvgUserRating { get; set; }

        /// <summary>
        /// Gets or sets user ratings.
        /// </summary>
        [IsFilterable]
        public string UserRatings { get; set; }

        /// <summary>
        /// Gets or sets website.
        /// </summary>
        [IsFilterable]
        public string Website { get; set; }

        /// <summary>
        /// Gets or sets average rating.
        /// </summary>
        [IsFilterable]
        public string AverageRating { get; set; }
    }
}
