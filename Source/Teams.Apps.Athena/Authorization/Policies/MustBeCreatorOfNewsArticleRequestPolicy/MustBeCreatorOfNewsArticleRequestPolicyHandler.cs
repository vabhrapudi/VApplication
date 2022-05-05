// <copyright file="MustBeCreatorOfNewsArticleRequestPolicyHandler.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authorization.Policies
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Teams.Apps.Athena.Common;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// This handler authorizes user to allow accessing/modifying news article request if the user is
    /// owner of the news article request which being accessed.
    /// </summary>
    public class MustBeCreatorOfNewsArticleRequestPolicyHandler : AuthorizationHandler<MustBeCreatorOfNewsArticleRequestPolicyRequirement>
    {
        private const string TableIdQueryParamKey = "tableId";

        private readonly INewsRepository newsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MustBeCreatorOfNewsArticleRequestPolicyHandler"/> class.
        /// </summary>
        /// <param name="newsRepository">The instance of <see cref="INewsRepository"/>.</param>
        public MustBeCreatorOfNewsArticleRequestPolicyHandler(INewsRepository newsRepository)
        {
            this.newsRepository = newsRepository;
        }

        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeCreatorOfNewsArticleRequestPolicyRequirement requirement)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Resource is AuthorizationFilterContext resource
                && resource.HttpContext.Request.RouteValues.TryGetValue(TableIdQueryParamKey, out object tableId)
                && tableId != null
                && !tableId.ToString().IsEmptyOrInvalidGuid())
            {
                var oidClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == Constants.OidClaimType);
                var newsArticleRequest = await this.newsRepository.GetAsync(NewsTableMetadata.NewsPartitionKey, tableId.ToString());

                if (newsArticleRequest != null && newsArticleRequest.CreatedBy == oidClaim?.Value)
                {
                    context.Succeed(requirement);
                }
            }

            await Task.CompletedTask;
        }
    }
}
