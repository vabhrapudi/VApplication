// <copyright file="GraphTokenProvider.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Authentication
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.Graph;
    using Microsoft.Identity.Web;
    using AthenaConstants = Teams.Apps.Athena.Constants.Constants;

    /// <summary>
    /// Add access token to Microsoft Graph API.
    /// </summary>
    public class GraphTokenProvider : IAuthenticationProvider
    {
        /// <summary>
        /// Get the default graph scope.
        /// </summary>
        private const string ScopeDefault = "https://graph.microsoft.com/.default";

        /// <summary>
        /// Authorization scheme.
        /// </summary>
        private const string BearerAuthorizationScheme = "Bearer";

        /// <summary>
        /// Token acquisition instance to fetch authentication token on-behalf of user.
        /// </summary>
        private readonly ITokenAcquisition tokenAcquisition;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphTokenProvider"/> class.
        /// </summary>
        /// <param name="tokenAcquisition">MSAL.NET token acquisition service.</param>
        public GraphTokenProvider(ITokenAcquisition tokenAcquisition)
        {
            this.tokenAcquisition = tokenAcquisition ?? throw new ArgumentNullException(nameof(tokenAcquisition));
        }

        /// <summary>
        /// Intercepts HttpRequest and add Bearer token.
        /// </summary>
        /// <param name="request">Represents a HttpRequestMessage.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            request = request ?? throw new ArgumentNullException(nameof(request), "HTTP request cannot be null");

            var permissionType = this.ExtractPermissionType(request.Headers);
            string accessToken = await this.GetAccessTokenAsync(permissionType);
            request.Headers.Remove(AthenaConstants.PermissionTypeKey);

            // Append the access token to the request.
            request.Headers.Authorization = new AuthenticationHeaderValue(BearerAuthorizationScheme, accessToken);
        }

        /// <summary>
        /// Gets access token for user (for delegate permission type) or application (for application permission type).
        /// </summary>
        /// <param name="permissionType">Microsoft Graph permission type. See <see cref="GraphPermissionType"/></param>
        /// <returns>Access token for provided permission.</returns>
        private async Task<string> GetAccessTokenAsync(string permissionType)
        {
            string accessToken;
            if (permissionType.Equals(GraphPermissionType.Application.ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                // Using MSAL.NET to get a token to call the API for application.
                accessToken = await this.tokenAcquisition.GetAccessTokenForAppAsync(new[] { ScopeDefault });
            }
            else
            {
                // Using MSAL.NET to get a token to call the API On Behalf Of the current user.
                accessToken = await this.tokenAcquisition.GetAccessTokenForUserAsync(new[] { ScopeDefault });
            }

            return accessToken;
        }

        /// <summary>
        /// Extract permission type from HTTP request header.
        /// </summary>
        /// <param name="headers">Collection of HTTP request headers.</param>
        /// <returns>Returns permission type if permission header key is present or empty string.</returns>
        private string ExtractPermissionType(HttpRequestHeaders headers)
        {
            if (headers != null && headers.Contains(AthenaConstants.PermissionTypeKey))
            {
                var permissionType = headers.GetValues(AthenaConstants.PermissionTypeKey).FirstOrDefault();
                return permissionType;
            }

            return string.Empty;
        }
    }
}