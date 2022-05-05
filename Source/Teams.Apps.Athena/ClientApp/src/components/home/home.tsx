// <copyright file="home.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { Flex, Text, Card, Skeleton, Segment } from "@fluentui/react-northstar";
import { RouteComponentProps, withRouter } from "react-router-dom";
import AthenaSplash from "../athena-splash/athena-splash";
import NewToAthenaCard from "../home/new-to-athena-card/new-to-athena-card";
import YourDailyBriefingCard from "../home/your-daily-briefing-card/your-daily-briefing-card";
import { getDailyBriefingArticlesOfUserForCentralTeamAsync, getDailyBriefingArticlesOfUserForCoiTeamAsync, getNewToAthenaArticlesForCentralTeamAsync, getNewToAthenaArticlesForCoiTeamAsync, getStatusBarDetailsForCentralTeamAsync, getStatusBarDetailsForCoiTeamAsync } from "../../api/home-api";
import { StatusCodes } from "http-status-codes";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import IHomeStatusBarConfiguration from "../../models/home-status-bar-configuration";
import IHomeConfigurationArticle from "../../models/home-configuration-article";
import IDailyBriefingHomeArticle from "../../models/daily-briefing-home-article";
import StatusBar from "../common/status-bar/status-bar";
import NoContent from "../common/no-content/no-content";
import { useTranslation } from 'react-i18next';
import IDiscoveryTreeNodeType from "../../models/discovery-tree-node-type";
import { getDiscoveryNodeTypeAsync } from "../../api/discovery-tree-api";
import { getResourceImagePath } from "../../helpers/image-helper";

import "./home.scss";

interface IHomeProps extends RouteComponentProps {
}

