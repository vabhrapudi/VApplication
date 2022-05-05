// <copyright file="AthenaNewsSourcesBlobRepository.cs" company="NPS Foundation">
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
    /// Describes the blob repository for Athena news sources.
    /// </summary>
    public class AthenaNewsSourcesBlobRepository : BaseBlobRepository<IEnumerable<AthenaNewsSource>>, IAthenaNewsSourcesBlobRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AthenaNewsSourcesBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public AthenaNewsSourcesBlobRepository(
            ILogger<AthenaNewsSourcesBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 AthenaNewsSourcesBlobMetadata.ContainerName)
        {
        }

        /// <inheritdoc />
        public async Task<AthenaNewsSource> GetNewsSourceById(int newsSourceId)
        {
            var newsSources = await this.GetBlobJsonFileContentAsync(AthenaNewsSourcesBlobMetadata.FileName);
            try
            {
                var newsSource = newsSources.Single(x => x.NewsSourceId == newsSourceId);
                return newsSource;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
