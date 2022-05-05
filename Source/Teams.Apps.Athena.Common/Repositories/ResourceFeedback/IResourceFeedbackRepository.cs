// <copyright file="IResourceFeedbackRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>
namespace Teams.Apps.Athena.Common.Repositories.ResourceFeedback
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for resource feedback Data repository.
    /// </summary>
    public interface IResourceFeedbackRepository : IRepository<ResourceFeedback>
    {
        /// <summary>
        /// Fetch resource feedback based on resource type.
        /// </summary>
        /// <param name="resourceType">Resource type Id.</param>
        /// <param name="resourceIds">List of resource Ids</param>
        /// <param name="userObjectId">User object Id.</param>
        /// <returns>List of matching resource feedback entities.</returns>
        Task<IEnumerable<ResourceFeedback>> GetFeedbackByResourceTypeAsync(int resourceType, IEnumerable<string> resourceIds, string userObjectId);
    }
}