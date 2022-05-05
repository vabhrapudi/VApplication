// <copyright file="digest-notifications.tsx">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import * as React from 'react';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { Flex, Text, Button, Divider, RadioGroup } from '@fluentui/react-northstar';
import IUserSettings from '../../models/user-settings';
import NotificationFrequency from "../../models/notification-frequency";

interface IDigestNotificationsProps extends WithTranslation {
    onBackClick: (userDetails: IUserSettings) => void,
    onNextClick: (userDetails: IUserSettings) => void,
    userDetails: IUserSettings
}

interface IDigestNotificationsState {
    checkedValue: number,
}

class DigestNotifications extends React.Component<IDigestNotificationsProps, IDigestNotificationsState>  {
    localize: TFunction;
    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        this.state = {
            checkedValue: this.props.userDetails.tableId ? this.props.userDetails.notificationFrequency! : NotificationFrequency.Daily,
        };

    }
    // On back button click.
    onBackClick = () => {
        this.props.onBackClick(this.props.userDetails);
    }

    // On preview button click.
    onPreviewButtonClick = () => {
        let userDetails = { ...this.props.userDetails };
        userDetails.notificationFrequency = this.state.checkedValue;
        this.props.onNextClick(userDetails);
    }

    onValueChange = (e:any, props:any) => {
        this.setState({ checkedValue: props.value });
    }

    getItems() {
        return [
            {
                label: this.localize("dailyText"),
                key: 'daily',
                value: NotificationFrequency.Daily
            },
            {
                label: this.localize("weeklyText"),
                key: 'weekly',
                value: NotificationFrequency.Weekly
            },
            {
                label: this.localize("monthlyText"),
                key: 'monthly',
                value: NotificationFrequency.Monthly
            },
        ]
    }

    // return a component.
    render() {
        return (
            <Flex column gap="gap.medium" className="task-module-container">
                <Text className="page-heading" content={this.localize("digestNotificationsText")} />
                <Divider size={2} color="brand" />
                <Flex column gap="gap.smaller" className="form-fields">
                    <Text content={this.localize("digestNotificationsPageContentText")} />
                    <RadioGroup
                        vertical
                        items={this.getItems()}
                        checkedValue={this.state.checkedValue}
                        onCheckedValueChange={this.onValueChange}
                    />
                </Flex>
                <Flex gap="gap.smaller">
                    <Flex.Item push>
                        <Button content={this.localize("backButtonText")} id="backbtn" onClick={this.onBackClick} />
                    </Flex.Item>
                    <Button className="athena-button" content={this.localize("previewButtonText")} id="previewbtn" onClick={this.onPreviewButtonClick} />
                </Flex>
            </Flex>
        );
    }
    
}

export default withTranslation()(DigestNotifications);