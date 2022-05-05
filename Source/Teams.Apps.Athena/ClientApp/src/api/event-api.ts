// <copyright file="event-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import { AxiosResponse } from 'axios';

/**
 * Gets event by event's table id.
 * @param eventTableId The event's table Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getEventByTableIdAsync = async (
    eventTableId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/events/${eventTableId}`;
    return await axios.get(url, handleTokenAccessFailure);
}

/**
 * Rates event.
 * @param eventTableId The event's table Id.
 * @param rating The rating.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const rateEventAsync = async (
    eventTableId: string,
    rating: number,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/events/rate/${eventTableId}/${rating}`;
    return await axios.post(url, handleTokenAccessFailure);
}