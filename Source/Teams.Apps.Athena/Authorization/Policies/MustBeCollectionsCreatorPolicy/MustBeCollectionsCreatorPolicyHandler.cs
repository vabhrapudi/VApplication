// <copyright file="MustBeCollectionsCreatorPolicyHandler.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authorization.Policies
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// This authorization handler is created to handle manager access policy.
    /// The class implements AuthorizationHandler for handling MustBeCollectionsCreatorPolicyRequirement authorization.
    /// </summary>
    public class MustBeCollectionsCreatorPolicyHandler : IAuthorizationHandler
    {
        /// <summary>
        /// Instance of repository for fetching valid collections.
        /// </summary>
        private readonly IMyCollectionsRepository myCollectionsRepository;

        /// <summary>
        /// The instance of HTTP context accessors.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MustBeCollectionsCreatorPolicyHandler"/> class.
        /// </summary>
        /// <param name="myCollectionsRepository">Instance of repository for fetching valid collections.</param>
        /// <param name="httpContextAccessor">The instance of HTTP context accessors.</param>
        public MustBeCollectionsCreatorPolicyHandler(
             IMyCollectionsRepository myCollectionsRepository,
             IHttpContextAccessor httpContextAccessor)
        {
            this.myCollectionsRepository = myCollectionsRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// This method handles the authorization requirement.
        /// </summary>
        /// <param name="context">AuthorizationHandlerContext instance.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            var oidClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";

            var oidClaim = context.User.Claims.FirstOrDefault(p => oidClaimType == p.Type);

            foreach (var requirement in context.Requirements)
            {
                if (requirement is MustBeCollectionsCreatorRequirement)
                {
                    if (context.Resource is AuthorizationFilterContext authorizationFilterContext)
                    {
                        var isValuePresent = authorizationFilterContext.HttpContext.Request.RouteValues.TryGetValue("collectionId", out object collectionIdFromRoute);

                        if (isValuePresent)
                        {
                            var collectionId = collectionIdFromRoute.ToString();
                            if (await this.ValidateIfManagerCreatedCollectionAsync(Guid.Parse(oidClaim.Value), collectionId))
                            {
                                context.Succeed(requirement);
                            }
                        }
                    }
                }
            }
        }

        private async Task<bool> ValidateIfManagerCreatedCollectionAsync(Guid userAadObjectId, string collectionId)
        {
            var collections = await this.myCollectionsRepository.GetAsync(MyCollectionsTableMetadata.MyCollectionsPartition, collectionId);
            if (collections.CreatedBy == userAadObjectId.ToString())
            {
                return true;
            }

            return false;
        }
    }
}
