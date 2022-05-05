// <copyright file="my-requests.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import moment from "moment";
import { Segment, Button, MenuIcon, AddIcon, EditIcon, TrashCanIcon, Flex, FilterIcon, Input, SearchIcon, Divider, Menu, Table, Checkbox, TableRowProps, MenuItemProps, Skeleton, SkeletonLine, Text, ArrowUpIcon, ArrowDownIcon, Dialog, Popup, Label } from "@fluentui/react-northstar";
import { withTranslation, WithTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { getLocalizedCOIType, getLocalizedRequestStatus } from "../../helpers/localization-helper";
import NoContent from "../common/no-content/no-content";
import ICoi from "../../models/coi";
import INews from "../../models/news";
import RequestStatus from "../../models/request-status";
import { deleteRequestsAsync, getNewsArticleRequestsAsync } from "../../api/news-requests-api";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { StatusCodes } from "http-status-codes";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import RequestType from "../../models/request-type";
import StatusBar from "../common/status-bar/status-bar";
import Constants from "../../constants/constants";
import { cloneDeepCois, cloneDeepNewsArticles } from "../../helpers/clone-helper";
import IFilterItem from "../../models/filter-item";
import FilterPopup from "../common/filter-popup/filter-popup";
import { deleteCoiRequestsAsync, getCoiRequestsAsync } from "../../api/coi-requests-api";
import IKeyword from "../../models/keyword";
import SortOrder from "../../models/sort-order";
import CoiSortColumn from "../../models/coi-sort-column";
import NewsArticleSortColumn from "../../models/news-article-sort-column";
import InfiniteScroll from "react-infinite-scroller";
import AthenaSplash from "../athena-splash/athena-splash";
import { getAllKeywordsAsync } from "../../api/keyword-api";

import "./my-requests.scss";

enum MenuItems {
    CommunityOfInterests,
    NewsArticles
}

interface ICoiRequestsSortedColumn {
    key: CoiSortColumn | undefined,
    sortOrder: SortOrder
}

interface INewsArticleRequestsSortedColumn {
    key: NewsArticleSortColumn | undefined,
    sortOrder: SortOrder
}

enum TableColumnHeaderKeys {
    CheckBox = "checkbox-column-header",
    Title = "title-column-header",
    RequestedOn = "requested-on-column-header",
    Status = "status-column-header",
    COIType = "coi-type-column-header",
    Keywords = "keywords-column-header"
}

interface IMyRequestsState {
    isMenuOpen: boolean;
    activeMenuItemIndex: number;
    isEditRequestButtonEnabled: boolean;
    isDeleteRequestsButtonEnabled: boolean;
    isTableHeaderCheckboxChecked: boolean;
    tableHeader: TableRowProps;
    isLoadingCoiRequests: boolean;
    isLoadingNewsRequests: boolean;
    coiRequests: ICoi[];
    newsArticleRequests: INews[];
    searchText: string;
    coiSortedColumn: ICoiRequestsSortedColumn | undefined;
    newsSortedColumn: INewsArticleRequestsSortedColumn | undefined;
    status: IStatusBar;
    isDeletingRequests: boolean;
    selectedStatusKeysInFilter: RequestStatus[];
    selectedStatusKeysInFilterForCoi: RequestStatus[];
    selectedRequestsCount: number;
    hasMoreNewsArticlesToLoad: boolean;
    hasMoreCoiRequestsToLoad: boolean;
    searchTextForCoiTable: string;
    keywordsData: IKeyword[];
    isLoadingKeywords: boolean;
}

interface IMyRequestsProps extends WithTranslation, RouteComponentProps {
}

const SearchTimeoutInMilliseconds: number = 1800;
const CoiRequestsPerPageCount: number = 30;
const NewsArticleRequestsPerPageCount: number = 30;

class MyRequests extends React.Component<IMyRequestsProps, IMyRequestsState> {
    readonly newsRequestsTableHeader: TableRowProps = {} as TableRowProps;
    readonly menuItems: MenuItemProps[];
    readonly localize: TFunction;
    readonly statusFilterItems: IFilterItem[];

    coiRequestsTableHeader: TableRowProps = {} as TableRowProps;
    searchTimeoutId: number = -1;
    searchTimeoutIdForCoi: number = -1;
    newsScrollPageNumber: number = 0;
    coiScrollPageNumber: number = 0;

    constructor(props) {
        super(props);

        this.localize = this.props.t;

        this.menuItems = [
            { key: MenuItems.CommunityOfInterests, content: this.localize("myRequestsMenuItemCOI") } as MenuItemProps,
            { key: MenuItems.NewsArticles, content: this.localize("myRequestsMenuItemNewsArticles") } as MenuItemProps
        ];

        this.statusFilterItems = [
            { key: RequestStatus.Draft, header: getLocalizedRequestStatus(RequestStatus.Draft, this.localize), isChecked: false } as IFilterItem,
            { key: RequestStatus.Pending, header: getLocalizedRequestStatus(RequestStatus.Pending, this.localize), isChecked: false } as IFilterItem,
            { key: RequestStatus.Approved, header: getLocalizedRequestStatus(RequestStatus.Approved, this.localize), isChecked: false } as IFilterItem,
            { key: RequestStatus.Rejected, header: getLocalizedRequestStatus(RequestStatus.Rejected, this.localize), isChecked: false } as IFilterItem,
        ];

        this.state = {
            isMenuOpen: true,
            activeMenuItemIndex: MenuItems.CommunityOfInterests,
            isEditRequestButtonEnabled: false,
            isDeleteRequestsButtonEnabled: false,
            isTableHeaderCheckboxChecked: false,
            tableHeader: this.coiRequestsTableHeader,
            isLoadingCoiRequests: true,
            isLoadingNewsRequests: true,
            coiRequests: [],
            newsArticleRequests: [],
            searchText: "",
            searchTextForCoiTable: "",
            coiSortedColumn: undefined,
            newsSortedColumn: undefined,
            status: { id: 0, message: "", type: ActivityStatus.None },
            isDeletingRequests: false,
            selectedStatusKeysInFilter: [],
            selectedStatusKeysInFilterForCoi: [],
            selectedRequestsCount: 0,
            hasMoreNewsArticlesToLoad: false,
            hasMoreCoiRequestsToLoad: false,
            keywordsData: [],
            isLoadingKeywords: true
        }
    }

    // The component life cycle method get called when component is mounted.
    componentDidMount() {
        this.getCOIRequestsAsync("", 0, undefined, undefined, []);
        this.getNewsArticleRequestsAsync("", 0, undefined, undefined, []);
        this.fetchKeywords();
    }

    // Fetches all keywords.
    fetchKeywords = async () => {
        let response = await getAllKeywordsAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let keywordsData = response.data as IKeyword[];
            this.setState({ keywordsData, isLoadingKeywords: false });
        }
    }


    /**
     * Gets the COI requests.
     * @param sortColumn The column to be sorted.
     * @param sortOrder The order in which column to be sorted.
     */
    getCOIRequestsAsync = async (searchText: string, pageNumber: number, sortColumn: CoiSortColumn | undefined, sortOrder: SortOrder | undefined, statusFilter: RequestStatus[]) => {
        if (pageNumber === 0) {
            this.setState({
                isLoadingCoiRequests: true,
                hasMoreCoiRequestsToLoad: false,
                selectedRequestsCount: 0,
                isDeleteRequestsButtonEnabled: false,
                isEditRequestButtonEnabled: false
            });
        }

        this.coiScrollPageNumber = pageNumber;

        let apiResponse = await getCoiRequestsAsync(
            searchText?.trim() ?? null,
            pageNumber,
            sortColumn ?? CoiSortColumn.CreatedOn,
            sortOrder ?? SortOrder.Ascending,
            statusFilter,
            this.handleTokenAccessFailure);

        if (apiResponse && apiResponse.status === StatusCodes.OK) {
            let data = apiResponse.data as ICoi[];

            this.setState((prevState: IMyRequestsState) => ({
                isLoadingCoiRequests: false,
                hasMoreCoiRequestsToLoad: data.length >= CoiRequestsPerPageCount,
                coiRequests: pageNumber === 0 ? data : [...prevState.coiRequests, ...data],
                isTableHeaderCheckboxChecked: false,
                selectedStatusKeysInFilterForCoi: statusFilter
            }));
        }
        else {
            this.setState((prevState: IMyRequestsState) => ({
                isLoadingCoiRequests: false,
                hasMoreCoiRequestsToLoad: false,
                status: { id: prevState.status.id + 1, message: this.localize("failedToLoadCoiRequestsMessage"), type: ActivityStatus.Error }
            }));
        }
    }

    /**
     * Gets the news article requests.
     * @param sortColumn The column to be sorted.
     * @param sortOrder The sort order.
     */
    getNewsArticleRequestsAsync = async (searchText: string, pageNumber: number, sortColumn: NewsArticleSortColumn | undefined, sortOrder: SortOrder | undefined, statusFilter: RequestStatus[]) => {
        if (pageNumber === 0) {
            this.setState({
                isLoadingNewsRequests: true,
                hasMoreNewsArticlesToLoad: false,
                selectedRequestsCount: 0,
                isDeleteRequestsButtonEnabled: false,
                isEditRequestButtonEnabled: false
            });
        }

        this.newsScrollPageNumber = pageNumber;

        let apiResponse = await getNewsArticleRequestsAsync(
            searchText?.trim() ?? null,
            pageNumber,
            sortColumn ?? NewsArticleSortColumn.CreatedAt,
            sortOrder ?? SortOrder.Ascending,
            statusFilter,
            this.handleTokenAccessFailure);

        if (apiResponse && apiResponse.status === StatusCodes.OK) {
            let data = apiResponse.data as INews[];

            this.setState((prevState: IMyRequestsState) => ({
                isLoadingNewsRequests: false,
                hasMoreNewsArticlesToLoad: data.length >= NewsArticleRequestsPerPageCount,
                newsArticleRequests: pageNumber === 0 ? data : [...prevState.newsArticleRequests, ...data],
                isTableHeaderCheckboxChecked: false,
                selectedStatusKeysInFilter: statusFilter
            }));
        }
        else {
            this.setState((prevState: IMyRequestsState) => ({
                isLoadingNewsRequests: false,
                hasMoreNewsArticlesToLoad: false,
                status: { id: prevState.status.id + 1, message: this.localize("failedToLoadNewsArticleRequestsMessage"), type: ActivityStatus.Error }
            }));
        }
    }

    // Event handler called when click on menu button.
    onMenuButtonClick = () => {
        this.setState((prevState: IMyRequestsState) => ({
            isMenuOpen: !prevState.isMenuOpen
        }));
    }

    /**
     * Event handler called when menu item selection change.
     * @param event The event details.
     * @param eventData The event data.
     */
    onMenuItemSelectionChange = (event: any, eventData: any) => {
        let totalSelectableRequestsCount: number = 0;
        let selectedRequestsCount: number = 0;

        if (eventData.activeIndex === MenuItems.CommunityOfInterests) {
            totalSelectableRequestsCount = this.state.coiRequests
                .filter((coiRequest: ICoi) => coiRequest.status !== RequestStatus.Approved && coiRequest.status !== RequestStatus.Rejected)
                .length;

            selectedRequestsCount = this.state.coiRequests
                .filter((request: ICoi) => request.isChecked).length;
        }
        else {
            totalSelectableRequestsCount = this.state.newsArticleRequests
                .filter((newsRequest: INews) => newsRequest.status !== RequestStatus.Approved && newsRequest.status !== RequestStatus.Rejected)
                .length;

            selectedRequestsCount = this.state.newsArticleRequests
                .filter((request: INews) => request.isChecked).length;
        }

        this.setState({
            activeMenuItemIndex: eventData.activeIndex,
            isEditRequestButtonEnabled: selectedRequestsCount === 1,
            isDeleteRequestsButtonEnabled: selectedRequestsCount > 0,
            isTableHeaderCheckboxChecked: totalSelectableRequestsCount > 0 && totalSelectableRequestsCount === selectedRequestsCount,
            selectedRequestsCount
        });
    }

    /**
     * Event handler called when COI request get checked or unchecked.
     * @param eventDetails The event details.
     * @param coiTableId The COI Id.
     * @param isChecked Indicates whether request is checked.
     */
    onCOIRequestCheckChanged = (eventDetails: any, coiTableId: string, isChecked: boolean) => {
        eventDetails.stopPropagation();

        let coiRequests: ICoi[] = cloneDeepCois(this.state.coiRequests);

        let coiRequestIndex: number = coiRequests.findIndex((request: ICoi) => request.tableId === coiTableId);
        coiRequests[coiRequestIndex].isChecked = isChecked;

        let isTableHeaderCheckboxChecked: boolean = this.isHeaderCheckboxChecked(coiRequests, RequestType.CommunityOfInterest);

        let selectedRequestsCount: number = coiRequests
            .filter((request: ICoi) => request.isChecked).length;

        this.setState({
            coiRequests,
            isEditRequestButtonEnabled: selectedRequestsCount === 1,
            isDeleteRequestsButtonEnabled: selectedRequestsCount > 0,
            isTableHeaderCheckboxChecked,
            selectedRequestsCount
        });
    }

    /**
     * Event handler called when news article request get checked or unchecked.
     * @param eventDetails The event details.
     * @param newsTableId The news article table Id.
     * @param isChecked Indicates whether request is checked.
     */
    onNewsRequestCheckChanged = (eventDetails: any, newsTableId: string, isChecked: boolean) => {
        eventDetails.stopPropagation();

        let newsRequests: INews[] = cloneDeepNewsArticles(this.state.newsArticleRequests);

        let newsRequestIndex: number = newsRequests.findIndex((request: INews) => request.tableId === newsTableId);
        newsRequests[newsRequestIndex].isChecked = isChecked;

        let isTableHeaderCheckboxChecked: boolean = this.isHeaderCheckboxChecked(newsRequests, RequestType.News);

        let selectedRequestsCount: number = newsRequests
            .filter((request: INews) => request.isChecked).length;

        this.setState({
            newsArticleRequests: newsRequests,
            isEditRequestButtonEnabled: selectedRequestsCount === 1,
            isDeleteRequestsButtonEnabled: selectedRequestsCount > 0,
            isTableHeaderCheckboxChecked,
            selectedRequestsCount
        });
    }

    /**
     * Event handler called when table header checkbox value change.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onHeaderCheckboxCheckedChange = (eventDetails: any, eventData: any) => {
        let checkedRequestsCount: number = 0;

        if (this.state.activeMenuItemIndex === MenuItems.CommunityOfInterests) {
            let coiRequests: ICoi[] = cloneDeepCois(this.state.coiRequests);

            for (let i = 0; i < coiRequests.length; i++) {
                coiRequests[i].isChecked = coiRequests[i].status !== RequestStatus.Approved
                    && coiRequests[i].status !== RequestStatus.Rejected && eventData.checked;

                checkedRequestsCount += coiRequests[i].isChecked ? 1 : 0;
            }

            this.setState({
                coiRequests,
                isEditRequestButtonEnabled: eventData.checked && checkedRequestsCount === 1,
                isDeleteRequestsButtonEnabled: checkedRequestsCount > 0 && eventData.checked,
                isTableHeaderCheckboxChecked: eventData.checked,
                selectedRequestsCount: checkedRequestsCount
            });
        }
        else {
            let newsArticleRequests: INews[] = cloneDeepNewsArticles(this.state.newsArticleRequests);

            for (let i = 0; i < newsArticleRequests.length; i++) {
                newsArticleRequests[i].isChecked = newsArticleRequests[i].status !== RequestStatus.Approved
                    && newsArticleRequests[i].status !== RequestStatus.Rejected && eventData.checked;

                checkedRequestsCount += newsArticleRequests[i].isChecked ? 1 : 0;
            }

            this.setState({
                newsArticleRequests,
                isEditRequestButtonEnabled: eventData.checked && checkedRequestsCount === 1,
                isDeleteRequestsButtonEnabled: checkedRequestsCount > 0 && eventData.checked,
                isTableHeaderCheckboxChecked: eventData.checked,
                selectedRequestsCount: checkedRequestsCount
            });
        }
    }

    /**
     * Event handler called when search text get changed. This searches table with specific search string
     * and update the table accordingly.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onSearchTextChange = (eventDetails: any, eventData: any) => {
        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            this.setState({ searchText: eventData.value });

            if (this.searchTimeoutId) {
                clearTimeout(this.searchTimeoutId);
            }

            this.searchTimeoutId = window.setTimeout(() => {
                this.getNewsArticleRequestsAsync(
                    eventData.value,
                    0,
                    this.state.newsSortedColumn?.key,
                    this.state.newsSortedColumn?.sortOrder,
                    this.state.selectedStatusKeysInFilter
                );
            }, SearchTimeoutInMilliseconds);
        }
        else {
            this.setState({ searchTextForCoiTable: eventData.value });

            if (this.searchTimeoutIdForCoi) {
                clearTimeout(this.searchTimeoutIdForCoi);
            }

            this.searchTimeoutIdForCoi = window.setTimeout(() => {
                this.getCOIRequestsAsync(
                    eventData.value,
                    0,
                    this.state.coiSortedColumn?.key,
                    this.state.coiSortedColumn?.sortOrder,
                    this.state.selectedStatusKeysInFilterForCoi
                );
            }, SearchTimeoutInMilliseconds);
        }
    }

    /**
     * Event handler called when click on COI table header.
     * @param key The key of table column which is clicked.
     */
    onCoiTableHeaderItemClick = (key: CoiSortColumn) => {
        if (this.state.isLoadingCoiRequests) {
            return;
        }

        if (this.state.coiSortedColumn && this.state.coiSortedColumn.key === key) {
            let sortOrder: SortOrder = this.state.coiSortedColumn.sortOrder === SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            this.setState({ coiSortedColumn: { key, sortOrder } });
            this.getCOIRequestsAsync(this.state.searchTextForCoiTable, 0, key, sortOrder, this.state.selectedStatusKeysInFilterForCoi);
        }
        else {
            this.setState({ coiSortedColumn: { key, sortOrder: SortOrder.Ascending } });
            this.getCOIRequestsAsync(this.state.searchTextForCoiTable, 0, key, SortOrder.Ascending, this.state.selectedStatusKeysInFilterForCoi);
        }
    }

    /**
     * Event handler called when click on news article requests table header.
     * @param key The key of table column which is clicked.
     */
    onNewsTableHeaderItemClick = (key: NewsArticleSortColumn) => {
        if (this.state.isLoadingNewsRequests) {
            return;
        }

        if (this.state.newsSortedColumn && this.state.newsSortedColumn.key === key) {
            let sortOrder: SortOrder = this.state.newsSortedColumn.sortOrder === SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            this.setState({ newsSortedColumn: { key, sortOrder } });
            this.getNewsArticleRequestsAsync(this.state.searchText, 0, key, sortOrder, this.state.selectedStatusKeysInFilter);
        }
        else {
            this.setState({ newsSortedColumn: { key, sortOrder: SortOrder.Ascending } });
            this.getNewsArticleRequestsAsync(this.state.searchText, 0, key, SortOrder.Ascending, this.state.selectedStatusKeysInFilter);
        }
    }

    // Event handler called click on 'New request' button.
    onNewRequestButtonClick = () => {
        microsoftTeams.tasks.startTask({
            title: this.localize("createRequestTaskModuleTitle"),
            width: 600,
            height: 600,
            url: `${window.location.origin}/new-request`
        } as microsoftTeams.TaskInfo, (error: string, result: any) => {
            if (result && result.data) {
                if (result.isCreatedAsDraft) {
                    this.onNewRequestCreate(result.data, result.type, true);
                }
                else {
                    this.onNewRequestCreate(result.data, result.type, false);
                }
            }
        });
    }

    /**
     * Event handler called when click on news requests table row.
     * @param newsTableId The news table Id of which details to see.
     */
    onNewsRequestClick = (newsTableId: string) => {
        microsoftTeams.tasks.startTask({
            title: this.localize("requestDetailsTaskModule"),
            width: 600,
            height: 600,
            url: `${window.location.origin}/new-request?${Constants.UrlParamRequestIdToEditOrDeleteRequest}=${newsTableId}&${Constants.UrlParamIsReadonlyToEditOrDeleteRequest}=true&${Constants.UrlParamRequestType}=${RequestType.News}`
        } as microsoftTeams.TaskInfo, (error: string, result: any) => {
            if (result && result.data) {
                if (result.isUpdated) {
                    this.onRequestUpdate(RequestType.News, false);
                }
                else if (result.isCreatedNewRequest) {
                    this.onRequestUpdate(RequestType.News, true);
                }
                else if (result.isDeleted) {
                    this.onRequestsDeleted(RequestType.News);
                }
            }
        });
    }

    /**
     * Event handler called when click on COI requests table row.
     * @param coiTableId The COI Table Id of which details to see.
     */
    onCoiRequestClick = (coiTableId: string) => {
        microsoftTeams.tasks.startTask({
            title: this.localize("requestDetailsTaskModule"),
            width: 600,
            height: 600,
            url: `${window.location.origin}/new-request?${Constants.UrlParamRequestIdToEditOrDeleteRequest}=${coiTableId}&${Constants.UrlParamIsReadonlyToEditOrDeleteRequest}=true&${Constants.UrlParamRequestType}=${RequestType.CommunityOfInterest}`
        } as microsoftTeams.TaskInfo, (error: string, result: any) => {
            if (result && result.data) {
                if (result.isUpdated) {
                    this.onRequestUpdate(RequestType.CommunityOfInterest, false);
                }
                else if (result.isCreatedNewRequest) {
                    this.onRequestUpdate(RequestType.CommunityOfInterest, true);
                }
                else if (result.isDeleted) {
                    this.onRequestsDeleted(RequestType.CommunityOfInterest);
                }
            }
        });
    }

    /** Event handler called when to edit selected request. */
    onEditRequestButtonClick = () => {
        let requestId: string | undefined = undefined;

        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            let selectedRequest: INews | undefined = this.state.newsArticleRequests
                .find((newsArticleRequest: INews) => newsArticleRequest.isChecked && newsArticleRequest.status === RequestStatus.Draft);

            if (selectedRequest) {
                requestId = selectedRequest.tableId;
            }
        }
        else {
            let selectedRequest: ICoi | undefined = this.state.coiRequests
                .find((coiRequest: ICoi) => coiRequest.isChecked && coiRequest.status === RequestStatus.Draft);

            if (selectedRequest) {
                requestId = selectedRequest.tableId;
            }
        }

        if (!requestId) {
            this.setState((prevState: IMyRequestsState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("onlyDraftedRequestsEditError"), type: ActivityStatus.Error }
            }));

            return;
        }

        microsoftTeams.tasks.startTask({
            title: this.state.activeMenuItemIndex === MenuItems.NewsArticles ? this.localize("editNewsArticleTaskModuleTitle") : this.localize("editCOIRequestTaskModuleTitle"),
            width: 600,
            height: 600,
            url: `${window.location.origin}/new-request?${Constants.UrlParamRequestIdToEditOrDeleteRequest}=${requestId}&${Constants.UrlParamRequestType}=${this.state.activeMenuItemIndex === MenuItems.NewsArticles ? RequestType.News : RequestType.CommunityOfInterest}`
        } as microsoftTeams.TaskInfo, (error: any, result: any) => {
            if (result && result.data) {
                if (result.isUpdated) {
                    this.onRequestUpdate(result.type, false);
                }
                else if (result.isCreatedNewRequest) {
                    this.onRequestUpdate(result.type, true);
                }
            }
        });
    }

    // Event handler called when click on confirm dialog button while deleting requests.
    onConfirmDialogButtonClick = async () => {
        this.setState({ isDeletingRequests: true });

        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            let requestIdsToDelete: string[] = this.state.newsArticleRequests
                .filter((newsArticleRequest: INews) => newsArticleRequest.isChecked === true
                    && (newsArticleRequest.status === RequestStatus.Draft || newsArticleRequest.status === RequestStatus.Pending))
                .map((newsArticleRequest: INews) => newsArticleRequest.tableId);

            var apiResponse = await deleteRequestsAsync(requestIdsToDelete, this.handleTokenAccessFailure);

            if (apiResponse && apiResponse.status === StatusCodes.OK) {
                this.onRequestsDeleted(RequestType.News);
            }
            else {
                this.setState((prevState: IMyRequestsState) => ({
                    isDeletingRequests: false,
                    status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
                }));
            }
        }
        else {
            let requestIdsToDelete: string[] = this.state.coiRequests
                .filter((coiRequest: ICoi) => coiRequest.isChecked === true
                    && (coiRequest.status === RequestStatus.Draft || coiRequest.status === RequestStatus.Pending))
                .map((coiRequest: ICoi) => coiRequest.tableId);

            var apiResponse = await deleteCoiRequestsAsync(requestIdsToDelete, this.handleTokenAccessFailure);

            if (apiResponse && apiResponse.status === StatusCodes.OK) {
                this.onRequestsDeleted(RequestType.CommunityOfInterest);
            }
            else {
                this.setState((prevState: IMyRequestsState) => ({
                    isDeletingRequests: false,
                    status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
                }));
            }
        }
    }

    /**
     * Event handler called when status filter get changed.
     * @param updatedStatusKeysInFilter The selected status keys.
     */
    onStatusFilterChange = (updatedStatusKeysInFilter: any[]) => {
        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            this.setState({ selectedStatusKeysInFilter: updatedStatusKeysInFilter });

            this.getNewsArticleRequestsAsync(
                this.state.searchText,
                0,
                this.state.newsSortedColumn?.key,
                this.state.newsSortedColumn?.sortOrder,
                updatedStatusKeysInFilter
            );
        }
        else {
            this.setState({ selectedStatusKeysInFilterForCoi: updatedStatusKeysInFilter })

            this.getCOIRequestsAsync(
                this.state.searchTextForCoiTable,
                0,
                this.state.coiSortedColumn?.key,
                this.state.coiSortedColumn?.sortOrder,
                updatedStatusKeysInFilter
            );
        }
    }

    /**
     * Removes requests from table based on request type.
     * @param requestType The request type.
     */
    onRequestsDeleted = (requestType: RequestType) => {
        if (requestType === RequestType.News) {
            this.setState((prevState: IMyRequestsState) => ({
                isDeletingRequests: false,
                status: { id: prevState.status.id + 1, message: this.localize("newsRequestsDeletedSuccessfully"), type: ActivityStatus.Success }
            }));

            this.getNewsArticleRequestsAsync(
                "",
                0,
                this.state.newsSortedColumn?.key,
                this.state.newsSortedColumn?.sortOrder,
                []
            );
        }
        else {
            this.setState((prevState: IMyRequestsState) => ({
                isDeletingRequests: false,
                status: { id: prevState.status.id + 1, message: this.localize("coiRequestsDeletedSuccessfully"), type: ActivityStatus.Success }
            }));

            this.getCOIRequestsAsync(
                "",
                0,
                this.state.coiSortedColumn?.key,
                this.state.coiSortedColumn?.sortOrder,
                []
            );
        }
    }

    /**
     * Adds a newly created request in table.
     * @param request The request details to add.
     * @param requestType The request type.
     * @param isDraft Indicates whether a request is draft one.
     */
    onNewRequestCreate = (request: any, requestType: RequestType, isDraft: boolean) => {
        if (requestType === RequestType.News) {
            this.setState((prevState: IMyRequestsState) => ({
                status: {
                    id: prevState.status.id + 1,
                    message: isDraft ? this.localize("requestSavedAsDraftSuccessfully") : this.localize("newsArticleRequestCreated"),
                    type: ActivityStatus.Success
                }
            }));

            this.getNewsArticleRequestsAsync(
                "",
                0,
                this.state.newsSortedColumn?.key,
                this.state.newsSortedColumn?.sortOrder,
                []
            );
        }
        else {
            this.setState((prevState: IMyRequestsState) => ({
                status: {
                    id: prevState.status.id + 1,
                    message: isDraft ? this.localize("requestSavedAsDraftSuccessfully") : this.localize("coiRequestCreated"),
                    type: ActivityStatus.Success
                }
            }));

            this.getCOIRequestsAsync(
                "",
                0,
                this.state.coiSortedColumn?.key,
                this.state.coiSortedColumn?.sortOrder,
                []
            );
        }
    }

    /**
     * Updates a draft request in table.
     * @param requestType The request type.
     * @param isSubmitted Indicated whether a drafted request submitted.
     */
    onRequestUpdate = (requestType: RequestType, isSubmitted: boolean) => {
        if (requestType === RequestType.News) {
            this.setState((prevState: IMyRequestsState) => ({
                status: {
                    id: prevState.status.id + 1,
                    message: isSubmitted ? this.localize("newsArticleRequestCreated") : this.localize("newsRequestUpdatedSuccessfully"),
                    type: ActivityStatus.Success
                }
            }));

            this.getNewsArticleRequestsAsync(
                "",
                0,
                this.state.newsSortedColumn?.key,
                this.state.newsSortedColumn?.sortOrder,
                []);
        }
        else {
            this.setState((prevState: IMyRequestsState) => ({
                status: {
                    id: prevState.status.id + 1,
                    message: isSubmitted ? this.localize("coiRequestCreated") : this.localize("coiRequestUpdatedSuccessfully"),
                    type: ActivityStatus.Success
                }
            }));

            this.getCOIRequestsAsync(
                "",
                0,
                this.state.coiSortedColumn?.key,
                this.state.coiSortedColumn?.sortOrder,
                []);
        }
    }

    // Indicates whether header checkbox is checked.
    isHeaderCheckboxChecked = (requests: any, requestType: RequestType, searchText: string = ""): boolean => {
        if (!requests) {
            return false;
        }

        let requestsToBeValidated = requests
            .filter((request: INews | ICoi) => request.status !== RequestStatus.Approved && request.status !== RequestStatus.Rejected);

        return requestsToBeValidated.length > 0 && !requestsToBeValidated
            .some((request: any) => !request.isChecked);
    }

    /**
     * Loads the requests based on page number.
     * @param pageNumber The page number for which data to load.
     */
    loadMoreRequests = (pageNumber: number) => {
        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            this.getNewsArticleRequestsAsync(
                this.state.searchText,
                pageNumber,
                this.state.newsSortedColumn?.key,
                this.state.newsSortedColumn?.sortOrder,
                this.state.selectedStatusKeysInFilter
            );
        }
        else {
            this.getCOIRequestsAsync(
                this.state.searchTextForCoiTable,
                pageNumber,
                this.state.coiSortedColumn?.key,
                this.state.coiSortedColumn?.sortOrder,
                this.state.selectedStatusKeysInFilterForCoi
            );
        }
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    handleTokenAccessFailure = (error: string) => {
        this.props.history.push("/signin");
    }

    /**
     * Gets the COI table header item.
     * @param key The column key.
     * @param title The column title.
     */
    getCoiTableHeaderItemContent = (key: CoiSortColumn, title: string) => {
        return <Flex vAlign="center" gap="gap.small">
            <Text content={title} />
            {this.state.coiSortedColumn?.key === key ? this.state.coiSortedColumn.sortOrder === SortOrder.Ascending ? <ArrowUpIcon size="small" /> : <ArrowDownIcon size="small" /> : ""}
        </Flex>
    }

    /**
     * Gets the news table header item.
     * @param key The column key.
     * @param title The column title.
     */
    getNewsTableHeaderItemContent = (key: NewsArticleSortColumn, title: string) => {
        return <Flex vAlign="center" gap="gap.small">
            <Text content={title} />
            {this.state.newsSortedColumn?.key === key ? this.state.newsSortedColumn.sortOrder === SortOrder.Ascending ? <ArrowUpIcon size="small" /> : <ArrowDownIcon size="small" /> : ""}
        </Flex>
    }

    // Prepares and returns table header based on request type.
    getTableHeader = (): JSX.Element => {
        if (this.state.activeMenuItemIndex === MenuItems.CommunityOfInterests) {
            return <Table.Row className="header row" header>
                <Table.Cell className="checkbox-column-header" key={TableColumnHeaderKeys.CheckBox} content={<Checkbox disabled={this.state.isLoadingCoiRequests} onChange={this.onHeaderCheckboxCheckedChange} checked={this.state.isTableHeaderCheckboxChecked} />} />
                <Table.Cell className="title-column-header" key={TableColumnHeaderKeys.Title} content={this.getCoiTableHeaderItemContent(CoiSortColumn.Name, this.localize("myRequestsTitleColumn"))} onClick={() => this.onCoiTableHeaderItemClick(CoiSortColumn.Name)} />
                <Table.Cell className="requested-on-column-header" key={TableColumnHeaderKeys.RequestedOn} content={this.getCoiTableHeaderItemContent(CoiSortColumn.CreatedOn, this.localize("myRequestsRequestedOnColumn"))} onClick={() => this.onCoiTableHeaderItemClick(CoiSortColumn.CreatedOn)} />
                <Table.Cell key={TableColumnHeaderKeys.Status} content={this.getCoiTableHeaderItemContent(CoiSortColumn.Status, this.localize("myRequestsStatusColumn"))} onClick={() => this.onCoiTableHeaderItemClick(CoiSortColumn.Status)} />
                <Table.Cell key={TableColumnHeaderKeys.COIType} content={this.getCoiTableHeaderItemContent(CoiSortColumn.Type, this.localize("myRequestsCOITypeColumn"))} onClick={() => this.onCoiTableHeaderItemClick(CoiSortColumn.Type)} />
                <Table.Cell className="keywords-column-header" key={TableColumnHeaderKeys.Keywords} content={this.localize("myRequestsKeywordsColumn")} />
            </Table.Row>;
        }

        return <Table.Row className="header row" header>
            <Table.Cell className="checkbox-column-header" key={TableColumnHeaderKeys.CheckBox} content={<Checkbox disabled={this.state.isLoadingNewsRequests} onChange={this.onHeaderCheckboxCheckedChange} checked={this.state.isTableHeaderCheckboxChecked} />} />
            <Table.Cell className="title-column-header" key={TableColumnHeaderKeys.Title} content={this.getNewsTableHeaderItemContent(NewsArticleSortColumn.Title, this.localize("myRequestsTitleColumn"))} onClick={() => this.onNewsTableHeaderItemClick(NewsArticleSortColumn.Title)} />
            <Table.Cell className="requested-on-column-header" key={TableColumnHeaderKeys.RequestedOn} content={this.getNewsTableHeaderItemContent(NewsArticleSortColumn.CreatedAt, this.localize("myRequestsRequestedOnColumn"))} onClick={() => this.onNewsTableHeaderItemClick(NewsArticleSortColumn.CreatedAt)} />
            <Table.Cell key={TableColumnHeaderKeys.Status} content={this.getNewsTableHeaderItemContent(NewsArticleSortColumn.Status, this.localize("myRequestsStatusColumn"))} onClick={() => this.onNewsTableHeaderItemClick(NewsArticleSortColumn.Status)} />
            <Table.Cell className="keywords-column-header" key={TableColumnHeaderKeys.Keywords} content={this.localize("myRequestsKeywordsColumn")} />
        </Table.Row>;
    }

    // Gets the loading skeleton for table.
    getSkeletonOfTableRows = () => {
        let tableSkeletonRow = <Table.Row className="row">
            <Table.Cell className="checkbox-column-header" content={<Skeleton animation="wave"><SkeletonLine width="1.8rem" height="1.4rem" /></Skeleton>} />
            <Table.Cell className="title-column-header" content={<Skeleton animation="wave"><SkeletonLine width="25rem" height="1.4rem" /></Skeleton>} />
            <Table.Cell className="requested-on-column-header" content={<Skeleton animation="wave"><SkeletonLine width="13rem" height="1.4rem" /></Skeleton>} />
            <Table.Cell content={<Skeleton animation="wave"><SkeletonLine width="9rem" height="1.4rem" /></Skeleton>} />
            {this.state.activeMenuItemIndex === MenuItems.CommunityOfInterests ? <Table.Cell content={<Skeleton animation="wave"><SkeletonLine width="9rem" height="1.4rem" /></Skeleton>} /> : null}
            <Table.Cell className="keywords-column-header" content={<Skeleton animation="wave"><SkeletonLine width="16rem" height="1.4rem" /></Skeleton>} />
        </Table.Row>

        return [tableSkeletonRow, tableSkeletonRow, tableSkeletonRow, tableSkeletonRow];
    }

    // Gets the table rows based on request type.
    getTableRows = (): JSX.Element[] => {
        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            if (this.state.isLoadingNewsRequests) {
                return this.getSkeletonOfTableRows();
            }

            return this.getNewsArticleTableRows();
        }

        if (this.state.isLoadingCoiRequests) {
            return this.getSkeletonOfTableRows();
        }

        return this.getCOIRequestsTableRows();
    }

    // Gets the table rows for COI requests.
    getCOIRequestsTableRows = (): JSX.Element[] => {
        return this.state.coiRequests.map((request: ICoi) => {
            let keywords: string = this.getKeywords(request.keywords);

            return <Table.Row className="row" onClick={() => this.onCoiRequestClick(request.tableId)}>
                <Table.Cell className="checkbox-column-header" content={<Checkbox disabled={request.status === RequestStatus.Approved || request.status === RequestStatus.Rejected} checked={request.isChecked} onChange={(event, data) => this.onCOIRequestCheckChanged(event, request.tableId, data?.checked ?? false)} />} />
                <Table.Cell className="title-column-header" content={request.coiName} title={request.coiName} truncateContent={true} />
                <Table.Cell className="requested-on-column-header" content={request.createdOn ? moment(request.createdOn).format("DD-MMM-YYYY hh:mm A") : "NA"} truncateContent={true} />
                <Table.Cell content={getLocalizedRequestStatus(request.status, this.localize)} truncateContent={true} />
                <Table.Cell content={getLocalizedCOIType(request.type, this.localize)} truncateContent={true} />
                <Table.Cell className="keywords-column-header" content={keywords} title={keywords} truncateContent={true} />
            </Table.Row>
        });
    }

    // Gets the filter count string in '(1)' format. If the applied filter count is 0, then empty string will be returned.
    getFilterCount = () => {
        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            if (this.state.selectedStatusKeysInFilter?.length) {
                return `(${this.state.selectedStatusKeysInFilter.length})`;
            }
        }
        else if (this.state.selectedStatusKeysInFilterForCoi?.length) {
            return `(${this.state.selectedStatusKeysInFilterForCoi.length})`;
        }

        return "";
    }

    /**
     * Gets the comma separated keywords string.
     * @param keywords The keywords.
     */
    getKeywords = (keywords: number[]): string => {
        if (this.state.isLoadingKeywords) {
            return this.localize("loadingLabel");
        }

        let keywordIdsStringArray = keywords.map(String);

        let keywordsTitleArray: string[] = this.state.keywordsData
            .filter((keyword: IKeyword) => keywordIdsStringArray.includes(keyword.keywordId))
            .map((keyword: IKeyword) => keyword.title);

        return keywordsTitleArray.length ? keywordsTitleArray.join(", ") : "NA";
    }

    // Gets the news article requests table rows.
    getNewsArticleTableRows = (): JSX.Element[] => {
        return this.state.newsArticleRequests.map((request: INews) => {
            let keywords: string = this.getKeywords(request.keywords);

            return <Table.Row className="row" onClick={() => this.onNewsRequestClick(request.tableId)}>
                <Table.Cell className="checkbox-column-header" content={<Checkbox disabled={request.status === RequestStatus.Approved || request.status === RequestStatus.Rejected} checked={request.isChecked} onChange={(event, data) => this.onNewsRequestCheckChanged(event, request.tableId, data?.checked ?? false)} />} />
                <Table.Cell className="title-column-header" content={request.title} title={request.title} truncateContent={true} />
                <Table.Cell className="requested-on-column-header" content={moment(request.createdAt).format("DD-MMM-YYYY hh:mm A")} truncateContent={true} />
                <Table.Cell content={getLocalizedRequestStatus(request.status, this.localize)} truncateContent={true} />
                <Table.Cell className="keywords-column-header" content={keywords} title={keywords} truncateContent={true} />
            </Table.Row>
        });
    }

    renderTable = () => {
        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            return <Flex.Item grow>
                <Flex column>
                    <Table key="news-table" className="requests-table">
                        {this.getTableHeader()}
                        <InfiniteScroll
                            pageStart={this.newsScrollPageNumber}
                            hasMore={this.state.hasMoreNewsArticlesToLoad}
                            useWindow={false}
                            initialLoad={false}
                            loader={this.getSkeletonOfTableRows()}
                            loadMore={this.loadMoreRequests}
                        >
                            {this.getTableRows()}
                        </InfiniteScroll>
                    </Table>
                </Flex>
            </Flex.Item>;
        }

        return <Flex.Item grow>
            <Flex column>
                <Table key="coi-table" className="requests-table">
                    {this.getTableHeader()}
                    <InfiniteScroll
                        pageStart={this.coiScrollPageNumber}
                        hasMore={this.state.hasMoreCoiRequestsToLoad}
                        useWindow={false}
                        initialLoad={false}
                        loader={this.getSkeletonOfTableRows()}
                        loadMore={this.loadMoreRequests}
                    >
                        {this.getTableRows()}
                    </InfiniteScroll>
                </Table>
            </Flex>
        </Flex.Item>;
    }

    // Renders the table and details associated with the selected menu.
    renderMenuDetails = () => {
        if ((this.state.activeMenuItemIndex === MenuItems.CommunityOfInterests && !this.state.coiRequests?.length && !this.state.isLoadingCoiRequests)
            || (this.state.activeMenuItemIndex === MenuItems.NewsArticles && !this.state.newsArticleRequests?.length && !this.state.isLoadingNewsRequests)) {
            return <NoContent
                message={(this.state.activeMenuItemIndex === MenuItems.NewsArticles && (this.state.selectedStatusKeysInFilter?.length > 0 || this.state.searchText?.trim().length > 0))
                    || (this.state.activeMenuItemIndex === MenuItems.CommunityOfInterests && (this.state.searchTextForCoiTable?.trim().length > 0 || this.state.selectedStatusKeysInFilterForCoi?.length > 0)) ? this.localize("noRequestsExistsSearchMessage") : this.localize("myRequestsNoRequestsFoundMessage")}
            />;
        }

        return <Flex.Item grow>
            <Flex className="menu-details" column>
                {this.state.selectedRequestsCount > 0 &&
                    <Label
                        className="menu-details-status"
                        content={
                            <Text content={this.localize("myRequestsFooterStatusBar", { selectedRequestsCount: this.state.selectedRequestsCount })} weight="semibold" />}
                    />}
                {
                    this.renderTable()
                }
            </Flex>
        </Flex.Item>;
    }

    // Renders component.
    render() {
        return (
            <Flex className="my-requests" column fill>
                <StatusBar status={this.state.status} isMobile={false} />
                <AthenaSplash description={this.localize("requestsTabDescription")} heading={this.localize("requestsTabHeading")} />
                <Segment
                    className="toolbar-box"
                    content={<Flex vAlign="center">
                        <Button text iconOnly primary={this.state.isMenuOpen} icon={<MenuIcon />} onClick={this.onMenuButtonClick} />
                        <Button text disabled={this.state.isDeletingRequests} icon={<AddIcon />} content={this.localize("myRequestsNewRequestButtonContent")} onClick={this.onNewRequestButtonClick} />
                        <Button text icon={<EditIcon />} content={this.localize("editButtonContent")} disabled={!this.state.isEditRequestButtonEnabled || this.state.isDeletingRequests} onClick={this.onEditRequestButtonClick} />
                        <Dialog
                            cancelButton={this.localize("cancelButtonContent")}
                            confirmButton={this.localize("okButtonContent")}
                            header={this.localize("deleteRequestsDialogHeader")}
                            content={this.localize("deleteRequestsDialogContent")}
                            onConfirm={this.onConfirmDialogButtonClick}
                            trigger={<Button text loading={this.state.isDeletingRequests} icon={<TrashCanIcon />} content={this.localize("deleteButtonContent")} disabled={!this.state.isDeleteRequestsButtonEnabled || this.state.isDeletingRequests} />}
                        />
                        <Flex.Item push>
                            <Flex>
                                <Popup
                                    trigger={<Button
                                        text
                                        primary={((this.state.activeMenuItemIndex === MenuItems.NewsArticles && this.state.selectedStatusKeysInFilter?.length !== 0)
                                            || (this.state.activeMenuItemIndex === MenuItems.CommunityOfInterests && this.state.selectedStatusKeysInFilterForCoi?.length !== 0)) ?? false}
                                        disabled={this.state.isDeletingRequests}
                                        icon={<FilterIcon />}
                                        content={`${this.localize("filterButtonContent")} ${this.getFilterCount()}`}
                                    />}
                                    content={<FilterPopup
                                        title={this.localize("myRequestsStatusColumn")}
                                        clearText={this.localize("clearText")}
                                        items={this.statusFilterItems}
                                        disabled={this.state.activeMenuItemIndex === MenuItems.NewsArticles ? this.state.isLoadingNewsRequests : this.state.isLoadingCoiRequests}
                                        selectedFilterItemKeys={this.state.activeMenuItemIndex === MenuItems.NewsArticles ? this.state.selectedStatusKeysInFilter : this.state.selectedStatusKeysInFilterForCoi}
                                        onCheckedChange={(updatedStatusKeysInFilter) => this.onStatusFilterChange(updatedStatusKeysInFilter)}
                                    />}
                                    position="below"
                                    align="end"
                                />
                                <Input inverted icon={<SearchIcon />} placeholder={this.localize("myRequestsFindInputPlaceholder")} type="text" value={this.state.activeMenuItemIndex === MenuItems.NewsArticles ? this.state.searchText : this.state.searchTextForCoiTable} onChange={this.onSearchTextChange} />
                            </Flex>
                        </Flex.Item>
                    </Flex>
                    }
                />
                <Flex className="container" fill>
                    {
                        this.state.isMenuOpen && <Flex className="menu-container">
                            <Menu className="menu-items"
                                vertical
                                pointing
                                items={this.menuItems}
                                activeIndex={this.state.activeMenuItemIndex}
                                defaultActiveIndex={MenuItems.CommunityOfInterests}
                                onActiveIndexChange={this.onMenuItemSelectionChange} />
                            <Flex.Item push>
                                <Divider className="divider" vertical />
                            </Flex.Item>
                        </Flex>
                    }
                    {this.renderMenuDetails()}
                </Flex>
            </Flex>
        );
    }
}

export default withTranslation()(withRouter(MyRequests));