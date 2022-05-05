// <copyright file="news-home.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { Flex, ChevronDownIcon, Button, SearchIcon, Text, MenuButton, Image, Loader, Dropdown, DropdownItemProps, Toolbar, Pill } from "@fluentui/react-northstar";
import InfiniteScroll from "react-infinite-scroller";
import { FilterIcon } from '@fluentui/react-icons-mdl2';
import { IResearchNews } from "../../../models/type";
import NewsArticle from "../news-article/news-article";
import { useTranslation } from 'react-i18next';
import NewsTypeFilterPopup from "../news-type-filter-popup/news-type-filter-popup";
import { getAthenaNewsSourcesAsync, searchCOINewsAsync, searchNewsAsync, getNodeTypesForNewsAsync, getNewsKeywordIdsAsync } from "../../../api/news-api";
import Constants from "../../../constants/constants";
import StatusBar from "../../common/status-bar/status-bar";
import IStatusBar from "../../../models/status-bar";
import { ActivityStatus } from "../../../models/activity-status";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { StatusCodes } from "http-status-codes";
import AthenaSplash from "../../athena-splash/athena-splash";
import { getAllKeywordsAsync, getCoiTeamKeywordsAsync } from "../../../api/keyword-api";
import IKeyword from "../../../models/keyword";
import { IAthenaNewsSource } from "../../../models/athena-news-source";
import IDiscoveryTreeNodeType from "../../../models/discovery-tree-node-type";
import INewsFilterParameters from "../../../models/news-filter-parameters";
import { cloneDeep } from "lodash";

import "./news-home.scss";

interface INewsHomeProps extends RouteComponentProps {
}

