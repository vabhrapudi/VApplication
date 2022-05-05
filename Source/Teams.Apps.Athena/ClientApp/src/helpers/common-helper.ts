// <copyright file="url-helper.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

/**
 * Gets string joined by separator.
 * @param collection The array.
 * @param separator The separator.
 */
export const getStringJoinedBySeparator = (collection: any[]| undefined, separator: string) => {
    if (!collection?.length) {
        return "NA";
    }
    return collection.join(separator);
}