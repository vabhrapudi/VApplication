// <copyright file="comments-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import { AxiosResponse, AxiosRequestConfig } from 'axios';

/**
 * Gets the comments of resource.
 * @param resourceTableId The resource's table Id.
 * @param resourceTypeId The resource's type Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getResourceCommentsAsync = async (
    resourceTableId: string,
    resourceTypeId: number,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/comments`;
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({ resourceTableId: resourceTableId, resourceTypeId: resourceTypeId });
    return await axios.get(url, handleTokenAccessFailure, config);
}

/**
 * Adds comment to the resource.
 * @param resourceTableId The resource's table Id.
 * @param resourceTypeId The resource's type Id.
 * @param comment The comment.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const addResourceCommentAsync = async (
    resourceTableId: string,
    resourceTypeId: number,
    comment: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/comments`;
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({ resourceTableId: resourceTableId, resourceTypeId: resourceTypeId, comment: comment });
    return await axios.post(url, handleTokenAccessFailure, undefined, config);
}