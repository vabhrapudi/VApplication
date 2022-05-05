// <copyright file="IResearchRequestBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to ResearchRequest blob repository.
    /// </summary>
    public interface IResearchRequestBlobRepository : IBlobBaseRepository<IEnumerable<ResearchRequestJson>>
    {
    }
}
