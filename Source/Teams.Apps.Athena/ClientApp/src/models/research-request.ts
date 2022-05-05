// <copyright file="research-request.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import IKeyword from "./keyword";

export default interface IResearchRequest {
    tableId: string;
    researchRequestId: number;
    title: string;
    nodeTypeId: number;
    keywords: number[];
    description: string;
    details: string;
    priority: number;
    topicType: string;
    securityLevel: number;
    lastUpdate?: Date;
    focusQuestion1: string;
    focusQuestion2: string;
    focusQuestion3: string;
    focusQuestion4: string;
    focusQuestion5: string;
    startDate?: Date;
    completionTime: string;
    endorsements: string;
    potentialFunding: string;
    desiredCurriculum1: string;
    desiredCurriculum2: string;
    desiredCurriculum3: string;
    desiredCurriculum4: string;
    desiredCurriculum5: string;
    erbTrbOrg: string;
    importance: number;
    sumOfRatings?: number;
    numberOfRatings?: number;
    userRating?: number;
    keywordsJson: IKeyword[];
    sponsors: string;
    status: string;
    fiscalYear: number;
    topicNotes: string;
    createDate?: Date;
    irefTitle: string;
    completionDate?: Date;
    contributingStudentsCount: number;
    notesByUser: string;
}