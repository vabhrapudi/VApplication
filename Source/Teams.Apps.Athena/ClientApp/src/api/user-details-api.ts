// <copyright file="user-details-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import { AxiosResponse } from 'axios';


// Get logged-in user details from graph.
export const getUserDetailsAsync = async (
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/graph/users/me`;
    return await axios.get(url, handleTokenAccessFailure);
}

// Gets user details by user id from graph.
export const GetUserDetailsByUserIdAsync = async (
    userId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/graph/users/${userId}`;
    return await axios.get(url, handleTokenAccessFailure);
}

// Get logged-in user image from graph.
export const getUserImage = async (
    userAADId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/graph/users/${userAADId}/image`;
    return await axios.get(url, handleTokenAccessFailure);
}

// Get users details from graph.
export const getUsersDetailsFromGraphAsync = async (
    userIds: string[],
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/graph/users/details`;
    return await axios.post(url, handleTokenAccessFailure, userIds);
}