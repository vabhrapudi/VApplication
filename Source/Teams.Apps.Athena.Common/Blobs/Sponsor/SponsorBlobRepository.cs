// <copyright file="SponsorBlobRepository.cs" company="NPS Foundation">
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
    /// Describes the blob repository for sponsors.
    /// </summary>
    public class SponsorBlobRepository : BaseBlobRepository<IEnumerable<SponsorJson>>, ISponsorBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SponsorBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public SponsorBlobRepository(
            ILogger<SponsorBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 SponsorBlobMetadata.ContainerName)
        {
        }
    }
}
