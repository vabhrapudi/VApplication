// <copyright file="MustBeTeamMemberPolicyHandler.cs" company="NPS Foundation">
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
    /// This handler authorizes user to validate whether the user is member of a team.
    /// </summary>
    public class MustBeTeamMemberPolicyHandler : AuthorizationHandler<MustBeTeamMemberPolicyRequirement>
    {
        private const string TeamIdQueryParamKey = "teamId";

        private readonly ITeamService teamService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MustBeTeamMemberPolicyHandler"/> class.
        /// </summary>
        /// <param name="teamService">The instance of <see cref="TeamService"/> class.</param>
        public MustBeTeamMemberPolicyHandler(ITeamService teamService)
        {
            this.teamService = teamService;
        }

        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeTeamMemberPolicyRequirement requirement)
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

                var teamMembers = await this.teamService.GetTeamMembersAsync(teamId.ToString());
                var isUserMemberOfTeam = teamMembers.Any(teamMember => teamMember.UserId == oidClaim.Value.ToString());

                if (isUserMemberOfTeam)
                {
                    context.Succeed(requirement);
                }
            }

            await Task.CompletedTask;
        }
    }
}