// <copyright file="DiscoveryTreeTaxonomyBlobMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The discovery tree taxonomy blob repository metadata.
    /// </summary>
    public static class DiscoveryTreeTaxonomyBlobMetadata
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "discovery-tree-taxonomy";

        /// <summary>
        /// The blob storage file name.
        /// </summary>
        public const string FileName = FileNames.AthenaTaxonomy + ".json";
    }
}
