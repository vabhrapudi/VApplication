// <copyright file="IUserPersistentDataRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for User persistent data Repository.
    /// </summary>
    public interface IUserPersistentDataRepository : IRepository<UserPersistentDataEntity>
    {
    }
}
