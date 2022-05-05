// <copyright file="INewsSyncJobStatusRecordMapper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Mappers
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes mapper methods related to news sync job status.
    /// </summary>
    public interface INewsSyncJobStatusRecordMapper
    {
        /// <summary>
        /// Returns the news sync job status record.
        /// </summary>
        /// <param name="hasSucceeded">Indicates whether last function run is successful.</param>
        /// <param name="resesonForFailure">The reason for failure.</param>
        /// <returns>The news sync job status record entity.</returns>
        NewsSyncJobStatusRecordEntity MapToCreateModel(bool hasSucceeded, string resesonForFailure);
    }
}
