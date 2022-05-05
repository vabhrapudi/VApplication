// <copyright file="tab-configuration.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Text, Flex, Card } from "@fluentui/react-northstar";
import Constants from "../../constants/constants";
import * as microsoftTeams from "@microsoft/teams-js";
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";

interface ITab {
    id: number,
    entityId: string,
    contentUrl: string,
    suggestedDisplayName: string,
    imageSrc: string,
    content: string,
}

interface IAdminTeamTabConfigurationState {
    selectedTab: ITab;
}

class AdminTeamTabConfiguration extends React.Component<WithTranslation, IAdminTeamTabConfigurationState> {
    localize: TFunction;
    athenaAdminTeamsTab: ITab[];

    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        this.athenaAdminTeamsTab = [{
            id: 1,
            entityId: "request-tab",
            contentUrl: `${Constants.getBaseURL}/admin-requests-tab`,
            content: this.localize("adminTeamRequestsConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("adminTeamRequestsTabText")
        },
        {
            id: 2,
            entityId: "feedback-tab",
            contentUrl: `${Constants.getBaseURL}/admin-feedback-tab`,
            content: this.localize("adminTeamFeedbackConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("adminTeamFeedbackTabText")
        },
        {
            id: 3,
            entityId: "home-configuration-tab",
            contentUrl: `${Constants.getBaseURL}/home-configuration`,
            content: this.localize("homeConfigurationConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("homeConfigurationConfigTabDisplayName")
        },
        {
            id: 4,
            entityId: "insights-admin-tab",
            contentUrl: `${Constants.getBaseURL}/insights-admin-tab`,
            content: this.localize("insightsAdminConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("insightsAdminTabDisplayName")
        },
        {
            id: 5,
            entityId: "ingestion-admin-tab",
            contentUrl: `${Constants.getBaseURL}/athena-ingestion`,
            content: this.localize("athenaIngestionTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("athenaIngestionTabContent")
        }
        ];

        this.state = {
            selectedTab: this.athenaAdminTeamsTab[0]
        }
    }

    public componentDidMount() {
        microsoftTeams.initialize();
        microsoftTeams.appInitialization.notifySuccess();
        microsoftTeams.settings.registerOnSaveHandler( (saveEvent) => {
            microsoftTeams.settings.setSettings({
                entityId: this.state.selectedTab.entityId,
                contentUrl: this.state.selectedTab.contentUrl,
                suggestedDisplayName: this.state.selectedTab.suggestedDisplayName
            });
            saveEvent.notifySuccess();
        });
        microsoftTeams.settings.setValidityState(true);
    }

    /**
     * Event handler called when any tab gets clicked.
     * @param selectedTab The selected tab.
     */
    onTabClicked = (selectedTab: ITab) => {
        this.setState({ selectedTab });
    }

    // Render the component
    render() {
        return (
            <div className="tab-configuration task-module-container header-inner-container">
                <Flex column fill gap="gap.medium">
                    {
                        this.athenaAdminTeamsTab.map((tab: ITab) => {
                            return <Flex>
                                <Flex.Item grow align="center">
                                    <Card className="tab-card" ghost selected={this.state.selectedTab.id === tab.id} onClick={() => this.onTabClicked(tab)}>
                                        <Card.Body className="tab-card-body">
                                            <Flex vAlign="center" gap="gap.small">
                                                <img src={tab.imageSrc} width="45" height="45" />
                                                <Text content={tab.content} title={tab.content} truncated />
                                            </Flex>
                                        </Card.Body>
                                    </Card>
                                </Flex.Item>
                            </Flex>
                        })
                    }
                </Flex>
            </div>
        )
    }
}

export default withTranslation()(AdminTeamTabConfiguration);