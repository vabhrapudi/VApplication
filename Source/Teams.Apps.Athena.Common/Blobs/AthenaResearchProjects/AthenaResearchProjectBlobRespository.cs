// <copyright file="AthenaResearchProjectBlobRespository.cs" company="NPS Foundation">
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
    /// The repository for AthenaResearchProject blob storage.
    /// </summary>
    public class AthenaResearchProjectBlobRespository : BaseBlobRepository<IEnumerable<ResearchProjectJson>>, IAthenaResearchProjectBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaResearchProjectBlobRespository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public AthenaResearchProjectBlobRespository(
            ILogger<AthenaInfoResourceBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaResearchProjectBlobMetadata.ContainerName)
        {
        }
    }
}