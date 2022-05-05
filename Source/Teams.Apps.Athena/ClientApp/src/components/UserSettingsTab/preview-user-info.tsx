// <copyright file="preview-user-info.tsx">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import * as React from 'react';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import * as microsoftTeams from "@microsoft/teams-js";
import IUserSettings from '../../models/user-settings';
import { Flex, Text, Divider, Button } from '@fluentui/react-northstar';
import Constants from '../../constants/constants';
import { ICoiEntity } from '../../models/coi-entity';
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import StatusBar from "../common/status-bar/status-bar";
import NotificationFrequency from '../../models/notification-frequency';
import AthenaSplash from "../athena-splash/athena-splash";
import { getStringJoinedBySeparator } from '../../helpers/common-helper';
import CardImage from '../common/card-image/card-image';
import IKeyword from '../../models/keyword';
import { withRouter, RouteComponentProps } from "react-router-dom";
import { getAllKeywordsAsync } from '../../api/keyword-api';
import { StatusCodes } from "http-status-codes";

interface IPreviewUserDetailsProps extends WithTranslation, RouteComponentProps {
    userDetails: IUserSettings,
    onUpdate: (result: any) => void
}

export interface IPreviewUserDetailsState {
    userDetails: IUserSettings;
    errorMessage: string,
    successMessage: string,
    status: IStatusBar,
    keywords: IKeyword[],
    isLoadingKeywords: boolean
}

class PreviewUserInfo extends React.Component<IPreviewUserDetailsProps, IPreviewUserDetailsState> {
    localize: TFunction;
    constructor(props: any) {
        super(props);

        this.localize = this.props.t;
        this.state = {
            userDetails: this.props.userDetails,
            errorMessage: "",
            successMessage: "",
            status: { id: 0, message: "", type: ActivityStatus.None },
            keywords: [],
            isLoadingKeywords: true
        }
    }

    componentDidMount() {
        this.fetchKeywords();
    }

    public UNSAFE_componentWillReceiveProps(nextProps: IPreviewUserDetailsProps, nextState: IPreviewUserDetailsState) {
        if (nextProps !== this.props && nextProps.userDetails !== this.props.userDetails) {
            this.setState({ userDetails: nextProps.userDetails });
        }
    }

    // Fetches all keywords.
    fetchKeywords = async () => {
        let response = await getAllKeywordsAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let keywords = response.data as IKeyword[];
            this.setState({ keywords, isLoadingKeywords: false });
        }
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    handleTokenAccessFailure = (error: string) => {
        this.props.history.push("/signin");
    }

    // Returns the keywords string.
    getKeywordsString = () => {
        if (this.state.isLoadingKeywords) {
            return this.localize("loadingLabel");
        }

        let keywordIdsStringArray = this.props.userDetails.keywords.map(String);

        let keywordsTitleArray: string[] = this.state.keywords
            .filter((keyword: IKeyword) => keywordIdsStringArray.includes(keyword.keywordId))
            .map((keyword: IKeyword) => keyword.title);

        return keywordsTitleArray.length ? keywordsTitleArray.join(", ") : "NA";
    }

    // Returns the job title string.
    getJobTitleString = () => {
        let jobTitleStrings = this.props.userDetails.jobTitle;
        return getStringJoinedBySeparator(jobTitleStrings, ", ")
    }

    // Open task module to update user details.
    openUpdateTaskModule = () => {
        microsoftTeams.tasks.startTask({
            title: this.localize("fillDetailsText"),
            height: Constants.taskModuleHeight,
            width: Constants.taskModuleWidth,
            url: `${Constants.getBaseURL}/userContactInfo`,
            fallbackUrl: `${Constants.getBaseURL}/userContactInfo`,
        }, (err: string, result: any) => {
            if (result) {
                if (result.data) {
                    this.setState({ status: { id: this.state.status.id + 1, message: this.localize("userUpdateSuccessText"), type: ActivityStatus.Success } });
                    this.props.onUpdate(result);
                }
                else {
                    this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToUpdateUserText"), type: ActivityStatus.Error } });
                }                
            }
        });
    }

    getCOI = () => {
        if (this.state.userDetails.communityOfInterests) {
            let coiList = JSON.parse(this.state.userDetails.communityOfInterests) as ICoiEntity[];
            if (coiList.length) {
                return coiList.map((coi: ICoiEntity) => { return coi.coiName }).toString();
            }
            return "NA";
        }
        return "NA";
    }

