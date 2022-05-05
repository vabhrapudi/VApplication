// <copyright file="UserJsonDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an User json DTO.
    /// </summary>
    public class UserJsonDTO
    {
        /// <summary>
        /// Gets or sets unique table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the user AAD Id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets user's first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets user's middle name.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets user's last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's job_title.
        /// </summary>
        public string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the keywords that user has searched in array of string.
        /// </summary>
        public IEnumerable<int> Keywords { get; set; }

        /// <summary>
        /// Gets or sets the user organization name.
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets current organization.
        /// </summary>
        public string CurrentOrganization { get; set; }

        /// <summary>
        /// Gets or sets undergraduate degree.
        /// </summary>
        public string UnderGraduateDegree { get; set; }

        /// <summary>
        /// Gets or sets the graduate degree program.
        /// </summary>
        public string GraduateDegreeProgram { get; set; }

        /// <summary>
        /// Gets or sets the department of study.
        /// </summary>
        public string DeptOfStudy { get; set; }

        /// <summary>
        /// Gets or sets professional certificates.
        /// </summary>
        public string ProfessionalCertificates { get; set; }

        /// <summary>
        /// Gets or sets the professional organizations.
        /// </summary>
        public string ProfessionalOrganizations { get; set; }

        /// <summary>
        /// Gets or sets professional experience.
        /// </summary>
        public string ProfessionalExperience { get; set; }

        /// <summary>
        /// Gets or sets professional publications.
        /// </summary>
        public string ProfessionalPublications { get; set; }

        /// <summary>
        /// Gets or sets the Profile Picture Image URL.
        /// </summary>
        public string ProfilePictureImageURL { get; set; }

        /// <summary>
        /// Gets or sets the URL where user CV is stored.
        /// </summary>
        public string ResumeCVLink { get; set; }

        /// <summary>
        /// Gets or sets the list of COI Ids separated by semicolon.
        /// </summary>
        public string CommunityOfInterests { get; set; }

        /// <summary>
        /// Gets or sets notification frequency.
        /// </summary>
        public int NotificationFrequency { get; set; }

        /// <summary>
        /// Gets or sets the user's display name.
        /// </summary>
        public string UserDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the source user Id.
        /// </summary>
        public int SourceUserId { get; set; }

        /// <summary>
        /// Gets or sets the users security level.
        /// </summary>
        public int SecurityLevel { get; set; }

        /// <summary>
        /// Gets or sets the last updated date and time.
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets the users other contact info.
        /// </summary>
        public dynamic OtherContact { get; set; }

        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Gets or sets the rank.
        /// </summary>
        public string Rank { get; set; }

        /// <summary>
        /// Gets or sets the date of rank.
        /// </summary>
        public DateTime? DateOfRank { get; set; }

        /// <summary>
        /// Gets or sets the users pay grade.
        /// </summary>
        public string PayGrade { get; set; }

        /// <summary>
        /// Gets or sets the users specialty.
        /// </summary>
        public string Specialty { get; set; }

        /// <summary>
        /// Gets or sets the users graduated school.
        /// </summary>
        public string GradSchool { get; set; }

        /// <summary>
        /// Gets or sets the advisors.
        /// </summary>
        public string Advisors { get; set; }

        /// <summary>
        /// Gets or sets the date at post.
        /// </summary>
        public DateTime? DateAtPost { get; set; }

        /// <summary>
        /// Gets or sets the rotation date and time.
        /// </summary>
        public DateTime? RotationDate { get; set; }

        /// <summary>
        /// Gets or sets user type whether the user is registered or external.
        /// </summary>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Gets or sets the node type Id.
        /// </summary>
        public int NodeTypeId { get; set; }

        /// <summary>
        /// Gets or sets the collection of advisor Id's.
        /// </summary>
        public IEnumerable<int> AdvisorIds { get; set; }

        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// Gets or sets the collection of graduate program Id.
        /// </summary>
        public IEnumerable<int> GraduateProgramId { get; set; }

        /// <summary>
        /// Gets or sets the repository Id.
        /// </summary>
        public int RepositoryId { get; set; }
    }
}
