// <copyright file="PartnerEntity.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Azure.Search;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Represents an partners entity.
    /// </summary>
    public class PartnerEntity : TableEntity
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
                this.PartitionKey = PartnersTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the partner Id.
        /// </summary>
        [IsFilterable]
        public int PartnerId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id of an element.
        /// </summary>
        [IsFilterable]
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the partner's security level.
        /// </summary>
        [IsFilterable]
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets partner's description.
        /// </summary>
        [IsFilterable]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets partner's organization.
        /// </summary>
        [IsFilterable]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets partner's first name.
        /// </summary>
        [IsSearchable]
        [IsSortable]
        [IsFilterable]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets partner's last name.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets partners's title.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets partners's phone.
        /// </summary>
        [IsFilterable]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets partners's alternate contact info.
        /// </summary>
        [IsFilterable]
        public string OtherContactInfo { get; set; }

        /// <summary>
        /// Gets or sets the keywords associated with the element.
        /// </summary>
        [IsFilterable]
        [IsSearchable]
        public string Keywords { get; set; }

        /// <summary>
        /// Gets or sets keyword text separated by semicolon character.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string KeywordsText { get; set; }

        /// <summary>
        /// Gets or sets partner's project.
        /// </summary>
        [IsFilterable]
        public string Projects { get; set; }

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
