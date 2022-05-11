// <copyright file="localization-helper.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import { TFunction } from "i18next";
import { AthenaFeedbackEnum } from "../models/athena-feedback";
import COIType from "../models/coi-type";
import FeedbackCategory from "../models/feedback-category";
import FeedbackType from "../models/feedback-type";
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
 * Gets the localized string for feedback level.
 * @param feedback The feedback level.
 * @param localize The instance of TFunction.
 */
export function getFeedbackLevelTitle(feedback: AthenaFeedbackEnum, localize: TFunction): string {
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

/**
 * Gets the localized string for feedback type.
 * @param feedbackType The feedback type.
 * @param localize The instance of TFunction.
 */
export function getFeedbackTypeTitle(feedbackType: FeedbackType, localize: TFunction): string {
    switch (feedbackType) {
        case FeedbackType.Bug:
            return localize("feedbackTypeBug");
        case FeedbackType.UIIssue:
            return localize("feedbackTypeUIIssue");
        case FeedbackType.FutureFeatureRequest:
            return localize("feedbackTypeFutureFeatureRequest");
        default:
            return "NA";
    }
}

/**
 * Gets the localized string for feedback category.
 * @param feedbackCategory The feedback category.
 * @param localize The instance of TFunction.
 */
export function getFeedbackCategoryTitle(feedbackCategory: FeedbackCategory, localize: TFunction): string {
    switch (feedbackCategory) {
        case FeedbackCategory.Discover:
            return localize("discoverText");
        case FeedbackCategory.News:
            return localize("newsText");
        case FeedbackCategory.Insights:
            return localize("insightsText");
        case FeedbackCategory.Home:
            return localize("homeText");
        case FeedbackCategory.Admin:
            return localize("adminText");
        case FeedbackCategory.Other:
            return localize("otherText");
        default:
            return "NA";
    }
}