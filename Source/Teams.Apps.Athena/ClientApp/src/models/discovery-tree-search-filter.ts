// <copyright file="discovery-tree-search-filter.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>]

import { IDiscoveryTreeSelectedFilters } from "./discovery-filter";

export default interface IDiscoveryTreeSearchAndFilter {
    searchStrings: string[],
    searchKeywords: number[],
    selectedFilters: IDiscoveryTreeSelectedFilters[]
}