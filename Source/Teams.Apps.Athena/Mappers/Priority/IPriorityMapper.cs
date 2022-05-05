// <copyright file="IPriorityMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Mappers
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Exposes mapper methods related to priority.
    /// </summary>
    public interface IPriorityMapper
    {
        /// <summary>
        /// Maps priority entity model to priority view model.
        /// </summary>
        /// <param name="priorityEntity">The Athena priority entity model.</param>
        /// <returns>The priority view model.</returns>
        PriorityDTO MapForViewModel(PriorityEntity priorityEntity);

        /// <summary>
        /// Maps priority view model to priority entity model.
        /// </summary>
        /// <param name="priorityDTO">The priority view model.</param>
        /// <param name="userAadId">The user AAD Id who is creating the priority.</param>
        /// <returns>The priority entity model</returns>
        PriorityEntity MapForCreateModel(PriorityDTO priorityDTO, string userAadId);

        /// <summary>
        /// Maps priority view model to priority entity model.
        /// </summary>
        /// <param name="priorityDTO">The priority view model.</param>
        /// <param name="priorityEntity">The priority entity model.</param>
        /// <param name="userAadId">The user AAD Id who is creating the priority.</param>
        /// <returns>The priority entity model</returns>
        PriorityEntity MapForUpdateModel(PriorityDTO priorityDTO, PriorityEntity priorityEntity, string userAadId);
    }
}
