// <copyright file="DiscoveryTreeFiltersBlobMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Describes the metadata for discovery tree filters blob storage.
    /// </summary>
    public static class DiscoveryTreeFiltersBlobMetadata
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "discovery-filters";

        /// <summary>
        /// The blob storage file name.
        /// </summary>
        public const string FileName = FileNames.AthenaFilters + ".json";
    }
}
