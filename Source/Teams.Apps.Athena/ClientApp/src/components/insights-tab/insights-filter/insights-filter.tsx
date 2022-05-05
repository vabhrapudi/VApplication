// <copyright file="insights-filter.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { useTranslation } from 'react-i18next';
import { Flex, Divider, Text, Button, Popup, ChevronDownIcon, CloseIcon, Checkbox } from "@fluentui/react-northstar";
import IFilterItem from "../../../models/filter-item";
import { cloneDeep } from "lodash";
import IPriority from "../../../models/priority";
import KeywordContext from "../../../contexts/keywords-context";
import IKeyword from "../../../models/keyword";

import "./insights-filter.scss";

interface IInsightsFilterProps extends RouteComponentProps {
    className?: string;
    priorities: IPriority[];
    content: string;
    disabled: boolean;
    onApply: (selectedItems: string[], selectedKeywords: number[]) => void;
}

const InsightsFilter: React.FunctionComponent<IInsightsFilterProps> = (props: IInsightsFilterProps) => {
    const localize = React.useRef(useTranslation().t).current;

    const [isFilterOpen, setFilterOpen] = React.useState(false);
    const [priorities, setPriorities] = React.useState([] as IPriority[]);
    const [keywords, setKeywords] = React.useState([] as IFilterItem[]);

    const keywordsContext = React.useContext(KeywordContext);

    React.useEffect(() => {
        if (props.priorities?.length) {
            setPriorities(props.priorities.map((priority: IPriority) => {
                return { ...priority, isChecked: true } as IPriority
            }));

            let keywords = getKeywordsForPriorities(props.priorities);
            setKeywords(keywords);
        }
    }, [props.priorities]);

    /**
     * Gets the keywords for priorities.
     * @param priorities The collection of priorities.
     */
    const getKeywordsForPriorities = (priorities: IPriority[]) => {
        return keywordsContext
            ?.filter((keyword: IKeyword) => priorities
                .some((priority: IPriority) => priority.keywords.some((keywordId: number) => keywordId === Number(keyword.keywordId))))
            .map((keyword: IKeyword) => {
                return { key: Number(keyword.keywordId), header: keyword.title, isChecked: true } as IFilterItem
            }) ?? [];
    }

    // Event handler called when filter needs to be closed.
    const onCloseFilter = () => {
        setFilterOpen(false);
    }

    // Event handler called when filter open changes.
    const onFilterOpenChange = () => {
        setFilterOpen(prevState => !prevState);
    }

    // Event handler called when click on clear all button.
    const onClearAllButtonClick = () => {
        let isAnyItemChecked: boolean = priorities.some((priority: IPriority) => priority.isChecked === true);

        if (isAnyItemChecked) {
            let filterItems = priorities.map((priority: IPriority) => { return { ...priority, isChecked: false } as IPriority });
            setPriorities(filterItems);
        }

        let isAnyKeywordChecked: boolean = keywords.some((keyword: IFilterItem) => keyword.isChecked === true);

        if (isAnyKeywordChecked) {
            let filterKeywords = keywords.map((filterItem: any) => { return { ...filterItem, isChecked: false } as IFilterItem });
            setKeywords(filterKeywords);
        }
    }

    // Event handler called when click on apply button.
    const onApplyButtonClick = () => {
        setFilterOpen(false);

        let selectedPriorityIds = priorities
            .filter((priority: IPriority) => priority.isChecked === true)
            .map((priority: IPriority) => priority.id);

        let selectedKeywordIds = keywords
            .filter((filterItem: IFilterItem) => filterItem.isChecked === true)
            .map((filterItem: IFilterItem) => filterItem.key);

        props.onApply(selectedPriorityIds, selectedKeywordIds);
    }

    /**
     * Event handler called when item checked changed.
     * @param itemId The item Id.
     * @param isChecked Indicates whether the item is checked.
     */
    const onItemCheckedChange = (itemId: string, isChecked: boolean) => {
        let filterItems = cloneDeep(priorities);

        let item = filterItems.find((priority: IPriority) => priority.id === itemId);

        if (item) {
            item.isChecked = isChecked;
            setPriorities(filterItems);

            let selectedPriorities = filterItems.filter((priority: IPriority) => priority.isChecked === true);
            let selectedPrioritiesKeywords = getKeywordsForPriorities(selectedPriorities);
            setKeywords(selectedPrioritiesKeywords);
        }
    }

    /**
     * Event handler called when keyword checked changed.
     * @param keywordId The keyword Id.
     * @param isChecked Indicates whether the keyword is checked.
     */
    const onKeywordCheckedChange = (keywordId: number, isChecked: boolean) => {
        let filterKeywords = cloneDeep(keywords);

        let keyword = filterKeywords.find((filterItem: IFilterItem) => filterItem.key === keywordId);

        if (keyword) {
            keyword.isChecked = isChecked;
            setKeywords(filterKeywords);
        }
    }

    // Gets the DOM elements to render items.
    const getItemsToRender = React.useMemo(() => {
        return <Flex className="items-container" column gap="gap.small">
            {
                priorities.map((priority: IPriority) => {
                    return (
                        <Flex vAlign="center">
                            <Text content={priority.title} />
                            <Flex.Item push>
                                <Checkbox checked={priority.isChecked} onChange={(eventDetails: any, eventData: any) => onItemCheckedChange(priority.id, eventData.checked)} />
                            </Flex.Item>
                        </Flex>
                    );
                })
            }
        </Flex>
    }, [priorities]);

    // Gets the DOM elements to render keywords.
    const getKeywordsToRender = React.useMemo(() => {
        return <Flex className="keywords-container" column gap="gap.small">
            {
                keywords.map((keyword: any) => {
                    return (
                        <Flex vAlign="center">
                            <Text content={keyword.header} />
                            <Flex.Item push>
                                <Checkbox checked={keyword.isChecked} onChange={(eventDetails: any, eventData: any) => onKeywordCheckedChange(keyword.key, eventData.checked)} />
                            </Flex.Item>
                        </Flex>
                    );
                })
            }
        </Flex>
    }, [keywords]);

    return (
        <div className={props.className}>
            <Popup
                open={isFilterOpen}
                position="below"
                align="start"
                trigger={<Button disabled={props.disabled} text icon={<ChevronDownIcon />} iconPosition="after" content={props.content} />}
                onOpenChange={onFilterOpenChange}
                content={
                    <Flex className="insights-filter" column gap="gap.small">
                        <Flex column>
                            <Flex vAlign="center">
                                <Text content={localize("filtersText")} weight="semibold" size="large" />
                                <Flex.Item push>
                                    <Button text icon={<CloseIcon />} iconOnly onClick={onCloseFilter} />
                                </Flex.Item>
                            </Flex>
                        </Flex>
                        <Divider />
                        <Flex column gap="gap.smaller">
                            <Text content={localize("prioritiesLabel")} weight="semibold" />
                            {getItemsToRender}
                        </Flex>
                        <Divider />
                        <Text content={localize("myRequestsKeywordsColumn")} weight="semibold" />
                        {getKeywordsToRender}
                        <Flex.Item push>
                            <Flex column gap="gap.small">
                                <Divider />
                                <Flex gap="gap.small">
                                    <Button text primary content={localize("clearAllButtonContent")} onClick={onClearAllButtonClick} />
                                    <Flex.Item push>
                                        <Button content={localize("cancelButtonContent")} onClick={onCloseFilter} />
                                    </Flex.Item>
                                    <Button className="athena-button" content={localize("applyButtonContent")} onClick={onApplyButtonClick} />
                                </Flex>
                            </Flex>
                        </Flex.Item>
                    </Flex>
                }
            />
        </div>
    );
}

export default React.memo(withRouter(InsightsFilter));