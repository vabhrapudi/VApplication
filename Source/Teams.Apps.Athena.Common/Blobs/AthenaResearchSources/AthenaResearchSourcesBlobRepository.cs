// <copyright file="AthenaResearchSourcesBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// Describes the blob repository for Athena research sources.
    /// </summary>
    public class AthenaResearchSourcesBlobRepository : BaseBlobRepository<IEnumerable<AthenaResearchSource>>, IAthenaResearchSourcesBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaResearchSourcesBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public AthenaResearchSourcesBlobRepository(
            ILogger<AthenaResearchSourcesBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaResearchSourcesBlobMetadata.ContainerName)
        {
        }

        /// <inheritdoc />
        public async Task<AthenaResearchSource> GetResearchSourceById(int researchSourceId)
        {
            var researchSources = await this.GetBlobJsonFileContentAsync(AthenaResearchSourcesBlobMetadata.FileName);
            try
            {
                var researchSource = researchSources.Single(x => x.ResearchSourceId == researchSourceId);
                return researchSource;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
