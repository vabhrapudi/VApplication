// <copyright file="insights-card.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { useTranslation } from 'react-i18next';
import { Flex, Text, Skeleton } from "@fluentui/react-northstar";
import InsightsFilter from "../insights-filter/insights-filter";
import PieChart, * as PieChartComponent from 'devextreme-react/pie-chart';
import * as ChartComponent from 'devextreme-react/chart';
import IPriority from "../../../models/priority";
import { StatusCodes } from "http-status-codes";
import { getInsightsAsync } from "../../../api/priority-api";
import KeywordContext from "../../../contexts/keywords-context";
import IKeyword from "../../../models/keyword";
import IPriorityInsight from "../../../models/priority-insight";
import { cloneDeep } from "lodash";

import "./insights-card.scss";

interface IInsightsCardProps extends RouteComponentProps {
    doughnutChartTitle: string;
    barChartTitle: string;
    priorities: IPriority[];
    isLoading: boolean;
    priorityType: number;
}

const InsightsCard: React.FunctionComponent<IInsightsCardProps> = (props: IInsightsCardProps) => {
    const localize = React.useRef(useTranslation().t).current;

    const [isLoading, setLoading] = React.useState(props.isLoading);
    const [insightsData, setInsightsData] = React.useState([] as IPriorityInsight[]);
    const [priorities, setPriorities] = React.useState([] as IPriority[]);

    const keywordsContext = React.useContext(KeywordContext);

    React.useEffect(() => {
        if (props.isLoading === false) {
            let prioritiesData = cloneDeep(props.priorities);
            prioritiesData = prioritiesData?.filter((priority: IPriority) => priority.type === props.priorityType);

            if (prioritiesData?.length) {
                setPriorities(prioritiesData);

                let priorityIds = prioritiesData.map((priority: IPriority) => priority.id);

                let keywordIds = keywordsContext
                    ?.filter((keyword: IKeyword) => prioritiesData
                        .some((priority: IPriority) => priority.keywords.some((keywordId: number) => keywordId === Number(keyword.keywordId))))
                    .map((keyword: IKeyword) => Number(keyword.keywordId)) ?? [];

                getInsightsDataAsync(priorityIds, keywordIds);
            }
            else {
                setLoading(false);
            }
        }
    }, [props.isLoading]);

    /**
     * Gets the insights data.
     * @param selectedPriorityIds The selected priority Ids.
     * @param selectedKeywordIds The selected keyword Ids.
     */
    const getInsightsDataAsync = async (selectedPriorityIds: string[], selectedKeywordIds: number[]) => {
        let response = await getInsightsAsync(selectedPriorityIds, selectedKeywordIds, handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let insights = response.data as IPriorityInsight[];

            for (let i = 0; i < insights.length; i++) {
                let data = insights[i];
                data.val = data.proposed + data.current + data.completed;
            }

            setInsightsData(insights);
        }

        setLoading(false);
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    /**
     * Event handler called when filter get applied.
     * @param selectedPriorityIds The selected priority Ids.
     * @param selectedKeywordIds The selected keyword Ids.
     */
    const onApplyFilter = (selectedPriorityIds: string[], selectedKeywordIds: number[]) => {
        setLoading(true);
        getInsightsDataAsync(selectedPriorityIds, selectedKeywordIds);
    }

    /**
     * Custom donut chart tooltip.
     * @param point The point details.
     */
    const customDonutChartTooltip = (point: any) => {
        return { text: `${point.percentText}` };
    }

    /**
     * Custom bar chart tooltip.
     * @param point The point details.
     */
    const customBarChartTooltip = (point: any) => {
        return { text: point.valueText };
    }

    // Gets the loading skeleton for donut chart.
    const getSkeletonForDonutChart = React.useMemo(() => {
        return <Skeleton animation="wave"><Skeleton.Shape width="296px" height="170px" /></Skeleton>;
    }, [isLoading]);

    // Gets the loading skeleton for bar chart.
    const getSkeletonForBarChart = React.useMemo(() => {
        return <Skeleton animation="wave"><Skeleton.Shape width="296px" height="222px" /></Skeleton>;
    }, [isLoading]);

    return (
        <Flex className="insights-card" column>
            <div className="card-layout donut-chart-card">
                <Flex column>
                    <Flex vAlign="center">
                        <Text content={props.doughnutChartTitle} weight="bold" />
                        <InsightsFilter
                            className="align-right"
                            content={localize("filterButtonContent")}
                            priorities={priorities}
                            disabled={isLoading}
                            onApply={onApplyFilter} />
                    </Flex>
                    {
                        isLoading ? getSkeletonForDonutChart :
                            <PieChart
                                type="doughnut"
                                palette="Soft Pastel"
                                dataSource={insightsData}
                            >
                                <ChartComponent.Size
                                    height={170}
                                />
                                <PieChartComponent.Series argumentField="title">
                                </PieChartComponent.Series>
                                <PieChartComponent.Legend
                                    itemTextPosition="right"
                                    verticalAlignment="center"
                                    horizontalAlignment="right"
                                />
                                <ChartComponent.Tooltip
                                    enabled={true}
                                    customizeTooltip={customDonutChartTooltip}
                                />
                            </PieChart>
                    }
                </Flex>
            </div>
            <div className="card-layout bar-chart-card">
                <Flex column gap="gap.small">
                    <Text content={props.barChartTitle} weight="bold" />
                    {
                        isLoading ? getSkeletonForBarChart :
                            <ChartComponent.Chart
                                palette="Soft"
                                dataSource={insightsData}
                            >
                                <ChartComponent.Size
                                    height={220}
                                />
                                <ChartComponent.ValueAxis
                                    visible={false}
                                />
                                <ChartComponent.CommonSeriesSettings
                                    argumentField="title"
                                    type="bar"
                                    barPadding={0.5}
                                />
                                <ChartComponent.Series valueField="proposed" name={localize("proposedLabel")} />
                                <ChartComponent.Series valueField="current" name={localize("currentLabel")} />
                                <ChartComponent.Series valueField="completed" name={localize("completedLabel")} />
                                <ChartComponent.Legend
                                    itemTextPosition="right"
                                    verticalAlignment="right"
                                    horizontalAlignment="center"
                                />
                                <ChartComponent.Tooltip
                                    enabled={true}
                                    customizeTooltip={customBarChartTooltip}
                                />
                            </ChartComponent.Chart>
                    }
                </Flex>
            </div>
        </Flex>
    );
}

export default React.memo(withRouter(InsightsCard));