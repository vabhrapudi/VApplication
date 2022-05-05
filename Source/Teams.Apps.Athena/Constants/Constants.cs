// <copyright file="Constants.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Constants
{
    using System;

    /// <summary>
    /// Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Result page count.
        /// </summary>
        public const int ResultPageCount = 30;

        /// <summary>
        /// Get the user read all scope.
        /// </summary>
        public const string UserReadAll = "User.Read.All";

        /// <summary>
        /// Get the header key for graph permission type.
        /// </summary>
        public const string PermissionTypeKey = "x-api-permission";

        /// <summary>
        /// Represents Athena source.
        /// </summary>
        public const int SourceAthena = -1;

        /// <summary>
        /// Represents the start day of a week.
        /// </summary>
        public const DayOfWeek StartDayOfWeek = DayOfWeek.Sunday;

        /// <summary>
        /// Represents the maximum number of articles are allowed to configure in home configurations.
        /// </summary>
        public const int MaxNumberOfArticlesCanBeConfigured = 5;

        /// <summary>
        /// Represents the maximum number of priorities allowed under a priority type.
        /// </summary>
        public const int MaxNumberOfPriorities = 5;

        /// <summary>
        /// Represents the filter Id for proposed projects.
        /// </summary>
        public const int ProposedResearchFilterId = 7;

        /// <summary>
        /// Represents the filter Id for in-progress projects.
        /// </summary>
        public const int InProgressResearchFilterId = 5;

        /// <summary>
        /// Represents the filter Id for completed projects.
        /// </summary>
        public const int CompletedResearchFilterId = 4;
    }
}