// <copyright file="MustBeCreatorOfCoiRequestPolicyHandler.cs" company="NPS Foundation">
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
    /// This handler authorizes user to allow accessing/modifying COI request if the user is
    /// owner of the COI request which being accessed.
    /// </summary>
    public class MustBeCreatorOfCoiRequestPolicyHandler : AuthorizationHandler<MustBeCreatorOfCoiRequestPolicyRequirement>
    {
        private const string TableIdQueryParamKey = "tableId";

        private readonly ICoiRepository coiRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MustBeCreatorOfCoiRequestPolicyHandler"/> class.
        /// </summary>
        /// <param name="coiRepository">The instance of <see cref="ICoiRepository"/>.</param>
        public MustBeCreatorOfCoiRequestPolicyHandler(ICoiRepository coiRepository)
        {
            this.coiRepository = coiRepository;
        }

        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeCreatorOfCoiRequestPolicyRequirement requirement)
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
                var coiRequest = await this.coiRepository.GetAsync(CoiTableMetadata.PartitionKey, tableId.ToString());

                if (coiRequest != null && coiRequest.CreatedByObjectId == oidClaim?.Value)
                {
                    context.Succeed(requirement);
                }
            }

            await Task.CompletedTask;
        }
    }
}
