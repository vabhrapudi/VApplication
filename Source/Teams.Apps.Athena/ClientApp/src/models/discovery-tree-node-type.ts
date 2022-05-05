// <copyright file="discovery-tree-node-type.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

export default interface IDiscoveryTreeNodeType {
    nodeTypeId: number;
    title: string;
    jsonFile: string;
    notes: string;
    dateFieldName: string;
    icon: string;
    isChecked: boolean;
}