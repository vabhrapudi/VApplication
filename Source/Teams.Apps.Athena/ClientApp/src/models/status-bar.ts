// <copyright file="status-bar.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import { ActivityStatus } from "./activity-status";

export default interface IStatusBar {
    id: number
    message: string,
    type: ActivityStatus
}