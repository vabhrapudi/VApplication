// <copyright file="BaseController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Mvc;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// Base controller to handle API operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private const string ClaimTypeNameForUserName = "name";

        /// <summary>
        /// Holds the instance of application insights telemetry client.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="telemetryClient">The application insights telemetry client.</param>
        public BaseController(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        /// <summary>
        /// Gets the user tenant id from the HttpContext.
        /// </summary>
        protected string UserTenantId
        {
            get
            {
                var tenantClaimType = "http://schemas.microsoft.com/identity/claims/tenantid";
                var claim = this.User.Claims.FirstOrDefault(p => tenantClaimType.Equals(p.Type, StringComparison.OrdinalIgnoreCase));
                if (claim == null)
                {
                    return null;
                }

                return claim.Value;
            }
        }

        /// <summary>
        /// Gets the user Azure Active Directory id from the HttpContext.
        /// </summary>
        protected string UserAadId
        {
            get
            {
                var oidClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
                var claim = this.User.Claims.FirstOrDefault(p => oidClaimType.Equals(p.Type, StringComparison.OrdinalIgnoreCase));
                if (claim == null)
                {
                    return null;
                }

                return claim.Value;
            }
        }

        /// <summary>
        /// Gets the user name from the HttpContext.
        /// </summary>
        protected string Upn
        {
            get
            {
                var claim = this.User.Claims.FirstOrDefault(p => "name".Equals(p.Type, StringComparison.OrdinalIgnoreCase));
                if (claim == null)
                {
                    return null;
                }

                return claim.Value;
            }
        }

        /// <summary>
        /// Gets the user's full name.
        /// </summary>
        protected string UserName
        {
            get
            {
                var claim = this.User.Claims.FirstOrDefault(p => p.Type.Equals(ClaimTypeNameForUserName, StringComparison.OrdinalIgnoreCase));

                if (claim == null)
                {
                    return null;
                }

                return claim.Value;
            }
        }

        /// <summary>
        /// Records event data to Application Insights telemetry client.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="requestStatus">The status of a request which is being processed.</param>
        /// <param name="payload">Payload which needs to be logged against event.</param>
        public void RecordEvent(string eventName, RequestType requestStatus, IDictionary<string, string> payload = null)
        {
            var payloadDictionary = new Dictionary<string, string>
            {
                { "userId", this.UserAadId },
                { "requestType", Enum.GetName(typeof(RequestType), requestStatus) },
            };

            if (payload != null)
            {
                foreach (var item in payload)
                {
                    payloadDictionary.Add(item.Key, item.Value);
                }
            }

            this.telemetryClient.TrackEvent(eventName, payloadDictionary);
        }
    }
}