    getNotificationFrequency = () => {
        switch (this.state.userDetails.notificationFrequency) {
            case NotificationFrequency.Daily:
                return this.localize("dailyText");
            case NotificationFrequency.Monthly:
                return this.localize("monthlyText");
            case NotificationFrequency.Weekly:
                return this.localize("weeklyText");
            default:
                return "NA";
        }
    }

    // Renders the component
    render() {
        return (
            <div>
                <StatusBar status={this.state.status} isMobile={false} />
                <AthenaSplash description={this.localize("userSettingsDescription")} heading={this.localize("userSettingsHeading")} />
                <Flex column gap="gap.medium" fill styles={{ overflowY: "auto", height:"92vh"}}>
                    <Flex className="overflow-y user-setting-sub-container" column gap="gap.medium">
                        <Text weight="bold" content={this.localize("userContactInformationText")} />

                        <Flex gap="gap.small">
                            <Flex.Item size="size.quarter">
                                <Flex gap="gap.smaller" column>
                                    <Flex.Item>
                                        <Text as="pre" className="preview-page-label" content={this.localize("firstLastNameText")} />
                                    </Flex.Item>
                                    <Flex.Item>
                                        <Text as="pre" className="preview-page-label" content={this.localize("titleRankText")} />
                                    </Flex.Item>
                                    <Flex.Item>
                                        <Text as="pre" className="preview-page-label" content={this.localize("otherContactText")} />
                                    </Flex.Item>
                                    <Flex.Item>
                                        <Text className="preview-page-label" as="pre" content={this.localize("emailText")} />
                                    </Flex.Item>
                                </Flex>
                            </Flex.Item>
                            <Flex.Item size="size.quarter">
                                <Flex gap="gap.smaller" column>
                                    <Flex.Item>
                                        <Text className="preview-page-label-content" as="pre" content={this.props.userDetails.lastName + ", " + this.props.userDetails.firstName} />
                                    </Flex.Item>
                                    <Flex.Item>
                                        <Text className="preview-page-label-content" as="pre" content={this.getJobTitleString() ?? "NA"} />
                                    </Flex.Item>
                                    <Flex.Item>
                                        <Text as="pre" className="preview-page-label-content" content={this.props.userDetails.otherContact ?? "NA"} />
                                    </Flex.Item>
                                    <Flex.Item>
                                        <Text as="pre" className="preview-page-label-content" content={this.props.userDetails.emailAddress} />
                                    </Flex.Item>
                                </Flex>
                            </Flex.Item>

                            <Flex.Item size="size.small" push>
                                <div
                                    style={{
                                        position: 'relative',
                                    }}
                                >
                                    <CardImage imageSrc={this.props.userDetails.profilePictureImageURL!} className="preview-user-image" />
                                </div>
                            </Flex.Item>
                        </Flex>

                        <Divider size={1} />
                        <Flex gap="gap.small" column>
                            <Flex.Item>
                                <Text weight="bold" content={this.localize("researchCommunitiesText")} />
                            </Flex.Item>
                            <Flex.Item>
                                <Text weight="semibold" content={this.localize("currentCommunitiesText")} />
                            </Flex.Item>
                            <Flex.Item>
                                <Text content={this.getCOI()} />
                            </Flex.Item>
                        </Flex>

                        <Divider size={1} />
                        <Flex gap="gap.small" column>
                            <Flex.Item>
                                <Text weight="bold" content={this.localize("researchAreaText")} />
                            </Flex.Item>
                            <Flex.Item>
                                <Text weight="semibold" content={this.localize("currentAreaOfInterestText")} />
                            </Flex.Item>
                            <Flex.Item>
                                <Text content={this.getKeywordsString()} />
                            </Flex.Item>
                        </Flex>

                        <Divider size={1} />
                        <Flex gap="gap.small" column>
                            <Flex.Item>
                                <Text weight="bold" content={this.localize("digestNotificationsText")} />
                            </Flex.Item>
                            <Flex.Item>
                                <Text content={this.getNotificationFrequency()} />
                            </Flex.Item>
                        </Flex>
                        <Flex.Item align="end">
                            <Button className="athena-button" content={this.localize("editButtonText")} onClick={this.openUpdateTaskModule} id="updatebtn" />
                        </Flex.Item>
                    </Flex>
                </Flex>
            </div>
        );
    }
}

export default withTranslation()(withRouter(PreviewUserInfo));