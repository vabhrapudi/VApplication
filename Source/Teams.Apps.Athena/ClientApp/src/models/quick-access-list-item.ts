// <copyright file="quick-access-list-item.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

export default interface IQuickAccessListItem {
    quickAccessItemId?: string;
    taxonomyId: string;
    parentId: number;
    nodeTypeId: number;
}