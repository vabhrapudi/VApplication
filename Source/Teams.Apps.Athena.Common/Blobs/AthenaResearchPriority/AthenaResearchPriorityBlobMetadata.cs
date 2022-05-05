// <copyright file="AthenaResearchPriorityBlobMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// Describes the metadata for athena research priorities blob storage.
    /// </summary>
    public static class AthenaResearchPriorityBlobMetadata
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "athena-research-priorities";

        /// <summary>
        /// The blob storage file name.
        /// </summary>
        public const string FileName = FileNames.AthenaResearchPriorities + ".json";
    }
}