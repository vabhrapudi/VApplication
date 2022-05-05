// <copyright file="discovery-filter.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

export interface IDiscoveryFilter {
    key: number;
    value: string;
    isChecked: boolean;
    subFilter: IDiscoveryFilter[];
}

export interface IDiscoveryGroup {
    filterGroupId: number;
    filters: IDiscoveryFilter[]
}

export interface IDiscoveryTreeFilter {
    dbEntity: string;
    dbField: string;
    dbValue: number[];
    defaultOn: boolean;
    filterId: number;
    isChecked: boolean;
    parentId: number;
    title: string;
    toolbarIcon: string;
    dbFieldType: string;
    isVisible: boolean;
    enabled: boolean | string;
}

export interface IDiscoveryTreeSelectedFilters {
    type: number;
    filters: IDiscoveryTreeFilter[];
}