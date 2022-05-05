// <copyright file="KeywordsBlobRepositoryMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Represents the metadata for keywords repository.
    /// </summary>
    public static class KeywordsBlobRepositoryMetadata
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "keywords";

        /// <summary>
        /// The blob storage file name.
        /// </summary>
        public const string FileName = FileNames.AthenaKeywords + ".json";
    }
}
