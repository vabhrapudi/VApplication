// <copyright file="partner-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import { AxiosResponse } from 'axios';

/**
 * Gets partner by partner's table id.
 * @param partnerTableId The partner's table Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getPartnerByTableIdAsync = async (
    partnerTableId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/partners/${partnerTableId}`;
    return await axios.get(url, handleTokenAccessFailure);
}

/**
 * Rates partner.
 * @param partnerTableId The partner's table Id.
 * @param rating The rating.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const ratePartnerAsync = async (
    partnerTableId: string,
    rating: number,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/partners/rate/${partnerTableId}/${rating}`;
    return await axios.post(url, handleTokenAccessFailure);
}