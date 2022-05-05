// <copyright file="AthenaNewsKeywordsBlobMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Blobs
{
    /// <summary>
    /// Describes the metadata for Athena news keywords blob storage.
    /// </summary>
    public static class AthenaNewsKeywordsBlobMetadata
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public const string ContainerName = "athena-news-keywords";

        /// <summary>
        /// The blob storage file name.
        /// </summary>
        public const string FileName = "AthenaNewsKeywords.json";
    }
}