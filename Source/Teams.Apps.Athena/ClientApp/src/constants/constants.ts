// <copyright file="constants.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

export default class Constants {
    public static readonly taskModuleHeight: number = 746;
    public static readonly taskModuleWidth: number = 600;

    // The max screen width up to which mobile view is enabled.
    public static readonly maxWidthForMobileView: number = 750;

    public static readonly UrlParamRequestIdToEditOrDeleteRequest: string = "requestId";
    public static readonly UrlParamIsReadonlyToEditOrDeleteRequest: string = "isReadOnly";
    public static readonly UrlParamRequestType: string = "type";
    public static readonly UrlParamRequestIdToApproveOrRejectRequest: string = "requestId";
    public static readonly UrlParamHomeConfigArticleId: string = "articleId";
    public static readonly UrlParamInsightsConfigPriorityId: string = "priorityId";

    public static readonly lazyLoadNewsCount: number = 30;

    // The maximum result count for feedbacks.
    public static readonly lazyLoadFeedbacksCount: number = 30;

    /** The base URL for API */
    public static readonly apiBaseURL = window.location.origin + "/api";

    /** The base URL to fetch task */
    public static readonly getBaseURL = window.location.origin;

    /** The base URL to fetch image */
    public static readonly getArtifactsPath = "/Artifacts/";

    /** The value of Id if its data source is Athena (not json) */
    public static readonly createdByAthena = -1;

    // The maximum result count for keywords.
    public static readonly KeywordSearchResultMaxCount: number = 200;
}

// Indicates Teams theme names
export enum Themes {
    dark = "dark",
    contrast = "contrast",
    light = "light",
    default = "default"
}

// Project card navigation command.
export enum NavigationCommand {
    forward,
    backward,
    default
}

// Formating model type.
export enum ModelType {
    member,
    task
}

// Indicates UI steps rendered while creating new project.
export enum AddProjectUISteps {
    step1 = 1,
    step2 = 2
}


export enum ItemTypes {
    CommunityOfInterest = "CommunityOfInterest",
    NewsEntity = "NewsEntity"
}

/** Indicates the response status codes */
export enum ResponseStatus {
    OK = 200,
    CREATED = 201
}