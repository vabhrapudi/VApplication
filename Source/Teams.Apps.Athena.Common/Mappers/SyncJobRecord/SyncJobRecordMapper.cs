// <copyright file="SyncJobRecordMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provide methods related sync job record model mappings.
    /// </summary>
    public class SyncJobRecordMapper : ISyncJobRecordMapper
    {
        /// <inheritdoc/>
        public SyncJobRecordEntity MapToCreateOrUpdateModel(bool status, DateTime lastRunAt)
        {
            return new SyncJobRecordEntity()
            {
                SyncJob = SyncJobNames.NewsSyncJob,
                IsLastRunSuccessful = status,
                LastRunAt = lastRunAt,
            };
        }
    }
}
