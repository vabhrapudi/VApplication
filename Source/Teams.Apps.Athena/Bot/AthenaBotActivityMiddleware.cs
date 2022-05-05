// <copyright file="AthenaBotActivityMiddleware.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Bot
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Athena.Models;
    using Teams.Apps.Athena.Resources;

    /// <summary>
    /// A class that represents middleware that can operate on incoming activities.
    /// </summary>
    public class AthenaBotActivityMiddleware : IMiddleware
    {
        /// <summary>
        /// Represents a set of key/value application configuration properties for Bot.
        /// </summary>
        private readonly IOptions<BotSettings> options;

        /// <summary>
        /// Logger implementation to send logs to the logger service.
        /// </summary>
        private readonly ILogger<AthenaBotActivityMiddleware> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaBotActivityMiddleware"/> class.
        /// </summary>
        /// <param name="options"> A set of key/value application configuration properties.</param>
        /// <param name="logger">Logger implementation to send logs to the logger service.</param>
        public AthenaBotActivityMiddleware(IOptions<BotSettings> options, ILogger<AthenaBotActivityMiddleware> logger)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.logger = logger;
        }

        /// <summary>
        ///  Processes an incoming activity in middleware.
        /// </summary>
        /// <param name="turnContext">The context object for this turn.</param>
        /// <param name="next">The delegate to call to continue the bot middleware pipeline.</param>
        /// <param name="cancellationToken"> A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns><see cref="Task"/> A task that represents the work queued to execute.</returns>
        /// <remarks>
        /// Middleware calls the next delegate to pass control to the next middleware in
        /// the pipeline. If middleware doesn’t call the next delegate, the adapter does
        /// not call any of the subsequent middleware’s request handlers or the bot’s receive
        /// handler, and the pipeline short circuits.
        /// The turnContext provides information about the incoming activity, and other data
        /// needed to process the activity.
        /// </remarks>
        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
            next = next ?? throw new ArgumentNullException(nameof(next));

            if (turnContext.Activity?.Type != ActivityTypes.Event && !this.IsActivityFromExpectedTenant(turnContext))
            {
                this.logger.LogWarning($"Unexpected tenant id {turnContext.Activity?.Conversation?.TenantId}");

                if (turnContext.Activity?.Type == ActivityTypes.Message)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(Strings.InvalidTenantText));
                }
            }
            else
            {
                await next(cancellationToken);
            }
        }

        /// <summary>
        /// Verify if the tenant Id in the message is the same tenant Id used when application was configured.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <returns>True if context is from expected tenant else false.</returns>
        private bool IsActivityFromExpectedTenant(ITurnContext turnContext)
        {
            return turnContext.Activity?.Conversation?.TenantId == this.options.Value.TenantId;
        }
    }
}