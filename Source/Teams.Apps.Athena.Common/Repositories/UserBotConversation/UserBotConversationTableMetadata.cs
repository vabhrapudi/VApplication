// <copyright file="UserBotConversationTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// The metadata for user-Bot conversation table.
    /// </summary>
    public static class UserBotConversationTableMetadata
    {
        /// <summary>
        /// The table name to store user-Bot coversation details.
        /// </summary>
        public const string TableName = "UserBotConversationEntity";

        /// <summary>
        /// The user-Bot conversation table partition key name.
        /// </summary>
        public const string PartitionKey = "UserBotConversation";
    }
}
