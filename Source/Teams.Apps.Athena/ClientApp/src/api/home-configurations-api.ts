// <copyright file="home-configurations-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from "../api/axios-decorator";
import IHomeConfigurationArticle from "../models/home-configuration-article";
import IHomeStatusBarConfiguration from "../models/home-status-bar-configuration";

/**
 * Creates a new home configuration article.
 * @param teamId The team Id.
 * @param articleDetails The article details.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const createHomeConfigurationArticleAsync = async (
    teamId: string,
    articleDetails: IHomeConfigurationArticle,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/home/configuration/${teamId}`;

    return axios.post(apiEndpoint, handleTokenAccessFailure, articleDetails);
}

/**
 * Updates a home configuration article.
 * @param teamId The team Id.
 * @param articleDetails The article details.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const updateHomeConfigurationArticleAsync = async (
    teamId: string,
    articleDetails: IHomeConfigurationArticle,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/home/configuration/${teamId}`;

    return axios.patch(apiEndpoint, handleTokenAccessFailure, articleDetails);
}

/**
 * Deletes the home configuration articles.
 * @param teamId The team Id.
 * @param articleIds The article Ids to delete.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const deleteHomeConfigurationArticlesAsync = async (
    teamId: string,
    articleIds: string[],
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/home/configuration/${teamId}`;

    return axios.delete(apiEndpoint, handleTokenAccessFailure, undefined, articleIds);
}

/**
 * Gets the home configuration article details.
 * @param teamId The team Id.
 * @param articleId The article Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getHomeConfigurationArticleAsync = async (
    teamId: string,
    articleId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/home/configuration/${teamId}/${articleId}`;

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the all home configuration articles of a team.
 * @param teamId The team Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getHomeConfigurationArticlesAsync = async (
    teamId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/home/configuration/${teamId}`;

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Creates the home status bar configuration.
 * @param teamId The team Id.
 * @param homeStatusBarConfigurationDetails The home status bar configuration details.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const createHomeStatusBarConfigurationAsync = async (
    teamId: string,
    homeStatusBarConfigurationDetails: IHomeStatusBarConfiguration,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/home/configuration/statusbar/${teamId}`;

    return axios.post(apiEndpoint, handleTokenAccessFailure, homeStatusBarConfigurationDetails);
}

/**
 * Updates the home status bar configuration.
 * @param teamId The team Id.
 * @param homeStatusBarConfigurationDetails The home status bar configuration details.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const updateHomeStatusBarConfigurationAsync = async (
    teamId: string,
    homeStatusBarConfigurationDetails: IHomeStatusBarConfiguration,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/home/configuration/statusbar/${teamId}`;

    return axios.patch(apiEndpoint, handleTokenAccessFailure, homeStatusBarConfigurationDetails);
}

/**
 * Gets the home status bar configuration details.
 * @param teamId The team Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getHomeStatusBarConfigurationAsync = async (
    teamId: string,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint: string = `/home/configuration/statusbar/${teamId}`;

    return axios.get(apiEndpoint, handleTokenAccessFailure);
}