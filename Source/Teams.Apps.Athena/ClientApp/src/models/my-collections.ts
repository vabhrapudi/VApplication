// <copyright file="my-collections.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

export interface IMyCollections {
    collectionId?: string,
    name: string,
    description: string,
    imageURL: string
    items?: Item[]
}

export interface Item {
    itemId: string,
    itemType: number
}

export interface IMyCollectionsItem {
    collectionId: string,
    collectionItemName: string,
    collectionItemId: string,
    createdBy: string,
    createdOn: string,
    category: string,
    createdByName: string,
    createdByProfilePhoto: string
    collectionItemType: number,
    externalLink: string,
    source: string
}