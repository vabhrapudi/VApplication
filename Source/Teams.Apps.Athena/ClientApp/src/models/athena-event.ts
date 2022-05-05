// <copyright file="athena-event.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import IKeyword from "./keyword";

export default interface IAthenaEvent {
    tableId: string;
    eventId: number;
    nodeTypeId: number;
    securityLevel: number;
    keywords: number[];
    dateOfEvent?: Date;
    title: string;
    description: string;
    organization: string;
    location: string;
    webSite: string;
    otherContactInfo: string;
    sumOfRatings?: number;
    numberOfRatings?: number;
    userRating?: number;
    keywordsJson: IKeyword[];
}