// <copyright file="IPriorityHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Provides helper methods for managing priority.
    /// </summary>
    public interface IPriorityHelper
    {
        /// <summary>
        /// Gets priority details by priority Id.
        /// </summary>
        /// <param name="priorityId">The priority Id.</param>
        /// <returns>Returns priority details.</returns>
        Task<PriorityDTO> GetPriorityByIdAsync(string priorityId);

        /// <summary>
        /// Gets all the priorities.
        /// </summary>
        /// <returns>The collection of priorities.</returns>
        Task<IEnumerable<PriorityDTO>> GetPrioritiesAsync();

        /// <summary>
        /// Updates the priority.
        /// </summary>
        /// <param name="priorityDTO">The priority view DTO model.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <returns>Returns updated priority details.</returns>
        Task<PriorityDTO> UpdatePriorityAsync(PriorityDTO priorityDTO, string userAadId);

        /// <summary>
        /// Creates a new priority.
        /// </summary>
        /// <param name="priorityDTO">The priority view DTO model.</param>
        /// <param name="userAadId">The logged-in user AAD Id.</param>
        /// <returns>Returns priority details.</returns>
        Task<PriorityDTO> CreatePriorityAsync(PriorityDTO priorityDTO, string userAadId);

        /// <summary>
        /// Deletes the priorities.
        /// </summary>
        /// <param name="priorityIds">The collection of priority Ids.</param>
        /// <returns>A task representing delete priorities operation.</returns>
        Task DeletePrioritiesAsync(IEnumerable<Guid> priorityIds);

        /// <summary>
        /// Gets all the priority type data from json.
        /// </summary>
        /// <returns>Returns collection of priority Id and title of priority type from json file.</returns>
        Task<IEnumerable<PriorityType>> GetPriorityTypesAsync();

        /// <summary>
        /// Gets the priorities insights.
        /// </summary>
        /// <param name="priorityIds">The collection of priority Ids of which insights to get.</param>
        /// <param name="keywordIds">The collection of keyword Ids based on which insights data to filter.</param>
        /// <returns>The collection of priorities describing insights data.</returns>
        Task<IEnumerable<PriorityInsight>> GetPrioritiesInsightsAsync(IEnumerable<Guid> priorityIds, IEnumerable<int> keywordIds);
    }
}