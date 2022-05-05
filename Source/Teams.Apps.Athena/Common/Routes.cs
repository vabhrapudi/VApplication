// <copyright file="Routes.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The route for different UI components.
    /// </summary>
    public static class Routes
    {
        /// <summary>
        /// Route to display page related to approve or reject COI request.
        /// </summary>
        public const string ViewCoiRequest = "/new-request?isReadOnly=true&type=1&requestId={0}";

        /// <summary>
        /// Route to display page related to approve or reject COI request.
        /// </summary>
        public const string ViewNewsArticleRequest = "/new-request?isReadOnly=true&type=0&requestId={0}";

        /// <summary>
        /// The route for error page.
        /// </summary>
        public const string ErrorPage = "/error";

        /// <summary>
        /// The URL parameter type.
        /// </summary>
        private const string UrlParamRequestId = "requestId";

        /// <summary>
        /// The URL parameter type.
        /// </summary>
        private const string UrlParamRequestType = "type";

        /// <summary>
        /// Gets approve or reject request route.
        /// </summary>
        /// <param name="requestId">The request Id to be approved or rejected.</param>
        /// <param name="requestType">The request type.</param>
        /// <returns>The route to approve or reject request route.</returns>
        public static string GetApprovedOrRejectRequestRoute(string requestId, UserRequestType requestType)
        {
            return $"/approve-reject-requests?{UrlParamRequestId}={requestId}&{UrlParamRequestType}={(int)requestType}";
        }

        /// <summary>
        /// Gets readonly request route.
        /// </summary>
        /// <param name="requestId">The request Id.</param>
        /// <param name="requestType">The request type.</param>
        /// <returns>The route to approve or reject request route.</returns>
        public static string GetReadonlyRequestRoute(string requestId, UserRequestType requestType)
        {
            return $"/new-request?isReadOnly=true&{UrlParamRequestId}={requestId}&{UrlParamRequestType}={(int)requestType}";
        }
    }
}