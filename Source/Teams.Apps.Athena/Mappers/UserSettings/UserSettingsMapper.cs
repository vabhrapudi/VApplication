// <copyright file="UserSettingsMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using System.Linq;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Models.Enums;
    using Teams.Apps.Athena.Constants;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// A model class that contains methods related to user entity model mappings.
    /// </summary>
    public class UserSettingsMapper : IUserSettingsMapper
    {
        private const string KeywordsSeparator = " ";
        private const string SemicolonSeparator = ";";

        /// <inheritdoc />
        public UserEntity MapForCreateModel(UserSettingsCreateDTO userSettingsCreateModel, string userId)
        {
            userSettingsCreateModel = userSettingsCreateModel ?? throw new ArgumentNullException(nameof(userSettingsCreateModel));
            return new UserEntity
            {
                TableId = Guid.NewGuid().ToString(),
                UserId = userId,
                ExternalUserId = Constants.SourceAthena,
                UserType = (int)UserType.Registered,
                FirstName = userSettingsCreateModel.FirstName,
                MiddleName = userSettingsCreateModel.MiddleName,
                LastName = userSettingsCreateModel.LastName,
                JobTitle = userSettingsCreateModel.JobTitle == null ? null : string.Join(SemicolonSeparator, userSettingsCreateModel.JobTitle),
                OtherContact = userSettingsCreateModel.OtherContact,
                EmailAddress = userSettingsCreateModel.EmailAddress,
                KeywordNames = userSettingsCreateModel.KeywordsJson == null ? null : string.Join(SemicolonSeparator, userSettingsCreateModel.KeywordsJson.Select(x => x.Title)),
                Keywords = userSettingsCreateModel.KeywordsJson == null ? null : string.Join(KeywordsSeparator, userSettingsCreateModel.KeywordsJson.Select(x => x.KeywordId)),
                Organization = userSettingsCreateModel.Organization,
                Specialty = userSettingsCreateModel.Specialty,
                CurrentOrganization = userSettingsCreateModel.CurrentOrganization,
                UnderGraduateDegree = userSettingsCreateModel.UnderGraduateDegree,
                GraduateDegreeProgram = userSettingsCreateModel.GraduateDegreeProgram,
                DeptOfStudy = userSettingsCreateModel.DeptOfStudy,
                ProfessionalCertificates = userSettingsCreateModel.ProfessionalCertificates,
                ProfessionalOrganizations = userSettingsCreateModel.ProfessionalOrganizations,
                ProfessionalExperience = userSettingsCreateModel.ProfessionalExperience,
                ProfessionalPublications = userSettingsCreateModel.ProfessionalPublications,
                ProfilePictureImageURL = userSettingsCreateModel.ProfilePictureImageURL,
                ResumeCVLink = userSettingsCreateModel.ResumeCVLink,
                CommunityOfInterests = userSettingsCreateModel.CommunityOfInterests,
                NotificationFrequency = userSettingsCreateModel.NotificationFrequency,
                LastUpdate = DateTime.UtcNow,
                DateOfRank = DateTime.UtcNow,
                DateAtPost = DateTime.UtcNow,
                RotationDate = DateTime.UtcNow,
                SecondaryEmailAddress = userSettingsCreateModel.SecondaryEmailAddress,
                SecondaryOtherContact = userSettingsCreateModel.SecondaryOtherContact,
                NPSDegreeProgram = userSettingsCreateModel.NPSDegreeProgram,
            };
        }

        /// <inheritdoc />
        public UserEntity MapForCreateModel(UserDetails userDetails)
        {
            userDetails = userDetails ?? throw new ArgumentNullException(nameof(userDetails));
            return new UserEntity
            {
                TableId = Guid.NewGuid().ToString(),
                UserId = userDetails.Id,
                ExternalUserId = Constants.SourceAthena,
                UserType = (int)UserType.Registered,
                FirstName = userDetails.FirstName,
                LastName = userDetails.Surname,
                OtherContact = userDetails.MobilePhone,
                EmailAddress = userDetails.UserPrincipalName,
                UserDisplayName = userDetails.FirstName + " " + userDetails.Surname,
                LastUpdate = DateTime.UtcNow,
                DateOfRank = DateTime.UtcNow,
                DateAtPost = DateTime.UtcNow,
                RotationDate = DateTime.UtcNow,
            };
        }

        /// <inheritdoc />
        public UserEntity MapForUpdateModel(UserSettingsUpdateDTO userSettingsUpdateModel, UserEntity userEntity)
        {
            userSettingsUpdateModel = userSettingsUpdateModel ?? throw new ArgumentNullException(nameof(userSettingsUpdateModel));
            userEntity = userEntity ?? throw new ArgumentNullException(nameof(userEntity));

            userEntity.UserType = (int)UserType.Registered;
            userEntity.FirstName = userSettingsUpdateModel.FirstName;
            userEntity.MiddleName = userSettingsUpdateModel.MiddleName;
            userEntity.LastName = userSettingsUpdateModel.LastName;
            userEntity.JobTitle = userSettingsUpdateModel.JobTitle == null ? null : string.Join(SemicolonSeparator, userSettingsUpdateModel.JobTitle);
            userEntity.OtherContact = userSettingsUpdateModel.OtherContact;
            userEntity.EmailAddress = userSettingsUpdateModel.EmailAddress;
            userEntity.KeywordNames = userSettingsUpdateModel.KeywordsJson == null ? null : string.Join(SemicolonSeparator, userSettingsUpdateModel.KeywordsJson.Select(x => x.Title));
            userEntity.Keywords = userSettingsUpdateModel.KeywordsJson == null ? null : string.Join(KeywordsSeparator, userSettingsUpdateModel.KeywordsJson.Select(x => x.KeywordId));
            userEntity.Organization = userSettingsUpdateModel.Organization;
            userEntity.Specialty = userSettingsUpdateModel.Specialty;
            userEntity.CurrentOrganization = userSettingsUpdateModel.CurrentOrganization;
            userEntity.UnderGraduateDegree = userSettingsUpdateModel.UnderGraduateDegree;
            userEntity.GraduateDegreeProgram = userSettingsUpdateModel.GraduateDegreeProgram;
            userEntity.DeptOfStudy = userSettingsUpdateModel.DeptOfStudy;
            userEntity.ProfessionalCertificates = userSettingsUpdateModel.ProfessionalCertificates;
            userEntity.ProfessionalOrganizations = userSettingsUpdateModel.ProfessionalOrganizations;
            userEntity.ProfessionalExperience = userSettingsUpdateModel.ProfessionalExperience;
            userEntity.ProfessionalPublications = userSettingsUpdateModel.ProfessionalPublications;
            userEntity.ProfilePictureImageURL = userSettingsUpdateModel.ProfilePictureImageURL;
            userEntity.ResumeCVLink = userSettingsUpdateModel.ResumeCVLink;
            userEntity.CommunityOfInterests = userSettingsUpdateModel.CommunityOfInterests;
            userEntity.NotificationFrequency = userSettingsUpdateModel.NotificationFrequency;
            userEntity.LastUpdate = DateTime.UtcNow;
            userEntity.DateOfRank = DateTime.UtcNow;
            userEntity.DateAtPost = DateTime.UtcNow;
            userEntity.RotationDate = DateTime.UtcNow;
            userEntity.SecondaryOtherContact = userSettingsUpdateModel.SecondaryOtherContact;
            userEntity.SecondaryEmailAddress = userSettingsUpdateModel.SecondaryEmailAddress;
            userEntity.NPSDegreeProgram = userSettingsUpdateModel.NPSDegreeProgram;

            return userEntity;
        }

        /// <inheritdoc />
        public UserSettingsViewDTO MapForViewModel(UserEntity userEntity)
        {
            userEntity = userEntity ?? throw new ArgumentNullException(nameof(userEntity));
            return new UserSettingsViewDTO
            {
                TableId = userEntity.TableId,
                UserId = userEntity.UserId,
                FirstName = userEntity.FirstName,
                MiddleName = userEntity.MiddleName,
                LastName = userEntity.LastName,
                JobTitle = string.IsNullOrEmpty(userEntity.JobTitle) ? Array.Empty<string>() : userEntity.JobTitle.Split(SemicolonSeparator),
                OtherContact = userEntity.OtherContact,
                EmailAddress = userEntity.EmailAddress,
                Organization = userEntity.Organization,
                Specialty = userEntity.Specialty,
                CurrentOrganization = userEntity.CurrentOrganization,
                UnderGraduateDegree = userEntity.UnderGraduateDegree,
                GraduateDegreeProgram = userEntity.GraduateDegreeProgram,
                DeptOfStudy = userEntity.DeptOfStudy,
                ProfessionalCertificates = userEntity.ProfessionalCertificates,
                ProfessionalOrganizations = userEntity.ProfessionalOrganizations,
                ProfessionalExperience = userEntity.ProfessionalExperience,
                ProfessionalPublications = userEntity.ProfessionalPublications,
                ProfilePictureImageURL = userEntity.ProfilePictureImageURL,
                ResumeCVLink = userEntity.ResumeCVLink,
                CommunityOfInterests = userEntity.CommunityOfInterests,
                NotificationFrequency = userEntity.NotificationFrequency,
                UserDisplayName = userEntity.UserDisplayName,
                Keywords = string.IsNullOrWhiteSpace(userEntity.Keywords) ? Array.Empty<int>() : Array.ConvertAll(userEntity.Keywords.Split(KeywordsSeparator), int.Parse),
                NodeTypeId = userEntity.NodeTypeId,
                DateOfRank = userEntity.DateOfRank,
                DateAtPost = userEntity.DateAtPost,
                RotationDate = userEntity.RotationDate,
                WebSite = userEntity.WebSite,
                Advisors = userEntity.Advisors,
                SecondaryEmailAddress = userEntity.SecondaryEmailAddress,
                SecondaryOtherContact = userEntity.SecondaryOtherContact,
                NPSDegreeProgram = userEntity.NPSDegreeProgram,
            };
        }
    }
}