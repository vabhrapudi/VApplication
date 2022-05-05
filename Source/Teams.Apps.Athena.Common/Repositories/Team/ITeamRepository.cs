// <copyright file="ITeamRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Interface for Team Data Repository.
    /// </summary>
    public interface ITeamRepository : IRepository<TeamEntity>
    {
    }
}