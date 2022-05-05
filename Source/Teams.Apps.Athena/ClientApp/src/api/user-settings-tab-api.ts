// <copyright file="user-setting-tab-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from "./axios-decorator";
import Constants from "../constants/constants";
import IUserSettings from "../models/user-settings";
import IDiscoveryTreePersistentData from "../models/discovery-tree-persistent-data";

/**
* Creates a new user
* @param userDetails The user details that needs to be created
*/
export const createUserAsync = async (userDetails: IUserSettings): Promise<any> => {
    let url = `${Constants.apiBaseURL}/usersetting`;
    return await axios.post(url, () => void 0, userDetails, undefined);
}

// Get logged-in user settings from database.
export const getUserSettingAsync = async (): Promise<any> => {
    let url = `${Constants.apiBaseURL}/usersetting`;
    return await axios.get(url, () => void 0, undefined);
}

// Get logged-in user details from graph.
export const getUserDetailsAsync = async (): Promise<any> => {
    let url = `${Constants.apiBaseURL}/graph/users/me`;
    return axios.get(url, () => void 0);
}

// Get user details by email address.
export const getUserDetailsByEmailAddressAsync = async (emailAddress: string): Promise<any> => {
    let url = `${Constants.apiBaseURL}/usersetting/${emailAddress}`;
    return axios.get(url, () => void 0);
}

// Update user settings
export const updateUserSettingAsync = async (userDetails: IUserSettings): Promise<any> => {
    let url = `${Constants.apiBaseURL}/usersetting`;
    return await axios.patch(url, () => void 0, userDetails, undefined)
}

// Get graduation degrees dropdown values.
export const getGraduationDegreesAsync = async () => {
    let url = `${Constants.apiBaseURL}/graduationDegree`;
    return await axios.get(url, () => void 0, undefined);
}

// Get organizations dropdown values.
export const getOrganizationsAsync = async () => {
    let url = `${Constants.apiBaseURL}/organization`;
    return await axios.get(url, () => void 0, undefined)
}

// Get Specialty dropdown values.
export const getSpecialtyAsync = async () => {
    let url = `${Constants.apiBaseURL}/specialty`;
    return await axios.get(url, () => void 0, undefined)
}

// Get study departments dropdown values.
export const getStudyDepartmentsAsync = async () => {
    let url = `${Constants.apiBaseURL}/studyDepartment`;
    return await axios.get(url, () => void 0, undefined)
}

/**
 * Gets job titles dropdown values.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getJobTitlesAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/usersetting/job-types";
    return axios.get(apiEndpoint, handleTokenAccessFailure);
}


/**
 *Save persistent data.
 * @param discoveryTreePersistentData The discovery tree persistent data.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const saveDiscoveryTreePersistentDataAsync = async (
    discoveryTreePersistentData: IDiscoveryTreePersistentData,
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/usersetting/discovery-tree/persistent-data/save";

    return axios.post(apiEndpoint, handleTokenAccessFailure, discoveryTreePersistentData);
}

/**
 * Gets the logged-in user's persistent data.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getUserPersistentDataAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/usersetting/persistent-data";
    return axios.get(apiEndpoint, handleTokenAccessFailure);
}

/**
 * Gets the Athena active users count who installed Athena app in personal scope.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getAthenaActiveUsersCountAsync = async (
    handleTokenAccessFailure: (error: string) => void) => {
    let apiEndpoint = "/usersetting/active-users/count";
    return await axios.get(apiEndpoint, handleTokenAccessFailure);
}