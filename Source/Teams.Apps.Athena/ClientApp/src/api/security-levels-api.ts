// <copyright file="security-levels-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import axios from "../api/axios-decorator";

/**
 * API request to fetch security levels.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getSecurityLevelsAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/security-levels";
    return axios.get(apiEndpoint, handleTokenAccessFailure);
}