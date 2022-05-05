// <copyright file="admin-requests.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import moment from "moment";
import { Segment, Button, AcceptIcon, CloseIcon, TableRowProps, TextArea, MenuItemProps, Checkbox, Table, Skeleton, SkeletonLine, ArrowUpIcon, Text, ArrowDownIcon, MenuIcon, Label, Flex, FilterIcon, Input, SearchIcon, Divider, Menu, Dialog, Popup } from "@fluentui/react-northstar";
import { withTranslation, WithTranslation } from "react-i18next";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { TFunction } from "i18next";
import StatusBar from "../common/status-bar/status-bar";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import RequestStatus from "../../models/request-status";
import IFilterItem from "../../models/filter-item";
import { getLocalizedCOIType, getLocalizedRequestStatus } from "../../helpers/localization-helper";
import FilterPopup from "../common/filter-popup/filter-popup";
import INews from "../../models/news";
import ICoi from "../../models/coi";
import { StatusCodes } from "http-status-codes";
import RequestType from "../../models/request-type";
import CoiSortColumn from "../../models/coi-sort-column";
import NewsArticleSortColumn from "../../models/news-article-sort-column";
import SortOrder from "../../models/sort-order";
import { cloneDeepCois, cloneDeepNewsArticles } from "../../helpers/clone-helper";
import Constants from "../../constants/constants";
import NoContent from "../common/no-content/no-content";
import InfiniteScroll from "react-infinite-scroller";
import { getAllCoiRequestAsync, getAllNewsRequestAsync } from "../../api/request-tab-api";
import AthenaSplash from "../athena-splash/athena-splash";
import { approveCoiRequests, approveNewsArticleRequests, rejectCoiRequests, rejectNewsArticleRequests } from "../../api/request-tab-api";

import "./admin-requests.scss";

const SearchTimeoutInMilliseconds: number = 1800;
const CoiRequestsPerPageCount: number = 30;
const NewsArticleRequestsPerPageCount: number = 30;

interface IAdminRequestsProps extends WithTranslation, RouteComponentProps {
}

interface IAdminRequestsState {
    isApprovingRequests: boolean,
    isRejectingRequests: boolean,
    statusBar: IStatusBar,
    isApproveRequestsButtonEnabled: boolean,
    isRejectRequestsButtonEnabled: boolean,
    activeMenuItemIndex: number,
    coiSearchText: string,
    newsSearchText: string,
    isMenuOpen: boolean,
    selectedStatusKeysInFilterForCoi: RequestStatus[],
    selectedStatusKeysInFilterForNews: RequestStatus[],
    coiRequests: ICoi[],
    newsArticleRequests: INews[],
    selectedRequestsCount: number,
    isTableHeaderCheckboxChecked: boolean,
    isLoadingCoiRequests: boolean,
    isLoadingNewsRequests: boolean,
    coiSortedColumn: ICoiRequestsSortedColumn | undefined,
    newsSortedColumn: INewsArticleRequestsSortedColumn | undefined,
    hasMoreNewsArticlesToLoad: boolean,
    hasMoreCoiRequestsToLoad: boolean,
    isConfirmationPortalOpen: boolean,
    rejectionComment: string
}

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
    RequestedBy = "requested-by-column-header"
}

