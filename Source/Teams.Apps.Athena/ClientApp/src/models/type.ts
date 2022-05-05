// <copyright file="type.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import IKeyword from "./keyword";

export interface IDropdownItem {
    key: string | null,
    header: string,
}

export interface IFaqCategoryItem {
    key: number,
    header: string,
}

export interface IFaqKeyword {
    id: string | null,
    title: string
}

export interface IFilterDropdownItem {
    id: string,
    title: string,
    isChecked: boolean | false
}

export interface IResearchNews {
    tableId: string,
    newsId: number,
    title: string,
    abstract: string,
    body: string,
    externalLink: string,
    imageUrl: string,
    keywordsJson: IKeyword[],
    createdAt: string,
    createdBy: string,
    isImportant: boolean,
    sumOfRatings: number,
    numberOfRatings: number,
    userRating: number,
    publishedDate: Date;
    keywords: number[];
    newsSourceId: number;
}

export interface IFaq {
    questionId: string,
    question: string,
    answer: string,
    keywords: IFaqKeyword[],
    createdBy: string,
    createdAt: string,
    updatedBy: string,
    updatedAt: string,
    isFaqViewExpanded: boolean | false
}