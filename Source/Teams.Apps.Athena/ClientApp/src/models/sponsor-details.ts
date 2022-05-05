// <copyright file="sponsor-details.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import IKeyword from "./keyword";

export default interface ISponsorDetails {
    tableId: string;
    sponsorId: number;
    nodeTypeId: number;
    firstName: string;
    lastName: string;
    profileImage: string;
    emailAddress: string;
    title: string;
    description: string;
    organization: string;
    service: string;
    phone: string;
    otherContactInfo: string;
    keywords: number[];
    sumOfRatings?: number;
    numberOfRatings?: number;
    userRating?: number;
    keywordsJson: IKeyword[];
}