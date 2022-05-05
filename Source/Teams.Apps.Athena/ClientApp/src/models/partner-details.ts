// <copyright file="partner-details.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import IKeyword from "./keyword";

export default interface IPartnerDetails {
    tableId: string;
    partnerId: number;
    nodeTypeId: number;
    description: string;
    organization: string;
    firstName: string;
    lastName: string;
    title: string;
    phone: string;
    otherContactInfo: string;
    keywords: number[];
    projects: string;
    sumOfRatings?: number;
    numberOfRatings?: number;
    userRating?: number;
    keywordsJson: IKeyword[];
}