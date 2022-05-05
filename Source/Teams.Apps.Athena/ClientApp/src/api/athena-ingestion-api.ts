// <copyright file="athena-ingestion-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from "../api/axios-decorator";
import { AxiosRequestConfig } from "axios";

/**
 *  Creates a athena ingestion update request.
 * @param filePath
 * @param dbEntity
 * @param handleTokenAccessFailure
 */
export const updateAthenaIngestionAsync = async (
    dbEntity: string,
    filePath: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/athenaIngestion/upsertEntity";
    let config: AxiosRequestConfig = axios.getAPIRequestConfigParams({
        entityName: dbEntity,
        path: filePath
    });

    return axios.post(apiEndpoint, handleTokenAccessFailure, config);
}

export const getAthenaIngestionDetailsAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = "/athenaIngestion";

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}
    