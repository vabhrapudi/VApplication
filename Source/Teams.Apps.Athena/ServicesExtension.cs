// <copyright file="ServicesExtension.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Azure.Search;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Graph;
    using Microsoft.Identity.Client;
    using Microsoft.Teams.Athena.Models;
    using Microsoft.Teams.Athena.Models.Configuration;
    using Teams.Apps.Athena.Authentication;
    using Teams.Apps.Athena.Bot;
    using Teams.Apps.Athena.Common.Blobs;
    using Teams.Apps.Athena.Common.Helpers;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Common.Repositories.GraduationDegree;
    using Teams.Apps.Athena.Common.Repositories.Organization;
    using Teams.Apps.Athena.Common.Repositories.ResourceFeedback;
    using Teams.Apps.Athena.Common.Repositories.Specialty;
    using Teams.Apps.Athena.Common.Repositories.StudyDepartment;
    using Teams.Apps.Athena.Common.Services;
    using Teams.Apps.Athena.Common.Services.Keywords;
    using Teams.Apps.Athena.Common.Services.Search;
    using Teams.Apps.Athena.Common.Services.Search.News;
    using Teams.Apps.Athena.Helpers;
    using Teams.Apps.Athena.Mappers;
    using Teams.Apps.Athena.Models.Configuration;
    using Teams.Apps.Athena.Services.AdaptiveCard;
    using Teams.Apps.Athena.Services.MicrosoftGraph;

    /// <summary>
    /// Class to extend ServiceCollection.
    /// </summary>
    public static class ServicesExtension
    {
        /// <summary>
        /// Adds localization settings to specified IServiceCollection.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void RegisterLocalizationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var defaultCulture = CultureInfo.GetCultureInfo(configuration.GetValue<string>("i18n:DefaultCulture"));
                var supportedCultures = configuration.GetValue<string>("i18n:SupportedCultures").Split(',')
                    .Select(culture => CultureInfo.GetCultureInfo(culture))
                    .ToList();

                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new AthenaLocalizationCultureProvider(),
                };
            });
        }

        /// <summary>
        /// Adds application configuration settings to specified IServiceCollection.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        public static void RegisterConfigurationSettings(this IServiceCollection services)
        {
            services.AddOptions<RepositoryOptions>()
                .Configure<IConfiguration>((repositoryOptions, configuration) =>
                {
                    repositoryOptions.StorageAccountConnectionString =
                        configuration.GetValue<string>("StorageAccountConnectionString");

                    // Setting this to true because the main application should ensure that all
                    // tables exist.
                    repositoryOptions.EnsureTableExists = true;
                });

            services.AddOptions<StorageSettings>()
                .Configure<IConfiguration>((options, configuration) =>
            {
                options.ConnectionString = configuration.GetValue<string>("StorageAccountConnectionString");
            });

            services.AddOptions<BotSettings>()
                .Configure<IConfiguration>((options, configuration) =>
                {
                    options.MicrosoftAppId = configuration.GetValue<string>("AzureAd:ClientId");
                    options.MicrosoftAppPassword = configuration.GetValue<string>("AzureAd:ClientSecret");
                    options.TenantId = configuration.GetValue<string>("AzureAD:TenantId");
                    options.AppBaseUri = configuration.GetValue<string>("App:AppBaseUri");
                    options.NewsPageSize = configuration.GetValue<int>("App:NewsPageSize");
                    options.CardCacheDurationInHour = configuration.GetValue<int>("App:CardCacheDurationInHour");
                    options.AdminTeamId = configuration.GetValue<string>("App:AdminTeamId");
                    options.UserManifestId = configuration.GetValue<string>("App:UserManifestId");
                    options.AadUserDetailsCacheDurationInDays = configuration.GetValue<int>("App:AadUserDetailsCacheDurationInDays");
                    options.AdminDetailsCacheDurationInMinutes = configuration.GetValue<int>("App:AdminDetailsCacheDurationInMinutes");
                    options.KeywordsCacheDurationInHours = configuration.GetValue<int>("App:KeywordsCacheDurationInHours");
                });

            services.AddOptions<AzureSettings>()
                .Configure<IConfiguration>((azureSettings, configuration) =>
                {
                    azureSettings.ApplicationIdURI = configuration.GetValue<string>("AzureAd:ApplicationIdURI");
                    azureSettings.ValidIssuers = configuration.GetValue<string>("AzureAd:ValidIssuers");
                    azureSettings.GraphScope = configuration.GetValue<string>("AzureAd:GraphScope");
                    azureSettings.TenantId = configuration.GetValue<string>("AzureAd:TenantId");
                    azureSettings.ClientId = configuration.GetValue<string>("AzureAd:ClientId");
                    azureSettings.Instance = configuration.GetValue<string>("AzureAd:Instance");
                });

            services.AddOptions<SearchServiceOptions>()
                .Configure<IConfiguration>((searchServiceOptions, configuration) =>
                {
                    searchServiceOptions.SearchServiceAdminApiKey = configuration.GetValue<string>("SearchService:AdminApiKey");
                    searchServiceOptions.SearchServiceName = configuration.GetValue<string>("SearchService:Name");
                    searchServiceOptions.SearchServiceQueryApiKey = configuration.GetValue<string>("SearchService:QueryApiKey");
                });
        }

        /// <summary>
        /// Add confidential credential provider to access api.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void RegisterConfidentialCredentialProvider(this IServiceCollection services, IConfiguration configuration)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            IConfidentialClientApplication confidentialClientApp = ConfidentialClientApplicationBuilder.Create(configuration["App:MicrosoftAppId"])
                .WithClientSecret(configuration["App:MicrosoftAppPassword"])
                .Build();
            services.AddSingleton<IConfidentialClientApplication>(confidentialClientApp);
        }

        /// <summary>
        /// Adds credential providers for authentication.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void RegisterCredentialProviders(this IServiceCollection services, IConfiguration configuration)
        {
            ICredentialProvider credentialProvider = new SimpleCredentialProvider(
                appId: configuration.GetValue<string>("App:MicrosoftAppId"),
                password: configuration.GetValue<string>("App:MicrosoftAppPassword"));

            services
                .AddSingleton(credentialProvider);
        }

        /// <summary>
        /// Register repositories for performing database operations.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IFaqRepository, FaqRepository>();
            services.AddSingleton<INewsRepository, NewsRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ICoiRepository, CoiRepository>();
            services.AddSingleton<IGraduationDegreeRepository, GraduationDegreeRepository>();
            services.AddSingleton<IOrganizationRepository, OrganizationRepository>();
            services.AddSingleton<IStudyDepartmentRepository, StudyDepartmentRepository>();
            services.AddSingleton<ISpecialtyRepository, SpecialtyRepository>();
            services.AddSingleton<IMyCollectionsRepository, MyCollectionsRepository>();
            services.AddSingleton<ICoiRepository, CoiRepository>();
            services.AddSingleton<ITeamRepository, TeamRepository>();
            services.AddSingleton<IResourceFeedbackRepository, ResourceFeedbackRepository>();
            services.AddSingleton<IUserBotConversationRepository, UserBotConversationRepository>();
            services.AddSingleton<IAthenaFeedbackRepository, AthenaFeedbackRepository>();
            services.AddSingleton<IActivityRepository, ActivityRepository>();
            services.AddSingleton<IDiscoveryTreeFiltersBlobRepository, DiscoveryTreeFiltersBlobRepository>();
            services.AddSingleton<IDiscoveryTreeTaxonomyBlobRepository, DiscoveryTreeTaxonomyBlobRepository>();
            services.AddSingleton<INodeTypeForDiscoveryTreeBlobRepository, NodeTypeForDiscoveryTreeBlobRepository>();
            services.AddSingleton<ISponsorRepository, SponsorRepository>();
            services.AddSingleton<IResearchProjectsRepository, ResearchProjectsRepository>();
            services.AddSingleton<IResearchRequestsRepository, ResearchRequestsRepository>();
            services.AddSingleton<IPartnersRepository, PartnersRepository>();
            services.AddSingleton<IEventsRepository, EventsRepository>();
            services.AddSingleton<ICommentsRepository, CommentsRepository>();
            services.AddSingleton<IResearchProposalsRepository, ResearchProposalsRepository>();
            services.AddSingleton<ISecurityLevelBlobRepository, SecurityLevelBlobRepository>();
            services.AddSingleton<IUserPersistentDataRepository, UserPersistentDataRepository>();
            services.AddSingleton<IKeywordsBlobRepository, KeywordsBlobRepository>();
            services.AddSingleton<IQuickAccessRepository, QuickAccessRepository>();
            services.AddSingleton<IHomeConfigurationsRepository, HomeConfigurationsRepository>();
            services.AddSingleton<IHomeStatusBarConfigurationRepository, HomeStatusBarConfigurationRepository>();
            services.AddSingleton<IAthenaInfoResourcesRepository, AthenaInfoResourcesRepository>();
            services.AddSingleton<IAthenaResearchImportanceBlobRepository, AthenaResearchImportanceBlobRepository>();
            services.AddSingleton<IAthenaResearchPriorityBlobRepository, AthenaResearchPriorityBlobRepository>();
            services.AddSingleton<IAthenaPrioritiesRepository, AthenaPrioritiesRepository>();
            services.AddSingleton<IAthenaPriorityTypesBlobRepository, AthenaPriorityTypesBlobRepository>();
            services.AddSingleton<IAthenaToolsRepository, AthenaToolsRepository>();
            services.AddSingleton<IAthenaNewsSourcesBlobRepository, AthenaNewsSourcesBlobRepository>();
            services.AddSingleton<IAthenaResearchSourcesBlobRepository, AthenaResearchSourcesBlobRepository>();
            services.AddSingleton<IAthenaNewsKeywordsBlobRepository, AthenaNewsKeywordsBlobRepository>();
            services.AddSingleton<IJobTitleBlobRepository, JobTitleBlobRepository>();
            services.AddSingleton<IUserBlobRepository, UserBlobRepository>();
            services.AddSingleton<IAthenaInfoResourceBlobRepository, AthenaInfoResourceBlobRepository>();
            services.AddSingleton<ICommunityOfInterestBlobRepository, CommunityOfInterestBlobRepository>();
            services.AddSingleton<IAthenaResearchProjectBlobRepository, AthenaResearchProjectBlobRespository>();
            services.AddSingleton<IAthenaIngestionRepository, AthenaIngestionRepository>();
            services.AddSingleton<IAthenaFileNamesBlobRepository, AthenaFileNamesBlobRepository>();
            services.AddSingleton<IAthenaToolsRepository, AthenaToolsRepository>();
            services.AddSingleton<IAthenaToolsBlobRepository, AthenaToolsBlobRepository>();
            services.AddSingleton<IAthenaResearchProposalsBlobRepository, AthenaResearchProposalsBlobRepository>();
            services.AddSingleton<IResearchRequestBlobRepository, ResearchRequestBlobRepository>();
            services.AddSingleton<IAthenaEventsBlobRepository, AthenaEventsBlobRepository>();
            services.AddSingleton<ISponsorBlobRepository, SponsorBlobRepository>();
            services.AddSingleton<IPartnerBlobRepository, PartnerBlobRepository>();
        }

        /// Register helpers for performing database operations.
        /// <summary>
        /// Register helpers.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        public static void RegisterHelpers(this IServiceCollection services)
        {
            services.AddScoped<IAthenaBotActivityHandlerHelper, AthenaBotActivityHandlerHelper>();
            services.AddSingleton<IAdaptiveCardHelper, AdaptiveCardHelper>();
            services.AddSingleton<IKeywordsHelper, KeywordsHelper>();
            services.AddScoped<IUserSettingsHelper, UserSettingsHelper>();
            services.AddScoped<ICoiHelper, CoiHelper>();
            services.AddScoped<ICoiRequestHelper, CoiRequestHelper>();
            services.AddScoped<INewsHelper, NewsHelper>();
            services.AddScoped<INewsRequestHelper, NewsRequestHelper>();
            services.AddSingleton<IFilterQueryHelper, FilterQueryHelper>();
            services.AddSingleton<IGraduationDegreeHelper, GraduationDegreeHelper>();
            services.AddSingleton<IOrganizationHelper, OrganizationHelper>();
            services.AddSingleton<ISpecialtyHelper, SpecialtyHelper>();
            services.AddSingleton<IStudyDepartmentHelper, StudyDepartmentHelper>();
            services.AddScoped<IUserGraphServiceHelper, UserGraphServiceHelper>();
            services.AddSingleton<IAdaptiveCardHelper, AdaptiveCardHelper>();
            services.AddSingleton<INotificationHelper, NotificationHelper>();
            services.AddScoped<IMyCollectionsHelper, MyCollectionsHelper>();
            services.AddScoped<IAdminHelper, AdminHelper>();
            services.AddScoped<IFeedbackHelper, FeedbackHelper>();
            services.AddScoped<IDiscoveryTreeHelper, DiscoveryTreeHelper>();
            services.AddSingleton<ISponsorHelper, SponsorHelper>();
            services.AddSingleton<IResearchProjectHelper, ResearchProjectHelper>();
            services.AddSingleton<IPartnerHelper, PartnerHelper>();
            services.AddSingleton<IEventHelper, EventHelper>();
            services.AddSingleton<IResearchRequestHelper, ResearchRequestHelper>();
            services.AddSingleton<ICommentsHelper, CommentsHelper>();
            services.AddSingleton<IResearchProposalHelper, ResearchProposalHelper>();
            services.AddSingleton<ISecurityLevelHelper, SecurityLevelHelper>();
            services.AddSingleton<IUserPersistentDataHelper, UserPersistentDataHelper>();
            services.AddSingleton<IQuickAccessHelper, QuickAccessHelper>();
            services.AddSingleton<IHomeConfigurationHelper, HomeConfigurationHelper>();
            services.AddSingleton<IHomeStatusBarConfigurationHelper, HomeStatusBarConfigurationHelper>();
            services.AddSingleton<IAthenaInfoResourcesHelper, AthenaInfoResourcesHelper>();
            services.AddScoped<IHomeHelper, HomeHelper>();
            services.AddScoped<IPriorityHelper, PriorityHelper>();
            services.AddSingleton<IAthenaToolHelper, AthenaToolHelper>();
            services.AddSingleton<IJobTitleHelper, JobTitleHelper>();
            services.AddSingleton<IAthenaIngestionHelper, AthenaIngestionHelper>();
        }

        /// <summary>
        /// Register services.
        /// </summary>
        /// <param name="services">The collection of services.</param>
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IAdaptiveCardService, AdaptiveCardService>();
            services.AddSingleton<INewsMapper, NewsMapper>();
            services.AddSingleton<ICoiMapper, CoiMapper>();
            services.AddSingleton<IUserGraphServiceMapper, UserGraphServiceMapper>();
        }

        /// <summary>
        /// Register service for mapping model.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        public static void RegisterMappers(this IServiceCollection services)
        {
            services.AddSingleton<IUserSettingsMapper, UserSettingsMapper>();
            services.AddSingleton<ICoiMapper, CoiMapper>();
            services.AddSingleton<INewsMapper, NewsMapper>();
            services.AddSingleton<IUserGraphServiceMapper, UserGraphServiceMapper>();
            services.AddSingleton<IMyCollectionsMapper, MyCollectionsMapper>();
            services.AddSingleton<IResearchProjectMapper, ResearchProjectMapper>();
            services.AddSingleton<IResearchRequestMapper, ResearchRequestMapper>();
            services.AddSingleton<ISponsorsMapper, SponsorsMapper>();
            services.AddSingleton<IPartnerMapper, PartnerMapper>();
            services.AddSingleton<IAthenaEventMapper, AthenaEventMapper>();
            services.AddSingleton<ICommentsMapper, CommentsMapper>();
            services.AddSingleton<IResearchProposalMapper, ResearchProposalMapper>();
            services.AddSingleton<IUserPersistentDataMapper, UserPersistentDataMapper>();
            services.AddSingleton<IKeywordMapper, KeywordMapper>();
            services.AddSingleton<IQuickAccessMapper, QuickAccessMapper>();
            services.AddSingleton<IHomeConfigurationMapper, HomeConfigurationMapper>();
            services.AddSingleton<IHomeStatusBarConfigurationMapper, HomeStatusBarConfigurationMapper>();
            services.AddSingleton<IAthenaInfoResourceMapper, AthenaInfoResourceMapper>();
            services.AddSingleton<IHomeMapper, HomeMapper>();
            services.AddSingleton<IAthenaFeedbackMapper, AthenaFeedbackMapper>();
            services.AddSingleton<IPriorityMapper, PriorityMapper>();
            services.AddSingleton<IAthenaToolMapper, AthenaToolMapper>();
            services.AddSingleton<IAthenaIngestionMapper, AthenaIngestionMapper>();
        }

        /// <summary>
        /// Register search services to search records.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void RegisterSearchServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISearchServiceClient>(new SearchServiceClient(configuration.GetValue<string>("SearchService:Name"), new SearchCredentials(configuration.GetValue<string>("SearchService:AdminApiKey"))));
            services.AddSingleton<ICoiSearchService, CoiSearchService>();
            services.AddSingleton<INewsSearchService, NewsSearchService>();
            services.AddSingleton<IKeywordsSearchService, KeywordsSearchService>();
            services.AddSingleton<IUsersSearchService, UsersSearchService>();
            services.AddSingleton<IResearchProjectsSearchService, ResearchProjectsSearchService>();
            services.AddSingleton<IResearchRequestsSearchService, ResearchRequestsSearchService>();
            services.AddSingleton<ISponsorsSearchService, SponsorsSearchService>();
            services.AddSingleton<IPartnersSearchService, PartnersSearchService>();
            services.AddSingleton<IAthenaEventsSearchService, AthenaEventsSearchService>();
            services.AddSingleton<IResearchProposalsSearchService, ResearchProposalsSearchService>();
            services.AddSingleton<IAthenaInfoResourcesSearchService, AthenaInfoResourcesSearchService>();
            services.AddSingleton<IAthenaFeedbackSearchService, AthenaFeedbackSearchService>();
            services.AddSingleton<IAthenaToolsSearchServices, AthenaToolsSearchServices>();
        }

        /// <summary>
        /// Registers services such as MS Graph, token acquisition etc.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        public static void RegisterGraphServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationProvider, GraphTokenProvider>();
            services.AddScoped<IGraphServiceClient, GraphServiceClient>();
            services.AddScoped<IGraphServiceFactory, GraphServiceFactory>();
            services.AddScoped<IUserService>(sp => sp.GetRequiredService<IGraphServiceFactory>().GetUserService());
            services.AddScoped<ITeamService, TeamService>();
        }
    }
}