﻿// <copyright file="sign-in-end.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import React, { useEffect } from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { RouteComponentProps } from "react-router-dom";

// Handles redirection after successful/failure sign in attempt.
const SignInSimpleEnd: React.FunctionComponent<RouteComponentProps> = props => {
    // Parse hash parameters into key-value pairs
    function getHashParameters() {
        const hashParams: any = {};
        window.location.hash.substr(1).split("&").forEach(function (item) {
            let s = item.split("="),
                k = s[0],
                v = s[1] && decodeURIComponent(s[1]);
            hashParams[k] = v;
        });
        return hashParams;
    }

    useEffect(() => {
        microsoftTeams.initialize();
        const hashParams: any = getHashParameters();
        if (hashParams["error"]) {
            // Authentication/authorization failed
            microsoftTeams.authentication.notifyFailure(hashParams["error"]);
        } else if (hashParams["id_token"]) {
            // Success
            microsoftTeams.authentication.notifySuccess();
            window.location.href = "/";
        } else {
            // Unexpected condition: hash does not contain error or access_token parameter
            microsoftTeams.authentication.notifyFailure("UnexpectedFailure");
        }
    });

    return (
        <></>
    );
};

export default SignInSimpleEnd;