// <copyright file="research-proposal-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from './axios-decorator';
import IResearchProposal from '../models/research-proposal';
import { AxiosResponse } from 'axios';

/**
 * Creates research proposal. 
 * @param researchProposalData The research proposal data.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const createResearchProposalAsync = async (
    researchProposalData: IResearchProposal,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = "/research-proposals";
    return await axios.post(url, handleTokenAccessFailure, researchProposalData);
}

/**
 * Gets research proposal by research proposal's table Id.
 * @param researchProposalTableId The research proposal's table Id.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const getResearchProposalByTableIdAsync = async (
    researchProposalTableId: string,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/research-proposals/${researchProposalTableId}`;
    return await axios.get(url, handleTokenAccessFailure);
}

/**
 * Rates research proposal.
 * @param researchProposalTableId The research proposal's table Id.
 * @param rating The rating.
 * @param handleTokenAccessFailure The callback function to handle token access failure.
 */
export const rateResearchProposalAsync = async (
    researchProposalTableId: string,
    rating: number,
    handleTokenAccessFailure: (error: string) => void): Promise<AxiosResponse<any>> => {
    let url = `/research-proposals/rate/${researchProposalTableId}/${rating}`;
    return await axios.post(url, handleTokenAccessFailure);
}