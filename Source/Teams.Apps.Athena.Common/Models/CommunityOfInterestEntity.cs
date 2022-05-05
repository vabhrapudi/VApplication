// <copyright file="CommunityOfInterestEntity.cs" company="NPS Foundation">
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
    ///  Represents a community of interest.
    /// </summary>
    public class CommunityOfInterestEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets COI Id.
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
                this.PartitionKey = CoiTableMetadata.PartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets COI Id.
        /// </summary>
        [IsFilterable]
        public int CoiId { get; set; }

        /// <summary>
        /// Gets or sets COI name.
        /// </summary>
        [IsSortable]
        [IsSearchable]
        [IsFilterable]
        public string CoiName { get; set; }

        /// <summary>
        /// Gets or sets team id.
        /// </summary>
        [IsFilterable]
        public string TeamId { get; set; }

        /// <summary>
        /// Gets or sets COI team description.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string CoiDescription { get; set; }

        /// <summary>
        /// Gets or sets COI team deep link.
        /// </summary>
        [IsFilterable]
        public string GroupLink { get; set; }

        /// <summary>
        /// Gets or sets COI image link.
        /// </summary>
        [IsFilterable]
        public string ImageLink { get; set; }

        /// <summary>
        /// Gets or sets news article request status of type <see cref="CoiRequestStatus"/>.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets discovery tree query for the COI.
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Gets or sets the keyword names added for COI.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string KeywordNames { get; set; }

        /// <summary>
        /// Gets or sets the keyword Ids added for COI.
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
        /// Gets or sets a value indicating whether to include COI in the search results if performed by end user.
        /// </summary>
        [IsFilterable]
        public bool IsIncludeInSearchResults { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether COI team has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the COI team type. The type contains value of type <see cref="CoiTeamType"/>.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets date time of Group creation.
        /// </summary>
        [IsSortable]
        [IsFilterable]
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets email address of the end user who created the group.
        /// </summary>
        [IsFilterable]
        public string CreatedByUserPrincipalName { get; set; }

        /// <summary>
        /// Gets or sets AAD Object Id of user who created group.
        /// </summary>
        [IsFilterable]
        public string CreatedByObjectId { get; set; }

        /// <summary>
        /// Gets or sets last date time on which group was updated.
        /// </summary>
        [IsFilterable]
        [IsSortable]
        public DateTime? UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the admin comment on approval or rejection.
        /// </summary>
        [IsFilterable]
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        [IsFilterable]
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the champion Ids.
        /// </summary>
        [IsFilterable]
        public string ChampionIds { get; set; }

        /// <summary>
        /// Gets or sets the contact Ids.
        /// </summary>
        [IsFilterable]
        public string ContactId { get; set; }

        /// <summary>
        /// Gets or sets the list of community members.
        /// </summary>
        [IsFilterable]
        public string CommunityMemberList { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        [IsFilterable]
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the number of members.
        /// </summary>
        [IsFilterable]
        public int NumberOfMembers { get; set; }

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

        /// <summary>
        /// Gets or sets the sponsor Ids.
        /// </summary>
        public string SponsorIds { get; set; }

        /// <summary>
        /// Gets or sets the organization.
        /// </summary>
        [IsFilterable]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        [IsFilterable]
        public string WebSite { get; set; }
    }
}