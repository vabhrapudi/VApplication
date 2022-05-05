﻿// <copyright file="BotController.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;

    /// <summary>
    /// The BotController is responsible for connecting the Asp.Net MVC pipeline to the <see cref="IBotFrameworkHttpAdapter" />
    /// and the underlying activity handler (<see cref="IBot" />).
    /// </summary>
    [Route("/api/messages")]
    [ApiController]
    public class BotController : ControllerBase
    {
        /// <summary>
        /// The BotFramework adapter.
        /// </summary>
        private readonly IBotFrameworkHttpAdapter botFrameworkHttpAdapter;

        /// <summary>
        /// The underlying activity handler.
        /// </summary>
        private readonly IBot activityHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotController"/> class.
        /// </summary>
        /// <param name="adapter">The BotFramework adapter.</param>
        /// <param name="activityHandler">The underlying activity handler.</param>
        public BotController(IBotFrameworkHttpAdapter adapter, IBot activityHandler)
        {
            this.botFrameworkHttpAdapter = adapter;
            this.activityHandler = activityHandler;
        }

        /// <summary>
        /// Handle inbound messages from the BotFramework service.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token for the underlying request.</param>
        /// <returns>A Task that resolves when the message is processed.</returns>
        [HttpPost]
        public async Task PostAsync(CancellationToken cancellationToken)
        {
            await this.botFrameworkHttpAdapter
                .ProcessAsync(
                    httpRequest: this.Request,
                    httpResponse: this.Response,
                    bot: this.activityHandler,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}