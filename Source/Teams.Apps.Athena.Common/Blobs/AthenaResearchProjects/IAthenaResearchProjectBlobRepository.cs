// <copyright file="IAthenaResearchProjectBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to AthenaResearchProject BlobRepository
    /// </summary>
    public interface IAthenaResearchProjectBlobRepository : IBlobBaseRepository<IEnumerable<ResearchProjectJson>>
    {
    }
}
