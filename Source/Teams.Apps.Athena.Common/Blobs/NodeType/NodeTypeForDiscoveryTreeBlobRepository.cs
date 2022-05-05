﻿// <copyright file="NodeTypeForDiscoveryTreeBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// The class which implements methods to get discovery tree filters.
    /// </summary>
    public class NodeTypeForDiscoveryTreeBlobRepository : BaseBlobRepository<IEnumerable<NodeType>>, INodeTypeForDiscoveryTreeBlobRepository
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="NodeTypeForDiscoveryTreeBlobRepository"/> class.
            /// </summary>
            /// <param name="logger">The logging service.</param>
            /// <param name="repositoryOptions">Options used to create the repository.</param>
            public NodeTypeForDiscoveryTreeBlobRepository(
                ILogger<NodeTypeForDiscoveryTreeBlobRepository> logger,
                IOptions<RepositoryOptions> repositoryOptions)
                : base(
                     logger,
                     repositoryOptions.Value.StorageAccountConnectionString,
                     NodeTypeForDiscoveryTreeBlobMetadata.ContainerName)
            {
            }
    }
}
