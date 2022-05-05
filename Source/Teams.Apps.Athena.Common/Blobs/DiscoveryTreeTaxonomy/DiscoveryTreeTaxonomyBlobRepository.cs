// <copyright file="DiscoveryTreeTaxonomyBlobRepository.cs" company="NPS Foundation">
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
    /// The class for discovery tree taxonomy blob repository.
    /// </summary>
    public class DiscoveryTreeTaxonomyBlobRepository : BaseBlobRepository<IEnumerable<DiscoveryTreeTaxonomyElement>>, IDiscoveryTreeTaxonomyBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryTreeTaxonomyBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public DiscoveryTreeTaxonomyBlobRepository(
            ILogger<DiscoveryTreeTaxonomyBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 DiscoveryTreeTaxonomyBlobMetadata.ContainerName)
        {
        }
    }
}
