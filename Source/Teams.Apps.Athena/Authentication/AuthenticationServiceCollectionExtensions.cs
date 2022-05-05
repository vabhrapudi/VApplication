// <copyright file="AuthenticationServiceCollectionExtensions.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Teams.Athena.Models.Configuration;
    using Teams.Apps.Athena.Common.Extensions;

    /// <summary>
    /// Extension class for registering authentication services in Dependency Injection container.
    /// </summary>
    public static class AuthenticationServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method to register the authentication services.
        /// </summary>
        /// <param name="services">IServiceCollection instance.</param>
        /// <param name="configuration">IConfiguration instance.</param>
        public static void RegisterAuthenticationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            services.AddProtectedWebApi(configuration)
                .AddProtectedWebApiCallsProtectedWebApi(configuration)
                .AddInMemoryTokenCaches();

            // This works specifically for single tenant application.
            var azureSettings = new AzureSettings();
            configuration.Bind("AzureAd", azureSettings);
            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = $"{azureSettings.Instance}/{azureSettings.TenantId}/v2.0";
                options.SaveToken = true;
                options.TokenValidationParameters.ValidAudiences = new List<string> { azureSettings.ClientId, azureSettings.ApplicationIdURI.ToUpperInvariant() };
                options.TokenValidationParameters.AudienceValidator = AuthenticationServiceCollectionExtensions.AudienceValidator;
                options.TokenValidationParameters.ValidIssuers = (azureSettings.ValidIssuers?
                    .Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries)?
                    .Select(p => p.Trim())).Select(validIssuer => validIssuer.Replace("TENANT_ID", azureSettings.TenantId, StringComparison.OrdinalIgnoreCase));
            });
        }

        /// <summary>
        /// Check whether a audience is valid or not.
        /// </summary>
        /// <param name="tokenAudiences">A collection of audience token.</param>
        /// <param name="securityToken">A security token.</param>
        /// <param name="validationParameters">Contains a set of parameters that are used by a Microsoft.IdentityModel.Tokens.SecurityTokenHandler
        /// when validating a Microsoft.IdentityModel.Tokens.SecurityToken.</param>
        /// <returns>A boolean value represents validity of audience.</returns>
        private static bool AudienceValidator(
            IEnumerable<string> tokenAudiences,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            if (tokenAudiences.IsNullOrEmpty())
            {
                throw new ApplicationException("No audience defined in token!");
            }

            var validAudiences = validationParameters.ValidAudiences;
            if (validAudiences.IsNullOrEmpty())
            {
                throw new ApplicationException("No valid audiences defined in validationParameters!");
            }

            return tokenAudiences.Intersect(tokenAudiences).Any();
        }
    }
}