// <copyright file="GraphServiceFactory.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Services.MicrosoftGraph
{
    using System;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Microsoft.Graph;
    using Microsoft.Teams.Athena.Models;

    /// <summary>
    /// Graph service factory.
    /// </summary>
    public class GraphServiceFactory : IGraphServiceFactory
    {
        private readonly IGraphServiceClient serviceClient;

        private readonly IMemoryCache memoryCache;

        private readonly IOptions<BotSettings> botOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphServiceFactory"/> class.
        /// </summary>
        /// <param name="botOptions">The Bot options.</param>
        /// <param name="serviceClient">Microsoft Graph service client.</param>
        /// <param name="memoryCache">The instance of <see cref="MemoryCache"/>.</param>
        public GraphServiceFactory(
            IOptions<BotSettings> botOptions,
            IGraphServiceClient serviceClient,
            IMemoryCache memoryCache)
        {
            this.serviceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
            this.memoryCache = memoryCache;
            this.botOptions = botOptions;
        }

        /// <summary>
        /// Creates an instance of <see cref="IUserService"/> implementation.
        /// </summary>
        /// <returns>Returns an implementation of <see cref="IUserService"/>.</returns>
        public IUserService GetUserService()
        {
            return new UserService(this.botOptions, this.serviceClient, this.memoryCache);
        }
    }
}