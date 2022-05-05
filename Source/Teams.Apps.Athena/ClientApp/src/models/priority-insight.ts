// <copyright file="priority-insight.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

export default interface IPriorityInsight {
    title: string;
    proposed: number;
    current: number;
    completed: number;
    val: number;
}