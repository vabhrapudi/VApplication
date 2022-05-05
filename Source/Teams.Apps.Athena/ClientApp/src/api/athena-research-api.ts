// <copyright file="athena-research-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from "../api/axios-decorator";

/**
 * Gets the athena research importance.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getAthenaResearchImportanceAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/athena-research/importance"

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the athena research priorities.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getAthenaResearchPrioritiesAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/athena-research/priorities"

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}