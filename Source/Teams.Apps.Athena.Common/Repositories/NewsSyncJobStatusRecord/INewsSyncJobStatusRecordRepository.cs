// <copyright file="INewsSyncJobStatusRecordRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for News Sync Status Record Data Repository.
    /// </summary>
    public interface INewsSyncJobStatusRecordRepository : IRepository<NewsSyncJobStatusRecordEntity>
    {
    }
}
