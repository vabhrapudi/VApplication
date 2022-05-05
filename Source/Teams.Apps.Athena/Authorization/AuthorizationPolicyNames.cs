// <copyright file="AuthorizationPolicyNames.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authorization
{
    /// <summary>
    /// Describes the names of authorization policies.
    /// </summary>
    public static class AuthorizationPolicyNames
    {
        /// <summary>
        /// Holds the policy name which authorize logged-in user to check whether a user is creator
        /// of COI request that is going to be accessed or modified.
        /// </summary>
        public const string MustBeCreatorOfCoiRequestPolicy = "MustBeCreatorOfCoiRequestPolicy";

        /// <summary>
        /// Holds the policy name which authorize logged-in user to check whether a user is creator
        /// of news article request that is going to be accessed or modified.
        /// </summary>
        public const string MustBeCreatorOfNewsArticleRequestPolicy = "MustBeCreatorOfNewsArticleRequestPolicy";

        /// <summary>
        /// Holds the policy name which authorize logged-in user to check whether a user is Athena user who successfully
        /// installed end-user APP.
        /// </summary>
        public const string MustBeUser = "MustBeUser";

        /// <summary>
        /// Holds the policy name which authorize logged-in user to check whether user is team owner.
        /// </summary>
        public const string MustBeTeamOwnerPolicy = "MustBeTeamOwnerPolicy";

        /// <summary>
        /// The name of the authorization policy, MustBeCollectionsCreatorPolicy. Indicates that user must be creator of the requested collection.
        /// </summary>
        public const string MustBeCollectionsCreatorPolicy = "MustBeCollectionsCreatorPolicy";

        /// <summary>
        /// Holds the policy name which authorize logged-in user to check whether user is team member.
        /// </summary>
        public const string MustBeTeamMemberPolicy = "MustBeTeamMemberPolicy";

        /// <summary>
        /// Holds the policy name which authorize logged-in user to check whether user is admin.
        /// </summary>
        public const string MustBeAdminPolicy = "MustBeAdminPolicy";
    }
}
