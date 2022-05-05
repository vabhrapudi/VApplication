// <copyright file="help-page.tsx">
// Copyright (c) NPS Foundation.
// Licensed under the MIT License.
// </copyright>

import * as React from 'react';
import { TFunction } from "i18next";
import { Flex, Text } from '@fluentui/react-northstar';
import { WithTranslation, withTranslation } from "react-i18next";

class HelpPage extends React.Component<WithTranslation> {
    localize: TFunction;
    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
    }

    // Render the component
    render() {
        return (
            <Flex column gap="gap.large" className="task-module-container athena-splash-page-content">
                <Text content={this.localize("helpPageContentText")} />
                    <Flex column gap="gap.medium">
                        <Text weight="bold" content={this.localize("userSpecificInfoText")} />
                        <Flex column gap="gap.smaller">
                            <Text content={this.localize("userCVText")} />
                            <Text content={this.localize("newsPreferencesText")} />
                            <Text content={this.localize("chartPreferencesText")} />
                            <Text content={this.localize("communitiesText")} />
                            <Text content={this.localize("chatPostText")} />
                        </Flex>
                     </Flex>
                    <Flex column gap="gap.medium">
                        <Text weight="bold" content={this.localize("whereStoredText")} />
                        <Flex column gap="gap.smaller">
                            <Text content={this.localize("userSettingsText")} />
                            <Text content={this.localize("athenaText")} />
                            <Text content={this.localize("athenaText")} />
                            <Text content={this.localize("userSettingsText")} />
                            <Text content={this.localize("microsoftTeamsText")} />
                        </Flex>
                    </Flex>
            </Flex>
        );
    }
}

export default withTranslation()(HelpPage);