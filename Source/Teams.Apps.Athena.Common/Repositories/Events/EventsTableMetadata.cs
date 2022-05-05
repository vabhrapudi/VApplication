// <copyright file="EventsTableMetadata.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories
{
    /// <summary>
    /// The metadata of events table.
    /// </summary>
    public static class EventsTableMetadata
    {
        /// <summary>
        /// The events table name where all events details get stored.
        /// </summary>
        public const string TableName = "EventsEntity";

        /// <summary>
        /// The events table partition key.
        /// </summary>
        public const string PartitionKey = "Events";
    }
}
