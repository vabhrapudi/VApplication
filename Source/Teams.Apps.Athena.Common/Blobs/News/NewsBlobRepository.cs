// <copyright file="NewsBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Common.Repositories;

    /// <summary>
    /// The class which implements methods to get news.
    /// </summary>
    public class NewsBlobRepository : BaseBlobRepository<IEnumerable<NewsJsonModel>>, INewsBlobRepository
    {
        private const char HyphenSeparator = '-';
        private const char ColonSeparator = ':';
        private const char DotSeparator = '.';

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsBlobRepository"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="repositoryOptions">Options used to create the repository.</param>
        public NewsBlobRepository(
            ILogger<NewsBlobRepository> logger,
            IOptions<RepositoryOptions> repositoryOptions)
            : base(
                 logger,
                 repositoryOptions.Value.StorageAccountConnectionString,
                 NewsBlobMetadata.ContainerName)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<NewsJsonModel>> GetNewsJsonData(DateTime lastRunAt)
        {
            var fileNames = await this.GetBlobFileList();
            var jsonData = new List<NewsJsonModel>();
            foreach (var fileName in fileNames)
            {
                var timestampStartIndex = fileName.IndexOf(HyphenSeparator, StringComparison.InvariantCulture);
                var timestampEndIndex = fileName.LastIndexOf(DotSeparator);
                var fileTimestamp = DateTime.Parse(fileName.Substring(timestampStartIndex + 1, timestampEndIndex - timestampStartIndex - 1).Replace(DotSeparator, ColonSeparator), null);
                if (DateTime.Compare(fileTimestamp, lastRunAt) > 0)
                {
                    var fileData = await this.GetBlobJsonFileContentAsync(fileName);
                    jsonData.AddRange(fileData);
                }
            }

            return jsonData;
        }
    }
}
