// <copyright file="SyncFunction.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.AzureFunctions
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Mappers;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Azure function to populate json data into table after specific time interval.
    /// </summary>
    public class SyncFunction
    {
        /// <summary>
        /// The instance of <see cref="Logger"/> class.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The instance of <see cref="SyncJobRecordRepository"/> class.
        /// </summary>
        private readonly ISyncJobRecordRepository syncJobRecordRepository;

        /// <summary>
        /// The instance of <see cref="NewsSyncJobStatusRecordMapper"/> class.
        /// </summary>
        private readonly INewsSyncJobStatusRecordMapper newsSyncJobStatusRecordMapper;

        /// <summary>
        /// The instance of <see cref="NewsSyncJobStatusRecordRepository"/> class.
        /// </summary>
        private readonly INewsSyncJobStatusRecordRepository newsSyncJobStatusRecordRepository;

        /// <summary>
        /// The instance of <see cref="NewsSyncJobHelper"/> class.
        /// </summary>
        private readonly INewsSyncJobHelper newsSyncJobHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncFunction"/> class.
        /// </summary>
        /// <param name="logger">The instance of <see cref="Logger"/> class.</param>
        /// <param name="syncJobRecordRepository">The instance of <see cref="SyncJobRecordRepository"/> class.</param>
        /// <param name="newsSyncJobStatusRecordMapper">The instance of <see cref="NewsSyncJobStatusRecordMapper"/> class.</param>
        /// <param name="newsSyncJobStatusRecordRepository">The instance of <see cref="NewsSyncJobStatusRecordRepository"/> class.</param>
        /// <param name="newsSyncJobHelper">The instance of <see cref="NewsSyncJobHelper"/> class.</param>
        public SyncFunction(
            ILogger<SyncFunction> logger,
            ISyncJobRecordRepository syncJobRecordRepository,
            INewsSyncJobStatusRecordMapper newsSyncJobStatusRecordMapper,
            INewsSyncJobStatusRecordRepository newsSyncJobStatusRecordRepository,
            INewsSyncJobHelper newsSyncJobHelper)
        {
            this.logger = logger;
            this.syncJobRecordRepository = syncJobRecordRepository;
            this.newsSyncJobStatusRecordMapper = newsSyncJobStatusRecordMapper;
            this.newsSyncJobStatusRecordRepository = newsSyncJobStatusRecordRepository;
            this.newsSyncJobHelper = newsSyncJobHelper;
        }

        /// <summary>
        /// Azure function to populate json data into table after specific time interval.
        /// </summary>
        /// <param name="myTimer">Timer instance with CRON expression.</param>
        /// <returns>The task representing the asynchronous operation.</returns>
        [FunctionName(SyncJobNames.NewsSyncJob)]
        public async Task Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer)
        {
            try
            {
                var syncJobRecord = await this.syncJobRecordRepository.GetSyncJobRecordAsync(SyncJobNames.NewsSyncJob);
                if (syncJobRecord != null)
                {
                    if (syncJobRecord.IsLastRunSuccessful == true)
                    {
                        await this.newsSyncJobHelper.CreateOrUpdateNewsJsonDataAsync(syncJobRecord.LastRunAt);
                        this.logger.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
                    }
                }
                else
                {
                    await this.newsSyncJobHelper.CreateOrUpdateNewsJsonDataAsync(DateTime.UtcNow);
                    this.logger.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
                }
            }
            catch (Exception ex)
            {
                var newsSyncJobStatusRecordEntity = this.newsSyncJobStatusRecordMapper.MapToCreateModel(false, ex.Message);
                await this.newsSyncJobStatusRecordRepository.CreateOrUpdateAsync(newsSyncJobStatusRecordEntity);
                this.logger.LogError(ex, $"News sync web job failed at: {DateTime.UtcNow}");
                throw;
            }
        }
    }
}
