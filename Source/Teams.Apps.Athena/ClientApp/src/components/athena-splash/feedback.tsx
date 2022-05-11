// <copyright file="athena-feedback.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Flex, Text, FormDropdown, TextArea, Button, CheckmarkCircleIcon, ErrorIcon, ArrowLeftIcon } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";
import { useTranslation } from 'react-i18next';
import { useEffect } from "react";
import { AthenaFeedbackEnum, AthenaFeedBackEntity } from "../../models/athena-feedback";
import { saveAthenaFeedbackAsync } from "../../api/feedback-api";
import { RouteComponentProps } from "react-router-dom";
import { StatusCodes } from "http-status-codes";
import StatusBar from "../common/status-bar/status-bar";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import FeedbackCategory from "../../models/feedback-category";
import FeedbackType from "../../models/feedback-type";

const Feedback: React.FunctionComponent<RouteComponentProps> = (props) => {
    const localize = useTranslation().t;
    const [feedback, setFeedback] = React.useState<AthenaFeedBackEntity>({} as AthenaFeedBackEntity);
    const [loading, setLoading] = React.useState<boolean>(false);
    const [isSubmitSuccess, setIsSubmitSuccess] = React.useState<boolean>(false);
    const [message, setMessage] = React.useState<string>("");
    const [selectedFeedbackItem, setSelectedFeedbackItem] = React.useState<any>(undefined);
    const [selectedCategory, setSelectedCategory] = React.useState<any>(undefined);
    const [selectedType, setSelectedType] = React.useState<any>(undefined);
    const [activityStatus, setActivityStatus] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });

    const feedbackList = [
        { key: "helpful", header: localize("helpfulFeedback"), value: AthenaFeedbackEnum.Helpful },
        { key: "nothelpful", header: localize("notHelpfulFeedback"), value: AthenaFeedbackEnum.NotHelpful },
        { key: "needimprovement", header: localize("needImprovementFeedback"), value: AthenaFeedbackEnum.NeedsImprovement }
    ];

    const categoryList = [
        { key: "category-discover", header: localize("discoverText"), value: FeedbackCategory.Discover },
        { key: "category-news", header: localize("newsText"), value: FeedbackCategory.News },
        { key: "category-insights", header: localize("insightsTabDisplayName"), value: FeedbackCategory.Insights },
        { key: "category-home", header: localize("homeText"), value: FeedbackCategory.Home },
        { key: "category-admin", header: localize("adminText"), value: FeedbackCategory.Admin },
        { key: "category-other", header: localize("otherText"), value: FeedbackCategory.Other }
    ];

    const typeList = [
        { key: "type-bug", header: localize("feedbackTypeBug"), value: FeedbackType.Bug },
        { key: "type-ui-issue", header: localize("feedbackTypeUIIssue"), value: FeedbackType.UIIssue },
        { key: "type-future-feature-request", header: localize("feedbackTypeFutureFeatureRequest"), value: FeedbackType.FutureFeatureRequest }
    ];

    useEffect(() => {
        microsoftTeams.initialize();
    }, []);

    /**
     * Event handler called when COI type get changed.
     * @param event The event object.
     * @param item The selected item details.
     */
    const onFeedbackSelectionChange = (event: any, item: any) => {
        let selectedDropdownItem = feedbackList.filter((feedback: any) => feedback.value === item.value.value);
        setSelectedFeedbackItem(selectedDropdownItem);
        setFeedback((prevState) => ({ ...prevState, feedback: item.value.value }));
    }

    /**
     * Redirects to the sign-in page when accessing token is failed in API.
     * @param error The error message.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    // Validates UI fields.
    const validateFields = () => {
        if (!feedback?.details) {
            setActivityStatus((prevState) => ({ id: prevState.id + 1, message: localize("feedbackDetailsRequiredMessage"), type: ActivityStatus.Error }));
            return false;
        }

        return true;
    }

    // Open task module to submit Athena feedback.
    const submitFeedback = async () => {
        let isAllFieldsValid = validateFields();

        if (!isAllFieldsValid) {
            return;
        }

        setLoading(true);
        let response = await saveAthenaFeedbackAsync(feedback, handleTokenAccessFailure);

        if (response) {
            if (response.status === StatusCodes.CREATED) {
                setMessage(localize("feedbackSubmittedSuccessfullyMessage"));
                setIsSubmitSuccess(true);
            }
            else {
                setMessage(localize("failedToSubmitFeedbackMessage"));
                setIsSubmitSuccess(false);
            }
        }
        else {
            setMessage(localize("failedToSubmitFeedbackMessage"));
            setIsSubmitSuccess(false);
        }
    }

    /**
     * Handles state updation of description field
     * @param event Text input event object
     */
    const onDescriptionChange = (event: any) => {
        let description = event.target.value;
        setFeedback((prevState) => ({ ...prevState, details: description }));
    }

    const onBackClick = () => {
        setMessage("");
        setIsSubmitSuccess(false);
    }

    /**
     * Event handler called when category gets changed.
     * @param event The event details.
     * @param eventData The event data.
     */
    const onCategorySelectionChange = (event: any, eventData: any) => {
        let selectedDropdownItem = categoryList.filter((category: any) => category.value === eventData.value.value);
        setSelectedCategory(selectedDropdownItem);
        setFeedback((prevState) => ({ ...prevState, category: eventData.value.value }));
    }

    /**
     * Event handler called when type gets changed.
     * @param event The event details.
     * @param eventData The event data.
     */
    const onTypeSelectionChange = (event: any, eventData: any) => {
        let selectedDropdownItem = typeList.filter((type: any) => type.value === eventData.value.value);
        setSelectedType(selectedDropdownItem);
        setFeedback((prevState) => ({ ...prevState, type: eventData.value.value }));
    }

    const renderComponent = () => {
        if (message && message.length > 0) {
            if (isSubmitSuccess) {
                return (<Flex column className="task-module-container athena-splash-page-content" hAlign="center" vAlign="center">
                    <CheckmarkCircleIcon size="largest" styles={{ color: "#237B4B" }} />
                    <Text content={message} />
                </Flex>)
            }
            else {
                return (<Flex column className="task-module-container athena-splash-page-content">
                    <Flex hAlign="center" vAlign="center" column className="form-fields">
                        <ErrorIcon size="largest" />
                        <Text content={message} />
                    </Flex>
                    <Flex.Item push>
                        <Flex>
                            <Button icon={<ArrowLeftIcon />} content={localize("backButtonText")} onClick={onBackClick} />
                        </Flex>
                    </Flex.Item>
                </Flex>)
            }
        }
        else {
            return (<div className="task-module-container athena-splash-page-content">
                <Flex fill gap="gap.small" column>
                    <StatusBar status={activityStatus} isMobile={false} />
                    <Flex.Item grow>
                        <Flex className="overflow-y" column gap="gap.medium">
                            <Flex column>
                                <Text content={`${localize("howCanWeMakeAthenaBetter")}*`} />
                                <TextArea
                                    design={{ height: "15rem", marginTop: ".4rem" }}
                                    placeholder={localize("howCanWeMakeAthenaBetterPlaceholder")}
                                    maxLength={500}
                                    onChange={onDescriptionChange}
                                    value={feedback.details}
                                    fluid />
                            </Flex>
                            <FormDropdown
                                label={localize("howWasExperience")}
                                placeholder={localize("select")}
                                items={feedbackList}
                                onChange={onFeedbackSelectionChange}
                                value={selectedFeedbackItem}
                                fluid />
                            <FormDropdown
                                label={localize("categoryText")}
                                placeholder={localize("select")}
                                items={categoryList}
                                onChange={onCategorySelectionChange}
                                value={selectedCategory}
                                fluid />
                            <FormDropdown
                                label={localize("typeText")}
                                placeholder={localize("select")}
                                items={typeList}
                                onChange={onTypeSelectionChange}
                                value={selectedType}
                                fluid />
                        </Flex>
                    </Flex.Item>
                    <Flex.Item push>
                        <Flex gap="gap.small">
                            <Flex.Item push>
                                <Button
                                    className="athena-button"
                                    content={localize("submitButtonContent")}
                                    disabled={loading}
                                    loading={loading}
                                    onClick={submitFeedback}
                                />
                            </Flex.Item>
                        </Flex>
                    </Flex.Item>
                </Flex>
            </div>);
        }
    }

    return (renderComponent());
};

export default Feedback;