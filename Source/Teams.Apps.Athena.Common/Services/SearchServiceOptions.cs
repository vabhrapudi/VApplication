// <copyright file="SearchServiceOptions.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Services
{
    /// <summary>
    /// A class that represents settings related to search service.
    /// </summary>
    public class SearchServiceOptions
    {
        /// <summary>
        /// Gets or sets search service name.
        /// </summary>
        public string SearchServiceName { get; set; }

        /// <summary>
        /// Gets or sets search service query API key.
        /// </summary>
        public string SearchServiceQueryApiKey { get; set; }

        /// <summary>
        /// Gets or sets search service admin API key.
        /// </summary>
        public string SearchServiceAdminApiKey { get; set; }
    }
}
