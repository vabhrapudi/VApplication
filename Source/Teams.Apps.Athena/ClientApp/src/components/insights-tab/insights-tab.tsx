// <copyright file="insights-tab.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import InsightsCard from "../insights-tab/insights-card/insights-card";
import { Flex, Menu, tabListBehavior, Table, Button, ContactGroupIcon, Skeleton, Text } from "@fluentui/react-northstar";
import { useTranslation } from 'react-i18next';
import AthenaSplash from "../../components/athena-splash/athena-splash";
import IPriorityType from "../../models/priority-type";
import { StatusCodes } from "http-status-codes";
import { getAllPrioritiesAsync, getPriorityTypesAsync } from "../../api/priority-api";
import StatusBar from "../common/status-bar/status-bar";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import IPriority from "../../models/priority";
import { getAllKeywordsAsync } from "../../api/keyword-api";
import IKeyword from "../../models/keyword";
import KeywordsContext from "../../contexts/keywords-context";
import ContentLoader from "../common/loader/loader";
import NoContent from "../common/no-content/no-content";
import { getAthenaActiveUsersCountAsync } from "../../api/user-settings-tab-api";
import { getTotalApprovedCoiCount } from "../../api/coi-requests-api";

import "./insights-tab.scss";

interface IInsightsTabProps extends RouteComponentProps {
}

const SetLoadingActionType: string = "LOADING_FALSE";
const SetValueActionType: string = "SET_VALUE";

// Reducer to set footer state.
const footerReducer = (state: any, action: any) => {
    switch (action.type) {
        case SetLoadingActionType:
            return {...state, isLoading: action?.payload?.isLoading ?? false};

        case SetValueActionType:
            return {...state, value: action?.payload?.value ?? 0};

        default:
            return state;
    }
}

