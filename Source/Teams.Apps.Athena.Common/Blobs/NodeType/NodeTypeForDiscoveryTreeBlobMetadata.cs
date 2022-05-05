// <copyright file="NodeTypeForDiscoveryTreeBlobMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Describes the metadata for discovery tree node type blob storage.
    /// </summary>
    public static class NodeTypeForDiscoveryTreeBlobMetadata
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "athena-node-types";

        /// <summary>
        /// The blob storage file name.
        /// </summary>
        public const string FileName = FileNames.AthenaNodeTypes + ".json";
    }
}
