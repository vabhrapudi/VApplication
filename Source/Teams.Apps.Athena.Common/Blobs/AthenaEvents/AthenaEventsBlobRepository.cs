// <copyright file="AthenaEventsBlobRepository.cs" company="NPS Foundation">
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
    /// The repository for AthenaEvents blob storage.
    /// </summary>
    public class AthenaEventsBlobRepository : BaseBlobRepository<IEnumerable<EventJson>>, IAthenaEventsBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaEventsBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public AthenaEventsBlobRepository(
            ILogger<AthenaEventsBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaEventsBlobMetadata.ContainerName)
    {
    }
    }
}
