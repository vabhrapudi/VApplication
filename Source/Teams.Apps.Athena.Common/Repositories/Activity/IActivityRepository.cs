// <copyright file="IActivityRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Models.Enums;

    /// <summary>
    /// Interface for Activity Repository.
    /// </summary>
    public interface IActivityRepository : IRepository<ActivityEntity>
    {
        /// <summary>
        /// Gets the partition key.
        /// </summary>
        /// <param name="itemId">The item Id.</param>
        /// <param name="itemType">The item type.</param>
        /// <returns>Gets the partition key string.</returns>
        string GetPartitionKey(string itemId, Itemtype itemType);
    }
}
