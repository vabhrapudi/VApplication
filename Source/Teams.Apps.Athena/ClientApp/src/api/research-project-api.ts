// <copyright file="research-project-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import IResearchProject from '../models/research-project';
import { AxiosResponse } from 'axios';

/**
 * Creates research project
 * @param researchProjectData
 * @param handleTokenAccessFailure
 */
export const createResearchProjectAsync = async (
    researchProjectData: IResearchProject,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = "/researchProjects";
    return await axios.post(url, handleTokenAccessFailure, researchProjectData);
}

/**
 * Gets research project by research project's table id.
 * @param researchProjectTableId
 * @param handleTokenAccessFailure
 */
export const getResearchProjectTableIdAsync = async (
    researchProjectTableId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/researchProjects/${researchProjectTableId}`;
    return await axios.get(url, handleTokenAccessFailure);
}

/**
 * Rates research project.
 * @param researchProjectTableId
 * @param rating
 * @param handleTokenAccessFailure
 */
export const rateResearchProjectAsync = async (
    researchProjectTableId: string,
    rating: number,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/researchProjects/rate/${researchProjectTableId}/${rating}`;
    return await axios.post(url, handleTokenAccessFailure);
}