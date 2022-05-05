// <copyright file="priority-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from "../api/axios-decorator";
import IPriority from "../models/priority";

/**
 * Gets all priorities.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getAllPrioritiesAsync = async (handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/priorities"
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Creates a new priority.
 * @param priorityDetails The priority details to be created.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const createPriorityAsync = async (
    priorityDetails: IPriority,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/priorities"
    return await axios.post(apiEndpoint, handleTokenAccessFailure, priorityDetails);
}

/**
 * Updates a priority details.
 * @param priorityDetails The priority details to be updated.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const updatePriorityAsync = async (
    priorityDetails: IPriority,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/priorities"
    return await axios.patch(apiEndpoint, handleTokenAccessFailure, priorityDetails);
}

/**
 * Deletes priorities.
 * @param priorityIds The priority Ids to be deleted.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const deletePrioritiesAsync = async (
    priorityIds: string[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/priorities"
    return await axios.delete(apiEndpoint, handleTokenAccessFailure, undefined, priorityIds);
}

/**
 * Gets the priority details.
 * @param priorityId The priority Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getPriorityByIdAsync = async (
    priorityId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = `/priorities/${priorityId}`;
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the priority types.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getPriorityTypesAsync = async (handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/priorities/types"
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the insights.
 * @param priorityIds The collection of priority Ids.
 * @param selectedKeywordIds The collection of selected keyword Ids.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getInsightsAsync = async (
    priorityIds: string[],
    selectedKeywordIds: number[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/priorities/insights"

    let data = {
        priorityIds,
        KeywordIdsFilter: selectedKeywordIds
    };

    return await axios.post(apiEndpoint, handleTokenAccessFailure, data);
}