export const Home: React.FunctionComponent<IHomeProps> = (props: IHomeProps) => {
    const localize = useTranslation().t;

    const [newToAthenaArticles, setNewToAthenaArticles] = React.useState([] as IHomeConfigurationArticle[]);
    const [yourDailyBriefingArticles, setYourDailyBriefingArticles] = React.useState([] as IDailyBriefingHomeArticle[]);
    const [isLoadingNewToAthenaCards, setLoadingNewToAthenaCards] = React.useState(true);
    const [isLoadingYourDailyBriefingCards, setLoadingYourDailyBriefingCards] = React.useState(true);
    const [status, setStatus] = React.useState({ id: 0, message: "", type: ActivityStatus.None } as IStatusBar);
    const [eventBarDetails, setEventBarDetails] = React.useState({} as IHomeStatusBarConfiguration);
    const [nodeTypes, setNodeTypes] = React.useState([] as IDiscoveryTreeNodeType[]);

    React.useEffect(() => {
        microsoftTeams.initialize();
        microsoftTeams.getContext(async (context: microsoftTeams.Context) => {
            let urlQueryParams = new URLSearchParams(window.location.search);
            let isCoiTeam = urlQueryParams.get("isCoiTeam") ?? "";

            await getNodeTypes();
            getYourDailyBriefingArticlesAsync(context.groupId!, isCoiTeam);
            getHomeStatusBarDetailsAsync(context.groupId!, isCoiTeam);
            getHomeNewToAthenaArticlesAsync(context.groupId!, isCoiTeam);
        });
    }, []);

    // Loads the node type data.
    const getNodeTypes = async () => {
        let response = await getDiscoveryNodeTypeAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setNodeTypes(response.data as IDiscoveryTreeNodeType[]);
        }
        else {
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }
    }

    // Gets the details of status bar configured for the team.
    const getHomeStatusBarDetailsAsync = async (teamId: string, isCoiTeam: string) => {
        if (isCoiTeam === "true") {
            let response = await getStatusBarDetailsForCoiTeamAsync(teamId, handleTokenAccessFailure);

            if (response && (response.status === StatusCodes.OK || response.status === StatusCodes.NO_CONTENT)) {
                if (response.data) {
                    setEventBarDetails(response.data as IHomeStatusBarConfiguration);
                }
            }
            else {
                setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
            }
        }
        else {
            let response = await getStatusBarDetailsForCentralTeamAsync(teamId, handleTokenAccessFailure);

            if (response && (response.status === StatusCodes.OK || response.status === StatusCodes.NO_CONTENT)) {
                if (response.data) {
                    setEventBarDetails(response.data as IHomeStatusBarConfiguration);
                }
            }
            else {
                setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
            }
        }
    }

    // Gets the 'New to Athena' section articles.
    const getHomeNewToAthenaArticlesAsync = async (teamId: string, isCoiTeam: string) => {
        if (isCoiTeam === "true") {
            let response = await getNewToAthenaArticlesForCoiTeamAsync(teamId, handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                setNewToAthenaArticles(response.data as IHomeConfigurationArticle[]);
                setLoadingNewToAthenaCards(false);
            }
            else {
                setLoadingNewToAthenaCards(false);
                setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
            }
        }
        else {
            let response = await getNewToAthenaArticlesForCentralTeamAsync(teamId, handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                setNewToAthenaArticles(response.data as IHomeConfigurationArticle[]);
                setLoadingNewToAthenaCards(false);
            }
            else {
                setLoadingNewToAthenaCards(false);
                setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
            }
        }
    }

    // Gets the daily briefing articles of logged-in user.
    const getYourDailyBriefingArticlesAsync = async (teamId: string, isCoiTeam: string) => {
        if (isCoiTeam === "true") {
            let response = await getDailyBriefingArticlesOfUserForCoiTeamAsync(teamId, handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                setYourDailyBriefingArticles(response.data as IDailyBriefingHomeArticle[]);
                setLoadingYourDailyBriefingCards(false);
            }
            else {
                setLoadingYourDailyBriefingCards(false);
                setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
            }
        }
        else {
            let response = await getDailyBriefingArticlesOfUserForCentralTeamAsync(teamId, handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                setYourDailyBriefingArticles(response.data as IDailyBriefingHomeArticle[]);
                setLoadingYourDailyBriefingCards(false);
            }
            else {
                setLoadingYourDailyBriefingCards(false);
                setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
            }
        }
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    // Gets 'New to Athena!' cards.
    const getNewToAthenaCards = React.useMemo(() => {
        if (isLoadingNewToAthenaCards) {
            let newToAthenaCardSkeleton = <Card className="new-to-athena-card" elevated>
                <Skeleton animation="wave">
                    <Flex gap="gap.medium">
                        <Skeleton.Shape className="image" />
                        <Flex column gap="gap.medium">
                            <Skeleton.Line width="28rem" height="1.4rem" />
                            <Flex column>
                                <Skeleton.Line width="35rem" height="1.2rem" />
                                <Skeleton.Line width="35rem" height="1.2rem" />
                                <Skeleton.Line width="35rem" height="1.2rem" />
                            </Flex>
                        </Flex>
                    </Flex>
                </Skeleton>
            </Card>;

            return [newToAthenaCardSkeleton, newToAthenaCardSkeleton];
        }

        return newToAthenaArticles.map((article: any) =>
            <NewToAthenaCard header={article.title} description={article.description} imageUrl={article.imageUrl} />);
    }, [isLoadingNewToAthenaCards]);

    // Gets the cards for 'Your daily briefing' section.
    const getYourDailyBriefingCards = React.useMemo(() => {
        if (isLoadingYourDailyBriefingCards) {
            let yourDailyBriefingSkeletonCard = <Card className="your-daily-briefing-card" elevated>
                <Skeleton animation="wave">
                    <Flex column gap="gap.small">
                        <Flex gap="gap.small" vAlign="center">
                            <Skeleton.Shape className="image" />
                            <Flex column>
                                <Skeleton.Line width="20rem" height="1.4rem" />
                                <Skeleton.Line width="10rem" height="1.2rem" />
                            </Flex>
                        </Flex>
                        <Flex column>
                            <Skeleton.Line width="28rem" height="1.2rem" />
                            <Skeleton.Line width="28rem" height="1.2rem" />
                            <Skeleton.Line width="28rem" height="1.2rem" />
                            <Skeleton.Line width="28rem" height="1.2rem" />
                        </Flex>
                    </Flex>
                </Skeleton>
            </Card>;

            return [yourDailyBriefingSkeletonCard, yourDailyBriefingSkeletonCard, yourDailyBriefingSkeletonCard];
        }

        if (!yourDailyBriefingArticles.length) {
            return <Text content={localize("homeTabNoDailyBriefingArticlesAvailableMessage")} />
        }

        return yourDailyBriefingArticles.map((article: IDailyBriefingHomeArticle) =>
            <YourDailyBriefingCard header={article.title} description={article.description} date={article.updatedOn} imageUrl={getResourceImagePath(nodeTypes, article.nodeTypeId)} articleUrl={article.articleUrl} />);
    }, [isLoadingYourDailyBriefingCards]);

    // Gets the sections.
    const getSections = () => {
        if (!isLoadingNewToAthenaCards
            && newToAthenaArticles.length === 0
            && !isLoadingYourDailyBriefingCards
            && yourDailyBriefingArticles.length === 0) {
            return <NoContent message={localize("homeTabNoUpdatesAvailableMessage")} />;
        }

        let sections: JSX.Element[] = [];

        if (isLoadingNewToAthenaCards || newToAthenaArticles.length > 0) {
            sections.push(<Flex className="new-to-athena-section" column gap="gap.small" fill>
                <Text className="new-to-athena-title" content={localize("homeTabNewToAthenaSectionTitle")} size="large" weight="bold" />
                <Flex className="new-to-athena-card-container" gap="gap.medium">
                    {getNewToAthenaCards}
                </Flex>
            </Flex>);
        }

        sections.push(<Flex className="your-daily-briefing-section" column gap="gap.small">
            <Text className="your-daily-briefing-title" content={localize("homeTabYourDailyBriefingSectionTitle")} size="large" weight="bold" />
            <Flex className="your-daily-briefing-container" gap="gap.medium" wrap>
                {getYourDailyBriefingCards}
            </Flex>
        </Flex>);

        return sections;
    };

    // Gets the status bar.
    const getStatusBar = React.useMemo(() => {
        if (eventBarDetails?.message?.trim()) {
            return <Segment design={{ minHeight: "5rem" }} color="brand" content={<Flex vAlign="center" hAlign="center" gap="gap.small">
                <Text content={eventBarDetails.message} title={eventBarDetails.message} weight="semibold" truncated />
                <a href={eventBarDetails.url} target="_blank">{eventBarDetails.linkLabel}</a>
            </Flex>} />;
        }
    }, [eventBarDetails]);

    // Renders component.
    return (
        <Flex className="home-tab" column>
            <StatusBar status={status} isMobile={false} />
            <AthenaSplash heading={localize("homeTabAthenaSplashHeader")} description={localize("homeTabAthenaSplashDescription")} />
            {getStatusBar}
            <Flex className="sections-container" column gap="gap.small" fill>
                {getSections()}
            </Flex>
        </Flex>
    );
}

export default withRouter(Home);