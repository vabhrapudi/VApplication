// <copyright file="INewsMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to news.
    /// </summary>
    public interface INewsMapper
    {
        /// <summary>
        /// Maps news view model to news entity model to create new request.
        /// </summary>
        /// <param name="newsViewModel">The news entity view model.</param>
        /// <param name="userAadId">The user AAD Id.</param>
        /// <param name="upn">The user's user principle name (UPN).</param>
        /// <returns>The news entity model.</returns>
        NewsEntity MapForCreateModel(NewsEntityDTO newsViewModel, Guid userAadId, string upn);

        /// <summary>
        /// Maps news view model to news entity model to create draft news request.
        /// </summary>
        /// <param name="newsViewModel">The news entity view model.</param>
        /// <param name="userAadId">The user AAD Id.</param>
        /// <param name="upn">The user's user principle name (UPN).</param>
        /// <returns>The news entity model.</returns>
        NewsEntity MapForCreateDraftModel(DraftNewsEntityDTO newsViewModel, Guid userAadId, string upn);

        /// <summary>
        /// Maps news view model to update provided news entity model.
        /// </summary>
        /// <param name="newsViewModel">The news view model details to be mapped.</param>
        /// <param name="newsEntityModel">The news entity model to which details to be mapped.</param>
        void MapForUpdateModel(NewsEntityDTO newsViewModel, NewsEntity newsEntityModel);

        /// <summary>
        /// Maps news view model to update provided news entity model.
        /// </summary>
        /// <param name="draftNewsViewModel">The draft news view model details to be mapped.</param>
        /// <param name="newsEntityModel">The news entity model to which details to be mapped.</param>
        void MapForUpdateModel(DraftNewsEntityDTO draftNewsViewModel, NewsEntity newsEntityModel);

        /// <summary>
        /// Maps news entity model to news view model.
        /// </summary>
        /// <param name="newsEntityModel">The news entity model.</param>
        /// <returns>The news entity view model.</returns>
        NewsEntityDTO MapForViewModel(NewsEntity newsEntityModel);

        /// <summary>
        /// Gets news entity from database sent to be as api response.
        /// </summary>
        /// <param name="newsEntity">News entity from database.</param>
        /// <returns>Returns a news entity model.</returns>
        public NewsRequestViewDTO MapForApprovalRequestViewModel(NewsEntity newsEntity);
    }
}
