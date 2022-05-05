// <copyright file="user-setting-tab-api.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import axios from "./axios-decorator";
import Constants from "../constants/constants";
import IKeyword from "../models/keyword";

/**
* Creates a new user
* @param selectedKeywords Selected keywords.
*/
export const getApprovedCoiRequestAsync = async (selectedKeywords: IKeyword[], fetchPublicOnly: boolean = false): Promise<any> => {
    let url = `${Constants.apiBaseURL}/coi/requests/approved?fetchPublicOnly=${fetchPublicOnly}`;
    return await axios.post(url, () => void 0, selectedKeywords, undefined);
}