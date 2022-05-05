// <copyright file="IAthenaResearchPriorityBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The interface which expose methods to get athena research priorities.
    /// </summary>
    public interface IAthenaResearchPriorityBlobRepository : IBlobBaseRepository<IEnumerable<AthenaResearchPriority>>
    {
    }
}
