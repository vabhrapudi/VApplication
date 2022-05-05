// <copyright file="MustBeTeamOwnerPolicyHandler.cs" company="NPS Foundation">
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
    using Teams.Apps.Athena.Services.MicrosoftGraph;

    /// <summary>
    /// This handler authorizes user to validate whether the user is owner of a team.
    /// </summary>
    public class MustBeTeamOwnerPolicyHandler : AuthorizationHandler<MustBeTeamOwnerPolicyRequirement>
    {
        private const string TeamIdQueryParamKey = "teamId";

        private readonly ITeamService teamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MustBeTeamOwnerPolicyHandler"/> class.
        /// </summary>
        /// <param name="teamService">The instance of <see cref="TeamService"/> class.</param>
        public MustBeTeamOwnerPolicyHandler(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeTeamOwnerPolicyRequirement requirement)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Resource is AuthorizationFilterContext resource
                && resource.HttpContext.Request.RouteValues.TryGetValue(TeamIdQueryParamKey, out object teamId)
                && teamId != null
                && !teamId.ToString().IsEmptyOrInvalidGuid())
            {
                var oidClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == Constants.OidClaimType);

                if (oidClaim == null || oidClaim.Value.IsEmptyOrInvalidGuid())
                {
                    context.Fail();
                    await Task.CompletedTask;
                    return;
                }

                var teamOwners = await this.teamService.GetTeamOwnersAsync(teamId.ToString());
                var isUserOwnerOfTeam = teamOwners.Any(teamOwner => teamOwner.Id == oidClaim.Value.ToString());

                if (isUserOwnerOfTeam)
                {
                    context.Succeed(requirement);
                }
            }

            await Task.CompletedTask;
        }
    }
}
