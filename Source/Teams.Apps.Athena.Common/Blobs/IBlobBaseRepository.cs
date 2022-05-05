// <copyright file="IBlobBaseRepository.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface for base blob repository.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    public interface IBlobBaseRepository<T>
    {
        /// <summary>
        /// Gets the JSON blob file content in provided type.
        /// </summary>
        /// <param name="blobFileName">The blob file name.</param>
        /// <returns>The blob file content.</returns>
        Task<T> GetBlobJsonFileContentAsync(string blobFileName);

        /// <summary>
        /// Gets the list of files in the provided container.
        /// </summary>
        /// <returns>The list of files in blob container.</returns>
        Task<IEnumerable<string>> GetBlobFileList();

        /// <summary>
        /// Gets the JSON blob file content in provided type for provided URL.
        /// </summary>
        /// <param name="path">path for blob file</param>
        /// <returns>The blob file content</returns>
        Task<T> GetBlobJsonFileContentByUrlAsync(string path);
    }
}
