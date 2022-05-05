// <copyright file="AthenaResearchProposalsBlobRepository.cs" company="NPS Foundation">
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
    /// Describes the blob repository for Athena research proposals.
    /// </summary>
    public class AthenaResearchProposalsBlobRepository : BaseBlobRepository<IEnumerable<ResearchProposalJson>>, IAthenaResearchProposalsBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaResearchProposalsBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public AthenaResearchProposalsBlobRepository(
            ILogger<AthenaResearchProposalsBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaResearchProposalsBlobMetadata.ContainerName)
        {
        }
    }
}
