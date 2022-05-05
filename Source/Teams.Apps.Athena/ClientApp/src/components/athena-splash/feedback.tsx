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

const Feedback: React.FunctionComponent<RouteComponentProps> = (props) => {

    const localize = useTranslation().t;
    const [feedback, setFeedback] = React.useState<AthenaFeedBackEntity>({} as AthenaFeedBackEntity);
    const [loading, setLoading] = React.useState<boolean>(false);
    const [isSubmitSuccess, setIsSubmitSuccess] = React.useState<boolean>(false);
    const [message, setMessage] = React.useState<string>("");
    const [selectedFeedbackItem, setSelectedFeedbackItem] = React.useState<any>(undefined);
    const feedbackList = [
        { key: "helpful", header: localize("helpfulFeedback"), value: AthenaFeedbackEnum.Helpful },
        { key: "nothelpful", header: localize("notHelpfulFeedback"), value: AthenaFeedbackEnum.NotHelpful },
        { key: "needimprovement", header: localize("needImprovementFeedback"), value: AthenaFeedbackEnum.NeedsImprovement }
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
        setFeedback({ details: feedback.details, feedback: item.value.value });
    }

    /**
     * Redirects to the sign-in page when accessing token is failed in API.
     * @param error The error message.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    // Open task module to submit Athena feedback.
    const submitFeedback = async () => {
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
        setFeedback({ details: description, feedback: feedback.feedback });
    }

    const onBackClick = () => {
        setMessage("");
        setIsSubmitSuccess(false);
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