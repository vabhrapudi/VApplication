// <copyright file="AthenaResearchImportanceBlobRepository.cs" company="NPS Foundation">
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
    /// The repository for athena research importance blob storage.
    /// </summary>
    public class AthenaResearchImportanceBlobRepository : BaseBlobRepository<IEnumerable<AthenaResearchImportance>>, IAthenaResearchImportanceBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaResearchImportanceBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public AthenaResearchImportanceBlobRepository(
            ILogger<AthenaResearchImportanceBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaResearchImportanceBlobMetadata.ContainerName)
        {
        }
    }
}