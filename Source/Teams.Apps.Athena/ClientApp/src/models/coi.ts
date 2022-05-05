// <copyright file="coi.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import COIType from "./coi-type";
import IKeyword from "./keyword";
import RequestStatus from "./request-status";

export default interface ICoi {
    tableId: string,
    coiId: number;
    coiName: string;
    coiDescription: string;
    type: COIType;
    keywordsJson: IKeyword[];
    status: RequestStatus;
    isChecked: boolean;
    createdOn?: Date;
    createdBy: string;
    createdByName: string;
    teamId: string;
    channelId: string;
    nodeTypeId: number;
    keywords: number[];
    securityLevel: number;
    sumOfRatings?: number,
    numberOfRatings?: number,
    userRating?: number,
    organization: string;
    webSite: string;
}