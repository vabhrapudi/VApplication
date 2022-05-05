// <copyright file="IHomeMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to home tab.
    /// </summary>
    public interface IHomeMapper
    {
        /// <summary>
        /// Maps research project view model to daily briefing home article view model.
        /// </summary>
        /// <param name="researchProjectDTO">The research project view model.</param>
        /// <returns>The daily briefing home article view model.</returns>
        DailyBriefingHomeArticleDTO MapForViewModel(ResearchProjectDTO researchProjectDTO);

        /// <summary>
        /// Maps research request view model to daily briefing home article view model.
        /// </summary>
        /// <param name="researchRequestViewDTO">The research request view model.</param>
        /// <returns>The daily briefing home article view model.</returns>
        DailyBriefingHomeArticleDTO MapForViewModel(ResearchRequestViewDTO researchRequestViewDTO);

        /// <summary>
        /// Maps research proposal view model to daily briefing home article view model.
        /// </summary>
        /// <param name="researchProposalViewDTO">The research proposal view model.</param>
        /// <returns>The daily briefing home article view model.</returns>
        DailyBriefingHomeArticleDTO MapForViewModel(ResearchProposalViewDTO researchProposalViewDTO);

        /// <summary>
        /// Maps event view model to daily briefing home article view model.
        /// </summary>
        /// <param name="eventDTO">The event view model.</param>
        /// <returns>The daily briefing home article view model.</returns>
        DailyBriefingHomeArticleDTO MapForViewModel(EventDTO eventDTO);

        /// <summary>
        /// Maps COI view model to daily briefing home article view model.
        /// </summary>
        /// <param name="coiEntityDTO">The COI view model.</param>
        /// <returns>The daily briefing home article view model.</returns>
        DailyBriefingHomeArticleDTO MapForViewModel(CoiEntityDTO coiEntityDTO);

        /// <summary>
        /// Maps news view model to daily briefing home article view model.
        /// </summary>
        /// <param name="newsEntityDTO">The news view model.</param>
        /// <returns>The daily briefing home article view model.</returns>
        DailyBriefingHomeArticleDTO MapForViewModel(NewsEntityDTO newsEntityDTO);

        /// <summary>
        /// Maps info resource view model to daily briefing home article view model.
        /// </summary>
        /// <param name="athenaInfoResourceDTO">The info resource view model.</param>
        /// <returns>The daily briefing home article view model.</returns>
        DailyBriefingHomeArticleDTO MapForViewModel(AthenaInfoResourceDTO athenaInfoResourceDTO);
    }
}
