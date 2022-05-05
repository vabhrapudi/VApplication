// <copyright file="IMyCollectionsMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes methods that manages my collection entity model mappings.
    /// </summary>
    public interface IMyCollectionsMapper
    {
        /// <summary>
        /// Gets my collection model to be inserted in database.
        /// </summary>
        /// <param name=" myCollectionCreateModel">my collection create model.</param>
        /// <param name="userId">The user Id.</param>
        /// <returns>Returns a my collection entity model.</returns>
        MyCollectionsEntity MapForCreateModel(MyCollectionsCreateDTO myCollectionCreateModel, string userId);

        /// <summary>
        /// Gets my collection model to be updated in database.
        /// </summary>
        /// <param name="myCollectionUpdateModel">my collection update model.</param>
        /// <param name="myCollectionEntity">my collection model.</param>
        /// <returns>Returns a my collection model.</returns>
        MyCollectionsEntity MapForUpdateModel(MyCollectionsUpdateDTO myCollectionUpdateModel, MyCollectionsEntity myCollectionEntity);

        /// <summary>
        /// Gets  my collection model sent to be as API response.
        /// </summary>
        /// <param name="myCollectionEntity"> my collection model.</param>
        /// <returns>Returns a  my collection view model.</returns>
        MyCollectionsViewDTO MapForViewModel(MyCollectionsEntity myCollectionEntity);

        /// <summary>
        /// Gets  my collection item model sent to be as API response.
        /// </summary>
        /// <param name="newsEntity">The news entity.</param>
        /// <param name="collectionId">The collection id.</param>
        /// <param name="newsSourceTitle">The news source title.</param>
        /// <returns>Returns a  my collection items view model.</returns>
        MyCollectionsItemsViewDTO MapForNewsCollectionItemViewModel(NewsEntity newsEntity, string collectionId, string newsSourceTitle);

        /// <summary>
        /// Gets  my collection item model sent to be as API response.
        /// </summary>
        /// <param name="coiEntity"> COI entity.</param>
        /// <param name="collectionId"> collection id.</param>
        /// <returns>Returns a  my collection items view model.</returns>
        MyCollectionsItemsViewDTO MapForCoiCollectionItemViewModel(CommunityOfInterestEntity coiEntity, string collectionId);

        /// <summary>
        /// Gets  my collection item model sent to be as API response.
        /// </summary>
        /// <param name="researchPaper"> research paper.</param>
        /// <param name="collectionId"> collection id.</param>
        /// <returns>Returns a  my collection items view model.</returns>
        MyCollectionsItemsViewDTO MapForResearchPaperCollectionItemViewModel(ResearchPaper researchPaper, string collectionId);

        /// <summary>
        /// Gets  my collection item model sent to be as API response.
        /// </summary>
        /// <param name="researchProject">The research project.</param>
        /// <param name="collectionId">The collection id.</param>
        /// <param name="researchSourceTitle">The news source title.</param>
        /// <returns>Returns a  my collection items view model.</returns>
        MyCollectionsItemsViewDTO MapForResearchProjectCollectionItemViewModel(ResearchProjectEntity researchProject, string collectionId, string researchSourceTitle);

        /// <summary>
        /// Gets  my collection item model sent to be as API response.
        /// </summary>
        /// <param name="researchRequest"> research request.</param>
        /// <param name="collectionId"> collection id.</param>
        /// <param name="researchSourceTitle">The news source title.</param>
        /// <returns>Returns a  my collection items view model.</returns>
        MyCollectionsItemsViewDTO MapForResearchRequestCollectionItemViewModel(ResearchRequestEntity researchRequest, string collectionId, string researchSourceTitle);

        /// <summary>
        /// Gets  my collection item model sent to be as API response.
        /// </summary>
        /// <param name="userEntity"> user entity.</param>
        /// <param name="collectionId"> collection id.</param>
        /// <param name="userName"> name of user who have created the collection item.</param>
        /// <param name="userProfilePhoto"> profile photo of user who have created the collection item.</param>
        /// <returns>Returns a  my collection items view model.</returns>
        MyCollectionsItemsViewDTO MapForUserCollectionItemViewModel(UserEntity userEntity, string collectionId, string userName, string userProfilePhoto);

        /// <summary>
        /// Gets  my collection item model sent to be as API response.
        /// </summary>
        /// <param name="researchProposal"> research proposal.</param>
        /// <param name="collectionId"> collection id.</param>
        /// <param name="researchSourceTitle">The news source title.</param>
        /// <returns>Returns a  my collection items view model.</returns>
        MyCollectionsItemsViewDTO MapForResearchProposalCollectionItemViewModel(ResearchProposalEntity researchProposal, string collectionId, string researchSourceTitle);

        /// <summary>
        /// Gets  my collection item model sent to be as API response.
        /// </summary>
        /// <param name="eventEntity"> event entity.</param>
        /// <param name="collectionId"> collection id.</param>
        /// <returns>Returns a  my collection items view model.</returns>
        MyCollectionsItemsViewDTO MapForEventCollectionItemViewModel(EventEntity eventEntity, string collectionId);

        /// <summary>
        /// Gets  my collection item model sent to be as API response.
        /// </summary>
        /// <param name="partnerEntity"> partner entity.</param>
        /// <param name="collectionId"> collection id.</param>
        /// <returns>Returns a  my collection items view model.</returns>
        MyCollectionsItemsViewDTO MapForPartnerCollectionItemViewModel(PartnerEntity partnerEntity, string collectionId);

        /// <summary>
        /// Gets  my collection item model sent to be as API response.
        /// </summary>
        /// <param name="sponsorEntity"> sponsor entity.</param>
        /// <param name="collectionId"> collection id.</param>
        /// <returns>Returns a  my collection items view model.</returns>
        MyCollectionsItemsViewDTO MapForSponsorCollectionItemViewModel(SponsorEntity sponsorEntity, string collectionId);
    }
}