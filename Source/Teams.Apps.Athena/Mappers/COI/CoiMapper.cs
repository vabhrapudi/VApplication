// <copyright file="CoiMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using System.Linq;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides mapper methods for COI model mappings.
    /// </summary>
    public class CoiMapper : ICoiMapper
    {
        private const string KeywordIdsSeparator = " ";

        /// <inheritdoc/>
        public CommunityOfInterestEntity MapForCreateDraftModel(DraftCoiEntityDTO draftCoiViewModel, Guid userAadId, string upn)
        {
            if (draftCoiViewModel == null)
            {
                throw new ArgumentNullException(nameof(draftCoiViewModel));
            }

            return new CommunityOfInterestEntity
            {
                TableId = Guid.NewGuid().ToString(),
                CoiId = draftCoiViewModel.CoiId,
                CoiName = draftCoiViewModel.CoiName.Trim(),
                CoiDescription = draftCoiViewModel.CoiDescription?.Trim(),
                Status = (int)CoiRequestStatus.Draft,
                Type = draftCoiViewModel.Type,
                KeywordNames = draftCoiViewModel.KeywordsJson == null ? null : string.Join(";", draftCoiViewModel.KeywordsJson.Select(keyword => keyword.Title)),
                Keywords = draftCoiViewModel.KeywordsJson == null ? null : string.Join(KeywordIdsSeparator, draftCoiViewModel.KeywordsJson.Select(keyword => keyword.KeywordId)),
                CreatedByObjectId = userAadId.ToString(),
                CreatedByUserPrincipalName = upn,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };
        }

        /// <inheritdoc/>
        public CommunityOfInterestEntity MapForCreateModel(CoiEntityDTO coiViewModel, Guid userAadId, string upn)
        {
            if (coiViewModel == null)
            {
                throw new ArgumentNullException(nameof(coiViewModel));
            }

            return new CommunityOfInterestEntity
            {
                TableId = Guid.NewGuid().ToString(),
                CoiId = coiViewModel.CoiId,
                CoiName = coiViewModel.CoiName.Trim(),
                CoiDescription = coiViewModel.CoiDescription.Trim(),
                Status = (int)CoiRequestStatus.Pending,
                Type = coiViewModel.Type,
                KeywordNames = string.Join(";", coiViewModel.KeywordsJson.Select(keyword => keyword.Title)),
                Keywords = string.Join(KeywordIdsSeparator, coiViewModel.KeywordsJson.Select(keyword => keyword.KeywordId)),
                CreatedByObjectId = userAadId.ToString(),
                CreatedByUserPrincipalName = upn,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
            };
        }

        /// <inheritdoc/>
        public void MapForUpdateModel(CoiEntityDTO coiViewModel, CommunityOfInterestEntity coiEntityModel)
        {
            if (coiViewModel == null)
            {
                throw new ArgumentNullException(nameof(coiViewModel));
            }

            if (coiEntityModel == null)
            {
                throw new ArgumentNullException(nameof(coiEntityModel));
            }

            coiEntityModel.CoiName = coiViewModel.CoiName.Trim();
            coiEntityModel.CoiDescription = coiViewModel.CoiDescription.Trim();
            coiEntityModel.KeywordNames = string.Join(";", coiViewModel.KeywordsJson.Select(keyword => keyword.Title));
            coiEntityModel.Keywords = string.Join(KeywordIdsSeparator, coiViewModel.KeywordsJson.Select(keyword => keyword.KeywordId));
            coiEntityModel.Type = coiViewModel.Type;
            coiEntityModel.UpdatedOn = DateTime.UtcNow;
        }

        /// <inheritdoc/>
        public void MapForUpdateModel(DraftCoiEntityDTO draftCoiViewModel, CommunityOfInterestEntity coiEntityModel)
        {
            if (draftCoiViewModel == null)
            {
                throw new ArgumentNullException(nameof(draftCoiViewModel));
            }

            if (coiEntityModel == null)
            {
                throw new ArgumentNullException(nameof(coiEntityModel));
            }

            coiEntityModel.CoiName = draftCoiViewModel.CoiName.Trim();
            coiEntityModel.CoiDescription = draftCoiViewModel.CoiDescription?.Trim();
            coiEntityModel.KeywordNames = draftCoiViewModel.KeywordsJson == null ? null : string.Join(";", draftCoiViewModel.KeywordsJson.Select(keyword => keyword.Title));
            coiEntityModel.Keywords = draftCoiViewModel.KeywordsJson == null ? null : string.Join(KeywordIdsSeparator, draftCoiViewModel.KeywordsJson.Select(keyword => keyword.KeywordId));
            coiEntityModel.Type = draftCoiViewModel.Type;
            coiEntityModel.UpdatedOn = DateTime.UtcNow;
        }

        /// <inheritdoc/>
        public CoiEntityDTO MapForViewModel(CommunityOfInterestEntity coiEntityModel)
        {
            if (coiEntityModel == null)
            {
                throw new ArgumentNullException(nameof(coiEntityModel));
            }

            return new CoiEntityDTO
            {
                TableId = coiEntityModel.TableId,
                CoiId = coiEntityModel.CoiId,
                CoiName = coiEntityModel.CoiName,
                CoiDescription = coiEntityModel.CoiDescription,
                Keywords = string.IsNullOrWhiteSpace(coiEntityModel.Keywords) ? Array.Empty<int>() : Array.ConvertAll(coiEntityModel.Keywords.Split(KeywordIdsSeparator), int.Parse),
                Type = coiEntityModel.Type,
                Status = coiEntityModel.Status,
                CreatedOn = coiEntityModel.CreatedOn,
                CreatedBy = coiEntityModel.CreatedByObjectId,
                TeamId = coiEntityModel.TeamId,
                NodeTypeId = coiEntityModel.NodeTypeId,
                SecurityLevel = coiEntityModel.SecurityLevel,
                NumberOfRatings = coiEntityModel.NumberOfRatings,
                SumOfRatings = coiEntityModel.SumOfRatings,
                Organization = coiEntityModel.Organization,
                WebSite = coiEntityModel.WebSite,
                UpdatedOn = coiEntityModel.UpdatedOn,
            };
        }

        /// <summary>
        /// Gets coi entity from database sent to be as api response.
        /// </summary>
        /// <param name="communityOfInterest">Community of interest entity from database.</param>
        /// <returns>Returns a coi entity model.</returns>
        public CoiRequestViewDTO MapForRequestApprovalViewModel(CommunityOfInterestEntity communityOfInterest)
        {
            communityOfInterest = communityOfInterest ?? throw new ArgumentNullException(nameof(communityOfInterest));
            return new CoiRequestViewDTO
            {
                TableId = communityOfInterest.TableId,
                CoiId = communityOfInterest.CoiId,
                CoiName = communityOfInterest.CoiName,
                CoiDescription = communityOfInterest.CoiDescription,
                Type = communityOfInterest.Type,
                GroupLink = communityOfInterest.GroupLink,
                ImageLink = communityOfInterest.ImageLink,
                CreatedOn = communityOfInterest.CreatedOn,
                CreatedBy = communityOfInterest.CreatedByObjectId,
                Status = communityOfInterest.Status,
                Keywords = string.IsNullOrWhiteSpace(communityOfInterest.Keywords) ? Array.Empty<int>() : Array.ConvertAll(communityOfInterest.Keywords.Split(KeywordIdsSeparator), int.Parse),
            };
        }
    }
}