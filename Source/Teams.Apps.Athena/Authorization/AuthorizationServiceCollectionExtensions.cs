// <copyright file="AuthorizationServiceCollectionExtensions.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authorization
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.DependencyInjection;
    using Teams.Apps.Athena.Authorization.Policies;

    /// <summary>
    /// Provides the extension method for <see cref="IServiceCollection"/> to register authorization
    /// services in dependency injection container.
    /// </summary>
    public static class AuthorizationServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method to register authorization policies.
        /// </summary>
        /// <param name="services">The instance of <see cref="IServiceCollection"/>.</param>
        public static void RegisterAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    AuthorizationPolicyNames.MustBeCreatorOfCoiRequestPolicy,
                    policyBuilder => policyBuilder.AddRequirements(new MustBeCreatorOfCoiRequestPolicyRequirement()));

                options.AddPolicy(
                    AuthorizationPolicyNames.MustBeCreatorOfNewsArticleRequestPolicy,
                    policyBuilder => policyBuilder.AddRequirements(new MustBeCreatorOfNewsArticleRequestPolicyRequirement()));

                options.AddPolicy(
                    AuthorizationPolicyNames.MustBeUser,
                    policyBuilder => policyBuilder.AddRequirements(new MustBeUserPolicyRequirement()));

                options.AddPolicy(
                    AuthorizationPolicyNames.MustBeTeamOwnerPolicy,
                    policyBuilder => policyBuilder.AddRequirements(new MustBeTeamOwnerPolicyRequirement()));

                options.AddPolicy(
                    AuthorizationPolicyNames.MustBeTeamMemberPolicy,
                    policyBuilder => policyBuilder.AddRequirements(new MustBeTeamMemberPolicyRequirement()));

                options.AddPolicy(
                    AuthorizationPolicyNames.MustBeAdminPolicy,
                    policyBuilder => policyBuilder.AddRequirements(new MustBeAdminPolicyRequirement()));
            });

            services.AddTransient<IAuthorizationHandler, MustBeCreatorOfCoiRequestPolicyHandler>();
            services.AddTransient<IAuthorizationHandler, MustBeCreatorOfNewsArticleRequestPolicyHandler>();
            services.AddTransient<IAuthorizationHandler, MustBeUserPolicyHandler>();
            services.AddTransient<IAuthorizationHandler, MustBeTeamOwnerPolicyHandler>();
            services.AddTransient<IAuthorizationHandler, MustBeTeamMemberPolicyHandler>();
            services.AddTransient<IAuthorizationHandler, MustBeAdminPolicyHandler>();
        }
    }
}
