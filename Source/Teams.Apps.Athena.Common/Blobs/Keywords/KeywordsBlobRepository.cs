// <copyright file="KeywordsBlobRepository.cs" company="NPS Foundation">
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
    /// Provides methods related to keywords blob repository.
    /// </summary>
    public class KeywordsBlobRepository : BaseBlobRepository<IEnumerable<KeywordEntity>>, IKeywordsBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeywordsBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public KeywordsBlobRepository(
            ILogger<KeywordsBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 KeywordsBlobRepositoryMetadata.ContainerName)
        {
        }
    }
}
