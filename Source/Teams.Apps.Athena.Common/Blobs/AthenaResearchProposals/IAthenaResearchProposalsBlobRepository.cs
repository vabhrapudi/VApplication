// <copyright file="IAthenaResearchProposalsBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to Athena research proposals blob repository.
    /// </summary>
    public interface IAthenaResearchProposalsBlobRepository : IBlobBaseRepository<IEnumerable<ResearchProposalJson>>
    {
    }
}
