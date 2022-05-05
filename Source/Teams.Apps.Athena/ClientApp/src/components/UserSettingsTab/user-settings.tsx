// <copyright file="user-settings.tsx">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import * as React from 'react';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { Button, Image } from '@fluentui/react-northstar';
import * as microsoftTeams from "@microsoft/teams-js";
import { getUserDetailsAsync, getUserSettingAsync } from '../../api/user-settings-tab-api';
import Constants from '../../constants/constants';
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import "./user-settings.scss";
import IUserSettings from '../../models/user-settings';
import PreviewUserDetails from "./preview-user-info";
import StatusBar from "../common/status-bar/status-bar";
import Loader from "../common/loader/loader";

export interface IUsersettingState {
    loading: boolean;
    userDetails: IUserSettings;
    isNextPage: boolean;
    isFirstTimeLogin: boolean;
    status: IStatusBar;
}

class UserSettings extends React.Component<WithTranslation, IUsersettingState> {
    localize: TFunction;
    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        this.state = {
            loading: true,
            userDetails: {} as IUserSettings,
            isNextPage: false,
            isFirstTimeLogin: false,
            status: { id: 0, message: "", type: ActivityStatus.None },
        }
    }

    public componentDidMount() {
        microsoftTeams.initialize();
        this.getUserSettingDetails();
    }

    public getUserSettingDetails = async () => {
        this.setState({ loading: true });

        // get user details from database APi call.
        let response = await getUserSettingAsync();

        if (response && response.status === 404) {
            // get user details from graph APi call.
            let loggedInUserDetails = await getUserDetailsAsync();

            if (loggedInUserDetails && loggedInUserDetails.status === 200) {
                this.setState({ userDetails: loggedInUserDetails.data as IUserSettings, isFirstTimeLogin: true, loading: false });
            } else {
                this.setState({ loading: false });
            }
        }
        else {
            this.setState({
                userDetails: response.data as IUserSettings,
                loading: false
            });
        }
        
    }

    // Set state for back click.
    onBackClick = () => {
        this.setState({ isNextPage: false })
    }

    onBtnClickFetchTask = () => {
        microsoftTeams.tasks.startTask({
            title: this.localize("fillDetailsText"),
            height: Constants.taskModuleHeight,
            width: Constants.taskModuleWidth,
            url: `${Constants.getBaseURL}/userContactInfo`,
            fallbackUrl: `${Constants.getBaseURL}/userContactInfo`,
        }, (err: string, result: any) => {
            if (result && result.data) {
                if (result.data) {
                    this.setState({ status: { id: this.state.status.id + 1, message: this.localize("userUpdateSuccessText"), type: ActivityStatus.Success } });
                    this.getUserSettingDetails();
                }
                else {
                    this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToUpdateUserText"), type: ActivityStatus.Error } });
                }
            }
        });
    }

    onUpdateUserSettings = async (result: any) => {
        this.getUserSettingDetails();
    }

    // Return the component
    render() {
        if (this.state.loading) {
            return <Loader />;
        }
        else {
            return (
                <>
                    <StatusBar status={this.state.status} isMobile={false} />
                    {!this.state.userDetails.userId && this.state.isFirstTimeLogin
                        ? <div className="container-div">
                            <div className="container-subdiv">
                                <div className="welcome-message">
                                    <Image className="welcome-image" src="/images/userSettingWelcome.png" />
                                    <h3>{this.localize("welcomeMessageText")}</h3>
                                    <p className="welcome-text">{this.localize("welcomeText")}</p>
                                    <Button className="athena-button start-now-btn" content={this.localize("startNowButtonText")} onClick={this.onBtnClickFetchTask} id="startnowbtn" />
                                </div>
                            </div>
                        </div>
                        : <PreviewUserDetails userDetails={this.state.userDetails} onUpdate={this.onUpdateUserSettings} />}
                    </>
            )
        }
    }
}

export default withTranslation()(UserSettings);