// <copyright file="research-request-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import { AxiosResponse } from 'axios';

/**
 * Gets research request by table id.
 * @param researchRequestTableId The research request's table Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getResearchRequestByTableIdAsync = async (
    researchRequestTableId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/research-requests/${researchRequestTableId}`;
    return await axios.get(url, handleTokenAccessFailure);
}

/**
 * Rates research request.
 * @param researchRequestTableId The research request's table Id.
 * @param rating The rating.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const rateResearchRequestAsync = async (
    researchRequestTableId: string,
    rating: number,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/research-requests/rate/${researchRequestTableId}/${rating}`;
    return await axios.post(url, handleTokenAccessFailure);
}