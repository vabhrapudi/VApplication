// <copyright file="ISecurityLevelBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to security level blob repository.
    /// </summary>
    public interface ISecurityLevelBlobRepository : IBlobBaseRepository<IEnumerable<SecurityLevels>>
    {
    }
}
