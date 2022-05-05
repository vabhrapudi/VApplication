// <copyright file="Startup.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena
{
    using System;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Teams.Apps.Athena.Authentication;
    using Teams.Apps.Athena.Authorization;
    using Teams.Apps.Athena.Bot;

    /// <summary>
    /// The Startup class is responsible for configuring the DI container and acts as the composition root.
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The environment provided configuration.</param>
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Configure the composition root for the application.
        /// </summary>
        /// <param name="services">The stub composition root.</param>
        /// <remarks>
        /// For more information see: https://go.microsoft.com/fwlink/?LinkID=398940.
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(
                new MicrosoftAppCredentials(
                     this.configuration.GetValue<string>("App:MicrosoftAppId"),
                     this.configuration.GetValue<string>("App:MicrosoftAppPassword")));

            services.AddHttpContextAccessor();
            services.RegisterConfidentialCredentialProvider(this.configuration);

            services.AddControllers();
            services.AddMvc().AddMvcOptions(mvcopt => { mvcopt.EnableEndpointRouting = false; });
            services.AddSingleton<TelemetryClient>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services
                .AddApplicationInsightsTelemetry(this.configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"));

            services.RegisterCredentialProviders(this.configuration);
            services
                .AddTransient<IBotFrameworkHttpAdapter, BotFrameworkHttpAdapter>();

            services.RegisterLocalizationSettings(this.configuration);

            services.RegisterConfigurationSettings();

            services.RegisterRepositories();
            services.RegisterHelpers();
            services.RegisterMappers();
            services.RegisterServices();
            services.RegisterSearchServices(this.configuration);
            services.RegisterAuthenticationServices(this.configuration);
            services.RegisterGraphServices();
            services.RegisterAuthorizationPolicies();

            // In production, the React files will be served from this directory.
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSingleton<IChannelProvider, SimpleChannelProvider>();
            services.AddSingleton<IMemoryCache, MemoryCache>();

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddTransient<IBotFrameworkHttpAdapter, BotFrameworkHttpAdapter>();

            services.AddTransient<IBot, AthenaBotActivityHandler>();

            // Create the Activity middle-ware that will be added to the middle-ware pipeline in the AdapterWithErrorHandler.
            services.AddSingleton<AthenaBotActivityMiddleware>();
            services.AddTransient(serviceProvider => (BotFrameworkAdapter)serviceProvider.GetRequiredService<IBotFrameworkHttpAdapter>());
        }

        /// <summary>
        /// Configure the application request pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">Hosting Environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestLocalization();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.EnvironmentName.ToUpperInvariant() == "DEVELOPMENT")
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}