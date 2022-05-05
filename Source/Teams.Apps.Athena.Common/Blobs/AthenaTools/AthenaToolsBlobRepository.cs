// <copyright file="AthenaToolsBlobRepository.cs" company="NPS Foundation">
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
    /// The repository for AthenaTools blob storage.
    /// </summary>
    public class AthenaToolsBlobRepository : BaseBlobRepository<IEnumerable<AthenaToolJson>>, IAthenaToolsBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaToolsBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public AthenaToolsBlobRepository(
            ILogger<AthenaToolsBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaToolsBlobMetadata.ContainerName)
        {
        }
    }
}
