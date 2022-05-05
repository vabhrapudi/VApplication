// <copyright file="IAthenaBotActivityHandlerHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;

    /// <summary>
    /// Helper for handling bot related activities.
    /// </summary>
    public interface IAthenaBotActivityHandlerHelper
    {
        /// <summary>
        /// Sent welcome card to personal chat.
        /// </summary>
        /// <param name="turnContext">Provides context for a turn in a bot.</param>
        /// <returns>A task that represents a response.</returns>
        Task OnBotInstalledInPersonalAsync(ITurnContext<IConversationUpdateActivity> turnContext);

        /// <summary>
        /// Send a welcome card if bot is installed in Team scope.
        /// </summary>
        /// <param name="turnContext">Provides context for a turn in a bot.</param>
        /// <returns>A task that represents a response.</returns>
        Task OnBotInstalledInTeamAsync(ITurnContext<IConversationUpdateActivity> turnContext);

        /// <summary>
        /// Remove user details from storage if bot is uninstalled from Team scope.
        /// </summary>
        /// <param name="turnContext">Provides context for a turn in a bot.</param>
        /// <returns>A task that represents a response.</returns>
        Task OnBotUninstalledFromTeamAsync(ITurnContext<IConversationUpdateActivity> turnContext);

        /// <summary>
        /// Processes task module fetch operation.
        /// </summary>
        /// <param name="turnContext">Provides context for a turn in Bot.</param>
        /// <param name="taskModuleRequest">The task module request.</param>
        /// <returns>A task that represents response.</returns>
        Task<TaskModuleResponse> OnTaskModuleFetchRequestAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest);

        /// <summary>
        /// Gets the task module response.
        /// </summary>
        /// <param name="url">The URL to be rendered in task module.</param>
        /// <param name="title">The title of task module.</param>
        /// <param name="height">The height of task module.</param>
        /// <param name="width">The width of task module.</param>
        /// <returns>A task representing task module response operation.</returns>
        TaskModuleResponse GetTaskModuleResponse(Uri url, string title, int height = 746, int width = 600);

        /// <summary>
        /// Removes user details from storage if bot is uninstalled from personal scope.
        /// </summary>
        /// <param name="turnContext">Provides context for a turn in a bot.</param>
        /// <returns>A task that represents a response.</returns>
        Task OnBotUninstalledInPersonalAsync(ITurnContext<IConversationUpdateActivity> turnContext);
    }
}
