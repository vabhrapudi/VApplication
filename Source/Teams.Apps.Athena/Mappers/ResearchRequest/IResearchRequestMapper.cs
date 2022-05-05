// <copyright file="IResearchRequestMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to research requests.
    /// </summary>
    public interface IResearchRequestMapper
    {
        /// <summary>
        /// Maps research requests entity model to research requests view model.
        /// </summary>
        /// <param name="researchRequestEntity">The research requests entity model.</param>
        /// <returns>The news entity view model.</returns>
        ResearchRequestViewDTO MapForViewModel(ResearchRequestEntity researchRequestEntity);
    }
}
