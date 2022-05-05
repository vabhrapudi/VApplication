// <copyright file="OrganizationTableNames.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Common.Repositories.Organization
{
    /// <summary>
    /// Organization data table names.
    /// </summary>
    public static class OrganizationTableNames
    {
        /// <summary>
        /// Table name for the organization data table.
        /// </summary>
        public static readonly string TableName = "OrganizationEntity";

        /// <summary>
        /// Organization data partition key name.
        /// </summary>
        public static readonly string OrganizationPartition = "Organization";
    }
}