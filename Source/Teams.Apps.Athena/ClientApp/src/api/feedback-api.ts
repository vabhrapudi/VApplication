// <copyright file="coi-requests-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import axios from "../api/axios-decorator";
import { AthenaFeedBackEntity } from "../models/athena-feedback";
import { AxiosRequestConfig } from "axios";

/**
 * Saves Athena feedback.
 * @param coiRequestDetails The details of COI request to be created.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const saveAthenaFeedbackAsync = async (
    feedback: AthenaFeedBackEntity,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/feedback/athena";

    return axios.post(apiEndpoint, handleTokenAccessFailure, feedback);
}

/**
 * Gets Athena feedbacks.
 * @param pageNumber The page number value.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getAthenaFeedbacksAsync = async (
    pageNumber: number,
    feedbackFilterValues: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/feedback/athenaFeedbacks";
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({
        pageNumber
    });

    return axios.post(apiEndpoint, handleTokenAccessFailure, feedbackFilterValues, config);
}

/**
 * Gets Athena feedback details.
 * @param feedbackId The feedback Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getFeedbackDetailsAsync = async (
    feedbackId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/feedback/athena/${feedbackId}`;

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}