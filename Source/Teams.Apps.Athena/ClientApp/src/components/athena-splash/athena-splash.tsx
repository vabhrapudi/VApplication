// <copyright file="athena-splash.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Flex, MoreIcon, Text, MenuButton, InfoIcon, QuestionCircleIcon, Button } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";
import { useTranslation } from 'react-i18next';
import Constants from "../../constants/constants";
import { useEffect } from "react";

import "./athena-splash.scss";

interface IProps {
    heading: string;
    description?: string;
}

const AthenaSplash: React.FunctionComponent<IProps> = (props) => {

    const localize = useTranslation().t;

    useEffect(() => {
        microsoftTeams.initialize();
    }, []);

    // Open task module to submit Athena feedback.
    const openFeedbackTaskModule = () => {
        microsoftTeams.tasks.startTask({
            height: Constants.taskModuleHeight,
            width: Constants.taskModuleWidth,
            url: `${Constants.getBaseURL}/athena-feedback`,
            fallbackUrl: `${Constants.getBaseURL}/error`,
        });
    }

    // Open task module to submit Athena feedback.
    const openHelpTaskModule = () => {
        microsoftTeams.tasks.startTask({
            height: Constants.taskModuleHeight,
            width: Constants.taskModuleWidth,
            url: `${Constants.getBaseURL}/help-page`,
            fallbackUrl: `${Constants.getBaseURL}/error`,
        });
    }

    // Open task module to submit Athena feedback.
    const openAboutTaskModule = () => {
        microsoftTeams.tasks.startTask({
            height: Constants.taskModuleHeight,
            width: Constants.taskModuleWidth,
            url: `${Constants.getBaseURL}/about-page`,
            fallbackUrl: `${Constants.getBaseURL}/error`,
        });
    }

    // Menu items of header
    const menuItems = [
        {
            icon: (
                <QuestionCircleIcon
                    {...{
                        outline: true,
                    }}
                />
            ),
            key: '1',
            content: <Text content={localize("helpText")} />,
        },
        {
            icon: (
                <InfoIcon
                    {...{
                        outline: true,
                    }}
                />
            ),
            key: '2',
            content: localize("feedbackText"),
        },
        {
            icon: (
                <InfoIcon
                    {...{
                        outline: true,
                    }}
                />
            ),
            key: '3',
            content: localize("aboutText"),
        }
    ]

    /**
     * Handles the selection of menu items
     * @param event Menu button event object
     * @param menuItemProps Selected menu item
     */
    const handleMenuItemClick = (event: any, menuItemProps: any) => {
        switch (menuItemProps.index) {
            case 0: openHelpTaskModule();
                break;
            case 1: openFeedbackTaskModule();
                break;
            case 2: openAboutTaskModule();
                break;
            default:
                return;
        }
    }

    return (
        <Flex padding="padding.medium" className="splash" vAlign="center">
            <Text content={props.heading} size="large" weight="bold" />
            {
                props.description &&
                <Flex.Item push>
                    <Text className="athena-splash-description" content={props.description} align="center" />
                </Flex.Item>
            }          
            <Flex.Item push>
                <MenuButton
                    trigger={<Button className="more-menu-icon" icon={<MoreIcon />} iconOnly text title="More" />}
                    menu={menuItems}
                    onMenuItemClick={handleMenuItemClick}
                />
            </Flex.Item>
        </Flex>
    )
};

export default React.memo(AthenaSplash);