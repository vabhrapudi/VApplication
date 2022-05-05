// <copyright file="NewsSyncJobStatusRecordMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Mappers
{
    using System;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Provide methods related news sync job status record model mappings.
    /// </summary>
    public class NewsSyncJobStatusRecordMapper : INewsSyncJobStatusRecordMapper
    {
        /// <inheritdoc/>
        public NewsSyncJobStatusRecordEntity MapToCreateModel(bool hasSucceeded, string resesonForFailure)
        {
            return new NewsSyncJobStatusRecordEntity
            {
                NewsSyncJobId = Guid.NewGuid().ToString(),
                HasSucceeded = hasSucceeded,
                NewsSyncJobRanAt = DateTime.UtcNow,
                ReasonForFailure = resesonForFailure,
            };
        }
    }
}
