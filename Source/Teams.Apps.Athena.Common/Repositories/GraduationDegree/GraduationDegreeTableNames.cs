// <copyright file="GraduationDegreeTableNames.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories.GraduationDegree
{
    /// <summary>
    /// Graduation degree data table names.
    /// </summary>
    public static class GraduationDegreeTableNames
    {
        /// <summary>
        /// Table name for the graduation degree data table.
        /// </summary>
        public static readonly string TableName = "GraduationDegreeEntity";

        /// <summary>
        /// Graduation degree data partition key name.
        /// </summary>
        public static readonly string GraduationDegreePartition = "GraduationDegree";
    }
}