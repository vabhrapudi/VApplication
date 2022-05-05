// <copyright file="athena-tool.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

export default interface IAthenaTool {
    tableId: string;
    toolId: number;
    nodeTypeId: number;
    securityLevel: number;
    keywords: number[];
    submitterId: number;
    title: string;
    description: string;
    manufacturer: string;
    usageLicensing: string;
    userComments: string;
    avgUserRating: number;
    userRatings: number[];
    website: string;
}