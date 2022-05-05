// <copyright file="NewsSyncJobHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Helpers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Mappers;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Services.Search.News;

    /// <summary>
    /// Provides the helper methods for news sync job.
    /// </summary>
    public class NewsSyncJobHelper : INewsSyncJobHelper
    {
        /// <summary>
        /// The instance of <see cref="NewsBlobRepository" /> class.
        /// </summary>
        private readonly INewsBlobRepository newsBlobRepository;

        /// <summary>
        /// The instance of <see cref="NewsRepository" /> class.
        /// </summary>
        private readonly INewsRepository newsRepository;

        /// <summary>
        /// The instance of <see cref="SyncJobRecordRepository" /> class.
        /// </summary>
        private readonly ISyncJobRecordRepository syncJobRecordRepository;

        /// <summary>
        /// The instance of <see cref="NewsSyncJobMapper" /> class.
        /// </summary>
        private readonly INewsSyncJobMapper newsSyncMapper;

        /// <summary>
        /// The instance of <see cref="SyncJobRecordMapper" /> class.
        /// </summary>
        private readonly ISyncJobRecordMapper syncJobRecordMapper;

        /// <summary>
        /// The instance of <see cref="NewsSyncJobStatusRecordMapper"/> class.
        /// </summary>
        private readonly INewsSyncJobStatusRecordMapper newsSyncJobStatusRecordMapper;

        /// <summary>
        /// The instance of <see cref="NewsSyncJobStatusRecordRepository"/> class.
        /// </summary>
        private readonly INewsSyncJobStatusRecordRepository newsSyncJobStatusRecordRepository;

        /// <summary>
        /// The instance of <see cref="NewsSearchService"/> class.
        /// </summary>
        private readonly INewsSearchService newsSearchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsSyncJobHelper"/> class.
        /// </summary>
        /// <param name="newsBlobRepository">The instance of <see cref="NewsBlobRepository" /> class.</param>
        /// <param name="newsRepository">The instance of <see cref="NewsRepository" /> class.</param>
        /// <param name="syncJobRecordRepository">The instance of <see cref="SyncJobRecordRepository" /> class.</param>
        /// <param name="newsSyncMapper">The instance of <see cref="NewsSyncJobMapper" /> class.</param>
        /// <param name="syncJobRecordMapper">The instance of <see cref="SyncJobRecordMapper"/></param>"
        /// <param name="newsSyncJobStatusRecordMapper">The instance of <see cref="NewsSyncJobStatusRecordMapper"/> class.</param>
        /// <param name="newsSyncJobStatusRecordRepository">The instance of <see cref="NewsSyncJobStatusRecordRepository"/> class.</param>
        /// <param name="newsSearchService">The in stance of <see cref="NewsSearchService"/> class.</param>
        public NewsSyncJobHelper (
            INewsBlobRepository newsBlobRepository,
            INewsRepository newsRepository,
            ISyncJobRecordRepository syncJobRecordRepository,
            INewsSyncJobMapper newsSyncMapper,
            ISyncJobRecordMapper syncJobRecordMapper,
            INewsSyncJobStatusRecordMapper newsSyncJobStatusRecordMapper,
            INewsSyncJobStatusRecordRepository newsSyncJobStatusRecordRepository,
            INewsSearchService newsSearchService)
        {
            this.newsBlobRepository = newsBlobRepository;
            this.newsRepository = newsRepository;
            this.syncJobRecordRepository = syncJobRecordRepository;
            this.newsSyncMapper = newsSyncMapper;
            this.syncJobRecordMapper = syncJobRecordMapper;
            this.newsSyncJobStatusRecordMapper = newsSyncJobStatusRecordMapper;
            this.newsSyncJobStatusRecordRepository = newsSyncJobStatusRecordRepository;
            this.newsSearchService = newsSearchService;
        }

        /// <inheritdoc/>
        public async Task CreateOrUpdateNewsJsonDataAsync(DateTime lastRunAt)
        {
            try
            {
                var newsRecordUpdate = this.syncJobRecordMapper.MapToCreateOrUpdateModel(false, lastRunAt);
                await this.syncJobRecordRepository.InsertOrMergeAsync(newsRecordUpdate);

                var jsonData = await this.newsBlobRepository.GetNewsJsonData(lastRunAt);
                DateTime jsonDataReadAt = DateTime.UtcNow;

                var jsonDataBatches = jsonData.Split(1000);

                foreach (var jsonDataBatch in jsonDataBatches)
                {
                    var tasks = jsonDataBatch.Select(async jsonRecord =>
                    {
                        var existingRecord = await this.newsRepository.GetNewsDetailsByNewsId(jsonRecord.NewsId);

                        if (existingRecord != null)
                        {
                            try
                            {
                                var entity = this.newsSyncMapper.MapForUpdateModel(existingRecord, jsonRecord);
                                await this.newsRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to update news article with news Id: {jsonRecord.NewsId}. The exception details: {ex.Message}");
                            }
                        }
                        else
                        {
                            try
                            {
                                var entity = this.newsSyncMapper.MapForCreateModel(jsonRecord);
                                await this.newsRepository.CreateOrUpdateAsync(entity);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception($"Failed to insert news article with news Id: {jsonRecord.NewsId}. The exception details: {ex.Message}");
                            }
                        }
                    });
                    await Task.WhenAll(tasks);
                }

                var newsSyncJobStatusRecordEntity = this.newsSyncJobStatusRecordMapper.MapToCreateModel(true, null);
                await this.newsSyncJobStatusRecordRepository.CreateOrUpdateAsync(newsSyncJobStatusRecordEntity);

                var syncJobRecordEntity = this.syncJobRecordMapper.MapToCreateOrUpdateModel(true, jsonDataReadAt);
                await this.syncJobRecordRepository.InsertOrMergeAsync(syncJobRecordEntity);

                await this.newsSearchService.RunIndexerOnDemandAsync();
            }
            catch (Exception)
            {
                var syncJobRecordEntity = this.syncJobRecordMapper.MapToCreateOrUpdateModel(true, lastRunAt);
                await this.syncJobRecordRepository.InsertOrMergeAsync(syncJobRecordEntity);
                throw;
            }
        }
    }
}
