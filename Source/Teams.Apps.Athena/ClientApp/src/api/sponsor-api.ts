// <copyright file="sponsor-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import { AxiosResponse } from 'axios';

/**
 * Gets sponsor by sponsor's table Id.
 * @param sponsorTableId The sponsor's table Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getSponsorByTableIdAsync = async (
    sponsorTableId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/sponsors/${sponsorTableId}`;
    return await axios.get(url, handleTokenAccessFailure);
}

/**
 * Rates sponsor.
 * @param sponsorTableId The sponsor's table Id.
 * @param rating The rating.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const rateSponsorAsync = async (
    sponsorTableId: string,
    rating: number,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/sponsors/rate/${sponsorTableId}/${rating}`;
    return await axios.post(url, handleTokenAccessFailure);
}