// <copyright file="tab-configuration.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import Constants from "../../constants/constants";
import * as microsoftTeams from "@microsoft/teams-js";
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { Flex, Card, Text } from "@fluentui/react-northstar";

import "./tab-configuration.scss";

interface ITab {
    id: number,
    entityId: string;
    contentUrl: string;
    suggestedDisplayName: string;
    imageSrc: string;
    content: string;
}

interface ITabConfigurationState {
    selectedTab: ITab;
}

class TabConfiguration extends React.Component<WithTranslation, ITabConfigurationState> {
    localize: TFunction;
    isCoi: string = "";
    athenaCentralTeamTabs: ITab[];
    athenaCoiTeamTabs: ITab[];

    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        let urlParams = new URLSearchParams(window.location.search);
        this.isCoi = urlParams.get("isCoi") || "false";

        this.athenaCentralTeamTabs = [{
            id: 1,
            entityId: "home-tab",
            contentUrl: `${Constants.getBaseURL}/home?isCoiTeam=${this.isCoi}`,
            content: this.localize("homeConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("homeTabDisplayName")
        },
        {
            id: 2,
            entityId: "discovery-tab",
            contentUrl: `${Constants.getBaseURL}/discover`,
            content: this.localize("athenaDiscoveryConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("athenaDiscoveryTabDisplayName")
        },
        {
            id: 3,
            entityId: "news-tab",
            contentUrl: `${Constants.getBaseURL}/news-home?isCoiId=${this.isCoi}`,
            content: this.localize("newsConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("NewsTabText")
        },
        {
            id: 4,
            entityId: "insights-tab",
            contentUrl: `${Constants.getBaseURL}/insights`,
            content: this.localize("insightsConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("insightsTabDisplayName")
        }];

        this.athenaCoiTeamTabs = [{
            id: 1,
            entityId: "home-tab",
            contentUrl: `${Constants.getBaseURL}/home?isCoiTeam=${this.isCoi}`,
            content: this.localize("homeConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("homeTabDisplayName")
        },
        {
            id: 2,
            entityId: "news-tab",
            contentUrl: `${Constants.getBaseURL}/news-home?isCoiId=${this.isCoi}`,
            content: this.localize("newsConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("NewsTabText")
        },
        {
            id: 3,
            entityId: "home-configuration-tab",
            contentUrl: `${Constants.getBaseURL}/home-configuration`,
            content: this.localize("homeConfigurationConfigTabContent"),
            imageSrc: "/images/logo.png",
            suggestedDisplayName: this.localize("homeConfigurationConfigTabDisplayName")
        }];

        this.state = {
            selectedTab: this.isCoi === "true" ? this.athenaCoiTeamTabs[0] : this.athenaCentralTeamTabs[0]
        }
    }

    public componentDidMount() {
        microsoftTeams.initialize();
        microsoftTeams.appInitialization.notifySuccess();
        microsoftTeams.settings.registerOnSaveHandler((saveEvent) => {
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
     * Event handler called when any tab get clicked.
     * @param selectedTab The selected tab.
     */
    onTabClicked = (selectedTab: ITab) => {
        this.setState({ selectedTab });
    }

    // Renders the 'Athena central' team tabs.
    renderAthenaCentralTeamTabs = () => {
        let tabs = this.athenaCentralTeamTabs.map((tab: ITab) => {
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
        });

        return (
            <Flex column fill gap="gap.medium">
                { tabs }
            </Flex>
        );
    }

    // Renders the COI team tabs.
    renderAthenaCoiTeamTabs = () => {
        let tabs = this.athenaCoiTeamTabs.map((tab: ITab) => {
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
        });

        return (
            <Flex column fill gap="gap.medium">
                {tabs}
            </Flex>
        );
    }

    // Render the component
    render() {
        return (
            <div className="tab-configuration task-module-container header-inner-container">
                {
                    this.isCoi === "true" ? this.renderAthenaCoiTeamTabs() : this.renderAthenaCentralTeamTabs()
                }
            </div>
        )
    }
}

export default withTranslation()(TabConfiguration);