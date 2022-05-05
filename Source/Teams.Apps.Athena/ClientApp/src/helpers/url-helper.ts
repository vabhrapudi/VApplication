// <copyright file="url-helper.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

/**
 * Validates whether a string is valid URL.
 * @param urlString The URL string to be validated.
 */
export function IsValidUrl(urlString: string) {
    let regExpString = /^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/gm;

    let regExp = new RegExp(regExpString);
    return regExp.test(urlString);
}