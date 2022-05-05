// <copyright file="app.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { AppRoute } from "./router/router";
import Constants, { Themes } from "./constants/constants";
import * as microsoftTeams from "@microsoft/teams-js";
import { Provider, teamsDarkTheme, teamsHighContrastTheme, teamsTheme, ThemeInput } from "@fluentui/react-northstar";
import i18n from "./i18n";

import "./styles/site.scss";

export interface IAppState {
    theme: string,
    isMobileView: boolean
}

export default class App extends React.Component<{}, IAppState> {

    constructor(props: any) {
        super(props);
        this.state = {
            theme: Themes.default,
            isMobileView: window.outerWidth <= Constants.maxWidthForMobileView
        }
    }

    componentDidMount() {
        window.addEventListener("resize", this.onScreenResize);

        microsoftTeams.initialize();
        microsoftTeams.getContext((context: microsoftTeams.Context) => {
            this.setState({ theme: context.theme! });
            i18n.changeLanguage(context.locale);
        });

        microsoftTeams.registerOnThemeChangeHandler((theme: string) => {
            this.setState({ theme: theme! });
        });
    }

    componentWillUnmount() {
        window.removeEventListener("resize", this.onScreenResize);
    }

    onScreenResize = () => {
        this.setState({ isMobileView: window.outerWidth <= Constants.maxWidthForMobileView });
    }

    // Renders component based on Microsoft Teams's theme.
    public renderComponentWithTeamsTheme = () => {
        switch (this.state.theme) {
            case Themes.dark:
                return this.renderThemeProvider(teamsDarkTheme, "dark-container");

            case Themes.contrast:
                return this.renderThemeProvider(teamsHighContrastTheme, "high-contrast-container");

            default:
                return this.renderThemeProvider(teamsTheme, "default-container")
        }
    }

    // Gets the theme provider and related CSS class based on Microsoft Teams theme.
    public renderThemeProvider = (teamsTheme: ThemeInput, cssClassName: string) => {
        return (
            <Provider theme={teamsTheme}>
                <div className="container-div">
                    <div className={cssClassName}>
                        <AppRoute />
                    </div>
                </div>
            </Provider>
        );
    }

    public getViewClassName = () => {
        return this.state.isMobileView ? "mobile" : "";
    }

    /**
    * Renders the component
    */
    public render(): JSX.Element {
        return (
            <div className={this.getViewClassName()}>
                {this.renderComponentWithTeamsTheme()}
            </div>
        );
    }
}