// <copyright file="IUserBotConversationRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes repository operations related to user-Bot conversations.
    /// </summary>
    public interface IUserBotConversationRepository : IRepository<UserBotConversationEntity>
    {
    }
}
