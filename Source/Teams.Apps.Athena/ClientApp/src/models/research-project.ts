// <copyright file="research-project.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import IKeyword from "./keyword";

export default interface IResearchProject {
    tableId: string;
    researchProjectId: string;
    nodeTypeId: number;
    title: string;
    abstract: string;
    status: string;
    dateStarted?: Date;
    keywords: number[];
    researchDept: string;
    sumOfRatings?: number;
    numberOfRatings?: number;
    userRating?: number;
    files: string;
    keywordsJson: IKeyword[];
    statusDescription: string;
    authors: string;
    advisors: string;
    secondReaders: string;
    reviewerNotes: string;
    authorsOrg: string;
    degreeProgram: string;
    degreeLevel: string;
    degreeTitles: string;
    dateCompleted?: Date;
    recognition: string;
    originatingRequest: string;
    publisher: string;
    useRights: string;
    priority: number;
    importance: number;
}