const NewsHome: React.FunctionComponent<INewsHomeProps> = (props: INewsHomeProps) => {
    const localize = useTranslation().t;
    const params = new URLSearchParams(window.location.search);
    const [isLoading, setIsLoading] = React.useState<boolean>(true);
    const [newsData, setNewsData] = React.useState<IResearchNews[]>([]);
    const [isFilterOpen, setIsFilterOpen] = React.useState<boolean>(false);
    const [selectedKeywords, setSelectedKeywords] = React.useState<IKeyword[]>([]);
    const [searchText, setSearchText] = React.useState<string>("");
    const [sortBy, setSortBy] = React.useState<number>(0);
    const [hasMore, setHasMore] = React.useState<Boolean>(true);
    const [key, setKey] = React.useState<number>(0);
    const [status, setStatus] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });
    const [teamId, setTeamId] = React.useState<string>("");
    const [allKeywords, setAllKeywords] = React.useState<IKeyword[]>([]);
    const [newsSources, setNewsSources] = React.useState<IAthenaNewsSource[]>([]);
    const [newsTypes, setNewsTypes] = React.useState<IDiscoveryTreeNodeType[]>([]);
    const [selectedNewsTypes, setSelectedNewsTypes] = React.useState<IDiscoveryTreeNodeType[]>([]);
    const [newsKeywords, setNewsKeywords] = React.useState<IKeyword[]>([]);
    const [searchResults, setSearchResults] = React.useState<DropdownItemProps[]>([]);
    const [overflowOpen, setOverflowOpen] = React.useState<boolean>(false);
    const [selectedKeywordsPillsData, setSelectedKeywordsPillsData] = React.useState<any[]>([]);

    // Sort by menu items
    const sortItems = [
        {
            key: '0',
            content: localize("newsSortByDateText"),
        },
        {
            key: '1',
            content: localize("newsSortBySignificaceText"),
        },
        {
            key: '2',
            content: localize("newsSortByRatingText"),
        }
    ];

    React.useEffect(() => {
        microsoftTeams.initialize();

        getInitialData();

        let isCoiId = params.get('isCoiId') ?? "";
        if (isCoiId === "true") {
            microsoftTeams.getContext(async (context: microsoftTeams.Context) => {
                const teamId = context.groupId!
                setTeamId(teamId);
            })
        }
        else {
            getNewsData(0);
        }
    }, []);

    React.useEffect(() => {
        if (teamId) {
            getNewsData(0);
            getNewsKeywords();
        }
    }, [teamId]);

    React.useEffect(() => {
        if (!isLoading) {
            setKey(key + 1);
            getNewsData(0);
        }
    }, [sortBy]);

    React.useEffect(() => {
        getPillsData();
    }, [selectedKeywords, isLoading, selectedNewsTypes]);

    React.useEffect(() => {
        getNewsKeywords();
    }, [allKeywords])

    // Fetches the initial data.
    const getInitialData = async () => {
        getNewsSources();
        getNewsTypes();
        getAllKeywords();
    }

    /**
     * Get news data from database.
     * @param pageCount The page count.
     */
    const getNewsData = async (pageCount: number) => {
        if (pageCount === 0) {
            setIsLoading(true);
        }

        var newsFilterParameters: INewsFilterParameters = {
            keywords: selectedKeywords,
            newsTypes: selectedNewsTypes
        }

        var response;
        if (teamId) {
            response = await searchCOINewsAsync(teamId, searchText, newsFilterParameters, pageCount, sortBy, handleTokenAccessFailure);
        }
        else {
            response = await searchNewsAsync(searchText, newsFilterParameters, pageCount, sortBy, handleTokenAccessFailure);
        }

        if (response && response.status === StatusCodes.OK) {
            setIsLoading(false);
            if (pageCount === 0) {
                setNewsData(response.data);
            }
            else {
                var existingData = newsData.map((researchNews: IResearchNews) => {
                    return {
                        ...researchNews
                    }
                });
                var updatedData = existingData.concat(response.data);
                setNewsData(updatedData);
            }

            if (response.data.length < Constants.lazyLoadNewsCount) {
                setHasMore(false);
            }
        }
        else if (response && response.status === StatusCodes.NOT_FOUND) {
            setIsLoading(false);
        }
        else {
            setIsLoading(false);
            setStatus({ id: status.id + 1, message: localize("generalErrorMessage"), type: ActivityStatus.Error });
        }
    }

    // Get all keywords.
    const getAllKeywords = async () => {
        let response = await getAllKeywordsAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let keywords = response.data as IKeyword[];
            setAllKeywords(keywords);
        }
    }

    // Returns the news keywords.
    const getNewsKeywords = async () => {
        if (teamId && allKeywords.length > 0) {
            var response = await getCoiTeamKeywordsAsync(teamId, handleTokenAccessFailure);
            if (response && response.status === StatusCodes.OK) {
                let keywordIds = response.data as number[];
                let keywordIdsStringArray = keywordIds.map(String);
                let newsKeywords = allKeywords.filter((keyword: IKeyword) => keywordIdsStringArray.some((keywordId: string) => keyword.keywordId === keywordId));
                setNewsKeywords(newsKeywords);
            }
        }
        else {
            if (allKeywords.length > 0) {
                let response = await getNewsKeywordIdsAsync(handleTokenAccessFailure);

                if (response && response.status === StatusCodes.OK) {
                    let keywordIds = response.data as number[];
                    let keywordIdsStringArray = keywordIds.map(String);
                    let newsKeywords = allKeywords.filter((keyword: IKeyword) => keywordIdsStringArray.some((keywordId: string) => keyword.keywordId === keywordId));
                    setNewsKeywords(newsKeywords);
                }
            }
        }
    }

    // Gets news sources.
    const getNewsSources = async () => {
        let response = await getAthenaNewsSourcesAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let newsSources = response.data as IAthenaNewsSource[];
            setNewsSources(newsSources);
        }
    }

    // Gets news types.
    const getNewsTypes = async () => {
        let response = await getNodeTypesForNewsAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let newsTypes = response.data as IDiscoveryTreeNodeType[];
            setNewsTypes(newsTypes);
        }
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    /**
     * Handles the sort by menu item selection.
     * @param event The event.
     * @param menuItemProps The menu item props.
     */
    const handleSortItemClick = (event: any, menuItemProps: any) => {
        setSortBy(Number(menuItemProps.index));
    }

    // Opens filter menu
    const handleFilterButtonCLick = () => {
        setIsFilterOpen(!isFilterOpen);
    }

    /**
     * Sets the selected news types.
     * @param newsTypes News types.
     */
    const getSelectedNewsTypes = (newsTypes: IDiscoveryTreeNodeType[]) => {
        let selectedNewsTypes: IDiscoveryTreeNodeType[] = newsTypes.filter((newsType: IDiscoveryTreeNodeType) => newsType.isChecked === true);
        setSelectedNewsTypes(selectedNewsTypes);
    }

    /**
     * Updates the news item.
     * @param newsItem The news items.
     */
    const updateNewsItem = (newsArticleData: IResearchNews[]) => {
        setNewsData(newsArticleData);
    }

    // Closes the filter dropdown if clicked outside.
    const handleFilterOutsideClick = () => {
        setIsFilterOpen(!isFilterOpen);
    }

    /**
     * Shows the status of update rating action.
     * @param updatedStatus The updated status.
     */
    const showUpdateRatingStatus = (updatedStatus: IStatusBar) => {
        setStatus({ id: status.id + 1, message: updatedStatus.message, type: updatedStatus.type });
    }

    // Event handler called when click on search button.
    const onSearchButtonClick = () => {
        if (!hasMore) {
            setHasMore(true);
        }
        getNewsData(0);
    }

    /**
     * Handles the selection of item from keywords dropdown items.
     * @param event The event.
     * @param dropdownProps The dropdown props.
     */
    const handleKeywordDropdownChange = (event: any, dropdownProps?: any) => {
        if (dropdownProps.value) {
            let existingSelectedKeywords = cloneDeep(selectedKeywords);
            let isPresent = existingSelectedKeywords.some((keyword: IKeyword) => keyword.keywordId === dropdownProps.value!.key);
            if (!isPresent) {
                existingSelectedKeywords.push({ keywordId: dropdownProps.value!.key, title: dropdownProps.value!.header });
                setSelectedKeywords(existingSelectedKeywords);
            }
            setSearchText("");
        }
    }

    /**
     * Intiates the search when query string of dropdown changes.
     * @param searchQuery The search query.
     */
    const initiateSearch = (searchQuery: string) => {
        setSearchText(searchQuery);
        var result: IKeyword[] = [];
        if (searchQuery) {
            result = newsKeywords.filter((keyword: IKeyword) => keyword.title.toLowerCase().includes(searchQuery.toLowerCase()));
        }
        else {
            result = newsKeywords;
        }
        let searchResults: DropdownItemProps[] = result.slice(0, Constants.KeywordSearchResultMaxCount)
            .sort((a: IKeyword, b: IKeyword) => a.title.localeCompare(b.title))
            .map((keyword: IKeyword) => {
                return {
                    key: keyword.keywordId,
                    value: keyword.title,
                    header: keyword.title,
                } as DropdownItemProps;
            });
        setSearchResults(searchResults);
    }

    /**
     * Deselects the selected keyword.
     * @param selectedKeyword The selected keyword.
     */
    const deselectSelectedKeywordPill = (selectedKeyword: IKeyword) => {
        let existingSelectedKeywords = cloneDeep(selectedKeywords);
        let index = existingSelectedKeywords.findIndex((keyword: IKeyword) => keyword.keywordId === selectedKeyword.keywordId);
        if (index !== -1) {
            existingSelectedKeywords.splice(index, 1);
            setSelectedKeywords(existingSelectedKeywords);
        }
    }

    /**
     * Deselects the selected news types.
     * @param selectedNewsType The selected news types.
     */
    const deselectSelectedNewsTypePill = (selectedNewsType: IDiscoveryTreeNodeType) => {
        let existingSelectedNewsTypes = cloneDeep(selectedNewsTypes);
        let index = existingSelectedNewsTypes.findIndex((newsType: IDiscoveryTreeNodeType) => newsType.nodeTypeId === selectedNewsType.nodeTypeId);
        if (index !== -1) {
            existingSelectedNewsTypes.splice(index, 1);
            setSelectedNewsTypes(existingSelectedNewsTypes);
        }
    }

    // Returns the selected keywords & news types Pills Data.
    const getPillsData = () => {
        let pillsData: any[] = [];

        selectedNewsTypes.map((newsType: IDiscoveryTreeNodeType) => {
            var title = `${localize('newsTypeTitle')} - ${newsType.title}`
            pillsData.push({
                key: newsType.nodeTypeId,
                title: title,
                kind: 'custom',
                fitted: 'horizontally',
                content: <Pill
                    actionable
                    onDismiss={() => { deselectSelectedNewsTypePill(newsType) }}
                    disabled={isLoading}
                >
                    {title}
                </Pill>,
                className: "toolbar-filter-item"
            })
        });

        selectedKeywords.map((keyword: IKeyword) => {
            pillsData.push({
                key: keyword.keywordId,
                title: keyword.title,
                kind: 'custom',
                fitted: 'horizontally',
                content: <Pill
                    actionable
                    onDismiss={() => { deselectSelectedKeywordPill(keyword) }}
                    disabled={isLoading}
                >
                    {keyword.title}
                </Pill>,
                className: "toolbar-filter-item"
            })
        });

        setSelectedKeywordsPillsData(pillsData);
    }

    return (
        <>
            <StatusBar status={status} isMobile={false} />
            <AthenaSplash heading={localize("researchNewsHeading")} />
            <Flex gap="gap.small" padding="padding.medium" column className="news-article-main-container">
                <Flex gap="gap.small" hAlign="end" vAlign="center">
                    <FilterIcon />
                    <Button icon={<ChevronDownIcon />} iconOnly text iconPosition="after" content={selectedNewsTypes.length ? `${localize("filterText")} (${selectedNewsTypes.length})` : localize("filterText")} className="icon-pointer button-text" onClick={handleFilterButtonCLick} disabled={isLoading} />
                    {
                        isFilterOpen &&
                        <NewsTypeFilterPopup getSelectedNewsTypes={getSelectedNewsTypes} handleClickOutside={handleFilterOutsideClick} selectedNewsTypes={selectedNewsTypes} newsTypes={newsTypes} />
                    }
                    <MenuButton
                        trigger={<Button icon={<ChevronDownIcon />} iconOnly text content={localize("sortByBtnText")} iconPosition="after" className="icon-pointer" disabled={isLoading} />}
                        menu={sortItems}
                        onMenuItemClick={handleSortItemClick}
                    />
                    <Flex gap="gap.smaller" vAlign="center">
                        <Dropdown
                            search
                            inverted
                            placeholder={localize("findText")}
                            items={searchResults}
                            toggleIndicator={<SearchIcon />}
                            onSearchQueryChange={(e, { searchQuery }) => {
                                initiateSearch(searchQuery!);
                            }}
                            onChange={handleKeywordDropdownChange}
                            noResultsMessage={localize("noKeywordsFoundDropdownMessage")}
                            className="news-search-box"
                            loading={isLoading}
                            loadingMessage={localize("loadingLabel")}
                            value={searchText}
                            disabled={isLoading}
                        />
                        <Button content={localize("searchBtnTxt")} className="athena-button icon-pointer" onClick={onSearchButtonClick} disabled={isLoading} />
                    </Flex>
                </Flex>
                {
                    selectedKeywordsPillsData.length > 0 &&
                    <Toolbar
                        items={selectedKeywordsPillsData}
                        overflow
                        overflowOpen={overflowOpen}
                        overflowItem={{
                            title: localize("moreLabel"),
                        }}
                        onOverflowOpenChange={(e, toolbarProps) => {
                            if (toolbarProps) {
                                setOverflowOpen(toolbarProps.overflowOpen!);
                            }
                        }}
                        getOverflowItems={startIndex => selectedKeywordsPillsData.slice(startIndex)}
                        className="news-toolbar"
                    />
                }
                <Flex column gap="gap.small" className="news-article-container">
                    {
                        isLoading ?
                            <Loader className="tab-loader" label="Loading..." />
                            :
                            <>
                                {
                                    newsData.length > 0 ?
                                        <div key={key} className="pagination-scroll-area">
                                            <InfiniteScroll
                                                pageStart={0}
                                                initialLoad={false}
                                                loader={<Loader />}
                                                useWindow={false}
                                                loadMore={getNewsData}
                                                hasMore={hasMore}
                                            >
                                                {
                                                    <NewsArticle updateNewsItem={updateNewsItem} showUpdateRatingStatus={showUpdateRatingStatus} newsArticleData={newsData} allKeywords={allKeywords} newsSources={newsSources} />
                                                }
                                            </InfiniteScroll>
                                        </div>
                                        :
                                        <Flex column gap="gap.medium" padding="padding.medium" hAlign="center" vAlign="center" >
                                            <Image src="Artifacts/Image6.png" className="no-news-image" />
                                            <Text content={localize("noNewsErrorMsg")} className="no-news-message" />
                                        </Flex>
                                }
                            </>
                    }
                </Flex>
            </Flex>
        </>
    )
};

export default withRouter(NewsHome);