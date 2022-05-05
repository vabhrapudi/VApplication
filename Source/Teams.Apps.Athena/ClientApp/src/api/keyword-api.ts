// <copyright file="keyword-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import { AxiosRequestConfig } from "axios";
import axios from "../api/axios-decorator";

/**
 * API request to fetch keywords
 * @param searchQuery The search query string.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getKeywordsAsync = async (
    searchQuery: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/keywords`;
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({ searchQuery });

    return axios.get(apiEndpoint, handleTokenAccessFailure, config);
}

/**
 * API request to fetch keywords
 * @param keywordIds The array of keyword Ids.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getKeywordsByKeywordIdsAsync = async (
    keywordIds: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/keywords/keywordIds`;

    return axios.post(apiEndpoint, handleTokenAccessFailure, keywordIds, undefined);
}

/**
 * API request to fetch keywords of COI team
 * @param teamId The team Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getCoiTeamKeywordsAsync = async (
    teamId: string,
    handleTakenAccessFailure: (error: string) => void) => {
    let url = "/keywords/coiTeamKeywords";
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({ teamId });

    return axios.get(url, handleTakenAccessFailure, config);
}

/**
 * Gets all keywords from blob storage.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getAllKeywordsAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/keywords/all`;
    return axios.get(apiEndpoint, handleTokenAccessFailure);
}