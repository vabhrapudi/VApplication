// <copyright file="IResearchProjectMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes model mapper methods related to reserach projects.
    /// </summary>
    public interface IResearchProjectMapper
    {
        /// <summary>
        /// Maps research project entity model to view model.
        /// </summary>
        /// <param name="researchProjectEntity">The research project entity model.</param>
        /// <returns>The research project view model.</returns>
        public ResearchProjectDTO MapForViewModel(ResearchProjectEntity researchProjectEntity);

        /// <summary>
        /// Maps research project create model to entity model.
        /// </summary>
        /// <param name="researchProjectCreateDTO">The research project create model.</param>
        /// <returns>The research project entity model.</returns>
        public ResearchProjectEntity MapForCreateModel(ResearchProjectCreateDTO researchProjectCreateDTO);
    }
}