class AdminRequests extends React.Component<IAdminRequestsProps, IAdminRequestsState> {
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
            { key: RequestStatus.Pending, header: getLocalizedRequestStatus(RequestStatus.Pending, this.localize), isChecked: false } as IFilterItem,
            { key: RequestStatus.Approved, header: getLocalizedRequestStatus(RequestStatus.Approved, this.localize), isChecked: false } as IFilterItem,
            { key: RequestStatus.Rejected, header: getLocalizedRequestStatus(RequestStatus.Rejected, this.localize), isChecked: false } as IFilterItem,
        ];

        this.state = {
            isMenuOpen: true,
            activeMenuItemIndex: MenuItems.CommunityOfInterests,
            isApproveRequestsButtonEnabled: false,
            isRejectRequestsButtonEnabled: false,
            isTableHeaderCheckboxChecked: false,
            isLoadingCoiRequests: true,
            isLoadingNewsRequests: true,
            coiRequests: [],
            newsArticleRequests: [],
            newsSearchText: "",
            coiSearchText: "",
            coiSortedColumn: undefined,
            newsSortedColumn: undefined,
            statusBar: { id: 0, message: "", type: ActivityStatus.None },
            isApprovingRequests: false,
            isRejectingRequests: false,
            selectedStatusKeysInFilterForNews: [],
            selectedStatusKeysInFilterForCoi: [],
            selectedRequestsCount: 0,
            hasMoreNewsArticlesToLoad: false,
            hasMoreCoiRequestsToLoad: false,
            isConfirmationPortalOpen: false,
            rejectionComment: ""
        }
    }

    // The component life cycle method get called when component is mounted.
    async componentDidMount() {
        microsoftTeams.initialize();
        await this.getCOIRequestsAsync("", 0, undefined, undefined, []);
        await this.getNewsArticleRequestsAsync("", 0, undefined, undefined, []);
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
                isApproveRequestsButtonEnabled: false,
                isRejectRequestsButtonEnabled: false
            });
        }

        this.coiScrollPageNumber = pageNumber;

        let apiResponse = await getAllCoiRequestAsync(
            searchText?.trim() ?? null,
            pageNumber,
            sortColumn ?? CoiSortColumn.Status,
            sortOrder ?? SortOrder.Ascending,
            statusFilter,
            this.handleTokenAccessFailure);

        if (apiResponse && apiResponse.status === StatusCodes.OK) {
            let data = apiResponse.data as ICoi[];

            this.setState((prevState: IAdminRequestsState) => ({
                isLoadingCoiRequests: false,
                hasMoreCoiRequestsToLoad: data.length >= CoiRequestsPerPageCount,
                coiRequests: pageNumber === 0 ? data : [...prevState.coiRequests, ...data],
                isTableHeaderCheckboxChecked: false,
                selectedStatusKeysInFilterForCoi: statusFilter
            }));
        }
        else {
            this.setState((prevState: IAdminRequestsState) => ({
                isLoadingCoiRequests: false,
                hasMoreCoiRequestsToLoad: false,
                statusBar: { id: prevState.statusBar.id + 1, message: this.localize("failedToLoadCoiRequestsMessage"), type: ActivityStatus.Error }
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
                isApproveRequestsButtonEnabled: false,
                isRejectRequestsButtonEnabled: false
            });
        }

        this.newsScrollPageNumber = pageNumber;

        let apiResponse = await getAllNewsRequestAsync(
            searchText?.trim() ?? null,
            pageNumber,
            sortColumn ?? NewsArticleSortColumn.Status,
            sortOrder ?? SortOrder.Ascending,
            statusFilter,
            this.handleTokenAccessFailure);

        if (apiResponse && apiResponse.status === StatusCodes.OK) {
            let data = apiResponse.data as INews[];

            this.setState((prevState: IAdminRequestsState) => ({
                isLoadingNewsRequests: false,
                hasMoreNewsArticlesToLoad: data.length >= NewsArticleRequestsPerPageCount,
                newsArticleRequests: pageNumber === 0 ? data : [...prevState.newsArticleRequests, ...data],
                isTableHeaderCheckboxChecked: false,
                selectedStatusKeysInFilterForNews: statusFilter
            }));
        }
        else {
            this.setState((prevState: IAdminRequestsState) => ({
                isLoadingNewsRequests: false,
                hasMoreNewsArticlesToLoad: false,
                statusBar: { id: prevState.statusBar.id + 1, message: this.localize("failedToLoadNewsArticleRequestsMessage"), type: ActivityStatus.Error }
            }));
        }
    }

    // Event handler called when click on menu button.
    onMenuButtonClick = () => {
        this.setState((prevState: IAdminRequestsState) => ({
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
            isApproveRequestsButtonEnabled: selectedRequestsCount > 0,
            isRejectRequestsButtonEnabled: selectedRequestsCount > 0,
            isTableHeaderCheckboxChecked: totalSelectableRequestsCount > 0 && totalSelectableRequestsCount === selectedRequestsCount,
            selectedRequestsCount,
            rejectionComment: ""
        });
    }

    /**
     * Event handler called when COI request get checked or unchecked.
     * @param eventDetails The event details.
     * @param coiTableId The COI Table Id.
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
            isApproveRequestsButtonEnabled: selectedRequestsCount > 0,
            isRejectRequestsButtonEnabled: selectedRequestsCount > 0,
            isTableHeaderCheckboxChecked,
            selectedRequestsCount
        });
    }

    /**
     * Event handler called when news article request get checked or unchecked.
     * @param eventDetails The event details.
     * @param tableId The news article Id.
     * @param isChecked Indicates whether request is checked.
     */
    onNewsRequestCheckChanged = (eventDetails: any, tableId: string, isChecked: boolean) => {
        eventDetails.stopPropagation();

        let newsRequests: INews[] = cloneDeepNewsArticles(this.state.newsArticleRequests);

        let newsRequestIndex: number = newsRequests.findIndex((request: INews) => request.tableId === tableId);
        newsRequests[newsRequestIndex].isChecked = isChecked;

        let isTableHeaderCheckboxChecked: boolean = this.isHeaderCheckboxChecked(newsRequests, RequestType.News);

        let selectedRequestsCount: number = newsRequests
            .filter((request: INews) => request.isChecked).length;

        this.setState({
            newsArticleRequests: newsRequests,
            isApproveRequestsButtonEnabled: selectedRequestsCount > 0,
            isRejectRequestsButtonEnabled: selectedRequestsCount > 0,
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
                isApproveRequestsButtonEnabled: checkedRequestsCount > 0,
                isRejectRequestsButtonEnabled: checkedRequestsCount > 0,
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
                isApproveRequestsButtonEnabled: checkedRequestsCount > 0,
                isRejectRequestsButtonEnabled: checkedRequestsCount > 0,
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
            this.setState({ newsSearchText: eventData.value });

            if (this.searchTimeoutId) {
                clearTimeout(this.searchTimeoutId);
            }

            this.searchTimeoutId = window.setTimeout(() => {
                this.getNewsArticleRequestsAsync(
                    eventData.value,
                    0,
                    this.state.newsSortedColumn?.key,
                    this.state.newsSortedColumn?.sortOrder,
                    this.state.selectedStatusKeysInFilterForNews
                );
            }, SearchTimeoutInMilliseconds);
        }
        else {
            this.setState({ coiSearchText: eventData.value });

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
            this.getCOIRequestsAsync(this.state.coiSearchText, 0, key, sortOrder, this.state.selectedStatusKeysInFilterForCoi);
        }
        else {
            this.setState({ coiSortedColumn: { key, sortOrder: SortOrder.Ascending } });
            this.getCOIRequestsAsync(this.state.coiSearchText, 0, key, SortOrder.Ascending, this.state.selectedStatusKeysInFilterForCoi);
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
            this.getNewsArticleRequestsAsync(this.state.newsSearchText, 0, key, sortOrder, this.state.selectedStatusKeysInFilterForNews);
        }
        else {
            this.setState({ newsSortedColumn: { key, sortOrder: SortOrder.Ascending } });
            this.getNewsArticleRequestsAsync(this.state.newsSearchText, 0, key, SortOrder.Ascending, this.state.selectedStatusKeysInFilterForNews);
        }
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
            url: `${window.location.origin}/approve-reject-requests?${Constants.UrlParamRequestIdToApproveOrRejectRequest}=${newsTableId}&${Constants.UrlParamRequestType}=${RequestType.News}`
        } as microsoftTeams.TaskInfo, (error: string, result: any) => {
            if (result && result.data) {
                if (result.data === "Approved") {
                    this.setState((prevState: IAdminRequestsState) => ({
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("newsArticleRequestsApprovedSuccessfully"), type: ActivityStatus.Success }
                    }));

                    this.getNewsArticleRequestsAsync(
                        this.state.newsSearchText,
                        0,
                        this.state.newsSortedColumn?.key,
                        this.state.newsSortedColumn?.sortOrder,
                        this.state.selectedStatusKeysInFilterForNews
                    );
                }
                else if (result.data === "Rejected") {
                    this.setState((prevState: IAdminRequestsState) => ({
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("newsRequestsRejectedSuccessfully"), type: ActivityStatus.Success }
                    }));

                    this.getNewsArticleRequestsAsync(
                        this.state.newsSearchText,
                        0,
                        this.state.newsSortedColumn?.key,
                        this.state.newsSortedColumn?.sortOrder,
                        this.state.selectedStatusKeysInFilterForNews
                    );
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
            url: `${window.location.origin}/approve-reject-requests?${Constants.UrlParamRequestIdToApproveOrRejectRequest}=${coiTableId}&${Constants.UrlParamRequestType}=${RequestType.CommunityOfInterest}`
        } as microsoftTeams.TaskInfo, (error: string, result: any) => {
            if (result && result.data) {
                if (result.data === "Approved") {
                    this.setState((prevState: IAdminRequestsState) => ({
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("coiRequestsApprovedSuccessfully"), type: ActivityStatus.Success }
                    }));

                    this.getCOIRequestsAsync(
                        this.state.coiSearchText,
                        0,
                        this.state.coiSortedColumn?.key,
                        this.state.coiSortedColumn?.sortOrder,
                        this.state.selectedStatusKeysInFilterForCoi
                    );
                }
                else if (result.data === "Rejected") {
                    this.setState((prevState: IAdminRequestsState) => ({
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("coiRequestsRejectedSuccessfully"), type: ActivityStatus.Success }
                    }));

                    this.getCOIRequestsAsync(
                        this.state.coiSearchText,
                        0,
                        this.state.coiSortedColumn?.key,
                        this.state.coiSortedColumn?.sortOrder,
                        this.state.selectedStatusKeysInFilterForCoi
                    );
                }
            }
        });
    }

    /**
     * Event handler called when status filter get changed.
     * @param updatedStatusKeysInFilter The selected status keys.
     */
    onStatusFilterChange = (updatedStatusKeysInFilter: any[]) => {
        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            this.setState({ selectedStatusKeysInFilterForNews: updatedStatusKeysInFilter });

            this.getNewsArticleRequestsAsync(
                this.state.newsSearchText,
                0,
                this.state.newsSortedColumn?.key,
                this.state.newsSortedColumn?.sortOrder,
                updatedStatusKeysInFilter
            );
        }
        else {
            this.setState({ selectedStatusKeysInFilterForCoi: updatedStatusKeysInFilter })

            this.getCOIRequestsAsync(
                this.state.coiSearchText,
                0,
                this.state.coiSortedColumn?.key,
                this.state.coiSortedColumn?.sortOrder,
                updatedStatusKeysInFilter
            );
        }
    }

    onConfirmApproveRequests = async () => {
        this.setState({
            isApprovingRequests: true
        });

        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            let selectedRequestIds = this.state.newsArticleRequests
                .filter((news: INews) => news.isChecked === true)
                .map((news: INews) => news.tableId);

            let response = await approveNewsArticleRequests(selectedRequestIds, this.handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                if (response.data === true) {
                    this.setState((prevState: IAdminRequestsState) => ({
                        isApprovingRequests: false,
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("newsArticleRequestsApprovedSuccessfully"), type: ActivityStatus.Success }
                    }));

                    this.getNewsArticleRequestsAsync(
                        this.state.newsSearchText,
                        0,
                        this.state.newsSortedColumn?.key,
                        this.state.newsSortedColumn?.sortOrder,
                        this.state.selectedStatusKeysInFilterForNews
                    );
                }
                else {
                    this.setState((prevState: IAdminRequestsState) => ({
                        isApprovingRequests: false,
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("alreadyApproveOrRejectedRequestsMessage"), type: ActivityStatus.Error }
                    }));
                }
            }
            else {
                this.setState((prevState: IAdminRequestsState) => ({
                    isApprovingRequests: false,
                    statusBar: { id: prevState.statusBar.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
                }));
            }
        }
        else {
            let selectedRequestIds = this.state.coiRequests
                .filter((coi: ICoi) => coi.isChecked === true)
                .map((coi: ICoi) => coi.tableId);

            let response = await approveCoiRequests(selectedRequestIds, this.handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                if (response.data === true) {
                    this.setState((prevState: IAdminRequestsState) => ({
                        isApprovingRequests: false,
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("coiRequestsApprovedSuccessfully"), type: ActivityStatus.Success }
                    }));

                    this.getCOIRequestsAsync(
                        this.state.coiSearchText,
                        0,
                        this.state.coiSortedColumn?.key,
                        this.state.coiSortedColumn?.sortOrder,
                        this.state.selectedStatusKeysInFilterForCoi
                    );
                }
                else {
                    this.setState((prevState: IAdminRequestsState) => ({
                        isApprovingRequests: false,
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("alreadyApproveOrRejectedRequestsMessage"), type: ActivityStatus.Error }
                    }));
                }
            }
            else {
                this.setState((prevState: IAdminRequestsState) => ({
                    isApprovingRequests: false,
                    statusBar: { id: prevState.statusBar.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
                }));
            }
        }
    }

    onConfirmRejectRequests = async() => {
        this.setState({
            isRejectingRequests: true,
            isConfirmationPortalOpen: false
        });

        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            let selectedRequestIds = this.state.newsArticleRequests
                .filter((news: INews) => news.isChecked === true)
                .map((news: INews) => news.tableId);

            let response = await rejectNewsArticleRequests(selectedRequestIds, this.state.rejectionComment, this.handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                if (response.data === true) {
                    this.setState((prevState: IAdminRequestsState) => ({
                        isRejectingRequests: false,
                        rejectionComment: "",
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("newsRequestsRejectedSuccessfully"), type: ActivityStatus.Success }
                    }));

                    this.getNewsArticleRequestsAsync(
                        this.state.newsSearchText,
                        0,
                        this.state.newsSortedColumn?.key,
                        this.state.newsSortedColumn?.sortOrder,
                        this.state.selectedStatusKeysInFilterForNews
                    );
                }
                else {
                    this.setState((prevState: IAdminRequestsState) => ({
                        isRejectingRequests: false,
                        rejectionComment: "",
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("alreadyApproveOrRejectedRequestsMessage"), type: ActivityStatus.Error }
                    }));
                }
            }
            else {
                this.setState((prevState: IAdminRequestsState) => ({
                    isRejectingRequests: false,
                    rejectionComment: "",
                    statusBar: { id: prevState.statusBar.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
                }));
            }
        }
        else {
            let selectedRequestIds = this.state.coiRequests
                .filter((coi: ICoi) => coi.isChecked === true)
                .map((coi: ICoi) => coi.tableId);

            let response = await rejectCoiRequests(selectedRequestIds, this.state.rejectionComment, this.handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                if (response.data === true) {
                    this.setState((prevState: IAdminRequestsState) => ({
                        isRejectingRequests: false,
                        rejectionComment: "",
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("coiRequestsRejectedSuccessfully"), type: ActivityStatus.Success }
                    }));

                    this.getCOIRequestsAsync(
                        this.state.coiSearchText,
                        0,
                        this.state.coiSortedColumn?.key,
                        this.state.coiSortedColumn?.sortOrder,
                        this.state.selectedStatusKeysInFilterForCoi
                    );
                }
                else {
                    this.setState((prevState: IAdminRequestsState) => ({
                        isRejectingRequests: false,
                        rejectionComment: "",
                        statusBar: { id: prevState.statusBar.id + 1, message: this.localize("alreadyApproveOrRejectedRequestsMessage"), type: ActivityStatus.Error }
                    }));
                }
            }
            else {
                this.setState((prevState: IAdminRequestsState) => ({
                    isRejectingRequests: false,
                    rejectionComment: "",
                    statusBar: { id: prevState.statusBar.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
                }));
            }
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
                this.state.newsSearchText,
                pageNumber,
                this.state.newsSortedColumn?.key,
                this.state.newsSortedColumn?.sortOrder,
                this.state.selectedStatusKeysInFilterForNews
            );
        }
        else {
            this.getCOIRequestsAsync(
                this.state.coiSearchText,
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
                <Table.Cell key={TableColumnHeaderKeys.COIType} content={this.getCoiTableHeaderItemContent(CoiSortColumn.Type, this.localize("myRequestsCOITypeColumn"))} onClick={() => this.onCoiTableHeaderItemClick(CoiSortColumn.Type)} />
                <Table.Cell className="requested-by-column-header" key={TableColumnHeaderKeys.RequestedBy} content={this.localize("requestedByText")} />
                <Table.Cell className="requested-on-column-header" key={TableColumnHeaderKeys.RequestedOn} content={this.getCoiTableHeaderItemContent(CoiSortColumn.CreatedOn, this.localize("myRequestsRequestedOnColumn"))} onClick={() => this.onCoiTableHeaderItemClick(CoiSortColumn.CreatedOn)} />
                <Table.Cell key={TableColumnHeaderKeys.Status} content={this.getCoiTableHeaderItemContent(CoiSortColumn.Status, this.localize("myRequestsStatusColumn"))} onClick={() => this.onCoiTableHeaderItemClick(CoiSortColumn.Status)} />
            </Table.Row>;
        }

        return <Table.Row className="header row" header>
            <Table.Cell className="checkbox-column-header" key={TableColumnHeaderKeys.CheckBox} content={<Checkbox disabled={this.state.isLoadingNewsRequests} onChange={this.onHeaderCheckboxCheckedChange} checked={this.state.isTableHeaderCheckboxChecked} />} />
            <Table.Cell className="title-column-header" key={TableColumnHeaderKeys.Title} content={this.getNewsTableHeaderItemContent(NewsArticleSortColumn.Title, this.localize("myRequestsTitleColumn"))} onClick={() => this.onNewsTableHeaderItemClick(NewsArticleSortColumn.Title)} />
            <Table.Cell className="requested-by-column-header" key={TableColumnHeaderKeys.RequestedBy} content={this.localize("requestedByText")} />
            <Table.Cell className="requested-on-column-header" key={TableColumnHeaderKeys.RequestedOn} content={this.getNewsTableHeaderItemContent(NewsArticleSortColumn.CreatedAt, this.localize("myRequestsRequestedOnColumn"))} onClick={() => this.onNewsTableHeaderItemClick(NewsArticleSortColumn.CreatedAt)} />
            <Table.Cell key={TableColumnHeaderKeys.Status} content={this.getNewsTableHeaderItemContent(NewsArticleSortColumn.Status, this.localize("myRequestsStatusColumn"))} onClick={() => this.onNewsTableHeaderItemClick(NewsArticleSortColumn.Status)} />
        </Table.Row>;
    }

    // Gets the loading skeleton for table.
    getSkeletonOfTableRows = () => {
        let tableSkeletonRow = <Table.Row className="row">
            <Table.Cell className="checkbox-column-header" content={<Skeleton animation="wave"><SkeletonLine width="1.8rem" height="1.4rem" /></Skeleton>} />
            <Table.Cell className="title-column-header" content={<Skeleton animation="wave"><SkeletonLine width="25rem" height="1.4rem" /></Skeleton>} />
            {this.state.activeMenuItemIndex === MenuItems.CommunityOfInterests ? <Table.Cell content={<Skeleton animation="wave"><SkeletonLine width="9rem" height="1.4rem" /></Skeleton>} /> : null}
            <Table.Cell className="requested-by-column-header" content={<Skeleton animation="wave"><SkeletonLine width="13rem" height="1.4rem" /></Skeleton>} />
            <Table.Cell className="requested-on-column-header" content={<Skeleton animation="wave"><SkeletonLine width="13rem" height="1.4rem" /></Skeleton>} />
            <Table.Cell content={<Skeleton animation="wave"><SkeletonLine width="9rem" height="1.4rem" /></Skeleton>} />
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
            return <Table.Row className="row" onClick={() => this.onCoiRequestClick(request.tableId)}>
                <Table.Cell className="checkbox-column-header" content={<Checkbox disabled={request.status === RequestStatus.Approved || request.status === RequestStatus.Rejected} checked={request.isChecked} onChange={(event, data) => this.onCOIRequestCheckChanged(event, request.tableId, data?.checked ?? false)} />} />
                <Table.Cell className="title-column-header" content={request.coiName} title={request.coiName} truncateContent={true} />
                <Table.Cell content={getLocalizedCOIType(request.type, this.localize)} truncateContent={true} />
                <Table.Cell className="requested-by-column-header" content={request.createdBy} title={request.createdBy} truncateContent={true} />
                <Table.Cell className="requested-on-column-header" content={request.createdOn ? moment(request.createdOn).format("DD-MMM-YYYY hh:mm A") : "NA"} title={request.createdOn ? moment(request.createdOn).format("DD-MMM-YYYY hh:mm A") : "NA"} truncateContent={true} />
                <Table.Cell content={getLocalizedRequestStatus(request.status, this.localize)} truncateContent={true} />
            </Table.Row>
        });
    }

    // Gets the filter count string in '(1)' format. If the applied filter count is 0, then empty string will be returned.
    getFilterCount = () => {
        if (this.state.activeMenuItemIndex === MenuItems.NewsArticles) {
            if (this.state.selectedStatusKeysInFilterForNews?.length) {
                return `(${this.state.selectedStatusKeysInFilterForNews.length})`;
            }
        }
        else if (this.state.selectedStatusKeysInFilterForCoi?.length) {
            return `(${this.state.selectedStatusKeysInFilterForCoi.length})`;
        }

        return "";
    }

    // Gets the news article requests table rows.
    getNewsArticleTableRows = (): JSX.Element[] => {
        return this.state.newsArticleRequests.map((request: INews) => {
            return <Table.Row className="row" onClick={() => this.onNewsRequestClick(request.tableId)}>
                <Table.Cell className="checkbox-column-header" content={<Checkbox disabled={request.status === RequestStatus.Approved || request.status === RequestStatus.Rejected} checked={request.isChecked} onChange={(event, data) => this.onNewsRequestCheckChanged(event, request.tableId, data?.checked ?? false)} />} />
                <Table.Cell className="title-column-header" content={request.title} title={request.title} truncateContent={true} />
                <Table.Cell className="requested-by-column-header" content={request.createdBy} title={request.createdBy} truncateContent={true} />
                <Table.Cell className="requested-on-column-header" content={moment(request.createdAt).format("DD-MMM-YYYY hh:mm A")} truncateContent={true} />
                <Table.Cell content={getLocalizedRequestStatus(request.status, this.localize)} truncateContent={true} />
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
                message={(this.state.activeMenuItemIndex === MenuItems.NewsArticles && (this.state.selectedStatusKeysInFilterForNews?.length > 0 || this.state.newsSearchText?.trim().length > 0))
                    || (this.state.activeMenuItemIndex === MenuItems.CommunityOfInterests && (this.state.coiSearchText?.trim().length > 0 || this.state.selectedStatusKeysInFilterForCoi?.length > 0)) ? this.localize("noRequestsExistsSearchMessage") : this.localize("myRequestsNoRequestsFoundMessage")}
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
            <Flex className="admin-requests" column fill>
                <StatusBar status={this.state.statusBar} isMobile={false} />
                <AthenaSplash description={this.localize("adminRequestsTabDescription")} heading={this.localize("adminRequestsTabTitle")} />
                <Segment
                    className="toolbar-box"
                    content={<Flex vAlign="center">
                        <Button text iconOnly primary={this.state.isMenuOpen} icon={<MenuIcon />} onClick={this.onMenuButtonClick} />
                        <Dialog
                            cancelButton={this.localize("cancelButtonContent")}
                            confirmButton={this.localize("okButtonContent")}
                            header={this.localize("approveRequestsDialogHeader")}
                            content={this.localize("approveRequestsDialogContent")}
                            onConfirm={this.onConfirmApproveRequests}
                            trigger={<Button text loading={this.state.isApprovingRequests} icon={<AcceptIcon />} content={this.localize("approveText")} disabled={!this.state.isApproveRequestsButtonEnabled || this.state.isApprovingRequests || this.state.isRejectingRequests} />}
                        />
                        <Popup
                            open={this.state.isConfirmationPortalOpen}
                            onOpenChange={(e: any, { open }: any) => this.setState({ isConfirmationPortalOpen: open, rejectionComment: "" })}
                            trigger={
                                <Button loading={this.state.isRejectingRequests} icon={<CloseIcon xSpacing="before" />} content={this.localize("rejectText")} text disabled={!this.state.isRejectRequestsButtonEnabled || this.state.isRejectingRequests || this.state.isApprovingRequests} onClick={() => { this.setState({ isConfirmationPortalOpen: true }); }} />
                            }
                            content={<Flex column gap="gap.small" styles={{ width: "40vw" }}>
                                <TextArea value={this.state.rejectionComment} onChange={(event: any) => { this.setState({ rejectionComment: event.target.value }); }} fluid placeholder="Type reason for rejection" />
                                <Flex gap="gap.small">
                                    <Flex.Item push>
                                        <Button content={this.localize("cancelButtonContent")} onClick={() => { this.setState({ rejectionComment: "" }); this.setState({ isConfirmationPortalOpen: false }); }} />
                                    </Flex.Item>
                                    <Button className="athena-button" content={this.localize("rejectText")} onClick={this.onConfirmRejectRequests} disabled={!this.state.rejectionComment?.trim()} />
                                </Flex>
                            </Flex>}
                            trapFocus
                        />
                        <Flex.Item push>
                            <Flex>
                                <Popup
                                    trigger={<Button
                                        text
                                        primary={((this.state.activeMenuItemIndex === MenuItems.NewsArticles && this.state.selectedStatusKeysInFilterForNews?.length !== 0)
                                            || (this.state.activeMenuItemIndex === MenuItems.CommunityOfInterests && this.state.selectedStatusKeysInFilterForCoi?.length !== 0)) ?? false}
                                        icon={<FilterIcon />}
                                        content={`${this.localize("filterButtonContent")} ${this.getFilterCount()}`}
                                    />}
                                    content={<FilterPopup
                                        title={this.localize("myRequestsStatusColumn")}
                                        clearText={this.localize("clearText")}
                                        items={this.statusFilterItems}
                                        disabled={ this.state.activeMenuItemIndex === MenuItems.NewsArticles ? this.state.isLoadingNewsRequests : this.state.isLoadingCoiRequests }
                                        selectedFilterItemKeys={this.state.activeMenuItemIndex === MenuItems.NewsArticles ? this.state.selectedStatusKeysInFilterForNews : this.state.selectedStatusKeysInFilterForCoi}
                                        onCheckedChange={(updatedStatusKeysInFilter) => this.onStatusFilterChange(updatedStatusKeysInFilter)}
                                    />}
                                    position="below"
                                    align="end"
                                />
                                <Input inverted icon={<SearchIcon />} placeholder={this.localize("myRequestsFindInputPlaceholder")} type="text" value={this.state.activeMenuItemIndex === MenuItems.NewsArticles ? this.state.newsSearchText : this.state.coiSearchText} onChange={this.onSearchTextChange} />
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

export default withTranslation()(withRouter(AdminRequests));
