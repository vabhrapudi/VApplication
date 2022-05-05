// <copyright file="helper-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import baseAxios, { AxiosRequestConfig } from "axios";

/**
 * Gets the API request configuration parameters
 * @param params The request parameters
 */
export const getAPIRequestConfigParams = (params: any) => {
    let config: AxiosRequestConfig = baseAxios.defaults;
    config.params = params;

    return config;
}