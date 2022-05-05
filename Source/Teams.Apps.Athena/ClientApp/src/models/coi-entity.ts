// <copyright file="coi-entity.ts">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import IKeyword from "./keyword";

export interface ICoiEntity {
    tableId: string,
    coiId: number;
    coiName: string | undefined;
    keywordsJson: IKeyword[];
    isChecked: boolean | false;
    keywords: number[];
}