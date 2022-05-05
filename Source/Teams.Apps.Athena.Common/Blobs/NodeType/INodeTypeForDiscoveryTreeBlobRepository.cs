// <copyright file="INodeTypeForDiscoveryTreeBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The interface which expose methods to get discovery tree node types.
    /// </summary>
    public interface INodeTypeForDiscoveryTreeBlobRepository : IBlobBaseRepository<IEnumerable<NodeType>>
    {
    }
}
