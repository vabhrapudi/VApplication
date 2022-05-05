// <copyright file="IDiscoveryTreeTaxonomyBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The blob repository interface for discovery tree taxonomy.
    /// </summary>
    public interface IDiscoveryTreeTaxonomyBlobRepository : IBlobBaseRepository<IEnumerable<DiscoveryTreeTaxonomyElement>>
    {
    }
}
