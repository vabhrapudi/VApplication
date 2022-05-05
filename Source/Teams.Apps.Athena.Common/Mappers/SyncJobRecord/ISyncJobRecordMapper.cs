// <copyright file="ISyncJobRecordMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes mapper methods related to sync job record.
    /// </summary>
    public interface ISyncJobRecordMapper
    {
        /// <summary>
        /// Returns the sync job record.
        /// </summary>
        /// <param name="status">The status of sync job.</param>
        /// <param name="lastRunAt">The last run date and time.</param>
        /// <returns>The sync job record entity.</returns>
        SyncJobRecordEntity MapToCreateOrUpdateModel(bool status, DateTime lastRunAt);
    }
}
