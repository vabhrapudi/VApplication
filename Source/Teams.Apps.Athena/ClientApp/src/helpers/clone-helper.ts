// <copyright file="clone-helper.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import { DropdownItemProps } from "@fluentui/react-northstar";
import ICoi from "../models/coi";
import IFilterItem from "../models/filter-item";
import INews from "../models/news";

/**
 * Deep clones the news articles collection.
 * @param newsArticles The collection of news articles to be cloned.
 */
export const cloneDeepNewsArticles = (newsArticles: INews[]): INews[] => {
    return newsArticles.map((newsArticle: INews) => {
        return {
            ...newsArticle,
            isChecked: newsArticle.isChecked === undefined ? false : newsArticle.isChecked
        }
    });
}

/**
 * Deep clones the filter bar items displayed in filter popup.
 * @param filterItems The filter items to be cloned.
 */
export const cloneFilterItems = (filterItems: IFilterItem[]): IFilterItem[] => {
    return filterItems.map((filterItem: IFilterItem) => {
        return {
            ...filterItem,
            isChecked: filterItem.isChecked === undefined ? false : filterItem.isChecked
        }
    });
}

/**
 * Deep clones the COI collection.
 * @param cois The collection of COIs to be cloned.
 */
export const cloneDeepCois = (cois: ICoi[]): ICoi[] => {
    return cois.map((coi: ICoi) => {
        return {
            ...coi,
            isChecked: coi.isChecked === undefined ? false : coi.isChecked
        }
    });
}

/**
 * Deep clones the dropdown item collection.
 * @param dropdownItems The collection of dropdown items.
 */
export const cloneDeepDropdownItems = (dropdownItems: DropdownItemProps[]): DropdownItemProps[] => {
    return dropdownItems.map((dropdownItem: DropdownItemProps) => {
        return {
            ...dropdownItem
        }
    })
}