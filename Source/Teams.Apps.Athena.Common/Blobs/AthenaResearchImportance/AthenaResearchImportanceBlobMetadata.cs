// <copyright file="AthenaResearchImportanceBlobMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Describes the metadata for athena research importance blob storage.
    /// </summary>
    public static class AthenaResearchImportanceBlobMetadata
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "athena-research-importance";

        /// <summary>
        /// The blob storage file name.
        /// </summary>
        public const string FileName = FileNames.AthenaResearchImportance + ".json";
    }
}
