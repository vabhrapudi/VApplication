// <copyright file="IDiscoveryTreeHelper.cs" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

namespace Teams.Apps.Athena.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Teams.Apps.Athena.Common.Models;
    using Teams.Apps.Athena.Models;

    /// <summary>
    /// The helper methods provider for discovery tree.
    /// </summary>
    public interface IDiscoveryTreeHelper
    {
        /// <summary>
        /// Gets a discovery node data based on keywords assosiated with the node.
        /// </summary>
        /// <param name="keywords">The collection of keywords.</param>
        /// <returns>A task representing get discovery tree node data.</returns>
        Task<DiscoveryTreeData> GetDiscoveryTreeNodeData(IEnumerable<int> keywords);

        /// <summary>
        /// Gets a discovery tree node type from blob storage.
        /// </summary>
        /// <returns>A task representing get discovery tree node data.</returns>
        Task<IEnumerable<NodeType>> GetDiscoveryTreeNodeTypeAsync();

        /// <summary>
        /// Gets a discovery tree filters from blob storage.
        /// </summary>
        /// <returns>A task representing get discovery tree node data.</returns>
        Task<IEnumerable<DiscoveryTreeFilterItems>> GetDiscoveryTreeFilters();

        /// <summary>
        /// Gets the users by keyword Ids.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword Ids.</param>
        /// <returns>The collection of users.</returns>
        Task<IEnumerable<UserDetails>> GetUsersByKeywordIds(IEnumerable<int> keywordIds);

        /// <summary>
        /// Follows a resource.
        /// </summary>
        /// <param name="keywordIds">The collection of keyword ids.</param>
        /// <param name="userId">User id.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task<UserEntity> FollowResourceAsync(IEnumerable<int> keywordIds, string userId);

        /// <summary>
        /// Finds a resource or filter discovery tree resources.
        /// </summary>
        /// <param name="searchTexts">The search strings of which matching resources to be find.</param>
        /// <param name="searchKeywordIds">The search keywords of which matching resources to be find.</param>
        /// <param name="selectedFilters">The selected filters.</param>
        /// <returns>The discovery tree resources.</returns>
        Task<DiscoveryTreeData> FindOrFilterDiscoveryTreeResourcesAsync(IEnumerable<string> searchTexts, IEnumerable<int> searchKeywordIds, IEnumerable<DiscoveryTreeSelectedFilter> selectedFilters);

        /// <summary>
        /// Gnerates the filter string based on field type.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="jsonFileName">The JSON file name.</param>
        /// <param name="nodeTypes">The collection of node types.</param>
        /// <param name="filterTypesByFile">The collection of filter types by file.</param>
        /// <returns>The filter query based on type of field.</returns>
        string GetFilterStringByType(DiscoveryTreeFilterItems filter, string jsonFileName, IEnumerable<NodeType> nodeTypes, Dictionary<string, List<string>> filterTypesByFile);

        /// <summary>
        /// Preapres the dictionary by JSON types.
        /// </summary>
        /// <param name="filter">The selected filter.</param>
        /// <param name="dictionary">The dictionaty to be filled-up.</param>
        /// <param name="resultType">The result type entity names.</param>
        /// <param name="nodeTypes">The collection of node types.</param>
        /// <param name="filterTypesByFile">The collection of filter types by file.</param>
        void PrepareDictionary(DiscoveryTreeSelectedFilter filter, Dictionary<int, Dictionary<string, List<string>>> dictionary, string resultType, IEnumerable<NodeType> nodeTypes, Dictionary<string, List<string>> filterTypesByFile);

        /// <summary>
        /// Generates filter query list for the applied filters.
        /// </summary>
        /// <param name="filters">The collection of filters.</param>
        /// <param name="jsonFileName">The JSON file name.</param>
        /// <param name="nodeTypes">The collection of node types.</param>
        /// <param name="filterTypesByFile">The collection of filter types by file.</param>
        /// <returns>The list of filter quries.</returns>
        List<string> GetFilterList(IEnumerable<DiscoveryTreeFilterItems> filters, string jsonFileName, IEnumerable<NodeType> nodeTypes, Dictionary<string, List<string>> filterTypesByFile);
    }
}
