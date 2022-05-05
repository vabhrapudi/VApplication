// <copyright file="IAthenaNewsKeywordsBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Exposes methods related to Athena news keywords blob repository.
    /// </summary>
    public interface IAthenaNewsKeywordsBlobRepository : IBlobBaseRepository<IEnumerable<AthenaNewsKeyword>>
    {
    }
}