// <copyright file="menu-items.ts">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import RequestStatus from "./request-status";

export interface ICoiMenuItems {
    coiName: string | undefined;
    type: number | undefined;
    status: RequestStatus;
    isChecked: boolean | false;
    coiId: string | undefined;
    groupLink: string | undefined;
    imageURL: string | undefined;
    keywords: string | undefined;
    coiDescription: string | undefined;
    createdOn: string | undefined
    createdByUserPrincipalName: string | undefined;
    createdByObjectId: string | undefined;
    updatedOn: string | undefined;
    isApproved: string | undefined;
    createdBy: string | undefined;
}

export interface INewsMenuItems {
    title: string | undefined;
    status: RequestStatus;
    isChecked: boolean | false;
    newsId: string | undefined;
    body: string | undefined;
    imageURL: string | undefined;
    keywords: string | undefined;
    externalLink: string | undefined;
    isApproved: string | undefined;
    createdAt: string | undefined;
    createdBy: string | undefined;
    isImportant: boolean;
}