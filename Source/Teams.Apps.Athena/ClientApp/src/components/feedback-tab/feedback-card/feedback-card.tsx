// <copyright file="feedback-card.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { Flex, Text, Card } from "@fluentui/react-northstar";
import { useTranslation } from 'react-i18next';
import moment from "moment";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { AthenaFeedBackEntity } from "../../../models/athena-feedback";
import ProfilePic from "../../common/person-avatar/person-avatar";
import { getBaseUrl } from "../../../configVariables";
import Constants from "../../../constants/constants";
import { getFeedbackType } from "../../../helpers/localization-helper";

import "./feedback-card.scss";

interface IFeedbackCardProps extends RouteComponentProps {
    feedbackData: AthenaFeedBackEntity[];
}

const FeedbackCard: React.FunctionComponent<IFeedbackCardProps> = (props: IFeedbackCardProps) => {
    const localize = useTranslation().t;

    React.useEffect(() => {
        microsoftTeams.initialize();
    }, []);

    /**
     * Opens the task module to display details feedback.
     * @param feedbackId The feedback Id.
     */
    const handleFeedbackItemClick = (feedbackId: string | undefined) => {
        microsoftTeams.tasks.startTask({
            title: localize("detailsTitle"),
            height: Constants.taskModuleHeight,
            width: Constants.taskModuleWidth,
            url: getBaseUrl() + `/feedback-details?feedbackId=${feedbackId}`,
            fallbackUrl: getBaseUrl() + `/feedback-details?feedbackId=${feedbackId}`,
        });
    }

    return (
        <>
            {
                props.feedbackData.map((feedback: AthenaFeedBackEntity, index: number) => {
                    return (
                        <Card fluid key={index} onClick={() => handleFeedbackItemClick(feedback.feedbackId)} className="icon-pointer feedback-card">
                            <Card.Header>
                                <Flex gap="gap.small">
                                    <ProfilePic profilePhoto={feedback?.createdBy?.profileImage ?? ""} userName={feedback?.createdBy?.displayName ?? "NA"} />
                                    <Flex column>
                                        <Text content={feedback?.createdBy?.displayName ?? "NA"} weight="bold" />
                                        <Text content={localize("feedbackCardSubTitle", { 0: feedback.createdAt ? moment(feedback.createdAt).format("DD-MMM-YYYY hh:mm A") : "NA", 1: getFeedbackType(feedback.feedback, localize) })} />
                                    </Flex>
                                </Flex>
                            </Card.Header>
                            <Card.Body>
                                <Text content={feedback.details} title={feedback.details} className="feedback-details" />
                            </Card.Body>
                        </Card>
                    )
                })
            }
        </>
    )
};

export default withRouter(FeedbackCard);