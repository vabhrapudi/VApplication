// <copyright file="IAthenaIngestionMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to entites sync operations.
    /// </summary>
    public interface IAthenaIngestionMapper
    {
        /// <summary>
        /// Maps Users from User json model to User entity model.
        /// </summary>
        /// <param name="userJson">The User json model.</param>
        /// <returns>The added User entity model.</returns>
        UserEntity MapForAddUserSyncModel(UserJson userJson);

        /// <summary>
        /// Maps fields from User json model to UserEntity model.
        /// </summary>
        /// <param name="userJson">The User json model.</param>
        /// <param name="existingUserDetails">The UserEntity model.</param>
        /// <returns>The updated User entity model.</returns>
        UserEntity MapForUpdateUserSyncModel(UserJson userJson, UserEntity existingUserDetails);

        /// <summary>
        /// Maps Athena info resource from Athena info resource json model to Athena info resource entity model.
        /// </summary>
        /// <param name="infoResourcesJson">The Athena info resource json model.</param>
        /// <returns>The added Athena info resource entity model.</returns>
        AthenaInfoResourceEntity MapForAddInfoResourceSyncModel(AthenaInfoResourceJson infoResourcesJson);

        /// <summary>
        /// Maps Athena info resource from Athena info resource json model to Athena info resource entity model.
        /// </summary>
        /// <param name="infoResourcesJson">The Athena info resource json model.</param>
        /// /// <param name="existingInfoRecord">TheAthena info resource entity model.</param>
        /// <returns>The updated  Athena info resource entity model.</returns>
        AthenaInfoResourceEntity MapForUpdateInfoResourceSyncModel(AthenaInfoResourceJson infoResourcesJson, AthenaInfoResourceEntity existingInfoRecord);

        /// <summary>
        /// Maps  community of interest from community of interest json model to  community of interest entity model.
        /// </summary>
        /// <param name="communityOfInterestJson">The community of interest json model.</param>
        /// <returns>The added community of interest entity model.</returns>
        CommunityOfInterestEntity MapForAddCommunitySyncModel(CommunityOfInterestJson communityOfInterestJson);

        /// <summary>
        /// Maps  community of interest from info community of interest json model to community of interest entity model.
        /// </summary>
        /// <param name="communityOfInterestJson">The community of interest json model.</param>
        /// /// <param name="existingCommunityRecord">The community of interest entity model.</param>
        /// <returns>The updated community of interest entity model.</returns>
        CommunityOfInterestEntity MapForUpdateCommunitySyncModel(CommunityOfInterestJson communityOfInterestJson, CommunityOfInterestEntity existingCommunityRecord);

        /// <summary>
        /// Maps research project from research project json model to research project entity model.
        /// </summary>
        /// <param name="researchProjectJson">The Athena research project json model.</param>
        /// <returns>The added Athena research project entity model.</returns>
        ResearchProjectEntity MapForAddResearchProjectSyncModel(ResearchProjectJson researchProjectJson);

        /// <summary>
        /// Maps research project from research project json model to research project entity model.
        /// </summary>
        /// <param name="researchProjectJson">The Athena research project json model.</param>
        /// <param name="existingResearchProjectRecord">Existing Athena research project model.</param>
        /// <returns>The updated Athena research project entity model.</returns>
        ResearchProjectEntity MapForUpdateResearchProjectSyncModel(ResearchProjectJson researchProjectJson, ResearchProjectEntity existingResearchProjectRecord);

        /// <summary>
        /// Maps Athena research proposal from Athena research proposal json model to Athena research proposal entity model.
        /// </summary>
        /// <param name="researchProposalJson">The Athena research proposal json model.</param>
        /// <returns>The Athena research proposal entity model.</returns>
        ResearchProposalEntity MapForAddResearchProposalsSyncModel(ResearchProposalJson researchProposalJson);

        /// <summary>
        /// Maps Athena research proposal from info Athena research proposal json model to Athena research proposal entity model.
        /// </summary>
        /// <param name="researchProposalJson">The Athena research proposal json model.</param>
        /// <param name="existingResearchProposalRecord">Existing Athena research proposal model.</param>
        /// <returns>The updated Athena research proposal entity model.</returns>
        ResearchProposalEntity MapForUpdateResearchProposalsSyncModel(ResearchProposalJson researchProposalJson, ResearchProposalEntity existingResearchProposalRecord);

        /// <summary>
        /// Maps sponsors from sponsor json model to sponsor entity model.
        /// </summary>
        /// <param name="sponsorJson">The sponsor json model.</param>
        /// <param name="existingSponsorRecord">Existing sponsor model.</param>
        /// <returns>The updated Sponsor entity model.</returns>
        SponsorEntity MapForUpdateSponsorsSyncModel(SponsorJson sponsorJson, SponsorEntity existingSponsorRecord);

        /// <summary>
        /// Maps sponsors from sponsor json model to sponsor entity model.
        /// </summary>
        /// <param name="sponsorJson">The Sponsor json model.</param>
        /// <returns>The added Sponsor entity model.</returns>
        SponsorEntity MapForAddSponsorsSyncModel(SponsorJson sponsorJson);

        /// <summary>
        /// Maps partners from partners json model to partners entity model
        /// </summary>
        /// <param name="partnerJson">The Partner json model.</param>
        /// <param name="existingPartnerRecord">Existing Partner model.</param>
        /// <returns>The updated Partner entity model.</returns>
        PartnerEntity MapForUpdatePartnersSyncModel(PartnerJson partnerJson, PartnerEntity existingPartnerRecord);

        /// <summary>
        /// Maps partners from partners json model to partners entity model
        /// </summary>
        /// <param name="partnerJson">The Partner json model.</param>
        /// <returns>The added Partner entity model.</returns>
        PartnerEntity MapForAddPartnersSyncModel(PartnerJson partnerJson);

        /// <summary>
        /// Maps research request from research request json model to research request entity model.
        /// </summary>
        /// <param name="researchRequestJson">The research request json model.</param>
        /// <param name="existingResearchRequestRecord">Existing ResearchRequest model.</param>
        /// <returns>The updated research request entity model.</returns>
        ResearchRequestEntity MapForUpdateResearchRequestSyncModel(ResearchRequestJson researchRequestJson, ResearchRequestEntity existingResearchRequestRecord);

        /// <summary>
        /// Maps research request from research request json model to research request entity model.
        /// </summary>
        /// <param name="researchRequestJson">The research request json model.</param>
        /// <returns>The added research request entity model.</returns>
        ResearchRequestEntity MapForAddResearchRequestSyncModel(ResearchRequestJson researchRequestJson);

        /// <summary>
        /// Maps events from event json model to event entity model.
        /// </summary>
        /// <param name="eventJson">The Event json model.</param>
        /// <param name="existingEventRecord">Existing Event model.</param>
        /// <returns>The updated event entity model.</returns>
        EventEntity MapForUpdateEventSyncModel(EventJson eventJson, EventEntity existingEventRecord);

        /// <summary>
        /// Maps events from event json model to event entity model.
        /// </summary>
        /// <param name="eventJson">The Event json model.</param>
        /// <returns>The added Event entity model.</returns>
        EventEntity MapForAddEventSyncModel(EventJson eventJson);

        /// <summary>
        /// Maps tools from Athena tool json model to Athena tool entity model.
        /// </summary>
        /// <param name="athenaToolJson">The Athena tool json model.</param>
        /// <param name="existingAthenaToolRecord">Existing Athena tool model.</param>
        /// <returns>The updated Athena tool entity model.</returns>
        AthenaToolEntity MapForUpdateAthenaToolSyncModel(AthenaToolJson athenaToolJson, AthenaToolEntity existingAthenaToolRecord);

        /// <summary>
        /// Maps tools from AthenaTool json model to AthenaTool entity model.
        /// </summary>
        /// <param name="athenaToolJson">The Athena tool json model.</param>
        /// <returns>The added Athena tool entity model.</returns>
        AthenaToolEntity MapForAddAthenaToolSyncModel(AthenaToolJson athenaToolJson);

        /// <summary>
        /// Maps Athena ingestion entity to the view model.
        /// </summary>
        /// <param name="athenaIngestionEntity">The Athena ingestion entity model.</param>
        /// <returns>The Athena ingestion view model.</returns>
        AthenaIngestionDTO MapForViewModel(AthenaIngestionEntity athenaIngestionEntity);

        /// <summary>
        /// Maps Athena ingestion fields to the Entity
        /// </summary>
        /// <param name="entityName">entity name that need to be updated.</param>
        /// <param name="path">file path of the entity</param>
        /// <param name="existingAthenaIngestionRecord">Existing Athena ingestion record</param>
        /// <returns>The updated Athena  ingestion entity model.</returns>
        AthenaIngestionEntity MapForUpdateModel(string entityName, string path, AthenaIngestionEntity existingAthenaIngestionRecord);
    }
}
