// <copyright file="IDiscoveryTreeFiltersBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The interface which expose methods to get discovery tree filters.
    /// </summary>
    public interface IDiscoveryTreeFiltersBlobRepository : IBlobBaseRepository<IEnumerable<DiscoveryTreeFilterItems>>
    {
    }
}
