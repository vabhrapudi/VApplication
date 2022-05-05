// <copyright file="Startup.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

[assembly: Microsoft.Azure.Functions.Extensions.DependencyInjection.FunctionsStartup(
    typeof(Teams.Apps.Athena.AzureFunctions.Startup))]

namespace Teams.Apps.Athena.AzureFunctions
{
    using System;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.Search;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Mappers;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Services;
    using Teams.Apps.Athena.Common.Services.Search.News;

    /// <summary>
    /// Azure function Startup Class.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <summary>
        /// Application startup configuration.
        /// </summary>
        /// <param name="builder">Webjobs builder.</param>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder = builder ?? throw new ArgumentNullException(nameof(builder));
            this.ConfigureServices(builder.Services);
        }

        /// <summary>
        /// Configure the Dependency Injection Container (Composition Root).
        /// </summary>
        /// <param name="services">The DI Container.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions<RepositoryOptions>()
                .Configure<IConfiguration>((repositoryOptions, configuration) =>
                {
                    repositoryOptions.StorageAccountConnectionString =
                        configuration.GetValue<string>("StorageAccountConnectionString");
                });
            services.AddOptions<SearchServiceOptions>()
                .Configure<IConfiguration>((searchServiceOptions, configuration) =>
                {
                    searchServiceOptions.SearchServiceAdminApiKey = configuration.GetValue<string>("SearchServiceAdminApiKey");
                    searchServiceOptions.SearchServiceName = configuration.GetValue<string>("SearchServiceName");
                    searchServiceOptions.SearchServiceQueryApiKey = configuration.GetValue<string>("SearchServiceQueryApiKey");
                });
            services.AddSingleton<INewsBlobRepository, NewsBlobRepository>();
            services.AddSingleton<INewsRepository, NewsRepository>();
            services.AddSingleton<ISyncJobRecordRepository, SyncJobRecordRepository>();
            services.AddSingleton<INewsSyncJobStatusRecordRepository, NewsSyncJobStatusRecordRepository>();

            services.AddSingleton<INewsSyncJobMapper, NewsSyncJobMapper>();
            services.AddSingleton<INewsSyncJobStatusRecordMapper, NewsSyncJobStatusRecordMapper>();
            services.AddSingleton<ISyncJobRecordMapper, SyncJobRecordMapper>();

            services.AddSingleton<INewsSyncJobHelper, NewsSyncJobHelper>();

            services.AddSingleton<INewsSearchService, NewsSearchService>();
            services.AddSingleton<ISearchServiceClient>(new SearchServiceClient(Environment.GetEnvironmentVariable("SearchServiceName"), new SearchCredentials(Environment.GetEnvironmentVariable("SearchServiceAdminApiKey"))));
        }
    }
}
