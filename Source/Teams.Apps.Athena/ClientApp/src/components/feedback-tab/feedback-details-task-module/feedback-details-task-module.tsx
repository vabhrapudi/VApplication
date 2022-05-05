// <copyright file="feedback-details-task-module.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { useTranslation } from 'react-i18next';
import { withRouter, RouteComponentProps } from "react-router-dom";
import { Flex, Text, Button } from "@fluentui/react-northstar";
import moment from "moment";
import { StatusCodes } from "http-status-codes";
import StatusBar from "../../common/status-bar/status-bar";
import IStatusBar from "../../../models/status-bar";
import { ActivityStatus } from "../../../models/activity-status";
import Loader from "../../common/loader/loader";
import { TFunction } from "i18next";
import { AthenaFeedBackEntity } from "../../../models/athena-feedback";
import ProfilePic from "../../common/person-avatar/person-avatar";
import { getFeedbackDetailsAsync } from "../../../api/feedback-api";
import { getFeedbackType } from "../../../helpers/localization-helper";

interface IFeedbackDetailsTaskModuleProps extends RouteComponentProps {
}

const FeedbackDetails: React.FunctionComponent<IFeedbackDetailsTaskModuleProps> = (props: IFeedbackDetailsTaskModuleProps) => {
    const localize: TFunction = useTranslation().t;
    const [feedbackDetails, setFeedbackDetails] = React.useState<AthenaFeedBackEntity>({} as AthenaFeedBackEntity);
    const [isLoading, setIsLoading] = React.useState<boolean>(true);
    const [statusBar, setStatusBar] = React.useState({ id: 0, message: "", type: ActivityStatus.None } as IStatusBar);

    React.useEffect(() => {
        microsoftTeams.initialize();
        let urlParams = new URLSearchParams(window.location.search);
        let feedbackId = urlParams.get("feedbackId") ?? "";
        getFeedbackDetails(feedbackId);
    }, []);

    /**
     * Gets the feedback details by feedback Id.
     * @param feedbackId The feedback Id.
     */
    const getFeedbackDetails = async (feedbackId: string) => {
        let response = await getFeedbackDetailsAsync(feedbackId, handleTokenAccessFailure);
        if (response && response.status === StatusCodes.OK) {
            setFeedbackDetails(response.data);
        }
        else {
            setStatusBar({ id: statusBar.id + 1, message: localize("generalErrorMessage"), type: ActivityStatus.Error });
        }
        setIsLoading(false);
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    // Closes the feedback details task module.
    const handleCloseButtonClick = () => {
        microsoftTeams.tasks.submitTask();
    }

    return (
        <>
            {
                isLoading ?
                    <Loader />
                    :
                    <>
                        <StatusBar isMobile={false} status={statusBar} />
                        <Flex fill column gap="gap.medium" className="task-module-container">
                            <Flex column gap="gap.small">
                                <Text content={`${localize("feedbackText")}:`} weight="semibold" />
                                <Text content={getFeedbackType(feedbackDetails.feedback, localize)} />
                            </Flex>
                            <Flex column gap="gap.small">
                                <Text content={`${localize("submittedByTitle")}:`} weight="semibold" />
                                <Flex vAlign="center" gap="gap.smaller">
                                    <ProfilePic profilePhoto={feedbackDetails?.createdBy?.profileImage ?? ""} userName={feedbackDetails?.createdBy?.displayName ?? "NA"} />
                                    <Text content={feedbackDetails?.createdBy?.displayName ?? "NA"} />
                                </Flex>
                            </Flex>
                            <Flex column gap="gap.small">
                                <Text content={`${localize("feedbackDetailsTitle")}:`} weight="semibold" />
                                <Text content={feedbackDetails.details} />
                            </Flex>
                            <Flex column gap="gap.small">
                                <Text content={`${localize("submittedOnTitle")}:`} weight="semibold" />
                                <Text content={feedbackDetails.createdAt ? moment(feedbackDetails.createdAt).format("DD-MMM-YYYY hh:mm A") : "NA"} />
                            </Flex>
                            <Flex.Item align="end" push>
                                <Button className="athena-button" content={localize("closeButtonTitle")} onClick={handleCloseButtonClick} />
                            </Flex.Item>
                        </Flex>
                    </>
            }
        </>
    )
};

export default withRouter(FeedbackDetails);