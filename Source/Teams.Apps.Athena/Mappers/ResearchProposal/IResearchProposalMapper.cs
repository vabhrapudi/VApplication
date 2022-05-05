// <copyright file="IResearchProposalMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to research proposals.
    /// </summary>
    public interface IResearchProposalMapper
    {
        /// <summary>
        /// Maps research proposal entity model to research proposal view model.
        /// </summary>
        /// <param name="researchProposalEntity">The research proposal entity model.</param>
        /// <returns>The research proposal view model.</returns>
        ResearchProposalViewDTO MapForViewModel(ResearchProposalEntity researchProposalEntity);

        /// <summary>
        /// Maps research proposal create model to research proposal entity model.
        /// </summary>
        /// <param name="researchProposalCreateDTO">The research proposal create model.</param>
        /// <param name="userAadId">The user add Id.</param>
        /// <returns>The research proposal entity model.</returns>
        ResearchProposalEntity MapForCreateModel(ResearchProposalCreateDTO researchProposalCreateDTO, string userAadId);
    }
}
