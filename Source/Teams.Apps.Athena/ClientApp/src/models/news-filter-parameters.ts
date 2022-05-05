// <copyright file="news-filter-parameters.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import IDiscoveryTreeNodeType from "./discovery-tree-node-type";
import IKeyword from "./keyword";

export default interface INewsFilterParameters {
    keywords: IKeyword[],
    newsTypes: IDiscoveryTreeNodeType[]
}