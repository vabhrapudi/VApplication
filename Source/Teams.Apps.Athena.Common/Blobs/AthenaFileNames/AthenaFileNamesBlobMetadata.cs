// <copyright file="AthenaFileNamesBlobMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    /// <summary>
    /// Describes the metadata for Athena file names blob storage.
    /// </summary>
    public static class AthenaFileNamesBlobMetadata
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "athena-file-names";

        /// <summary>
        /// The blob storage file name.
        /// </summary>
        public const string FileName = "AthenaFileNames.json";
    }
}