const InsightsTab: React.FunctionComponent<IInsightsTabProps> = (props: IInsightsTabProps) => {
    const localize = React.useRef(useTranslation().t).current;

    const [menuActiveIndex, setMenuActiveIndex] = React.useState(0);
    const [totalAthenaUsersState, setAthenaUsersState] = React.useReducer(footerReducer, {
        isLoading: true,
        value: 0
    });
    const [totalCoisState, setCoisState] = React.useReducer(footerReducer, {
        isLoading: false,
        value: 0
    });

    const [priorityTypes, setPriorityTypes] = React.useState([] as IPriorityType[]);
    const [priorities, setPriorities] = React.useState([] as IPriority[]);
    const [status, setStatus] = React.useState({ id: 0, message: "", type: ActivityStatus.None } as IStatusBar);
    const [keywords, setKeywords] = React.useState(undefined as IKeyword[] | undefined);
    const [isLoadingPriorityTypes, setLoadingPriorityTypes] = React.useState(true);
    const [isLoadingPriorities, setLoadingPriorities] = React.useState(true);

    React.useEffect(() => {
        getAllKeywords();
        getPriorityTypes();
        getPrioritiesAsync();
        getActiveUsersCountAsync();
        getTotalCoisCountAsync();
    }, []);

    // Gets the priority types.
    const getPriorityTypes = async () => {
        let response = await getPriorityTypesAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let priorityTypesData = response.data as IPriorityType[];
            setPriorityTypes(priorityTypesData);

            if (priorityTypesData?.length > 0) {
                setMenuActiveIndex(priorityTypesData[0].id);
            }
        }
        else {
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }

        setLoadingPriorityTypes(false);
    }

    // Gets all priorities.
    const getPrioritiesAsync = async () => {
        let response = await getAllPrioritiesAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setPriorities(response.data as IPriority[]);
        }
        else {
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }

        setLoadingPriorities(false);
    }

    // Gets all keywords.
    const getAllKeywords = async () => {
        let response = await getAllKeywordsAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setKeywords(response.data as IKeyword[]);
        }
        else {
            setKeywords([]);
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }
    }

    // Gets the active users count.
    const getActiveUsersCountAsync = async () => {
        let response = await getAthenaActiveUsersCountAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            if (response.data > 0) {
                setAthenaUsersState({ type: SetValueActionType, payload: { value: response.data } });
            }
        }
        else {
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }

        setAthenaUsersState({ type: SetLoadingActionType, payload: { isLoading: false } });
    }

    // Gets the total COIs count.
    const getTotalCoisCountAsync = async () => {
        let response = await getTotalApprovedCoiCount(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            if (response.data > 0) {
                setCoisState({ type: SetValueActionType, payload: { value: response.data } });
            }
        }
        else {
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }

        setCoisState({ type: SetLoadingActionType, payload: { isLoading: false } });
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }
    
    /**
     * Event handler called when menu active index changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    const onMenuActiveIndexChange = (eventDetails: any, eventData: any) => {
        setMenuActiveIndex(eventData.activeIndex);
    }

    // Gets the keywords' string separated by comma.
    const getKeywordsString = (keywordIds: number[]) => {
        if (keywords === undefined) {
            return localize("loadingLabel");
        }

        if (!keywordIds.length) {
            return "NA";
        }

        let filteredKeywords = keywords
            .filter((keyword: IKeyword) => keywordIds.some((keywordId: number) => keywordId === Number(keyword.keywordId)));

        if (!filteredKeywords.length) {
            return "NA";
        }

        return filteredKeywords
            .map((keyword: IKeyword) => keyword.title)
            .join(",");
    }

    // Gets the menu items.
    const getMenuItems = () => {
        return priorityTypes.map((priorityType: IPriorityType) => {
            return {
                key: `priority-type-${priorityType.id}`,
                index: priorityType.id,
                content: priorityType.title
            } as any
        });
    };

    // Gets the table header.
    const getTableHeader = React.useMemo(() => {
        return <Table.Row header>
            <Table.Cell design={{ minWidth: "30%" }} content={localize("prioritiesLabel")} title={localize("prioritiesLabel")} truncateContent={true} />
            <Table.Cell design={{ minWidth: "40%" }} content={localize("descriptionText")} title={localize("descriptionText")} truncateContent={true} />
            <Table.Cell design={{ minWidth: "30%" }} content={localize("myRequestsKeywordsColumn")} title={localize("myRequestsKeywordsColumn")} truncateContent={true} />
        </Table.Row>
    }, []);

    // Gets the table rows.
    const getTableRows = () => {
        return priorities
            .filter((priority: IPriority) => priority.type === menuActiveIndex)
            .map((priority: IPriority) => {
                let keywordString = getKeywordsString(priority.keywords);

                return <Table.Row key={`priority-${priority.id}`} className="row">
                    <Table.Cell design={{ minWidth: "30%" }} content={priority.title} title={priority.title} truncateContent={true} />
                    <Table.Cell design={{ minWidth: "40%" }} content={priority.description} title={priority.description} truncateContent={true} />
                    <Table.Cell design={{ minWidth: "30%" }} content={keywordString} title={keywordString} truncateContent={true} />
                </Table.Row>
            });
    };

    // Gets the priority insights cards.
    const getPriorityInsightsCards = React.useMemo(() => {
        return priorityTypes
            .map((priorityType: IPriorityType) => <InsightsCard
                barChartTitle={priorityType.title}
                doughnutChartTitle={localize("insightsDonutChartTitle", { title: priorityType.title })}
                isLoading={isLoadingPriorities || isLoadingPriorityTypes || keywords === undefined}
                priorities={priorities}
                priorityType={priorityType.id}
            />);
    }, [isLoadingPriorities, isLoadingPriorityTypes, keywords]);

    // Gets the priorities table.
    const getPrioritiesTable = React.useMemo(() => {
        if (isLoadingPriorities) {
            return <Skeleton animation="wave">
                <Skeleton.Line width="90px" height="31px" />
            </Skeleton>
        }

        if (!priorities?.length) {
            return <Text content={localize("insightsPrioritiesNotAvailable")} />;
        }

        return <React.Fragment>
            <Menu
                items={getMenuItems()}
                activeIndex={menuActiveIndex}
                underlined
                primary
                accessibility={tabListBehavior}
                onActiveIndexChange={onMenuActiveIndexChange}
            />
            <Table>
                {getTableHeader}
                {getTableRows()}
            </Table>
        </React.Fragment>
    }, [isLoadingPriorities, menuActiveIndex, priorityTypes, keywords]);

    if (isLoadingPriorityTypes) {
        return <ContentLoader />;
    }

    if (!priorityTypes?.length) {
        return <NoContent message={localize("insightsPriorityTypesNotFound")} />;
    }

    return (
        <Flex className="insights-tab" column>
            <StatusBar status={status} isMobile={false} />
            <AthenaSplash heading={localize("insightsTabDisplayName")} description={localize("insightsTabSplashDescription")} />
            <div className="content overflow-y">
                <Flex className="insights-cards overflow-x" gap="gap.small">
                    <KeywordsContext.Provider value={keywords!}>
                        {getPriorityInsightsCards}
                    </KeywordsContext.Provider>
                </Flex>
                <Flex className="priorities card-layout" column gap="gap.small">
                    {getPrioritiesTable}
                </Flex>
            </div>
            <Flex className="footer" vAlign="center">
                <Button
                    text
                    icon={<ContactGroupIcon />}
                    content={totalAthenaUsersState.isLoading === true ? <Skeleton animation="wave"><Skeleton.Line width="100px" height="18px" /></Skeleton> : localize("insightsTabTotalAthenaUsersContent", { value: totalAthenaUsersState.value })}
                />
                <Button
                    text
                    icon={<ContactGroupIcon />}
                    content={totalCoisState.isLoading === true ? <Skeleton animation="wave"><Skeleton.Line width="100px" height="18px" /></Skeleton> : localize("insightsTabTotalAthenaCoisContent", { value: totalCoisState.value })}
                />
            </Flex>
        </Flex>
    );
}

export default withRouter(InsightsTab);