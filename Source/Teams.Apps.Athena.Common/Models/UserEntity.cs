// <copyright file="UserEntity.cs" company="NPS Foundation">
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
    /// Represents an user entity.
    /// </summary>
    public class UserEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the user table Id.
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
                this.PartitionKey = UserTableMetadata.UserPartitionKey;
            }
        }

        /// <summary>
        /// Gets or sets the user AAD Id.
        /// </summary>
        [IsFilterable]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the external user Id.
        /// </summary>
        [IsFilterable]
        public int ExternalUserId { get; set; }

        /// <summary>
        /// Gets or sets user type whether the user is registered or external.
        /// </summary>
        [IsFilterable]
        public int UserType { get; set; }

        /// <summary>
        /// Gets or sets user's first name.
        /// </summary>
        [Required]
        [MaxLength(25)]
        [IsSortable]
        [IsSearchable]
        [IsFilterable]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets user's middle name.
        /// </summary>
        [Required]
        [MaxLength(25)]
        [IsSearchable]
        [IsFilterable]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets user's last name.
        /// </summary>
        [Required]
        [MaxLength(25)]
        [IsSearchable]
        [IsFilterable]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's job_title.
        /// </summary>
        [Required]
        [MaxLength(64)]
        [IsFilterable]
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the user's other contact.
        /// </summary>
        [Required]
        [MaxLength(64)]
        [IsFilterable]
        public string OtherContact { get; set; }

        /// <summary>
        /// Gets or sets the user's secondary other contact.
        /// </summary>
        [Required]
        [MaxLength(64)]
        [IsFilterable]
        public string SecondaryOtherContact { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [MaxLength(64)]
        [IsSearchable]
        [IsFilterable]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's secondary email address.
        /// </summary>
        [Required]
        [MaxLength(64)]
        [IsSearchable]
        [IsFilterable]
        public string SecondaryEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets keyword names separated by space character.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string KeywordNames { get; set; }

        /// <summary>
        /// Gets or sets keyword Ids separated by space character.
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
        /// Gets or sets the user organization name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [IsFilterable]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the speciality.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [IsFilterable]
        public string Specialty { get; set; }

        /// <summary>
        /// Gets or sets current organization.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [IsFilterable]
        public string CurrentOrganization { get; set; }

        /// <summary>
        /// Gets or sets undergraduate degree.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [IsFilterable]
        public string UnderGraduateDegree { get; set; }

        /// <summary>
        /// Gets or sets the graduate degree program.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [IsFilterable]
        public string GraduateDegreeProgram { get; set; }

        /// <summary>
        /// Gets or sets the department of study.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [IsFilterable]
        public string DeptOfStudy { get; set; }

        /// <summary>
        /// Gets or sets professional certificates.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [IsFilterable]
        public string ProfessionalCertificates { get; set; }

        /// <summary>
        /// Gets or sets the professional organizations.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [IsFilterable]
        public string ProfessionalOrganizations { get; set; }

        /// <summary>
        /// Gets or sets professional experience.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [IsFilterable]
        public string ProfessionalExperience { get; set; }

        /// <summary>
        /// Gets or sets professional publications.
        /// </summary>
        [Required]
        [MaxLength(100)]
        [IsFilterable]
        public string ProfessionalPublications { get; set; }

        /// <summary>
        /// Gets or sets the Profile Picture Image URL.
        /// </summary>
        [Url]
        [Required]
        [IsFilterable]
        public string ProfilePictureImageURL { get; set; }

        /// <summary>
        /// Gets or sets the URL where user CV is stored.
        /// </summary>
        [Url]
        [Required]
        [IsFilterable]
        public string ResumeCVLink { get; set; }

        /// <summary>
        /// Gets or sets the list of COI Ids separated by semicolon.
        /// </summary>
        [IsFilterable]
        public string CommunityOfInterests { get; set; }

        /// <summary>
        /// Gets or sets notification frequency.
        /// </summary>
        [IsFilterable]
        public int NotificationFrequency { get; set; }

        /// <summary>
        /// Gets or sets the user's display name.
        /// </summary>
        [IsSearchable]
        [IsFilterable]
        public string UserDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the source user Id.
        /// </summary>
        [IsFilterable]
        public int SourceUserId { get; set; }

        /// <summary>
        /// Gets or sets the user type Id.
        /// </summary>
        [IsFilterable]
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the security level.
        /// </summary>
        [IsFilterable]
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets last date time on which user details was updated.
        /// </summary>
        [IsFilterable]
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets date of rank.
        /// </summary>
        [IsFilterable]
        public DateTime? DateOfRank { get; set; }

        /// <summary>
        /// Gets or sets user's service.
        /// </summary>
        [IsFilterable]
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets user's rank.
        /// </summary>
        [IsFilterable]
        public string Rank { get; set; }

        /// <summary>
        /// Gets or sets user's pay grade.
        /// </summary>
        [IsFilterable]
        public string PayGrade { get; set; }

        /// <summary>
        /// Gets or sets user's graduation school.
        /// </summary>
        [IsFilterable]
        public string GradSchool { get; set; }

        /// <summary>
        /// Gets or sets advisors.
        /// </summary>
        [IsFilterable]
        public string Advisors { get; set; }

        /// <summary>
        /// Gets or sets date at post.
        /// </summary>
        [IsFilterable]
        public DateTime? DateAtPost { get; set; }

        /// <summary>
        /// Gets or sets rotation date.
        /// </summary>
        [IsFilterable]
        public DateTime? RotationDate { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        [IsFilterable]
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the repository Id.
        /// </summary>
        [IsFilterable]
        public int RepositoryId { get; set; }

        /// <summary>
        /// Gets or sets graduation program Ids separated by space character.
        /// </summary>
        [IsFilterable]
        public string GraduateProgramId { get; set; }

        /// <summary>
        /// Gets or sets advisor Ids separated by space character..
        /// </summary>
        [IsFilterable]
        public string AdvisorIds { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        [IsFilterable]
        public string WebSite { get; set; }

        /// <summary>
        /// Gets or sets the NPS degree program.
        /// </summary>
        [IsFilterable]
        public string NPSDegreeProgram { get; set; }
    }
}