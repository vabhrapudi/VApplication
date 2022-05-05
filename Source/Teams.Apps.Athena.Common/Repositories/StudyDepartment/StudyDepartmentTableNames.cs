// <copyright file="StudyDepartmentTableNames.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories.StudyDepartment
{
    /// <summary>
    /// Study Department data table names.
    /// </summary>
    public static class StudyDepartmentTableNames
    {
        /// <summary>
        /// Table name for the study department data table.
        /// </summary>
        public static readonly string TableName = "StudyDepartmentEntity";

        /// <summary>
        /// Study department data partition key name.
        /// </summary>
        public static readonly string StudyDepartmentPartition = "StudyDepartment";
    }
}