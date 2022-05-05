// <copyright file="IAthenaFileNamesBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to Athena file names blob repository.
    /// </summary>
    public interface IAthenaFileNamesBlobRepository : IBlobBaseRepository<IEnumerable<AthenaFileNames>>
    {
    }
}
