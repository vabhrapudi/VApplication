// <copyright file="sign-in-start.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import React, { useEffect } from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { RouteComponentProps } from "react-router-dom";
import { getAuthenticationConsentMetadata } from '../../api/authentication-metadata-api';

/** Initiates sign in request with authentication metadata */
const SignInSimpleStart: React.FunctionComponent<RouteComponentProps> = props => {
    useEffect(() => {
        microsoftTeams.initialize();
        microsoftTeams.getContext((context: microsoftTeams.Context) => {
            const windowLocationOriginDomain = window.location.origin.replace("https://", "");
            const login_hint = context.upn ? context.upn : "";

            getAuthenticationConsentMetadata(windowLocationOriginDomain, login_hint).then((result: any) => {
                window.location.assign(result.data);
            });
        });
    });

    return (
        <></>
    );
};

export default SignInSimpleStart;