// <copyright file="MustBeAdminPolicyHandler.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authorization.Policies
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Athena.Models;
    using Teams.Apps.Athena.Common;
    using Teams.Apps.Athena.Common.Extensions;
    using Teams.Apps.Athena.Common.Repositories;
    using Teams.Apps.Athena.Services.MicrosoftGraph;

    /// <summary>
    /// This handler authorizes user to verify whether a user is admin.
    /// </summary>
    public class MustBeAdminPolicyHandler : AuthorizationHandler<MustBeAdminPolicyRequirement>
    {
        private readonly ITeamRepository teamRepository;
        private readonly ITeamService teamService;
        private readonly IOptions<BotSettings> botOptions;
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="MustBeAdminPolicyHandler"/> class.
        /// </summary>
        /// <param name="teamRepository">The instance of <see cref="TeamRepository"/> class.</param>
        /// <param name="teamService">The instance of <see cref="TeamService"/> class.</param>
        /// <param name="botOptions">The options for application configuration.</param>
        /// <param name="memoryCache">Memory cache instance for caching authorization result.</param>
        public MustBeAdminPolicyHandler(
            ITeamRepository teamRepository,
            ITeamService teamService,
            IOptions<BotSettings> botOptions,
            IMemoryCache memoryCache)
        {
            this.teamRepository = teamRepository;
            this.teamService = teamService;
            this.botOptions = botOptions;
            this.memoryCache = memoryCache;
        }

        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeAdminPolicyRequirement requirement)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Resource is AuthorizationFilterContext resource)
            {
                var oidClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == Constants.OidClaimType);

                if (oidClaim == null || oidClaim.Value.IsEmptyOrInvalidGuid())
                {
                    context.Fail();
                    await Task.CompletedTask;
                    return;
                }

                var userAadId = oidClaim.Value.ToString();
                bool isValueAvailableInCache = this.memoryCache.TryGetValue(this.GetCacheKey(userAadId), out bool isUserAdmin);

                if (isValueAvailableInCache)
                {
                    if (isUserAdmin)
                    {
                        context.Succeed(requirement);
                    }

                    await Task.CompletedTask;
                    return;
                }

                // Get the details of admin team.
                var teamDetails = await this.teamRepository.GetAsync(TeamTableMetadata.TeamPartitionKey, this.botOptions.Value.AdminTeamId);

                if (teamDetails == null)
                {
                    await Task.CompletedTask;
                    return;
                }

                var teamMembers = await this.teamService.GetTeamMembersAsync(teamDetails.GroupId);
                var isUserMemberOfTeam = teamMembers.Any(teamMember => teamMember.UserId == userAadId);

                this.memoryCache.Set(this.GetCacheKey(userAadId), isUserMemberOfTeam, TimeSpan.FromMinutes(this.botOptions.Value.AdminDetailsCacheDurationInMinutes));

                if (isUserMemberOfTeam)
                {
                    context.Succeed(requirement);
                }
            }

            await Task.CompletedTask;
        }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="userAadId">The user AAD Id.</param>
        /// <returns>The cache key.</returns>
        private string GetCacheKey(string userAadId)
        {
            return $"is_admin_{userAadId}";
        }
    }
}
