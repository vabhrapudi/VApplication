// <copyright file="ResourceFeedbackTableNames.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories.ResourceFeedback
{
    /// <summary>
    /// Resource feedback data table names.
    /// </summary>
    public static class ResourceFeedbackTableNames
    {
        /// <summary>
        /// Table name for the resource feedback data table.
        /// </summary>
        public static readonly string TableName = "ResourceFeedbackEntity";

        /// <summary>
        /// Resource feedback data partition key name.
        /// </summary>
        public static readonly string ResourceFeedbackPartition = "ResourceFeedback";
    }
}