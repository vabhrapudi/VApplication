// <copyright file="athena-info-resource.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

export default interface IAthenaInfoResource {
    tableId: string;
    infoResourceId: number;
    nodeTypeId: number;
    securityLevel: number;
    keywords: number[];
    lastUpdate: Date;
    title: string;
    description: string;
    publishedDate: Date;
    publisher: string;
    provenance: string;
    collection: string;
    sourceOrg: string;
    sourceGroup: string;
    isPartOfSeries: string;
    website: string;
    docId: string;
    usageLicensing: string;
    userComments: string;
    avgUserRating: number;
}