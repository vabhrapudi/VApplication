// <copyright file="COIUpdatesFunction.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.AthenaFunction
{
    using System;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The function send COIs updates to users on weekly/monthly basis.
    /// </summary>
    public static class COIUpdatesFunction
    {
        /// <summary>
        /// Azure Function App triggered by time.
        /// Sends notifications to users regarding updates in COIs.
        /// </summary>
        /// <param name="myTimer">Timer instance with CRON expression.</param>
        /// <param name="log">Logger instance.</param>
        [FunctionName("Function")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
