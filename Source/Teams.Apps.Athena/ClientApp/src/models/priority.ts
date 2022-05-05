// <copyright file="priority.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

export default interface IPriority {
    id: string;
    title: string;
    description: string;
    type: number;
    keywords: number[];
    isChecked: boolean;
}