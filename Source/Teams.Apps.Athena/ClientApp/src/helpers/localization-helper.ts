// <copyright file="localization-helper.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import { TFunction } from "i18next";
import { AthenaFeedbackEnum } from "../models/athena-feedback";
import COIType from "../models/coi-type";
import RequestStatus from "../models/request-status";

/**
 * Gets the localized string by COI type.
 * @param coiType The COI type.
 * @param localize The instance of TFunction.
 */
export function getLocalizedCOIType(coiType: COIType, localize: TFunction): string {
    switch (coiType) {
        case COIType.Private:
            return localize("coiTypePrivate");

        case COIType.Public:
            return localize("coiTypePublic");

        default:
            return "";
    }
}

/**
 * Gets the localized string for request status.
 * @param status The request status.
 * @param localize The instance of TFunction.
 */
export function getLocalizedRequestStatus(status: RequestStatus, localize: TFunction): string {
    switch (status) {
        case RequestStatus.Draft:
            return localize("requestStatusDraft");

        case RequestStatus.Pending:
            return localize("requestStatusPending");

        case RequestStatus.Approved:
            return localize("requestStatusApproved");

        case RequestStatus.Rejected:
            return localize("requestStatusRejected");

        default:
            return "";
    }
}

/**
 * Gets the localized string for feedback.
 * @param feedback The feedback type.
 * @param localize The instance of TFunction.
 */
export function getFeedbackType(feedback: AthenaFeedbackEnum, localize: TFunction): string {
    switch (feedback) {
        case AthenaFeedbackEnum.Helpful:
            return localize("helpfulFeedback");
        case AthenaFeedbackEnum.NotHelpful:
            return localize("notHelpfulFeedback");
        case AthenaFeedbackEnum.NeedsImprovement:
            return localize("needImprovementFeedback");
        default:
            return "NA";
    }
}