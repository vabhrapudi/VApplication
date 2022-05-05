// <copyright file="discovery-tree-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from "../api/axios-decorator";
import IDiscoveryTreeSearchAndFilter from "../models/discovery-tree-search-filter";

/**
 * Gets the discovery tree taxonomy.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getTaxonomyAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/discovery-tree/taxonomy"

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the discovery tree node data.
 * @param keywords The associated node keywords of which data to get.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getNodeDataAsync = async (
    keywords: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/discovery-tree/node-data"

    return axios.post(apiEndpoint, handleTokenAccessFailure, keywords);
}

/**
 * Gets the discovery tree filters.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getDiscoveryTreeFiltersAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/discovery-tree/filters"

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the discovery tree node types.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getDiscoveryNodeTypeAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/discovery-tree/node-type"

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the interested users data.
 * @param keywords The associated node keywords of which data to get.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getInterestedUsersDataAsync = async (
    keywordIds: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/discovery-tree/users"

    return axios.post(apiEndpoint, handleTokenAccessFailure, keywordIds);
}

/**
 * Gets the interested sponsors data.
 * @param keywords The associated node keywords of which data to get.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getInterestedSponsorsDataAsync = async (
    keywordIds: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/sponsors"

    return axios.post(apiEndpoint, handleTokenAccessFailure, keywordIds);
}

/**
 * Follows a resource.
 * @param keywords The associated node keywords of which data to get.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const followResourceAsync = async (
    keywordIds: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/discovery-tree/keywords"

    return axios.patch(apiEndpoint, handleTokenAccessFailure, keywordIds);
}

/**
 * Search and filter discovery tree resources.
 * @param searchAndFilterOptions The search and filter parameters.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const searchAndFilterDiscoveryTreeResourcesAsync = async (
    searchAndFilterOptions: IDiscoveryTreeSearchAndFilter,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = `/discovery-tree/search-filter`;

    return await axios.post(apiEndpoint, handleTokenAccessFailure, searchAndFilterOptions);
}