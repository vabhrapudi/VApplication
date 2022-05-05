// <copyright file="news-requests-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import axios from "../api/axios-decorator";
import { AxiosRequestConfig } from "axios";
import INews from "../models/news";
import NewsArticleSortColumn from "../models/news-article-sort-column";
import SortOrder from "../models/sort-order";

/**
 * Creates a new news article request.
 * @param newsArticleRequestDetails The details of news article request to be created.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const createNewsArticleRequestAsync = async (
    newsArticleRequestDetails: INews,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/news/requests";

    return axios.post(apiEndpoint, handleTokenAccessFailure, newsArticleRequestDetails);
}

/**
 * Gets all active news article requests created by logged-in user.
 * @param searchText The search text to search in news article title or keywords.
 * @param pageNumber The page number for which data to be fetched.
 * @param sortColumn The column to be sorted.
 * @param sortOrder The order in which column to be sorted.
 * @param statusFilterValues The status filter values.
 * @param handleTokenAccessFailure The callback function to handle token access failure. 
 */
export const getNewsArticleRequestsAsync = async (
    searchText: string,
    pageNumber: number,
    sortColumn: NewsArticleSortColumn,
    sortOrder: SortOrder,
    statusFilterValues: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/news/requests/me";
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({
        searchText,
        pageNumber,
        sortColumn,
        sortOrder
    });

    return axios.post(apiEndpoint, handleTokenAccessFailure, statusFilterValues, config);
}

/**
 * Gets a news article details by news Id.
 * @param tableId The news table Id of which requests to get.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getNewsArticleRequestAsync = async (
    tableId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/news/requests/me/${tableId}`;

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Creates a new news article draft request.
 * @param newsArticleRequest The news article request details.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const saveRequestAsDraftAsync = async (
    newsArticleRequest: INews,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/news/requests/me/draft`;

    return axios.post(apiEndpoint, handleTokenAccessFailure, newsArticleRequest);
}

/**
 * Updates a drafted news article request.
 * @param newsArticleRequest The news article request details.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const updateDraftNewsArticleRequestAsync = async (
    newsArticleRequest: INews,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/news/requests/me/draft/update/${newsArticleRequest.tableId}`;

    return axios.patch(apiEndpoint, handleTokenAccessFailure, newsArticleRequest);
}

/**
 * Submits a drafted news article request.
 * @param newsArticleRequest The news article request details.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const submitDraftNewsArticleRequestAsync = async (
    newsArticleRequest: INews,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/news/requests/me/draft/submit/${newsArticleRequest.tableId}`;

    return axios.patch(apiEndpoint, handleTokenAccessFailure, newsArticleRequest);
}

/**
 * Deletes news article requests.
 * @param newsArticleRequestIds The news Ids to be deleted.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const deleteRequestsAsync = (
    newsArticleRequestIds: string[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/news/requests/me/delete`;

    return axios.patch(apiEndpoint, handleTokenAccessFailure, newsArticleRequestIds);
}