// <copyright file="coi-requests-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import axios from "../api/axios-decorator";
import ICoi from "../models/coi";
import SortOrder from "../models/sort-order";
import { AxiosRequestConfig } from "axios";
import CoiSortColumn from "../models/coi-sort-column";

/**
 * Creates a new COI request.
 * @param coiRequestDetails The details of COI request to be created.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const createCoiRequestAsync = async (
    coiRequestDetails: ICoi,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/coi/requests";

    return axios.post(apiEndpoint, handleTokenAccessFailure, coiRequestDetails);
}

/**
 * Gets all COI requests created by logged-in user.
 * @param searchText The search text to search in COI name or keywords.
 * @param pageNumber The page number for which data to be fetched.
 * @param sortColumn The column to be sorted.
 * @param sortOrder The order in which column to be sorted.
 * @param statusFilterValues The status filter values.
 * @param handleTokenAccessFailure The callback function to handle token access failure. 
 */
export const getCoiRequestsAsync = async (
    searchText: string,
    pageNumber: number,
    sortColumn: CoiSortColumn,
    sortOrder: SortOrder,
    statusFilterValues: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/coi/requests/me";
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({
        searchText,
        pageNumber,
        sortColumn,
        sortOrder
    });

    return axios.post(apiEndpoint, handleTokenAccessFailure, statusFilterValues, config);
}

/**
 * Gets a COI request details by COI Id.
 * @param tableId The COI Id of which requests to get.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getCoiRequestAsync = async (
    tableId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/coi/requests/me/${tableId}`;

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Creates a new COI draft request.
 * @param coiRequest The COI request details.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const saveCoiRequestAsDraftAsync = async (
    coiRequest: ICoi,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/coi/requests/me/draft`;

    return axios.post(apiEndpoint, handleTokenAccessFailure, coiRequest);
}

/**
 * Updates a drafted COI request.
 * @param coiRequest The COI request details.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const updateDraftCoiRequestAsync = async (
    coiRequest: ICoi,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/coi/requests/me/draft/update/${coiRequest.tableId}`;

    return axios.patch(apiEndpoint, handleTokenAccessFailure, coiRequest);
}

/**
 * Submits a drafted COI request.
 * @param coiRequest The COI request details.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const submitDraftCoiRequestAsync = async (
    coiRequest: ICoi,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/coi/requests/me/draft/submit/${coiRequest.tableId}`;

    return axios.patch(apiEndpoint, handleTokenAccessFailure, coiRequest);
}

/**
 * Deletes COI requests.
 * @param coiRequestIds The COI Ids to be deleted.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const deleteCoiRequestsAsync = (
    coiRequestIds: string[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/coi/requests/me/delete`;

    return axios.patch(apiEndpoint, handleTokenAccessFailure, coiRequestIds);
}

/**
 * Gets COI by table id.
 * @param coiTableId The COI table Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getCoiByTableIdAsync = async (
    coiTableId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/cois/${coiTableId}`;
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Rates COI.
 * @param coiTableId The COI table Id.
 * @param rating The rating.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const rateCoiAsync = async (
    coiTableId: string,
    rating: number,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/cois/rate/${coiTableId}/${rating}`;
    return await axios.post(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the total number of COIs created and approved in Athena.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getTotalApprovedCoiCount = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/cois/approved/count";
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}