// <copyright file="coi.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import IKeyword from "./keyword";
import RequestStatus from "./request-status";

export default interface INews {
    tableId: string,
    newsId: number;
    title: string;
    body: string;
    abstract: string;
    externalLink: string;
    imageUrl: string;
    status: RequestStatus;
    keywordsJson: IKeyword[];
    isChecked: boolean;
    createdAt: Date;
    isImportant: boolean;
    sumOfRatings: number;
    numberOfRatings: number;
    createdBy: string;
    nodeTypeId: number;
    keywords: number[];
    publishedDate?: Date;
    newsSourceId: number;
}