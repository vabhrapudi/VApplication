// <copyright file="SecurityLevelBlobMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    /// <summary>
    /// Describes the metadata for security level blob storage.
    /// </summary>
    public static class SecurityLevelBlobMetadata
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "athena-security-levels";

        /// <summary>
        /// The blob storage file name.
        /// </summary>
        public const string FileName = "AthenaSecurityLevels.json";
    }
}
