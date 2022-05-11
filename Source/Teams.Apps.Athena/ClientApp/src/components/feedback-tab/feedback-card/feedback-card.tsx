// <copyright file="feedback-card.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { Flex, Text, Card, ChatIcon } from "@fluentui/react-northstar";
import { useTranslation } from 'react-i18next';
import moment from "moment";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { AthenaFeedBackEntity } from "../../../models/athena-feedback";
import ProfilePic from "../../common/person-avatar/person-avatar";
import { getBaseUrl } from "../../../configVariables";
import Constants from "../../../constants/constants";
import { getFeedbackCategoryTitle, getFeedbackLevelTitle, getFeedbackTypeTitle } from "../../../helpers/localization-helper";

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
                        <Card fluid key={index} className="feedback-card">
                            <Card.Header>
                                <Flex gap="gap.small">
                                    <ProfilePic profilePhoto={feedback?.createdBy?.profileImage ?? ""} userName={feedback?.createdBy?.displayName ?? "NA"} />
                                    <Flex column onClick={() => handleFeedbackItemClick(feedback.feedbackId)} className="icon-pointer">
                                        <Text content={feedback?.createdBy?.displayName ?? localize('unknownText')} weight="bold" />
                                        <Text content={localize("feedbackCardSubTitle", { 0: feedback.createdAt ? moment(feedback.createdAt).format("DD-MMM-YYYY hh:mm A") : "NA", 1: getFeedbackLevelTitle(feedback.feedback, localize), 2: getFeedbackCategoryTitle(feedback.category, localize), 3: getFeedbackTypeTitle(feedback.type, localize) })} />
                                    </Flex>
                                    {
                                        feedback?.createdBy?.mail &&
                                        <Flex.Item push>
                                            <ChatIcon outline size="medium" className="icon-pointer" onClick={() => microsoftTeams.executeDeepLink("https://teams.microsoft.com/l/chat/0/0?users=" + feedback?.createdBy?.mail)} />
                                        </Flex.Item>
                                    }
                                </Flex>
                            </Card.Header>
                            <Card.Body onClick={() => handleFeedbackItemClick(feedback.feedbackId)} className="icon-pointer">
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