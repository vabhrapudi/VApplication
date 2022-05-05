// <copyright file="research-proposal.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import IKeyword from "./keyword";

export default interface IResearchProposal {
    tableId?: string;
    researchProposalId?: number;
    nodeTypeId?: number;
    keywords?: number[];
    lastUpdate?: Date;
    title: string;
    status?: string;
    details: string;
    priority: number;
    topicType: string;
    focusQuestion1: string;
    focusQuestion2: string;
    focusQuestion3: string;
    objectives: string;
    plan: string;
    deliverables: string;
    budget: string;
    startDate?: Date;
    completionTime: string;
    endorsements: string;
    potentialFunding: string;
    description: string;
    sumOfRatings?: number;
    numberOfRatings?: number;
    userRating?: number;
    submittedBy?: string;
    keywordsJson: IKeyword[];
    securityLevel: number;
}