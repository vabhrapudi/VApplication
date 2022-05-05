﻿// <copyright file="sign-in.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { RouteComponentProps } from "react-router-dom";
import { Text, Button } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";
import { useTranslation } from 'react-i18next';

import "./sign-in.scss";

const SignInPage: React.FunctionComponent<RouteComponentProps> = props => {
    const localize = useTranslation().t;

    function onSignIn() {
        microsoftTeams.initialize();
        microsoftTeams.authentication.authenticate({
            url: window.location.origin + "/signin-simple-start",
            successCallback: () => {
                console.log("Login succeeded!");
                window.location.href = "/discover";
            },
            failureCallback: (reason) => {
                console.log("Login failed: " + reason);
                window.location.href = "/errorpage";
            }
        });
    }

    return (
        <div className="sign-in-content-container">
            <div>
            </div>
            <Text
                content={localize('signInMessage')}
                size="medium"
            />
            <div className="space"></div>
            <Button content={localize("signInText")} className="athena-button sign-in-button" onClick={onSignIn} />
        </div>
    );
};

export default SignInPage;
