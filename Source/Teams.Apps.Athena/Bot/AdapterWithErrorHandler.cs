// <copyright file="AdapterWithErrorHandler.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Bot
{
    using System;
    using System.Net;
    using System.Threading;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Teams.Apps.Athena.Resources;

    /// <summary>
    /// A class that implements error handler.
    /// </summary>
    public class AdapterWithErrorHandler : BotFrameworkHttpAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdapterWithErrorHandler"/> class.
        /// </summary>
        /// <param name="configuration">Application configurations.</param>
        /// <param name="logger">Logger implementation to send logs to the logger service.</param>
        /// <param name="activityMiddleware">Represents middleware that can operate on incoming activities.</param>
        /// <param name="conversationState">A state management object for conversation state.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        public AdapterWithErrorHandler(
            IConfiguration configuration,
            ILogger<BotFrameworkHttpAdapter> logger,
            AthenaBotActivityMiddleware activityMiddleware,
            ConversationState conversationState = null,
            CancellationToken cancellationToken = default)
            : base(configuration)
        {
            activityMiddleware = activityMiddleware ?? throw new ArgumentNullException(nameof(activityMiddleware));

            // Add activity middleware to the adapter's middleware pipeline
            this.Use(activityMiddleware);

            this.OnTurnError = async (turnContext, exception) =>
            {
                var error = exception as ErrorResponseException;

                // Log any leaked exception from the application.
                logger.LogError(exception, $"Exception caught : {exception.Message}");

                // If Http error 'TooManyRequests' arises due to background service notification, do not send generic error message to user.
                if (error.Response.StatusCode != HttpStatusCode.TooManyRequests)
                {
                    // Send a catch-all apology to the user.
                    await turnContext.SendActivityAsync(Strings.GenericErrorMessage, cancellationToken: cancellationToken);
                }

                if (conversationState != null)
                {
                    try
                    {
                        // Delete the conversationState for the current conversation to prevent the
                        // Bot from getting stuck in a error-loop caused by being in a bad state.
                        // ConversationState should be thought of as similar to "cookie-state" in a Web pages.
                        await conversationState.DeleteAsync(turnContext);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"Exception caught on attempting to delete conversation state : {ex.Message}");
                        throw;
                    }
                }
            };
        }
    }
}