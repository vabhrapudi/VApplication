// <copyright file="news-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import { IResearchNews } from '../models/type';
import { AxiosRequestConfig, AxiosResponse } from 'axios';
import INewsFilterParameters from '../models/news-filter-parameters';
import { IAthenaNewsSource } from '../models/athena-news-source';

/**
 * API request to rate news
 * @param newsId The id of news
 * @param rating Rating for news
 * @param handleTakenAccessFailure The callback function to handle token access failure.
 */
export const rateNews = async (
    newsId: string,
    rating: number,
    handleTakenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/news/rate/${newsId}/${rating}`;
    return await axios.post(url, handleTakenAccessFailure);
}

/**
 * API request to get news by news table Id.
 * @param tableId The table id of news
 * @param handleTakenAccessFailure The callback function to handle token access failure.
 */
export const getNewsByTableIdAsync = async (
    tableId: string,
    handleTakenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/news/${tableId}`;
    return await axios.get(url, handleTakenAccessFailure);
}

/**
 * API request to get news data from search service
 * @param searchString The searched string
 * @param keywords The array of keywords that need to be filtered
 * @param pageCount The page count value.
 * @param sortBy The sort by filter
 * @param handleTakenAccessFailure The callback function to handle token access failure.
 */
export const searchNewsAsync = async (
    searchString: string,
    newsFilter: INewsFilterParameters,
    pageCount: number,
    sortBy: number,
    handleTakenAccessFailure: (error: string) => void): Promise<AxiosResponse<IResearchNews[]>> => {
    let url = "/news/search";
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({ searchString: searchString, pageCount: pageCount, sortBy: sortBy });

    return await axios.post(url, handleTakenAccessFailure, newsFilter, config);
}

/**
 * API request to get news data of COI team from search service
 * @param teamId Team Id.
 * @param searchString The searched string.
 * @param keywords The array of keywords that need to be filtered.
 * @param pageCount The page count value.
 * @param sortBy The sort by filter.
 * @param handleTakenAccessFailure The callback function to handle token access failure.
 */
export const searchCOINewsAsync = async (
    teamId: string,
    searchString: string,
    newsFilter: INewsFilterParameters,
    pageCount: number,
    sortBy: number,
    handleTakenAccessFailure: (error: string) => void): Promise<AxiosResponse<IResearchNews[]>> => {
    let url = "/news/coiNewsSearch";
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({ teamId: teamId, searchString: searchString, pageCount: pageCount, sortBy: sortBy });

    return await axios.post(url, handleTakenAccessFailure, newsFilter, config);
}

/**
 * Gets the athena news sources.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getAthenaNewsSourcesAsync = async (
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<IAthenaNewsSource[]>> => {
    let apiEndpoint = "/news/sources"

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * API request to fetch node types for news.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getNodeTypesForNewsAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/news/node-types";
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * API request to fetch keyword Ids for news.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getNewsKeywordIdsAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/news/keywordIds";
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * API to update new artcle.
 * @param tableId The table Id of news article.
 * @param isImportant Indicates whether the news article is important or not.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const updateNewsAsync = async (
    tableId: string,
    isImportant: boolean,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/news/update/${tableId}/${isImportant}`;
    return await axios.patch(apiEndpoint, handleTokenAccessFailure);
}