// <copyright file="JobTitleBlobMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    /// <summary>
    /// Describes the metadata for job title blob storage.
    /// </summary>
    public static class JobTitleBlobMetadata
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "job-titles";

        /// <summary>
        /// The blob storage file name.
        /// </summary>
        public const string FileName = "AthenaUserJobTitles.json";
    }
}
