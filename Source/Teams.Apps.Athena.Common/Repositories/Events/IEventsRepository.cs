// <copyright file="IEventsRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes table operations related to events repository.
    /// </summary>
    public interface IEventsRepository : IRepository<EventEntity>
    {
        /// <summary>
        /// Gets the Event by Id
        /// </summary>
        /// <param name="eventId"> Event Id</param>
        /// <returns>The Event record</returns>
        Task<EventEntity> GetEventDetailsByEventIdAsync(int eventId);
    }
}
