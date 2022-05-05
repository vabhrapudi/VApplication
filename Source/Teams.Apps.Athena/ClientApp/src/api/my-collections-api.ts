// <copyright file="my-collections-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import { IMyCollections, IMyCollectionsItem } from '../models/my-collections';
import {  AxiosResponse } from 'axios';

// Gets all collections data
export const getAllCollectionsDataAsync = async (
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<IMyCollections[]>> => {
    let url = "/collections";
    return await axios.get(url, handleTokenAccessFailure);
}

// Gets collection item details
export const getCollectionItemDetailsAsync = async (
    collectionId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<IMyCollectionsItem[]>> => {
    let url = `/collections/${collectionId}`;
    return await axios.get(url, handleTokenAccessFailure);
}

// Gets collection details by id.
export const getCollectionByIdAsync = async (
    collectionId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<IMyCollections>> => {
    let url = `/collections/collection/${collectionId}`;
    return await axios.get(url, handleTokenAccessFailure);
}

// Updates collection data
export const updateCollectionDataAsync = async (
    collectionId: string,
    collectionData: IMyCollections,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<IMyCollections>> => {
    let url = `/collections/${collectionId}`;
    return await axios.patch(url, handleTokenAccessFailure, collectionData);
}

// Creates collection
export const createCollectionAsync = async (
    collectionData: IMyCollections,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<IMyCollections>> => {
    let url = "/collections";
    return await axios.post(url, handleTokenAccessFailure, collectionData);
}

// Deletes collection
export const deleteCollectionAsync = async (
    collectionId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/collections/${collectionId}`;
    return await axios.delete(url, handleTokenAccessFailure);
}