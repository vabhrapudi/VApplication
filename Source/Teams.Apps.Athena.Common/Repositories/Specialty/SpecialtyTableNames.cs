// <copyright file="SpecialtyTableNames.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories.Specialty
{
    /// <summary>
    /// Specialty data table names.
    /// </summary>
    public static class SpecialtyTableNames
    {
        /// <summary>
        /// Table name for the specialty data table.
        /// </summary>
        public static readonly string TableName = "SpecialtyEntity";

        /// <summary>
        /// Specialty data partition key name.
        /// </summary>
        public static readonly string SpecialtyPartition = "Specialty";
    }
}