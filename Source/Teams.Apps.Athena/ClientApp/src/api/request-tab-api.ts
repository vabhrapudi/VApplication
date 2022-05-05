// <copyright file="request-tab-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from "./axios-decorator";
import Constants from "../constants/constants";
import { AxiosRequestConfig } from "axios";

/**
 * Gets all COI requests.
 * @param searchText The search text to search in COI name or keywords.
 * @param pageNumber The page number for which data to be fetched.
 * @param sortColumn The column to be sorted.
 * @param sortOrder The order in which column to be sorted.
 * @param statusFilterValues The status filter values.
 * @param handleTokenAccessFailure The callback function to handle token access failure. 
 */
export const getAllCoiRequestAsync = async (
    searchText: string,
    pageNumber: number,
    sortColumn: number|null,
    sortOrder: number|null,
    statusFilterValues: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/coi/requests/all/pending";
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({
        searchText,
        pageNumber,
        sortColumn,
        sortOrder
    });

    return axios.post(apiEndpoint, handleTokenAccessFailure, statusFilterValues, config);
}

/**
 * Gets all news requests.
 * @param searchText The search text to search in news name or keywords.
 * @param pageNumber The page number for which data to be fetched.
 * @param sortColumn The column to be sorted.
 * @param sortOrder The order in which column to be sorted.
 * @param statusFilterValues The status filter values.
 * @param handleTokenAccessFailure The callback function to handle token access failure. 
 */
export const getAllNewsRequestAsync = async (
    searchText: string,
    pageNumber: number,
    sortColumn: number | null,
    sortOrder: number | null,
    statusFilterValues: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/news/requests/all/pending";
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({
        searchText,
        pageNumber,
        sortColumn,
        sortOrder
    });

    return axios.post(apiEndpoint, handleTokenAccessFailure, statusFilterValues, config);
}

// Get coi request by id.
export const getCoiRequestByIdAsync = async (requestId: string): Promise<any> => {
    let url = `${Constants.apiBaseURL}/admin/coiRequest/${requestId}`;
    return await axios.get(url, () => void 0, undefined);
}

// Get news request by id.
export const getNewsRequestByIdAsync = async (requestId: string): Promise<any> => {
    let url = `${Constants.apiBaseURL}/admin/newsRequest/${requestId}`;
    return await axios.get(url, () => void 0, undefined);
}



/**
 * Approves the COI requests which are in pending state.
 * @param requestIds The request Ids to be approved.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const approveCoiRequests = async (
    requestIds: string[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/admin/requests/coi/approve`;

    return axios.post(apiEndpoint, handleTokenAccessFailure, requestIds);
}

/**
 * Rejects the COI requests which are in pending state.
 * @param requestIds The request Ids to be rejected.
 * @param rejectComments The reject comments.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const rejectCoiRequests = async (
    requestIds: string[],
    rejectComments: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/admin/requests/coi/reject`;
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({ rejectReason: rejectComments });

    return axios.post(apiEndpoint, handleTokenAccessFailure, requestIds, config);
}

/**
 * Approves the news article requests which are in pending state.
 * @param requestIds The request Ids to be approved.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const approveNewsArticleRequests = async (
    requestIds: string[],
    handleTokenAccessFailure: (error: string) => void,
    makeNewsArticleImportant: boolean | null = null) => {
    let apiEndpoint: string = `/admin/requests/news/approve`;
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({ makeNewsArticleImportant });

    return axios.post(apiEndpoint, handleTokenAccessFailure, requestIds, config);
}

/**
 * Rejects the news article requests which are in pending state.
 * @param requestIds The request Ids to be rejected.
 * @param rejectComments The reject comments.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const rejectNewsArticleRequests = async (
    requestIds: string[],
    rejectComments: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/admin/requests/news/reject`;
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({ rejectReason: rejectComments });

    return axios.post(apiEndpoint, handleTokenAccessFailure, requestIds, config);
}