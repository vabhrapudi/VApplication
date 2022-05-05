// <copyright file="discovery-tree-taxonomy-element.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

export default interface IDiscoveryTreeTaxonomyElement {
    taxonomyId: string;
    parentId: number;
    nodeTypeId: number;
    title: string;
    keywords: number[];
    type: any;
    tooltip: string;
    dataValue: any;
    date?: Date;
}