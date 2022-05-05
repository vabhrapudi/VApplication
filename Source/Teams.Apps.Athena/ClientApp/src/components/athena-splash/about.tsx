// <copyright file="about-page.tsx">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import * as React from 'react';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { Flex, Text } from '@fluentui/react-northstar';

class AboutPage extends React.Component<WithTranslation> {
    localize: TFunction;
    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
    }

    // Render the component
    render() {
        return (
            <Flex column gap="gap.medium" className="task-module-container athena-splash-page-content">
                <Text content={this.localize("aboutPageContentText1")} />
                <Text content={this.localize("aboutPageContentText2")} />
                <Flex gap="gap.large">
                    <a href="https://nps.edu/" target="_blank" style={{ textDecoration: "none" }}> {this.localize("websiteText")} </a>
                    <a href="https://nps.edu/" target="_blank" style={{ textDecoration: "none" }}> {this.localize("PrivacyText")} </a>
                    <a href="https://nps.edu/" target="_blank" style={{ textDecoration: "none" }}> {this.localize("TermsText")} </a>
                </Flex>
            </Flex>
        );
    }
}

export default withTranslation()(AboutPage);