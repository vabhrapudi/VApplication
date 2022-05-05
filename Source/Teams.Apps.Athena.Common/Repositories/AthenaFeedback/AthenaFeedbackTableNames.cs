// <copyright file="AthenaFeedbackTableNames.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// Athena feedback table options.
    /// </summary>
    public static class AthenaFeedbackTableNames
    {
        /// <summary>
        /// Table name for Athena feedback table.
        /// </summary>
        public static readonly string TableName = "AthenaFeedbackEntity";

        /// <summary>
        /// Athena feedback partition key name.
        /// </summary>
        public static readonly string AthenaFeedbackPartition = "AthenaFeedback";
    }
}