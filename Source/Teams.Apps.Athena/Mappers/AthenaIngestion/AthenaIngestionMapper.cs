// <copyright file="AthenaIngestionMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Models.Enums;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provide methods related to model mappings for entities sync operation.
    /// </summary>
    public class AthenaIngestionMapper : IAthenaIngestionMapper
    {
        private const string KeywordIdSeparator = " ";

        /// <inheritdoc/>
        public UserEntity MapForAddUserSyncModel(UserJson userJson /*IEnumerable<KeywordEntity> keywordEntities*/)
        {
            if (userJson == null)
            {
                throw new ArgumentNullException(nameof(userJson));
            }

            return new UserEntity
            {
                TableId = Guid.NewGuid().ToString(),
                Advisors = userJson.Advisors,
                CommunityOfInterests = userJson.CommunityOfInterests,
                JobTitle = userJson.JobTitle,
                DateAtPost = userJson.DateAtPost,
                SecurityLevel = userJson.SecurityLevel,
                CurrentOrganization = userJson.CurrentOrganization,
                Organization = userJson.Organization,
                UnderGraduateDegree = userJson.UnderGraduateDegree,
                GradSchool = userJson.GradSchool,
                GraduateDegreeProgram = userJson.GraduateDegreeProgram,
                OtherContact = Convert.ToString(userJson.OtherContact),
                DeptOfStudy = userJson.DeptOfStudy,
                EmailAddress = userJson.EmailAddress,
                RotationDate = userJson.RotationDate,
                UserDisplayName = userJson.FirstName + " " + userJson.MiddleName + " " + userJson.LastName,
                FirstName = userJson.FirstName,
                MiddleName = userJson.MiddleName,
                LastName = userJson.LastName,
                LastUpdate = userJson.LastUpdate,
                ProfilePictureImageURL = userJson.ProfilePictureImageURL,
                SourceUserId = userJson.SourceUserId,
                PayGrade = userJson.PayGrade,
                ProfessionalCertificates = userJson.ProfessionalCertificates,
                ProfessionalExperience = userJson.ProfessionalExperience,
                ProfessionalOrganizations = userJson.ProfessionalOrganizations,
                ProfessionalPublications = userJson.ProfessionalPublications,
                NotificationFrequency = userJson.NotificationFrequency,
                ResumeCVLink = userJson.ResumeCVLink,
                Rank = userJson.Rank,
                Specialty = userJson.Specialty,
                Service = userJson.Service,
                UserType = (int)UserType.External,
                ExternalUserId = userJson.UserId,
                UserTypeId = userJson.UserTypeId,
                DateOfRank = userJson.DateOfRank,
                NodeTypeId = userJson.NodeTypeId,
                Keywords = userJson.Keywords == null ? null : string.Join(KeywordIdSeparator, userJson.Keywords),
                AdvisorIds = userJson.AdvisorIds == null ? null : string.Join(KeywordIdSeparator, userJson.AdvisorIds),
                GraduateProgramId = userJson.GraduateProgramId == null ? null : string.Join(KeywordIdSeparator, userJson.GraduateProgramId),
                RepositoryId = userJson.RepositoryId,
                WebSite = userJson.WebSite,
                KeywordsText = userJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public UserEntity MapForUpdateUserSyncModel(UserJson userJson, UserEntity existingUserDetails)
        {
            if (userJson == null)
            {
                throw new ArgumentNullException(nameof(userJson));
            }

            return new UserEntity
            {
                TableId = existingUserDetails.TableId,
                Advisors = userJson.Advisors,
                CommunityOfInterests = userJson.CommunityOfInterests,
                JobTitle = userJson.JobTitle,
                DateAtPost = userJson.DateAtPost,
                SecurityLevel = userJson.SecurityLevel,
                CurrentOrganization = userJson.CurrentOrganization,
                Organization = userJson.Organization,
                UnderGraduateDegree = userJson.UnderGraduateDegree,
                GradSchool = userJson.GradSchool,
                GraduateDegreeProgram = userJson.GraduateDegreeProgram,
                OtherContact = Convert.ToString(userJson.OtherContact),
                DeptOfStudy = userJson.DeptOfStudy,
                EmailAddress = userJson.EmailAddress,
                RotationDate = userJson.RotationDate,
                UserDisplayName = userJson.FirstName + " " + userJson.MiddleName + " " + userJson.LastName,
                FirstName = userJson.FirstName,
                MiddleName = userJson.MiddleName,
                LastName = userJson.LastName,
                LastUpdate = userJson.LastUpdate,
                ProfilePictureImageURL = userJson.ProfilePictureImageURL,
                SourceUserId = userJson.SourceUserId,
                PayGrade = userJson.PayGrade,
                ProfessionalCertificates = userJson.ProfessionalCertificates,
                ProfessionalExperience = userJson.ProfessionalExperience,
                ProfessionalOrganizations = userJson.ProfessionalOrganizations,
                ProfessionalPublications = userJson.ProfessionalPublications,
                NotificationFrequency = userJson.NotificationFrequency,
                ResumeCVLink = userJson.ResumeCVLink,
                Rank = userJson.Rank,
                Specialty = userJson.Specialty,
                Service = userJson.Service,
                UserType = (int)UserType.External,
                ExternalUserId = userJson.UserId,
                UserTypeId = userJson.UserTypeId,
                DateOfRank = userJson.DateOfRank,
                NodeTypeId = userJson.NodeTypeId,
                Keywords = userJson.Keywords == null ? null : string.Join(KeywordIdSeparator, userJson.Keywords),
                AdvisorIds = userJson.AdvisorIds == null ? null : string.Join(KeywordIdSeparator, userJson.AdvisorIds),
                GraduateProgramId = userJson.GraduateProgramId == null ? null : string.Join(KeywordIdSeparator, userJson.GraduateProgramId),
                RepositoryId = userJson.RepositoryId,
                WebSite = userJson.WebSite,
                KeywordsText = userJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public AthenaInfoResourceEntity MapForAddInfoResourceSyncModel(AthenaInfoResourceJson infoResourcesJson /*IEnumerable<KeywordEntity> keywordEntities*/)
        {
            if (infoResourcesJson == null)
            {
                throw new ArgumentNullException(nameof(infoResourcesJson));
            }

            return new AthenaInfoResourceEntity
            {
                TableId = Guid.NewGuid().ToString(),
                AuthorIds = infoResourcesJson.AuthorIds == null ? null : string.Join(KeywordIdSeparator, infoResourcesJson.AuthorIds),
                Keywords = infoResourcesJson.Keywords == null ? null : string.Join(KeywordIdSeparator, infoResourcesJson.Keywords),
                Authors = infoResourcesJson.Authors,
                Sponsors = infoResourcesJson.Sponsors,
                SponsorIds = infoResourcesJson.SponsorIds == null ? null : string.Join(KeywordIdSeparator, infoResourcesJson.SponsorIds),
                SecurityLevel = infoResourcesJson.SecurityLevel,
                SourceGroup = infoResourcesJson.SourceGroup,
                SourceOrg = infoResourcesJson.SourceOrg,
                SubmitterId = infoResourcesJson.SubmitterId,
                IsPartOfSeries = infoResourcesJson.IsPartOfSeries,
                ResearchSourceId = infoResourcesJson.ResearchSourceId,
                AvgUserRating = infoResourcesJson.AvgUserRating,
                Collection = infoResourcesJson.Collection,
                Description = infoResourcesJson.Description,
                DocId = infoResourcesJson.DocId,
                InfoResourceId = infoResourcesJson.InfoResourceId,
                PublishedDate = infoResourcesJson.PublishedDate,
                LastUpdate = infoResourcesJson.LastUpdate,
                NodeTypeId = infoResourcesJson.NodeTypeId,
                Provenance = infoResourcesJson.Provenance,
                Publisher = infoResourcesJson.Publisher,
                UsageLicensing = infoResourcesJson.UsageLicensing,
                UserComments = infoResourcesJson.UserComments,
                Title = infoResourcesJson.Title,
                Website = infoResourcesJson.Website,
                KeywordsText = infoResourcesJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public AthenaInfoResourceEntity MapForUpdateInfoResourceSyncModel(AthenaInfoResourceJson infoResourcesJson, AthenaInfoResourceEntity existingInfoRecord)
        {
            if (infoResourcesJson == null)
            {
                throw new ArgumentNullException(nameof(infoResourcesJson));
            }

            return new AthenaInfoResourceEntity
            {
                TableId = existingInfoRecord.TableId,
                AuthorIds = infoResourcesJson.AuthorIds == null ? null : string.Join(KeywordIdSeparator, infoResourcesJson.AuthorIds),
                Keywords = infoResourcesJson.Keywords == null ? null : string.Join(KeywordIdSeparator, infoResourcesJson.Keywords),
                Authors = infoResourcesJson.Authors,
                Sponsors = infoResourcesJson.Sponsors,
                SponsorIds = infoResourcesJson.SponsorIds == null ? null : string.Join(KeywordIdSeparator, infoResourcesJson.SponsorIds),
                SecurityLevel = infoResourcesJson.SecurityLevel,
                SourceGroup = infoResourcesJson.SourceGroup,
                SourceOrg = infoResourcesJson.SourceOrg,
                SubmitterId = infoResourcesJson.SubmitterId,
                IsPartOfSeries = infoResourcesJson.IsPartOfSeries,
                ResearchSourceId = infoResourcesJson.ResearchSourceId,
                AvgUserRating = infoResourcesJson.AvgUserRating,
                Collection = infoResourcesJson.Collection,
                Description = infoResourcesJson.Description,
                DocId = infoResourcesJson.DocId,
                InfoResourceId = infoResourcesJson.InfoResourceId,
                PublishedDate = infoResourcesJson.PublishedDate,
                LastUpdate = infoResourcesJson.LastUpdate,
                NodeTypeId = infoResourcesJson.NodeTypeId,
                Provenance = infoResourcesJson.Provenance,
                Publisher = infoResourcesJson.Publisher,
                UsageLicensing = infoResourcesJson.UsageLicensing,
                UserComments = infoResourcesJson.UserComments,
                Title = infoResourcesJson.Title,
                Website = infoResourcesJson.Website,
                KeywordsText = infoResourcesJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public CommunityOfInterestEntity MapForAddCommunitySyncModel(CommunityOfInterestJson communityOfInterestJson)
        {
            if (communityOfInterestJson == null)
            {
                throw new ArgumentNullException(nameof(communityOfInterestJson));
            }

            return new CommunityOfInterestEntity
            {
                TableId = Guid.NewGuid().ToString(),
                CoiDescription = communityOfInterestJson.CoiDescription,
                ChampionIds = communityOfInterestJson.ChampionIds,
                CoiId = communityOfInterestJson.CoiId,
                CoiName = communityOfInterestJson.CoiName,
                CommunityMemberList = communityOfInterestJson.CommunityMemberList,
                ContactId = communityOfInterestJson.ContactId,
                CreatedOn = communityOfInterestJson.DateFounded,
                NodeTypeId = communityOfInterestJson.NodeTypeId,
                NumberOfMembers = communityOfInterestJson.NumberOfMembers,
                UpdatedOn = communityOfInterestJson.LastUpdate,
                SecurityLevel = communityOfInterestJson.SecurityLevel,
                Status = (int)CoiRequestStatus.Approved,
                Type = (int)CoiTeamType.Public,
                Keywords = communityOfInterestJson.Keywords == null ? null : string.Join(KeywordIdSeparator, communityOfInterestJson.Keywords),
                AdminComment = null,
                CreatedByObjectId = null,
                CreatedByUserPrincipalName = null,
                GroupLink = null,
                ImageLink = null,
                IsDeleted = false,
                IsIncludeInSearchResults = false,
                TeamId = null,
                SearchQuery = null,
                AverageRating = "0",
                Organization = communityOfInterestJson.Organization,
                WebSite = communityOfInterestJson.WebSite,
                SponsorIds = communityOfInterestJson.SponsorIds == null ? null : string.Join(" ", communityOfInterestJson.SponsorIds),
                KeywordsText = communityOfInterestJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public CommunityOfInterestEntity MapForUpdateCommunitySyncModel(CommunityOfInterestJson communityOfInterestJson, CommunityOfInterestEntity existingCommunityRecord/*, IEnumerable<KeywordEntity> keywordEntities*/)
        {
            if (communityOfInterestJson == null)
            {
                throw new ArgumentNullException(nameof(communityOfInterestJson));
            }

            return new CommunityOfInterestEntity
            {
                TableId = existingCommunityRecord.TableId,
                CoiDescription = communityOfInterestJson.CoiDescription,
                ChampionIds = communityOfInterestJson.ChampionIds,
                CoiId = communityOfInterestJson.CoiId,
                CoiName = communityOfInterestJson.CoiName,
                CommunityMemberList = communityOfInterestJson.CommunityMemberList,
                ContactId = communityOfInterestJson.ContactId,
                CreatedOn = communityOfInterestJson.DateFounded,
                NodeTypeId = communityOfInterestJson.NodeTypeId,
                NumberOfMembers = communityOfInterestJson.NumberOfMembers,
                UpdatedOn = communityOfInterestJson.LastUpdate,
                SecurityLevel = communityOfInterestJson.SecurityLevel,
                Status = (int)CoiRequestStatus.Approved,
                Type = (int)CoiTeamType.Public,
                Keywords = communityOfInterestJson.Keywords == null ? null : string.Join(KeywordIdSeparator, communityOfInterestJson.Keywords),
                AdminComment = null,
                CreatedByObjectId = null,
                CreatedByUserPrincipalName = null,
                GroupLink = null,
                ImageLink = null,
                IsDeleted = false,
                IsIncludeInSearchResults = false,
                TeamId = null,
                SearchQuery = null,
                AverageRating = "0",
                Organization = communityOfInterestJson.Organization,
                WebSite = communityOfInterestJson.WebSite,
                SponsorIds = communityOfInterestJson.SponsorIds == null ? null : string.Join(" ", communityOfInterestJson.SponsorIds),
                KeywordsText = communityOfInterestJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public ResearchProjectEntity MapForAddResearchProjectSyncModel(ResearchProjectJson researchProjectJson)
        {
            if (researchProjectJson == null)
            {
                throw new ArgumentNullException(nameof(researchProjectJson));
            }

            return new ResearchProjectEntity
            {
                TableId = Guid.NewGuid().ToString(),
                Keywords = researchProjectJson.Keywords == null ? null : string.Join(" ", researchProjectJson.Keywords),
                ResearchProjectId = researchProjectJson.ResearchProjectId,
                NodeTypeId = researchProjectJson.NodeTypeId,
                SecurityLevel = researchProjectJson.SecurityLevel,
                LastUpdate = researchProjectJson.LastUpdate,
                Title = researchProjectJson.Title,
                Abstract = researchProjectJson.Abstract,
                Status = researchProjectJson.Status,
                StatusDescription = researchProjectJson.StatusDescription,
                Authors = researchProjectJson.Authors,
                Advisors = researchProjectJson.Advisors,
                SecondReaders = researchProjectJson.SecondReaders,
                ReviewerNotes = researchProjectJson.ReviewerNotes,
                ResearchDept = researchProjectJson.ResearchDept,
                ResearchSourceId = researchProjectJson.ResearchSourceId,
                Files = researchProjectJson.Files,
                AuthorsOrg = researchProjectJson.AuthorsOrg,
                DegreeProgram = researchProjectJson.DegreeProgram,
                DegreeLevel = researchProjectJson.DegreeProgram,
                DegreeTitles = researchProjectJson.DegreeTitles,
                DateStarted = researchProjectJson.DateStarted,
                DateCompleted = researchProjectJson.DateCompleted,
                Recognition = researchProjectJson.Recognition,
                SponsorIds = researchProjectJson.SponsorIds == null ? null : string.Join(" ", researchProjectJson.SponsorIds),
                OriginatingRequest = researchProjectJson.OriginatingRequest,
                Publisher = researchProjectJson.Publisher,
                UseRights = researchProjectJson.UseRights,
                DocID = researchProjectJson.DocID,
                PartnerIds = researchProjectJson.PartnerIds == null ? null : string.Join(" ", researchProjectJson.PartnerIds),
                ServiceTypeId = researchProjectJson.ServiceTypeId,
                Importance = researchProjectJson.Importance,
                Priority = researchProjectJson.Priority,
                AverageRating = "0",
                RepositoryId = researchProjectJson.RepositoryId,
                AdvisorIds = researchProjectJson.AdvisorIds == null ? null : string.Join(" ", researchProjectJson.AdvisorIds),
                AuthorIds = researchProjectJson.AuthorIds == null ? null : string.Join(" ", researchProjectJson.AuthorIds),
                DepartmentId = researchProjectJson.DepartmentId == null ? null : string.Join(" ", researchProjectJson.DepartmentId),
                GraduateProgramId = researchProjectJson.GraduateProgramId == null ? null : string.Join(" ", researchProjectJson.DepartmentId),
                SecondReadersId = researchProjectJson.SecondReadersId == null ? null : string.Join(" ", researchProjectJson.SecondReadersId),
                KeywordsText = researchProjectJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public ResearchProjectEntity MapForUpdateResearchProjectSyncModel(ResearchProjectJson researchProjectJson, ResearchProjectEntity existingResearchProjectRecord)
        {
            if (researchProjectJson == null)
            {
                throw new ArgumentNullException(nameof(researchProjectJson));
            }

            return new ResearchProjectEntity
            {
                TableId = existingResearchProjectRecord.TableId,
                Keywords = researchProjectJson.Keywords == null ? null : string.Join(" ", researchProjectJson.Keywords),
                ResearchProjectId = researchProjectJson.ResearchProjectId,
                NodeTypeId = researchProjectJson.NodeTypeId,
                SecurityLevel = researchProjectJson.SecurityLevel,
                LastUpdate = researchProjectJson.LastUpdate,
                Title = researchProjectJson.Title,
                Abstract = researchProjectJson.Abstract,
                Status = researchProjectJson.Status,
                StatusDescription = researchProjectJson.StatusDescription,
                Authors = researchProjectJson.Authors,
                Advisors = researchProjectJson.Advisors,
                SecondReaders = researchProjectJson.SecondReaders,
                ReviewerNotes = researchProjectJson.ReviewerNotes,
                ResearchDept = researchProjectJson.ResearchDept,
                ResearchSourceId = researchProjectJson.ResearchSourceId,
                Files = researchProjectJson.Files,
                AuthorsOrg = researchProjectJson.AuthorsOrg,
                DegreeProgram = researchProjectJson.DegreeProgram,
                DegreeLevel = researchProjectJson.DegreeProgram,
                DegreeTitles = researchProjectJson.DegreeTitles,
                DateStarted = researchProjectJson.DateStarted,
                DateCompleted = researchProjectJson.DateCompleted,
                Recognition = researchProjectJson.Recognition,
                SponsorIds = researchProjectJson.SponsorIds == null ? null : string.Join(" ", researchProjectJson.SponsorIds),
                OriginatingRequest = researchProjectJson.OriginatingRequest,
                Publisher = researchProjectJson.Publisher,
                UseRights = researchProjectJson.UseRights,
                DocID = researchProjectJson.DocID,
                PartnerIds = researchProjectJson.PartnerIds == null ? null : string.Join(" ", researchProjectJson.PartnerIds),
                ServiceTypeId = researchProjectJson.ServiceTypeId,
                Importance = researchProjectJson.Importance,
                Priority = researchProjectJson.Priority,
                AverageRating = "0",
                RepositoryId = researchProjectJson.RepositoryId,
                AdvisorIds = researchProjectJson.AdvisorIds == null ? null : string.Join(" ", researchProjectJson.AdvisorIds),
                AuthorIds = researchProjectJson.AuthorIds == null ? null : string.Join(" ", researchProjectJson.AuthorIds),
                DepartmentId = researchProjectJson.DepartmentId == null ? null : string.Join(" ", researchProjectJson.DepartmentId),
                GraduateProgramId = researchProjectJson.GraduateProgramId == null ? null : string.Join(" ", researchProjectJson.DepartmentId),
                SecondReadersId = researchProjectJson.SecondReadersId == null ? null : string.Join(" ", researchProjectJson.SecondReadersId),
                KeywordsText = researchProjectJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public ResearchProposalEntity MapForAddResearchProposalsSyncModel(ResearchProposalJson researchProposalJson)
        {
            if (researchProposalJson == null)
            {
                throw new ArgumentNullException(nameof(researchProposalJson));
            }

            return new ResearchProposalEntity
            {
                TableId = Guid.NewGuid().ToString(),
                Keywords = researchProposalJson.Keywords == null ? null : string.Join(KeywordIdSeparator, researchProposalJson.Keywords),
                ResearchProposalId = researchProposalJson.ResearchProposalId,
                NodeTypeId = researchProposalJson.NodeTypeId,
                SecurityLevel = researchProposalJson.SecurityLevel,
                LastUpdate = researchProposalJson.LastUpdate,
                Title = researchProposalJson.Title,
                Status = researchProposalJson.Status,
                StartDate = researchProposalJson.StartDate,
                SubmitterId = researchProposalJson.SubmitterId,
                Budget = researchProposalJson.Budget,
                RelatedRequestIds = researchProposalJson.RelatedRequestIds == null ? null : string.Join(KeywordIdSeparator, researchProposalJson.RelatedRequestIds),
                Deliverables = researchProposalJson.Deliverables,
                CompletionTime = researchProposalJson.CompletionTime,
                Details = researchProposalJson.Details,
                Endorsements = researchProposalJson.Endorsements,
                Objectives = researchProposalJson.Objectives,
                FocusQuestion1 = researchProposalJson.FocusQuestion1,
                FocusQuestion2 = researchProposalJson.FocusQuestion2,
                FocusQuestion3 = researchProposalJson.FocusQuestion3,
                Plan = researchProposalJson.Plan,
                PotentialFunding = researchProposalJson.PotentialFunding,
                Priority = researchProposalJson.Priority,
                TopicType = researchProposalJson.TopicType,
                Description = researchProposalJson.Description,
                AverageRating = "0",
                AvgUserRating = researchProposalJson.AvgUserRating,
                KeywordsText = researchProposalJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public ResearchProposalEntity MapForUpdateResearchProposalsSyncModel(ResearchProposalJson researchProposalJson, ResearchProposalEntity existingResearchProposalRecord)
        {
            if (researchProposalJson == null)
            {
                throw new ArgumentNullException(nameof(researchProposalJson));
            }

            return new ResearchProposalEntity
            {
                TableId = existingResearchProposalRecord.TableId,
                Keywords = researchProposalJson.Keywords == null ? null : string.Join(KeywordIdSeparator, researchProposalJson.Keywords),
                ResearchProposalId = researchProposalJson.ResearchProposalId,
                NodeTypeId = researchProposalJson.NodeTypeId,
                SecurityLevel = researchProposalJson.SecurityLevel,
                LastUpdate = researchProposalJson.LastUpdate,
                Title = researchProposalJson.Title,
                Status = researchProposalJson.Status,
                StartDate = researchProposalJson.StartDate,
                SubmitterId = researchProposalJson.SubmitterId,
                Budget = researchProposalJson.Budget,
                RelatedRequestIds = researchProposalJson.RelatedRequestIds == null ? null : string.Join(KeywordIdSeparator, researchProposalJson.RelatedRequestIds),
                Deliverables = researchProposalJson.Deliverables,
                CompletionTime = researchProposalJson.CompletionTime,
                Details = researchProposalJson.Details,
                Endorsements = researchProposalJson.Endorsements,
                Objectives = researchProposalJson.Objectives,
                FocusQuestion1 = researchProposalJson.FocusQuestion1,
                FocusQuestion2 = researchProposalJson.FocusQuestion2,
                FocusQuestion3 = researchProposalJson.FocusQuestion3,
                Plan = researchProposalJson.Plan,
                PotentialFunding = researchProposalJson.PotentialFunding,
                Priority = researchProposalJson.Priority,
                TopicType = researchProposalJson.TopicType,
                Description = researchProposalJson.Description,
                AverageRating = "0",
                AvgUserRating = researchProposalJson.AvgUserRating,
                KeywordsText = researchProposalJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public SponsorEntity MapForAddSponsorsSyncModel(SponsorJson sponsorJson)
        {
            if (sponsorJson == null)
            {
                throw new ArgumentNullException(nameof(sponsorJson));
            }

            return new SponsorEntity
            {
                TableId = Guid.NewGuid().ToString(),
                SecurityLevel = sponsorJson.SecurityLevel,
                Service = sponsorJson.Service,
                Keywords = sponsorJson.Keywords == null ? null : string.Join(KeywordIdSeparator, sponsorJson.Keywords),
                Description = sponsorJson.Description,
                SponsorId = sponsorJson.SponsorId,
                FirstName = sponsorJson.FirstName,
                LastName = sponsorJson.LastName,
                Title = sponsorJson.Title,
                NodeTypeId = sponsorJson.NodeTypeId,
                Phone = sponsorJson.Phone,
                OtherContactInfo = sponsorJson.OtherContactInfo,
                Organization = sponsorJson.Organization,
                AverageRating = "0",
                KeywordsText = sponsorJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public SponsorEntity MapForUpdateSponsorsSyncModel(SponsorJson sponsorJson, SponsorEntity existingSponsorRecord)
        {
            if (sponsorJson == null)
            {
                throw new ArgumentNullException(nameof(sponsorJson));
            }

            return new SponsorEntity
            {
                TableId = existingSponsorRecord.TableId,
                SecurityLevel = sponsorJson.SecurityLevel,
                Service = sponsorJson.Service,
                Keywords = sponsorJson.Keywords == null ? null : string.Join(KeywordIdSeparator, sponsorJson.Keywords),
                Description = sponsorJson.Description,
                SponsorId = sponsorJson.SponsorId,
                FirstName = sponsorJson.FirstName,
                LastName = sponsorJson.LastName,
                Title = sponsorJson.Title,
                NodeTypeId = sponsorJson.NodeTypeId,
                Phone = sponsorJson.Phone,
                OtherContactInfo = sponsorJson.OtherContactInfo,
                Organization = sponsorJson.Organization,
                AverageRating = "0",
                KeywordsText = sponsorJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public PartnerEntity MapForAddPartnersSyncModel(PartnerJson partnerJson)
        {
            if (partnerJson == null)
            {
                throw new ArgumentNullException(nameof(partnerJson));
            }

            return new PartnerEntity
            {
                TableId = Guid.NewGuid().ToString(),
                PartnerId = partnerJson.PartnerId,
                Description = partnerJson.Description,
                Keywords = partnerJson.Keywords == null ? null : string.Join(KeywordIdSeparator, partnerJson.Keywords),
                NodeTypeId = partnerJson.NodeTypeId,
                Title = partnerJson.Title,
                Organization = partnerJson.Organization,
                OtherContactInfo = partnerJson.OtherContactInfo,
                SecurityLevel = partnerJson.SecurityLevel,
                FirstName = partnerJson.FirstName,
                LastName = partnerJson.LastName,
                Phone = partnerJson.Phone,
                Projects = partnerJson.Projects,
                AverageRating = "0",
                KeywordsText = partnerJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public PartnerEntity MapForUpdatePartnersSyncModel(PartnerJson partnerJson, PartnerEntity existingPartnerRecord)
        {
            if (partnerJson == null)
            {
                throw new ArgumentNullException(nameof(partnerJson));
            }

            return new PartnerEntity
            {
                TableId = existingPartnerRecord.TableId,
                PartnerId = partnerJson.PartnerId,
                Description = partnerJson.Description,
                Keywords = partnerJson.Keywords == null ? null : string.Join(KeywordIdSeparator, partnerJson.Keywords),
                NodeTypeId = partnerJson.NodeTypeId,
                Title = partnerJson.Title,
                Organization = partnerJson.Organization,
                OtherContactInfo = partnerJson.OtherContactInfo,
                SecurityLevel = partnerJson.SecurityLevel,
                FirstName = partnerJson.FirstName,
                LastName = partnerJson.LastName,
                Phone = partnerJson.Phone,
                Projects = partnerJson.Projects,
                AverageRating = "0",
                KeywordsText = partnerJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public ResearchRequestEntity MapForAddResearchRequestSyncModel(ResearchRequestJson researchRequestJson)
        {
            if (researchRequestJson == null)
            {
                throw new ArgumentNullException(nameof(researchRequestJson));
            }

            return new ResearchRequestEntity
            {
                TableId = Guid.NewGuid().ToString(),
                SecurityLevel = researchRequestJson.SecurityLevel,
                ResearchRequestId = researchRequestJson.ResearchRequestId,
                Keywords = researchRequestJson.Keywords == null ? null : string.Join(KeywordIdSeparator, researchRequestJson.Keywords),
                Description = researchRequestJson.Description,
                SponsorIds = researchRequestJson.SponsorIds == null ? null : string.Join(KeywordIdSeparator, researchRequestJson.SponsorIds),
                TopicId = researchRequestJson.TopicId,
                Title = researchRequestJson.Title,
                NodeTypeId = researchRequestJson.NodeTypeId,
                DesiredCurriculum1 = researchRequestJson.DesiredCurriculum1,
                DesiredCurriculum2 = researchRequestJson.DesiredCurriculum2,
                DesiredCurriculum3 = researchRequestJson.DesiredCurriculum3,
                DesiredCurriculum4 = researchRequestJson.DesiredCurriculum4,
                DesiredCurriculum5 = researchRequestJson.DesiredCurriculum5,
                CompletionTime = researchRequestJson.CompletionTime,
                StartDate = researchRequestJson.StartDate,
                Details = researchRequestJson.Details,
                Endorsements = researchRequestJson.Endorsements,
                FocusQuestion1 = researchRequestJson.FocusQuestion1,
                FocusQuestion2 = researchRequestJson.FocusQuestion2,
                FocusQuestion3 = researchRequestJson.FocusQuestion3,
                LastUpdate = researchRequestJson.LastUpdate,
                PotentialFunding = researchRequestJson.PotentialFunding,
                TopicType = researchRequestJson.TopicType,
                ErbTrbOrg = researchRequestJson.ErbTrbOrg,
                ProducedProjectIds = researchRequestJson.ProducedProjectIds,
                Priority = researchRequestJson.Priority,
                Importance = researchRequestJson.Importance,
                AverageRating = "0",
                AvgUserRating = researchRequestJson.AvgUserRating,
                Sponsors = researchRequestJson.Sponsors,
                Status = researchRequestJson.Status,
                CompletionDate = researchRequestJson.CompletionDate,
                CreateDate = researchRequestJson.CreateDate,
                ContributingStudentsCount = researchRequestJson.ContributingStudentsCount,
                ResearchSourceId = researchRequestJson.ResearchSourceId,
                TopicNotes = researchRequestJson.TopicNotes,
                FiscalYear = researchRequestJson.FiscalYear,
                FocusQuestion4 = researchRequestJson.FocusQuestion4,
                FocusQuestion5 = researchRequestJson.FocusQuestion5,
                IrefTitle = researchRequestJson.IrefTitle,
                ResearchTopicId = researchRequestJson.ResearchTopicId,
                RepositoryId = researchRequestJson.RepositoryId,
                ResearchEstimateId = researchRequestJson.ResearchEstimateId,
                NotesByUser = researchRequestJson.NotesByUser,
                KeywordsText = researchRequestJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public ResearchRequestEntity MapForUpdateResearchRequestSyncModel(ResearchRequestJson researchRequestJson, ResearchRequestEntity existingResearchRequestRecord)
        {
            if (researchRequestJson == null)
            {
                throw new ArgumentNullException(nameof(researchRequestJson));
            }

            return new ResearchRequestEntity
            {
                TableId = existingResearchRequestRecord.TableId,
                SecurityLevel = researchRequestJson.SecurityLevel,
                ResearchRequestId = researchRequestJson.ResearchRequestId,
                Keywords = researchRequestJson.Keywords == null ? null : string.Join(KeywordIdSeparator, researchRequestJson.Keywords),
                Description = researchRequestJson.Description,
                SponsorIds = researchRequestJson.SponsorIds == null ? null : string.Join(KeywordIdSeparator, researchRequestJson.SponsorIds),
                TopicId = researchRequestJson.TopicId,
                Title = researchRequestJson.Title,
                NodeTypeId = researchRequestJson.NodeTypeId,
                DesiredCurriculum1 = researchRequestJson.DesiredCurriculum1,
                DesiredCurriculum2 = researchRequestJson.DesiredCurriculum2,
                DesiredCurriculum3 = researchRequestJson.DesiredCurriculum3,
                DesiredCurriculum4 = researchRequestJson.DesiredCurriculum4,
                DesiredCurriculum5 = researchRequestJson.DesiredCurriculum5,
                CompletionTime = researchRequestJson.CompletionTime,
                StartDate = researchRequestJson.StartDate,
                Details = researchRequestJson.Details,
                Endorsements = researchRequestJson.Endorsements,
                FocusQuestion1 = researchRequestJson.FocusQuestion1,
                FocusQuestion2 = researchRequestJson.FocusQuestion2,
                FocusQuestion3 = researchRequestJson.FocusQuestion3,
                LastUpdate = researchRequestJson.LastUpdate,
                PotentialFunding = researchRequestJson.PotentialFunding,
                TopicType = researchRequestJson.TopicType,
                ErbTrbOrg = researchRequestJson.ErbTrbOrg,
                ProducedProjectIds = researchRequestJson.ProducedProjectIds,
                Priority = researchRequestJson.Priority,
                Importance = researchRequestJson.Importance,
                AverageRating = "0",
                AvgUserRating = researchRequestJson.AvgUserRating,
                Sponsors = researchRequestJson.Sponsors,
                Status = researchRequestJson.Status,
                CompletionDate = researchRequestJson.CompletionDate,
                CreateDate = researchRequestJson.CreateDate,
                ContributingStudentsCount = researchRequestJson.ContributingStudentsCount,
                ResearchSourceId = researchRequestJson.ResearchSourceId,
                TopicNotes = researchRequestJson.TopicNotes,
                FiscalYear = researchRequestJson.FiscalYear,
                FocusQuestion4 = researchRequestJson.FocusQuestion4,
                FocusQuestion5 = researchRequestJson.FocusQuestion5,
                IrefTitle = researchRequestJson.IrefTitle,
                ResearchTopicId = researchRequestJson.ResearchTopicId,
                RepositoryId = researchRequestJson.RepositoryId,
                ResearchEstimateId = researchRequestJson.ResearchEstimateId,
                NotesByUser = researchRequestJson.NotesByUser,
                KeywordsText = researchRequestJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public EventEntity MapForAddEventSyncModel(EventJson eventJson)
        {
            if (eventJson == null)
            {
                throw new ArgumentNullException(nameof(eventJson));
            }

            return new EventEntity
            {
                TableId = Guid.NewGuid().ToString(),
                EventId = eventJson.EventId,
                DateOfEvent = eventJson.DateOfEvent,
                Description = eventJson.Description,
                Keywords = eventJson.Keywords == null ? null : string.Join(KeywordIdSeparator, eventJson.Keywords),
                LastUpdate = eventJson.LastUpdate,
                Location = eventJson.Location,
                NodeTypeId = eventJson.NodeTypeId,
                Title = eventJson.Title,
                Organization = eventJson.Organization,
                OtherContactInfo = eventJson.OtherContactInfo,
                SecurityLevel = eventJson.SecurityLevel,
                WebSite = eventJson.WebSite,
                AverageRating = "0",
                KeywordsText = eventJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public EventEntity MapForUpdateEventSyncModel(EventJson eventJson, EventEntity existingEventRecord)
        {
            if (eventJson == null)
            {
                throw new ArgumentNullException(nameof(eventJson));
            }

            return new EventEntity
            {
                TableId = existingEventRecord.TableId,
                EventId = eventJson.EventId,
                DateOfEvent = eventJson.DateOfEvent,
                Description = eventJson.Description,
                Keywords = eventJson.Keywords == null ? null : string.Join(KeywordIdSeparator, eventJson.Keywords),
                LastUpdate = eventJson.LastUpdate,
                Location = eventJson.Location,
                NodeTypeId = eventJson.NodeTypeId,
                Title = eventJson.Title,
                Organization = eventJson.Organization,
                OtherContactInfo = eventJson.OtherContactInfo,
                SecurityLevel = eventJson.SecurityLevel,
                WebSite = eventJson.WebSite,
                AverageRating = "0",
                KeywordsText = eventJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public AthenaToolEntity MapForAddAthenaToolSyncModel(AthenaToolJson athenaToolJson)
        {
            if (athenaToolJson == null)
            {
                throw new ArgumentNullException(nameof(athenaToolJson));
            }

            return new AthenaToolEntity
            {
                TableId = Guid.NewGuid().ToString(),
                ToolId = athenaToolJson.ToolId,
                Title = athenaToolJson.Title,
                Description = athenaToolJson.Description,
                Manufacturer = athenaToolJson.Manufacturer,
                SecurityLevel = athenaToolJson.SecurityLevel,
                SubmitterId = athenaToolJson.SubmitterId,
                AvgUserRating = athenaToolJson.AvgUserRating,
                UsageLicensing = athenaToolJson.UsageLicensing,
                UserComments = athenaToolJson.UserComments,
                UserRatings = athenaToolJson.UserRatings == null ? null : string.Join(KeywordIdSeparator, athenaToolJson.UserRatings),
                Keywords = athenaToolJson.Keywords == null ? null : string.Join(KeywordIdSeparator, athenaToolJson.Keywords),
                NodeTypeId = athenaToolJson.NodeTypeId,
                Website = athenaToolJson.Website,
                KeywordsText = athenaToolJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public AthenaToolEntity MapForUpdateAthenaToolSyncModel(AthenaToolJson athenaToolJson, AthenaToolEntity existingAthenaToolRecord)
        {
            if (athenaToolJson == null)
            {
                throw new ArgumentNullException(nameof(athenaToolJson));
            }

            return new AthenaToolEntity
            {
                TableId = existingAthenaToolRecord.TableId,
                ToolId = athenaToolJson.ToolId,
                Title = athenaToolJson.Title,
                Description = athenaToolJson.Description,
                Manufacturer = athenaToolJson.Manufacturer,
                SecurityLevel = athenaToolJson.SecurityLevel,
                SubmitterId = athenaToolJson.SubmitterId,
                AvgUserRating = athenaToolJson.AvgUserRating,
                UsageLicensing = athenaToolJson.UsageLicensing,
                UserComments = athenaToolJson.UserComments,
                UserRatings = athenaToolJson.UserRatings == null ? null : string.Join(KeywordIdSeparator, athenaToolJson.UserRatings),
                Keywords = athenaToolJson.Keywords == null ? null : string.Join(KeywordIdSeparator, athenaToolJson.Keywords),
                NodeTypeId = athenaToolJson.NodeTypeId,
                Website = athenaToolJson.Website,
                KeywordsText = athenaToolJson.KeywordsText,
            };
        }

        /// <inheritdoc/>
        public AthenaIngestionDTO MapForViewModel(AthenaIngestionEntity athenaIngestionEntity)
        {
            return new AthenaIngestionDTO
            {
                DbEntity = athenaIngestionEntity.DbEntity,
                Frequency = athenaIngestionEntity.Frequency,
                /*FilePath = athenaIngestionEntity.Filepath,*/
                UpdatedAt = athenaIngestionEntity.UpdatedAt,
            };
        }

        /// <inheritdoc/>
        public AthenaIngestionEntity MapForUpdateModel(string entityName, string path, AthenaIngestionEntity existingAthenaIngestionRecord)
        {
            return new AthenaIngestionEntity
            {
                DbEntity = entityName,
                FilePath = path,
                UpdatedAt = DateTime.UtcNow,
            };
        }
    }
}
