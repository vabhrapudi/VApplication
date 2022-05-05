// <copyright file="AthenaResearchPriorityBlobRepository.cs" company="NPS Foundation">
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
    /// The repository for athena research priorities blob storage.
    /// </summary>
    public class AthenaResearchPriorityBlobRepository : BaseBlobRepository<IEnumerable<AthenaResearchPriority>>, IAthenaResearchPriorityBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaResearchPriorityBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public AthenaResearchPriorityBlobRepository(
            ILogger<AthenaResearchPriorityBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaResearchPriorityBlobMetadata.ContainerName)
        {
        }
    }
}