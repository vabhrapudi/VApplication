// <copyright file="quick-access-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import { AxiosResponse } from 'axios';
import IQuickAccessListItem from '../models/quick-access-list-item';

/**
 * Gets the list of quick access items.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getQuickAccessListAsync = async (
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/quick-access`;
    return await axios.get(url, handleTokenAccessFailure);
}

/**
 * Adds comment to the resource.
 * @param quickAccessItem The quick access item.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const addQuickAccessItemAsync = async (
    quickAccessItem: IQuickAccessListItem,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/quick-access`;
    return await axios.post(url, handleTokenAccessFailure, quickAccessItem);
}

/**
 * Deletes the quick access item.
 * @param quickAccessItemId The quick access item Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const deleteQuickAccessItemAsync = async (
    quickAccessItemId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/quick-access/${quickAccessItemId}`;
    return await axios.delete(url, handleTokenAccessFailure);
}