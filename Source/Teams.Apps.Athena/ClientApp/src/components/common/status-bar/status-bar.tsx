// <copyright file="status-bar.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { Flex, BookmarkIcon, Text, CloseIcon } from "@fluentui/react-northstar";
import IStatusBar from "../../../models/status-bar";
import { ActivityStatus } from "../../../models/activity-status";

import "./status-bar.scss";

interface IStatusBarProps {
    isMobile: boolean,
    status: IStatusBar
}

// The timespan for which notification will be active on screen.
const StatusBarActiveTimespan: number = 8000;

/**
 * The status bar which shows the recent status messages
 * @param props The props of type IStatusBarProps
 */
const StatusBar: React.FunctionComponent<IStatusBarProps> = props => {
    const [showStatusBar, setShowStatusBar] = React.useState(false);
    let timeoutId: number = 0;

    React.useEffect(() => {
        if (props.status.message?.length && props.status.type !== ActivityStatus.None) {
            onClose();

            setShowStatusBar(true);

            timeoutId = window.setTimeout(onClose, StatusBarActiveTimespan);
        }
    }, [props.status.id, props.status.message, props.status.type]);

    // Event handler called when status bar get closed
    function onClose() {
        setShowStatusBar(false);

        if (timeoutId) {
            window.clearTimeout(timeoutId);
        }
    }

    if (!showStatusBar) {
        return <></>;
    }

    return (
        <Flex
            className={`notification-toast ${props.status.type === ActivityStatus.Success ? "success" : "error"}`}
            vAlign="center"
            gap="gap.small"
            hAlign="center">
            {props.isMobile ? <BookmarkIcon /> : null}
            <Text className="notification" content={props.status.message} weight={props.isMobile ? "regular" : "semibold"} />
            <Flex.Item push>
                <CloseIcon className="cursor-pointer" size="small" onClick={onClose} />
            </Flex.Item>
        </Flex>
    );
}

export default StatusBar;