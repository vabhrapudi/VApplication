// <copyright file="athena-feedback.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import UserDetails from "./user-details";

export interface AthenaFeedBackEntity {
    feedbackId?: string,
    feedback: number,
    details: string,
    createdAt?: Date,
    createdBy?: UserDetails
}

export enum AthenaFeedbackEnum {
    /// <summary>
    /// Indicates Athena is helpful
    /// </summary>
    Helpful,

    /// <summary>
    /// Indicates Athena is not helpful
    /// </summary>
    NotHelpful,

    /// <summary>
    /// Indicates Athena needs improvements
    /// </summary>
    NeedsImprovement,
}

