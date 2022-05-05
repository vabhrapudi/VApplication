// <copyright file="home-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import axios from './axios-decorator';

/**
 * Gets the status bar details to be displayed on Athena central team home tab.
 * @param teamId The team Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getStatusBarDetailsForCentralTeamAsync = async (
    teamId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = `/home/${teamId}/statusbar`;
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the status bar details to be displayed on COI team home tab.
 * @param teamId The team Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getStatusBarDetailsForCoiTeamAsync = async (
    teamId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = `/home/coi/${teamId}/statusbar`;
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the new to Athena articles for Athena central team.
 * @param teamId The team Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getNewToAthenaArticlesForCentralTeamAsync = async (
    teamId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = `/home/${teamId}/new-articles`;
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the new to Athena articles for COI team.
 * @param teamId The team Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getNewToAthenaArticlesForCoiTeamAsync = async (
    teamId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = `/home/coi/${teamId}/new-articles`;
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the daily briefing articles of an user for Athena Central team.
 * @param teamId The team Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getDailyBriefingArticlesOfUserForCentralTeamAsync = async (
    teamId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = `/home/${teamId}/briefing-articles`;
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the daily briefing articles of an user for a COI team.
 * @param teamId The team Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getDailyBriefingArticlesOfUserForCoiTeamAsync = async (
    teamId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = `/home/coi/${teamId}/briefing-articles`;
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}