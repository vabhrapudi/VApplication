// <copyright file="BaseBlobRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// The base blob repository.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public abstract class BaseBlobRepository<T> : IBlobBaseRepository<T>
    {
        private readonly string container;

        private readonly BlobContainerClient blobContainerClient;

        private BlobClient blobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBlobRepository{T}"/> class.
        /// </summary>
        /// <param name="logger">The logging service.</param>
        /// <param name="storageAccountConnectionString">The storage account connection string.</param>
        /// <param name="containerName">The container name.</param>
        protected BaseBlobRepository(
            ILogger logger,
            string storageAccountConnectionString,
            string containerName)
        {
            this.Logger = logger;
            this.container = containerName;

            var blobServiceClient = new BlobServiceClient(storageAccountConnectionString);
            this.blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        }

        /// <summary>
        /// Gets the logger service.
        /// </summary>
        protected ILogger Logger { get; }

        /// <inheritdoc/>
        public async Task<T> GetBlobJsonFileContentAsync(string blobFileName)
        {
            try
            {
                this.blobClient = this.blobContainerClient.GetBlobClient(blobFileName);

                if (await this.blobClient.ExistsAsync())
                {
                    using (var stream = await this.blobClient.OpenReadAsync())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            using (var jr = new JsonTextReader(sr))
                            {
                                return JsonSerializer.CreateDefault().Deserialize<T>(jr);
                            }
                        }
                    }
                }
                else
                {
                    this.Logger.LogError($"No blob '{blobFileName}' exists in container '{this.container}'.");
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Error occurred while getting blob '{blobFileName}' from container '{this.container}'.");
                return default(T);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<string>> GetBlobFileList()
        {
            try
            {
                var blobFiles = this.blobContainerClient.GetBlobs();

                List<string> fileNames = new List<string>();
                foreach (var blobFile in blobFiles)
                {
                    fileNames.Add(blobFile.Name.ToString());
                }

                return fileNames;
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"Exception occurred while attempting to list files in blob: {ex.Message}");
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<T> GetBlobJsonFileContentByUrlAsync(string path)
        {
            try
            {
                WebClient wc = new WebClient();

                using (Stream stream = await wc.OpenReadTaskAsync(new Uri(path)))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        using (var jr = new JsonTextReader(sr))
                        {
                            return JsonSerializer.CreateDefault().Deserialize<T>(jr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to Deserialise Json due to invalid url path. {ex.Message}");
                this.Logger.LogError(ex, $"Error occurred while getting blob '{path}'.");
            }
        }
    }
}
