// <copyright file="MyCollectionsMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;
    using Teams.Apps.Athena.Models.Enums;
    using Teams.Apps.Athena.Resources;

    /// <summary>
    /// A model class that contains methods related to my collection entity model mappings.
    /// </summary>
    public class MyCollectionsMapper : IMyCollectionsMapper
    {
        /// <inheritdoc/>
        public MyCollectionsEntity MapForCreateModel(MyCollectionsCreateDTO myCollectionCreateModel, string userId)
        {
            myCollectionCreateModel = myCollectionCreateModel ?? throw new ArgumentNullException(nameof(myCollectionCreateModel));
            return new MyCollectionsEntity
            {
                CollectionId = Guid.NewGuid().ToString(),
                Name = myCollectionCreateModel.Name?.Trim(),
                Description = myCollectionCreateModel.Description?.Trim(),
                ImageURL = myCollectionCreateModel.ImageURL,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedAt = DateTime.UtcNow,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsEntity MapForUpdateModel(MyCollectionsUpdateDTO myCollectionUpdateModel, MyCollectionsEntity myCollectionEntity)
        {
            myCollectionUpdateModel = myCollectionUpdateModel ?? throw new ArgumentNullException(nameof(myCollectionUpdateModel));
            myCollectionEntity = myCollectionEntity ?? throw new ArgumentNullException(nameof(myCollectionEntity));

            myCollectionEntity.Name = myCollectionUpdateModel?.Name.Trim();
            myCollectionEntity.Description = myCollectionUpdateModel?.Description.Trim();
            myCollectionEntity.ImageURL = myCollectionUpdateModel.ImageURL;
            myCollectionEntity.UpdatedAt = DateTime.UtcNow;
            myCollectionEntity.Items = myCollectionEntity.Items;

            return myCollectionEntity;
        }

        /// <inheritdoc/>
        public MyCollectionsViewDTO MapForViewModel(MyCollectionsEntity myCollectionEntity)
        {
            myCollectionEntity = myCollectionEntity ?? throw new ArgumentNullException(nameof(myCollectionEntity));
            return new MyCollectionsViewDTO
            {
                CollectionId = myCollectionEntity.CollectionId,
                Name = myCollectionEntity.Name,
                Description = myCollectionEntity.Description,
                ImageURL = myCollectionEntity.ImageURL,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsItemsViewDTO MapForNewsCollectionItemViewModel(NewsEntity newsEntity, string collectionId, string newsSourceTitle)
        {
            newsEntity = newsEntity ?? throw new ArgumentNullException(nameof(newsEntity));
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return new MyCollectionsItemsViewDTO
            {
                CollectionId = collectionId,
                CollectionItemName = newsEntity.Title,
                CollectionItemId = newsEntity.TableId,
                CreatedBy = newsEntity.CreatedBy,
                CreatedOn = newsEntity.PublishedDate,
                Category = Strings.NewsText,
                CollectionItemType = (int)CollectionItemType.News,
                ExternalLink = newsEntity.ExternalLink,
                Source = newsSourceTitle,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsItemsViewDTO MapForCoiCollectionItemViewModel(CommunityOfInterestEntity coiEntity, string collectionId)
        {
            coiEntity = coiEntity ?? throw new ArgumentNullException(nameof(coiEntity));
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return new MyCollectionsItemsViewDTO
            {
                CollectionId = collectionId,
                CollectionItemName = coiEntity.CoiName,
                CollectionItemId = coiEntity.TableId,
                CreatedBy = coiEntity.CreatedByObjectId,
                CreatedOn = coiEntity.CreatedOn,
                Category = Strings.CoiText,
                CollectionItemType = (int)CollectionItemType.COI,
                ExternalLink = coiEntity.WebSite,
                Source = coiEntity.Organization,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsItemsViewDTO MapForResearchPaperCollectionItemViewModel(ResearchPaper researchPaper, string collectionId)
        {
            researchPaper = researchPaper ?? throw new ArgumentNullException(nameof(researchPaper));
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return new MyCollectionsItemsViewDTO
            {
                CollectionId = collectionId,
                CollectionItemName = researchPaper.Title,
                CollectionItemId = researchPaper.ResearchId,
                CreatedBy = researchPaper.CreatedBy,
                CreatedOn = researchPaper.CreatedAt,
                Category = Strings.ResearchPaperText,
                CollectionItemType = (int)CollectionItemType.ResearchPaper,
                ExternalLink = researchPaper.ExternalLink,
                Source = researchPaper.Source,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsItemsViewDTO MapForResearchProjectCollectionItemViewModel(ResearchProjectEntity researchProject, string collectionId, string researchSourceTitle)
        {
            researchProject = researchProject ?? throw new ArgumentNullException(nameof(researchProject));
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return new MyCollectionsItemsViewDTO
            {
                CollectionId = collectionId,
                CollectionItemName = researchProject.Title,
                CollectionItemId = researchProject.TableId,
                Category = Strings.ResearchProjectText,
                CollectionItemType = (int)CollectionItemType.ResearchProject,
                CreatedOn = researchProject.DateStarted,
                ExternalLink = researchProject.Files,
                Source = researchSourceTitle,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsItemsViewDTO MapForResearchRequestCollectionItemViewModel(ResearchRequestEntity researchRequest, string collectionId, string researchSourceTitle)
        {
            researchRequest = researchRequest ?? throw new ArgumentNullException(nameof(researchRequest));
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return new MyCollectionsItemsViewDTO
            {
                CollectionId = collectionId,
                CollectionItemName = researchRequest.Title,
                CollectionItemId = researchRequest.TableId,
                Category = Strings.ResearchRequestText,
                CollectionItemType = (int)CollectionItemType.ResearchRequest,
                CreatedOn = researchRequest.StartDate,
                Source = researchSourceTitle,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsItemsViewDTO MapForResearchProposalCollectionItemViewModel(ResearchProposalEntity researchProposal, string collectionId, string researchSourceTitle)
        {
            researchProposal = researchProposal ?? throw new ArgumentNullException(nameof(researchProposal));
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return new MyCollectionsItemsViewDTO
            {
                CollectionId = collectionId,
                CollectionItemName = researchProposal.Title,
                CollectionItemId = researchProposal.TableId,
                Category = Strings.ResearchProposalText,
                CollectionItemType = (int)CollectionItemType.ResearchProposal,
                CreatedOn = researchProposal.StartDate,
                Source = researchSourceTitle,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsItemsViewDTO MapForUserCollectionItemViewModel(UserEntity userEntity, string collectionId, string userName, string userProfilePhoto)
        {
            userEntity = userEntity ?? throw new ArgumentNullException(nameof(userEntity));
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return new MyCollectionsItemsViewDTO
            {
                CollectionId = collectionId,
                CollectionItemName = userName,
                CollectionItemId = userEntity.TableId,
                CreatedBy = userEntity.UserId,
                Category = Strings.UserText,
                CreatedByName = userName,
                CreatedByProfilePhoto = userProfilePhoto,
                CollectionItemType = (int)CollectionItemType.User,
                CreatedOn = userEntity.RotationDate,
                ExternalLink = userEntity.WebSite,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsItemsViewDTO MapForEventCollectionItemViewModel(EventEntity eventEntity, string collectionId)
        {
            eventEntity = eventEntity ?? throw new ArgumentNullException(nameof(eventEntity));
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return new MyCollectionsItemsViewDTO
            {
                CollectionId = collectionId,
                CollectionItemName = eventEntity.Title,
                CollectionItemId = eventEntity.TableId,
                Category = Strings.EventText,
                CollectionItemType = (int)CollectionItemType.Event,
                CreatedOn = eventEntity.DateOfEvent,
                ExternalLink = eventEntity.WebSite,
                Source = eventEntity.Organization,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsItemsViewDTO MapForPartnerCollectionItemViewModel(PartnerEntity partnerEntity, string collectionId)
        {
            partnerEntity = partnerEntity ?? throw new ArgumentNullException(nameof(partnerEntity));
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return new MyCollectionsItemsViewDTO
            {
                CollectionId = collectionId,
                CollectionItemName = partnerEntity.Title,
                CollectionItemId = partnerEntity.TableId,
                Category = Strings.PartnerText,
                CollectionItemType = (int)CollectionItemType.Partner,
                Source = partnerEntity.Organization,
            };
        }

        /// <inheritdoc/>
        public MyCollectionsItemsViewDTO MapForSponsorCollectionItemViewModel(SponsorEntity sponsorEntity, string collectionId)
        {
            sponsorEntity = sponsorEntity ?? throw new ArgumentNullException(nameof(sponsorEntity));
            collectionId = collectionId ?? throw new ArgumentNullException(nameof(collectionId));
            return new MyCollectionsItemsViewDTO
            {
                CollectionId = collectionId,
                CollectionItemName = sponsorEntity.Title,
                CollectionItemId = sponsorEntity.TableId,
                Category = Strings.SponsorText,
                CollectionItemType = (int)CollectionItemType.Sponsor,
                Source = sponsorEntity.Organization,
            };
        }
    }
}