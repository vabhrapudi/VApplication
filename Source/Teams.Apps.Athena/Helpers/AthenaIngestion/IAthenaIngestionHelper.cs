// <copyright file="IAthenaIngestionHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;

    /// <summary>
    /// The helper methods related to Athena ingestion.
    /// </summary>
    public interface IAthenaIngestionHelper
    {
        /// <summary>
        /// Add or update the json file data into table.
        /// </summary>
        /// <param name="entityName"> entityName for json file.</param>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddUpdateEntity(string entityName, string path);

        /// <summary>
        /// Add or update the users json file data to user table.
        /// </summary>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddUsersAsync(string path);

        /// <summary>
        /// Add or update the Athena info resource json file data to Athena info resources table.
        /// </summary>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddInfoResourceAsync(string path);

        /// <summary>
        /// Add or update the community of interest json file data to community of interest table.
        /// </summary>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddCommunitiesAsync(string path);

        /// <summary>
        /// Add or update the Athena research projects json file to Athena research projects table.
        /// </summary>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddResearchProjectsAsync(string path);

        /// <summary>
        /// Add or update the Athena research proposals json file to Athena research proposals table.
        /// </summary>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddResearchProposalsAsync(string path);

        /// <summary>
        /// Add or update the sponsor json file to sponsor table.
        /// </summary>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddSponsorsAsync(string path);

        /// <summary>
        /// Add or update the partner json file to partner table.
        /// </summary>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddPartnersAsync(string path);

        /// <summary>
        /// Add or update the research requests json file to research requests table.
        /// </summary>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddAthenaResearchRequestsAsync(string path);

        /// <summary>
        /// Add or update the events json file to events table.
        /// </summary>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddAthenaEventsAsync(string path);

        /// <summary>
        /// Add or update the tools json file to tools entity table.
        /// </summary>
        /// <param name="path">Json file path.</param>
        /// <returns>Returns task.</returns>
        Task AddAthenaToolsAsync(string path);

        /// <summary>
        /// Gets the Athena ingestion details.
        /// </summary>
        /// <returns>The collection of Athena ingestion entity.</returns>
        Task<IEnumerable<AthenaIngestionEntity>> GetAthenaIngestionDetailsAsync();
    }
}
