// <copyright file="ISyncJobRecordRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for Sync Job Record Data Repository.
    /// </summary>
    public interface ISyncJobRecordRepository : IRepository<SyncJobRecordEntity>
    {
        /// <summary>
        /// Gets the sync job record based on FileName.
        /// </summary>
        /// <param name="syncJobName">The sync job name.</param>
        /// <returns>Returns the sync job record data.</returns>
        Task<SyncJobRecordEntity> GetSyncJobRecordAsync(string syncJobName);
    }
}
