// <copyright file="MustBeUserPolicyHandler.cs" company="NPS Foundation">
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
    /// This handler authorizes user to verify whether a user is valid Athena user.
    /// </summary>
    public class MustBeUserPolicyHandler : AuthorizationHandler<MustBeUserPolicyRequirement>
    {
        private readonly IUserBotConversationRepository userBotConversationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MustBeUserPolicyHandler"/> class.
        /// </summary>
        /// <param name="userBotConversationRepository">The instance of <see cref="UserBotConversationRepository"/>.</param>
        public MustBeUserPolicyHandler(IUserBotConversationRepository userBotConversationRepository)
        {
            this.userBotConversationRepository = userBotConversationRepository;
        }

        /// <inheritdoc/>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeUserPolicyRequirement requirement)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            var oidClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == Constants.OidClaimType);

            if (!(context.Resource is AuthorizationFilterContext resource)
                || oidClaim?.Value == null
                || oidClaim.Value.IsEmptyOrInvalidGuid())
            {
                context.Fail();
                await Task.CompletedTask;

                return;
            }

            var userBotConversation = await this.userBotConversationRepository.GetAsync(UserBotConversationTableMetadata.PartitionKey, oidClaim.Value);

            if (userBotConversation != null)
            {
                context.Succeed(requirement);
            }

            await Task.CompletedTask;
        }
    }
}
