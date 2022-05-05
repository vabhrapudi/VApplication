// <copyright file="CommunityOfInterestBlobRepository.cs" company="NPS Foundation">
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
    /// Describes the blob repository for CommunityOfInterest.
    /// </summary>
    public class CommunityOfInterestBlobRepository : BaseBlobRepository<IEnumerable<CommunityOfInterestJson>>, ICommunityOfInterestBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommunityOfInterestBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public CommunityOfInterestBlobRepository(
            ILogger<CommunityOfInterestBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 CommunityOfInterestBlobMetadata.ContainerName)
        {
        }
    }
}
