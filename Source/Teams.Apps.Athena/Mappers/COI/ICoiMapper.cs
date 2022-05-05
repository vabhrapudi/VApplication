// <copyright file="ICoiMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to COI.
    /// </summary>
    public interface ICoiMapper
    {
        /// <summary>
        /// Maps COI view model to COI entity model to create new COI request.
        /// </summary>
        /// <param name="coiViewModel">The COI entity view model.</param>
        /// <param name="userAadId">The user AAD Id.</param>
        /// <param name="upn">The user's user principle name (UPN).</param>
        /// <returns>The COI entity model.</returns>
        CommunityOfInterestEntity MapForCreateModel(CoiEntityDTO coiViewModel, Guid userAadId, string upn);

        /// <summary>
        /// Maps COI view model to COI entity model to create draft COI request.
        /// </summary>
        /// <param name="draftCoiViewModel">The COI entity view model.</param>
        /// <param name="userAadId">The user AAD Id.</param>
        /// <param name="upn">The user's user principle name (UPN).</param>
        /// <returns>The COI entity model.</returns>
        CommunityOfInterestEntity MapForCreateDraftModel(DraftCoiEntityDTO draftCoiViewModel, Guid userAadId, string upn);

        /// <summary>
        /// Maps COI view model to update provided COI entity model.
        /// </summary>
        /// <param name="coiViewModel">The COI view model details to be mapped.</param>
        /// <param name="coiEntityModel">The COI entity model to which details to be mapped.</param>
        void MapForUpdateModel(CoiEntityDTO coiViewModel, CommunityOfInterestEntity coiEntityModel);

        /// <summary>
        /// Maps COI view model to update provided COI entity model.
        /// </summary>
        /// <param name="draftCoiViewModel">The draft COI view model details to be mapped.</param>
        /// <param name="coiEntityModel">The COI entity model to which details to be mapped.</param>
        public void MapForUpdateModel(DraftCoiEntityDTO draftCoiViewModel, CommunityOfInterestEntity coiEntityModel);

        /// <summary>
        /// Maps COI entity model to COI view model.
        /// </summary>
        /// <param name="coiEntityModel">The COI entity model.</param>
        /// <returns>The COI entity view model.</returns>
        CoiEntityDTO MapForViewModel(CommunityOfInterestEntity coiEntityModel);

        /// <summary>
        /// Gets coi entity from database sent to be as api response.
        /// </summary>
        /// <param name="communityOfInterest">Community of interest entity from database.</param>
        /// <returns>Returns a coi entity model.</returns>
        public CoiRequestViewDTO MapForRequestApprovalViewModel(CommunityOfInterestEntity communityOfInterest);
    }
}
