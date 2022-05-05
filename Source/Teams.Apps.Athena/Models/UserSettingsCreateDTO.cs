// <copyright file="UserSettingsCreateDTO.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Holds the details of a user entity.
    /// </summary>
    public class UserSettingsCreateDTO
    {
        /// <summary>
        /// Gets or sets user's first name.
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets user's middle name.
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string MiddleName { get; set; }

        /// <summary>
        /// Gets or sets user's last name.
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's job_title.
        /// </summary>
        [Required]
        public IEnumerable<string> JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the user's other contact.
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string OtherContact { get; set; }

        /// <summary>
        /// Gets or sets the user's secondary other contact.
        /// </summary>
        [MaxLength(64)]
        public string SecondaryOtherContact { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [MaxLength(64)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's secondary email address.
        /// </summary>
        [MaxLength(64)]
        public string SecondaryEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the keywords that user has searched in JSON array of string.
        /// </summary>
        [Required]
        public IEnumerable<KeywordDTO> KeywordsJson { get; set; }

        /// <summary>
        /// Gets or sets the user organization name.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets the speciality.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Specialty { get; set; }

        /// <summary>
        /// Gets or sets current organization.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string CurrentOrganization { get; set; }

        /// <summary>
        /// Gets or sets undergraduate degree.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string UnderGraduateDegree { get; set; }

        /// <summary>
        /// Gets or sets the graduate degree program.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string GraduateDegreeProgram { get; set; }

        /// <summary>
        /// Gets or sets the department of study.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string DeptOfStudy { get; set; }

        /// <summary>
        /// Gets or sets professional certificates.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ProfessionalCertificates { get; set; }

        /// <summary>
        /// Gets or sets the professional organizations.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ProfessionalOrganizations { get; set; }

        /// <summary>
        /// Gets or sets professional experience.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ProfessionalExperience { get; set; }

        /// <summary>
        /// Gets or sets professional publications.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ProfessionalPublications { get; set; }

        /// <summary>
        /// Gets or sets the Profile Picture Image URL.
        /// </summary>
        [Url]
        [Required]
        public string ProfilePictureImageURL { get; set; }

        /// <summary>
        /// Gets or sets the URL where user CV is stored.
        /// </summary>
        [Url]
        [Required]
        public string ResumeCVLink { get; set; }

        /// <summary>
        /// Gets or sets the list of community of interests.
        /// </summary>
        public string CommunityOfInterests { get; set; }

        /// <summary>
        /// Gets or sets the notification frequency.
        /// </summary>
        public int NotificationFrequency { get; set; }

        /// <summary>
        /// Gets or sets the NPS degree program.
        /// </summary>
        [Required]
        public string NPSDegreeProgram { get; set; }
    }
}