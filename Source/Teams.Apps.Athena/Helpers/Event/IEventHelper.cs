// <copyright file="IEventHelper.cs" company="NPS Foundation">
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
    /// Exposes helper methods related to Athena event entity.
    /// </summary>
    public interface IEventHelper
    {
        /// <summary>
        /// Gets the Athena events by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The collection of <see cref="EventDTO"/>.</returns>
        Task<IEnumerable<EventDTO>> GetEventsByKeywordsIdsAsync(IEnumerable<int> keywordIds);

        /// <summary>
        /// Gets Athena events.
        /// </summary>
        /// <param name="searchParametersDTO">The advanced search parameters.</param>
        /// <returns>The collection Athena events.</returns>
        Task<IEnumerable<EventDTO>> GetEventsAsync(SearchParametersDTO searchParametersDTO);

        /// <summary>
        /// Gets a event by table Id.
        /// </summary>
        /// <param name="eventTableId">The event table Id of the research project to fetch.</param>
        /// <param name="userAadObjectId">The user aad object Id.</param>
        /// <returns>Returns event entity details.</returns>
        Task<EventDTO> GetEventByTableIdAsync(string eventTableId, string userAadObjectId);

        /// <summary>
        /// Stores rating of user for a event.
        /// </summary>
        /// <param name="eventTableId">The event table Id for which rating to be submitted.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="userAadObjectId">User Id who submitted rating.</param>
        /// <returns>Returns task indicating operation result.</returns>
        Task RateEventAsync(string eventTableId, int rating, string userAadObjectId);

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <param name="keywords">The collection of keywords.</param>
        /// <param name="fromDate">The date and time from which events to get.</param>
        /// <param name="count">The number of events to get.</param>
        /// <returns>The collection of events.</returns>
        Task<IEnumerable<EventDTO>> GetEventsAsync(IEnumerable<int> keywords, DateTime fromDate, int? count);
    }
}
