// <copyright file="discovery-home.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import { Button, Button as FluentUIButton, ChatIcon, Pill, Toolbar, Dropdown, Checkbox, ChevronEndMediumIcon, StarIcon, CloseIcon, Dialog, SettingsIcon, Flex, Image, Loader, MenuButton, MenuItemProps, SaveIcon, Text, EyeIcon, RadioGroup, RadioGroupItemProps, Popup, SearchIcon, DropdownItemProps } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";
import ContextMenu from 'devextreme-react/context-menu';
import TreeList, { Column, ColumnChooser, Selection, Sorting } from 'devextreme-react/tree-list';
import 'devextreme/dist/css/dx.light.css';
import { StatusCodes } from "http-status-codes";
import { TFunction } from "i18next";
import { cloneDeep } from "lodash";
import moment from "moment";
import * as React from "react";
import { WithTranslation, withTranslation } from "react-i18next";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { getDiscoveryNodeTypeAsync, getDiscoveryTreeFiltersAsync, getInterestedSponsorsDataAsync, getInterestedUsersDataAsync, getNodeDataAsync, getTaxonomyAsync, searchAndFilterDiscoveryTreeResourcesAsync, followResourceAsync } from "../../../api/discovery-tree-api";
import { getAllKeywordsAsync } from "../../../api/keyword-api";
import { getUserPersistentDataAsync, saveDiscoveryTreePersistentDataAsync } from "../../../api/user-settings-tab-api";
import { getBaseUrl } from "../../../configVariables";
import Constants from "../../../constants/constants";
import { ActivityStatus } from "../../../models/activity-status";
import IAthenaEvent from "../../../models/athena-event";
import ICoi from "../../../models/coi";
import { IDiscoveryFilter, IDiscoveryGroup, IDiscoveryTreeFilter, IDiscoveryTreeSelectedFilters } from "../../../models/discovery-filter";
import ITreeNodeDataElement from "../../../models/discovery-tree-node-data-element";
import { DiscoveryNodeFileNames } from "../../../models/discovery-tree-node-file-names";
import IDiscoveryTreeNodeType from "../../../models/discovery-tree-node-type";
import IDiscoveryTreePersistentData from "../../../models/discovery-tree-persistent-data";
import ITaxonomyElement from "../../../models/discovery-tree-taxonomy-element";
import IKeyword from "../../../models/keyword";
import INews from "../../../models/news";
import IPartnerDetails from "../../../models/partner-details";
import IResearchProject from "../../../models/research-project";
import IResearchProposal from "../../../models/research-proposal";
import IResearchRequest from "../../../models/research-request";
import ISponsorDetails from "../../../models/sponsor-details";
import IStatusBar from "../../../models/status-bar";
import IUserDetails from "../../../models/user-details";
import IUserPersistentData from "../../../models/user-persistent-data";
import IUserSettings from "../../../models/user-settings";
import AthenaSplash from "../../athena-splash/athena-splash";
import ContentLoader from "../../common/loader/loader";
import ProfilePic from "../../common/person-avatar/person-avatar";
import StatusBar from "../../common/status-bar/status-bar";
import DiscoveryFilter from "../discovery-filter/discovery-filter";
import RateOrComment from "../rate-comment/rate-comment";
import IQuickAccessListItem from "../../../models/quick-access-list-item";
import { getQuickAccessListAsync, addQuickAccessItemAsync, deleteQuickAccessItemAsync } from "../../../api/quick-access-api";
import IAthenaInfoResource from "../../../models/athena-info-resource";
import IAthenaTool from "../../../models/athena-tool";
import { getResourceImagePath } from "../../../helpers/image-helper";
import { IAthenaResearchImportance, IAthenaResearchPriority } from "../../../models/athena-research";
import { getAthenaResearchImportanceAsync, getAthenaResearchPrioritiesAsync } from "../../../api/athena-research-api";
import { IAthenaNewsSource } from "../../../models/athena-news-source";
import { getAthenaNewsSourcesAsync } from "../../../api/news-api";
import IDiscoveryTreeSearchAndFilter from "../../../models/discovery-tree-search-filter";

import "./discovery-home.scss";

const TreeViewRootFolderValue: number = 0;
const TreeViewFolderNodeTypeValue: number = 1;
const TableIdAndMaxIdSeparatorInFilteredDiscoveryTree: string = "_";
const FilterGroupParentId: number = 0;
const ViewAsTreeViewId: number = 1;
const ViewAsFlatListId: number = 2;
const dbValueOfAllFilterItem: number = -1;
const TypeColumnSortIndex: number = 0;
const TitleColumnSortIndex: number = 2;
const DateColumnSortIndex: number = 1;

interface IDiscoveryHomeProps extends WithTranslation, RouteComponentProps {
}

export interface IDiscoveryHomeState {
    status: IStatusBar;
    selectedMenuItemId: string;
    selectedTreeItem: any;
    taxonomyTree: ITaxonomyElement[];
    leafNodeIds: string[];
    statusBar: any;
    isLoading: boolean;
    isLoadingNodeData: boolean;
    menuItems: any;
    taxonomyFolderNodeIds: string[];
    nodesExpandedAtLeastOnce: string[];
    filterData: IDiscoveryGroup[];
    initialFilterData: IDiscoveryTreeFilter[];
    isFilterOpen: boolean;
    closeDisplayDetails: boolean;
    nodeTypeData: IDiscoveryTreeNodeType[];
    interestedUsers: IUserDetails[];
    interestedSponsors: ISponsorDetails[];
    selectedFilters: IDiscoveryTreeSelectedFilters[];
    taxonomyKey: number;
    searchString: string;
    isSavingQuery: boolean;
    isClearingAppliedFiltersAndSearchQuery: boolean;
    isLoadingAllKeywords: boolean;
    allKeywords: IKeyword[];
    viewAsCheckedValue: number;
    flatListData: ITaxonomyElement[];
    quickAccessData: IQuickAccessListItem[];
    quickAccessListItems: MenuItemProps[];
    isUpdatingQuickAccessList: boolean;
    overflowOpen: boolean;
    filterPillsData: any[];
    researchImportanceData: IAthenaResearchImportance[];
    researchPriorities: IAthenaResearchPriority[];
    newsSources: IAthenaNewsSource[];
    selectedKeywords: IKeyword[];
    searchResults: DropdownItemProps[];
}

class DiscoveryHome extends React.Component<IDiscoveryHomeProps, IDiscoveryHomeState> {
    contextMenuRef: any;
    treeViewRef: any;
    coiMenuItems: any[];
    researchAreaMenuItems: any[];
    researchProjectMenuItems: any[];
    researchRequestMenuItems: any[];
    researchProposalMenuItems: any[];
    eventMenuItems: any[];
    partnerMenuItems: any[];
    sponsorMenuItems: any[];
    newsMenuItems: any[];
    sourceInstitutionMenuItems: any[];
    informationTypesMenuItems: any[];
    toolTypesMenuItems: any[];
    personMenuItems: any[];
    localize: TFunction;
    originalTaxonomyTree: ITaxonomyElement[];
    clearFilterMenuItems: MenuItemProps[];
    viewAsRadioGroupItems: RadioGroupItemProps[];

    constructor(props: any) {
        super(props);
        this.contextMenuRef = React.createRef();
        this.treeViewRef = React.createRef();
        this.localize = this.props.t;
        this.originalTaxonomyTree = [];
        this.coiMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'add-to-collection', text: this.localize('addToCollectionMenuItemText') },
            { id: 'follow-item', text: this.localize('followCoiMenuItemText') },
            { id: 'coi-home-page', text: this.localize('coiHomePageMenuItemText') },
            { id: 'rate-comment-item', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.researchAreaMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'add-to-quick-access-list', text: this.localize('addToQuickAccessListText') },
            { id: 'follow-item', text: this.localize('followResearchAreaMenuItemText') },
            { id: 'interested-users', text: this.localize('findInterestedUsersMenuItemText') },
            { id: 'interested-sponsors', text: this.localize('findInterestedSponsorsMenuItemText') },
            { id: 'suggest-research-project', text: this.localize('proposeNewResearchProjectMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.researchProjectMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'add-to-collection', text: this.localize('addToCollectionMenuItemText') },
            { id: 'follow-item', text: this.localize('followResearchProjectMenuItemText') },
            { id: 'rate-comment-item', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.researchRequestMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'add-to-collection', text: this.localize('addToCollectionMenuItemText') },
            { id: 'follow-item', text: this.localize('followResearchRequestMenuItemText') },
            { id: 'rate-comment-item', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.researchProposalMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'add-to-collection', text: this.localize('addToCollectionMenuItemText') },
            { id: 'follow-item', text: this.localize('followResearchProposalMenuItemText') },
            { id: 'rate-comment-item', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.personMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
        ];
        this.eventMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'add-to-collection', text: this.localize('addToCollectionMenuItemText') },
            { id: 'rate-comment-item', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.partnerMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'add-to-collection', text: this.localize('addToCollectionMenuItemText') },
            { id: 'rate-comment-item', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.sponsorMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'add-to-collection', text: this.localize('addToCollectionMenuItemText') },
            { id: 'rate-comment-item', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.newsMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'add-to-collection', text: this.localize('addToCollectionMenuItemText') },
            { id: 'rate-comment-item', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.sourceInstitutionMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'rate-comment-source-intitution', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.informationTypesMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'rate-comment-information-types', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];
        this.toolTypesMenuItems = [
            { id: 'display-details', text: this.localize('displayDetailsMenuItemText') },
            { id: 'rate-comment-tool-types', text: this.localize('rateOrCommentMenuItemText') },
            { id: 'share-with-colleague', text: this.localize('shareWithColleagueMenuItemText') }
        ];

        this.clearFilterMenuItems = [{
            content: this.localize("discoveryTreeClearAllFiltersButtonText"),
            onClick: this.onClearAppliedFiltersAndSearchQuery
        },
        {
            content: this.localize("discoveryTreeResetFilterMenuItemText"),
            onClick: this.resetFiltersToDefaultSelections
        }];

        this.viewAsRadioGroupItems = [{
            label: this.localize("discoveryTreeViewAsTreeViewRadioItemText"),
            value: ViewAsTreeViewId
        },
        {
            label: this.localize("discoveryTreeViewAsFlatListRadioItemText"),
            value: ViewAsFlatListId
        }];

        this.state = {
            status: { id: 0, message: "", type: ActivityStatus.None },
            selectedMenuItemId: "",
            selectedTreeItem: undefined,
            taxonomyTree: [],
            leafNodeIds: [],
            statusBar: [],
            isLoading: true,
            isLoadingNodeData: false,
            menuItems: [],
            taxonomyFolderNodeIds: [],
            nodesExpandedAtLeastOnce: [],
            filterData: [],
            initialFilterData: [],
            isFilterOpen: false,
            closeDisplayDetails: false,
            nodeTypeData: [],
            interestedUsers: [],
            interestedSponsors: [],
            selectedFilters: [],
            taxonomyKey: 0,
            searchString: "",
            isSavingQuery: false,
            isClearingAppliedFiltersAndSearchQuery: false,
            isLoadingAllKeywords: false,
            allKeywords: [],
            viewAsCheckedValue: ViewAsTreeViewId,
            flatListData: [],
            quickAccessData: [],
            quickAccessListItems: [],
            isUpdatingQuickAccessList: false,
            overflowOpen: false,
            filterPillsData: [],
            researchImportanceData: [],
            researchPriorities: [],
            newsSources: [],
            selectedKeywords: [],
            searchResults: []
        }
    }

    async componentDidMount() {
        await this.getInitialData();

        microsoftTeams.getContext((context: microsoftTeams.Context) => {
            if (context.subEntityId) {
                //Get nodeId and parentId from subEntityId field.
                var itemData = JSON.parse(context.subEntityId);
                let deeplinkNodeId = itemData.taxonomyId;
                let deepLinkNodeParentId = itemData.parentId;
                let nodeTypeId = itemData.nodeTypeId;
                this.selectedDeepLinkNode(deeplinkNodeId, deepLinkNodeParentId, nodeTypeId);
            }
        });
    }

    get treeView() {
        return this.treeViewRef.current.instance;
    }

    get contextMenu() {
        return this.contextMenuRef.current.instance;
    }

    // Loads the initial data.
    getInitialData = async () => {
        this.getAllKeywords();
        this.getNewsSources();

        let persistentData: IUserPersistentData | null = await this.getPersistentDataAsync();

        await this.getNodeTypeData();
        await this.fetchFilters(persistentData);
        await this.getDiscoveryTreeTaxonomyAsync();

        this.getAthenaResearchImportanceData();
        this.getAthenaResearchPriorities();

        this.searchAndFilterResourcesBasedOnPersistentData();
        await this.getQuickAccessData();
    }

    // Loads news sources.
    getNewsSources = async () => {
        let response = await getAthenaNewsSourcesAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let newsSources = response.data as IAthenaNewsSource[];
            this.setState({ newsSources });
        }
    }

    // Loads quick access data.
    getQuickAccessData = async () => {
        let response = await getQuickAccessListAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let quickAccessData = response.data;
            this.setState({ quickAccessData: quickAccessData }, this.getQuickAccessMenuItems);
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToLoadQuickAccessDataError"), type: ActivityStatus.Error } });
        }
    }

    // Loads athena reserach importance data.
    getAthenaResearchImportanceData = async () => {
        let response = await getAthenaResearchImportanceAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let researchImportanceData = response.data;
            this.setState({ researchImportanceData: researchImportanceData });
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToLoadAthenaResearchImportanceDataError"), type: ActivityStatus.Error } });
        }
    }

    // Loads athena reserach priorities.
    getAthenaResearchPriorities = async () => {
        let response = await getAthenaResearchPrioritiesAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let researchPriorities = response.data;
            this.setState({ researchPriorities: researchPriorities });
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToLoadAthenaResearchPrioritiesError"), type: ActivityStatus.Error } });
        }
    }

    // Returns quick access menu items.
    getQuickAccessMenuItems = () => {
        let quickAccessItems: any[] = [];
        if (this.state.quickAccessData.length) {
            this.state.quickAccessData.map((quickAccessListItem: IQuickAccessListItem) => {
                quickAccessItems.push({
                    key: quickAccessListItem.taxonomyId,
                    content: this.renderQuickAccessListItem(quickAccessListItem),
                })
            });
        }
        else {
            quickAccessItems.push({
                content: this.localize('noQuickAccessItemsFoundError'),
            })
        }
        this.setState({ quickAccessListItems: quickAccessItems });
    }

    /**
     * Renders quick access list item.
     * @param quickAccessListItem The quick access list item.
     */
    renderQuickAccessListItem = (quickAccessListItem: IQuickAccessListItem) => {
        return (
            <Flex>
                <Flex gap="gap.small" vAlign="center" onClick={() => this.expandQuickAccessNode(quickAccessListItem.taxonomyId)}>
                    <Image src={getResourceImagePath(this.state.nodeTypeData, quickAccessListItem.nodeTypeId)} styles={{ width: "2rem", height: "1.6rem" }} />
                    <Text content={this.getQuickAccessListItemTitle(quickAccessListItem.taxonomyId)} />
                </Flex>
                <Flex.Item push>
                    <Button className="quick-access-menu-item-button" text iconOnly icon={<CloseIcon size="small" outline />} title={this.localize('removeFromQuickAccessListTitle')} onClick={() => this.removeQuickAccessItem(quickAccessListItem)} />
                </Flex.Item>
            </Flex>
        )
    }

    /**
     * Expands quick access item node.
     * @param taxonomyId The taxonomy Id of quick access list item.
     */
    expandQuickAccessNode = async (taxonomyId: string) => {
        await this.collapseAllNodes();
        await this.treeViewRef?.instance?.expandRow(parseInt(taxonomyId));
        await this.expandNodesUptoParentNode(parseInt(taxonomyId), this.state.taxonomyTree);
        let nodeToBeSelected = this.state.taxonomyTree.find(x => x.taxonomyId.toString() === taxonomyId);
        if (nodeToBeSelected) {
            this.setStatusBarHierarchy(nodeToBeSelected);
        }
    }

    // Collapse all nodes.
    collapseAllNodes = async () => {
        let expandedNodeIds = cloneDeep(this.state.nodesExpandedAtLeastOnce);

        for (let i = 0; i < expandedNodeIds.length; i++) {
            await this.treeViewRef?.instance?.collapseRow(parseInt(expandedNodeIds[i]));
        }
    }

    /**
     * Removes the item from quick access list.
     * @param quickAccessItem The quick access list item.
     */
    removeQuickAccessItem = async (quickAccessItem: IQuickAccessListItem) => {
        this.setState({ isUpdatingQuickAccessList: true });
        let existingQuickAccessData: IQuickAccessListItem[] = cloneDeep(this.state.quickAccessData);
        let response = await deleteQuickAccessItemAsync(quickAccessItem.quickAccessItemId!, this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let index = existingQuickAccessData.findIndex((quickAccessItem: IQuickAccessListItem) => quickAccessItem.taxonomyId === quickAccessItem.taxonomyId);
            if (index !== -1) {
                existingQuickAccessData.splice(index, 1);
            }

            this.setState({
                quickAccessData: existingQuickAccessData,
                status: { id: this.state.status.id + 1, message: this.localize("quickAccessItemDeletedSuccessfully"), type: ActivityStatus.Success }
            }, this.getQuickAccessMenuItems);
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToDeleteQuickAccessItemError"), type: ActivityStatus.Error } });
        }
        this.setState({ isUpdatingQuickAccessList: false });
    }

    /**
     * Adds the selected tree item to quick access list.
     * @param selectedTreeItem The selected tree item.
     */
    addQuickAccessItem = async (selectedTreeItem: any) => {
        this.setState({ isUpdatingQuickAccessList: true });

        var quickAccessItem: IQuickAccessListItem = {
            taxonomyId: selectedTreeItem?.taxonomyId?.toString(),
            parentId: selectedTreeItem?.parentId,
            nodeTypeId: selectedTreeItem?.nodeTypeId,
        }

        let response = await addQuickAccessItemAsync(quickAccessItem, this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let quickAccessItem = response.data;

            let existingQuickAccessData: IQuickAccessListItem[] = cloneDeep(this.state.quickAccessData);
            existingQuickAccessData.push(quickAccessItem);

            this.setState({
                quickAccessData: existingQuickAccessData,
                status: { id: this.state.status.id + 1, message: this.localize("quickAccessItemAddedSuccessfully"), type: ActivityStatus.Success }
            }, this.getQuickAccessMenuItems);
        }
        else if (response && response.status === StatusCodes.CONFLICT) {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("quickAccessItemAlreadyExists"), type: ActivityStatus.Error } });
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToAddQuickAccessItemError"), type: ActivityStatus.Error } });
        }
        this.setState({ isUpdatingQuickAccessList: false });
    }

    /**
     * Returns title of quick access list item.
     * @param taxonomyId The taxonomy Id.
     */
    getQuickAccessListItemTitle = (taxonomyId: string) => {
        let taxonomyItem = this.state.taxonomyTree.find((taxonomy: ITaxonomyElement) => taxonomy.taxonomyId.toString() === taxonomyId.toString());
        if (taxonomyItem) {
            return taxonomyItem.title;
        }
        return "NA";
    }

    // Filter based on persistent data.
    searchAndFilterResourcesBasedOnPersistentData = () => {
        let selectedFilters: IDiscoveryTreeSelectedFilters[] = cloneDeep(this.state.selectedFilters);
        let filterData: IDiscoveryTreeFilter[] = cloneDeep(this.state.initialFilterData);

        let checkedFiltersExceptAllFilterItem: IDiscoveryTreeFilter[] = filterData.filter((data: IDiscoveryTreeFilter) =>
            data.isChecked === true);

        for (let i = 0; i < checkedFiltersExceptAllFilterItem.length; i++) {
            if (checkedFiltersExceptAllFilterItem[i].enabled === true) {
                this.prepareSelectedFiltersList(checkedFiltersExceptAllFilterItem[i], true, selectedFilters)
            }
        }

        this.setState({ selectedFilters }, () => this.searchAndFilterAsync(this.state.selectedFilters));
    }

    // Resets the filters with 'Default on' as checked.
    resetFiltersToDefaultSelections = () => {
        let selectedFilters: IDiscoveryTreeSelectedFilters[] = [];
        let filterData: IDiscoveryTreeFilter[] = cloneDeep(this.state.initialFilterData);

        for (let i = 0; i < filterData.length; i++) {
            if (filterData[i].defaultOn === true) {
                filterData[i].isChecked = true;
                if (filterData[i].enabled === true) {
                    this.prepareSelectedFiltersList(filterData[i], true, selectedFilters);
                }
            }
            else {
                filterData[i].isChecked = false;
            }
        }

        if (selectedFilters?.length > 0) {
            this.setState({ initialFilterData: filterData, selectedFilters }, () => {
                this.getFilters();
                this.getFilterPillsData();
                this.searchAndFilterAsync(this.state.selectedFilters);
            });
        }
    }

    // Loads the node type data.
    getNodeTypeData = async () => {
        let response = await getDiscoveryNodeTypeAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let nodeTypeData = response.data;
            this.setState({ nodeTypeData: nodeTypeData });
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error } });
        }
    }

    /**
     * Loads the filter data.
     * @param persistentData The logged-in user's persistent data.
     */
    fetchFilters = async (persistentData: IUserPersistentData | null) => {
        let selectedFilterIds: number[] | undefined = persistentData?.discoveryTreePersistentData?.selectedFilterIds;
        let selectedConfigureFilterIds: number[] | undefined = persistentData?.discoveryTreePersistentData?.selectedConfigureFilterIds;

        let response = await getDiscoveryTreeFiltersAsync(this.handleTokenAccessFailure);
        if (response && response.status === StatusCodes.OK) {
            let filterData = response.data;
            if (!selectedFilterIds) {
                selectedFilterIds = filterData.filter(filterItem => filterItem.defaultOn === true).map(filterItem => { return filterItem.filterId });
            }
            filterData = filterData.map(data => {
                return {
                    ...data,
                    isChecked: selectedFilterIds?.some((filterId: number) => data.filterId === filterId) ?? false
                }
            });
            let filterGroups = filterData.filter(x => x.parentId === FilterGroupParentId);
            filterGroups.forEach((group: any) => {
                let filtersValues = filterData.filter(x => x.parentId === group.filterId);
                filtersValues.map((filter: IDiscoveryTreeFilter) => {
                    let index = filterData.findIndex(filterItem => filterItem.filterId === filter.filterId);
                    filterData[index].isVisible = selectedConfigureFilterIds?.some((filterId: number) => filter.filterId === filterId) ?? true;
                })
            });

            this.setState({ initialFilterData: filterData }, () => {
                this.getFilters();
                this.getFilterPillsData();
            });
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToLoadFilterError"), type: ActivityStatus.Error } });
        }
    }

    // Sets the filter data.
    getFilters = () => {
        let filterGroups = this.state.initialFilterData.filter(x => x.parentId === FilterGroupParentId);
        let filterData: IDiscoveryGroup[] = [];
        filterGroups.forEach((group: any) => {
            let filtersValues = this.state.initialFilterData.filter(x => x.parentId === group.filterId && x.isVisible);
            let filters: IDiscoveryFilter[] = [];
            filtersValues.forEach(filterValue => {
                filters.push({
                    key: filterValue.filterId,
                    value: filterValue.title,
                    isChecked: filterValue.isChecked,
                    subFilter: this.getSubFilters(filterValue.filterId),
                });
            });
            filterData.push({
                filterGroupId: group.filterId,
                filters: filters,
            });
        });
        this.setState({ filterData: filterData });
    }

    /**
     * Sets the sub filters.
     * @param filterId The filter Id.
     */
    getSubFilters = (filterId: number) => {
        let filters: IDiscoveryFilter[] = [];
        let filtersValues = this.state.initialFilterData.filter(x => x.parentId === filterId);
        filtersValues.forEach(filterValue => {
            filters.push({
                key: filterValue.filterId,
                value: filterValue.title,
                isChecked: filterValue.isChecked,
                subFilter: this.getSubFilters(filterValue.filterId),
            });
        });
        return filters;
    }

    /**
     * Renders filter item.
     * @param item The filter item.
     */
    renderFilterMenuItem = (item: IDiscoveryFilter) => {
        return (
            <Flex vAlign="center">
                <Checkbox checked={item.isChecked} onChange={(event: any, v: any) => this.setSelectedFilterItem(event, v, item)} className="filter-checkbox" />
                <Text content={item.value} />
            </Flex>
        )
    }

    /**
     * Renders sub filter items.
     * @param subFilter The sub filter array.
     * @param filter The filter item.
     */
    renderSubFilterItems = (subFilter: IDiscoveryFilter[], filter: IDiscoveryFilter) => {
        let filterItems: any[] = [];
        filterItems.push({
            content: filter.value,
            className: "filter-menu-heading",
        })
        subFilter.map((item: IDiscoveryFilter) => {
            filterItems.push({
                key: item.key,
                menuOpen: true,
                content: this.renderFilterMenuItem(item),
                menu: item.subFilter.length !== 0 ? {
                    items: this.renderSubFilterItems(item.subFilter, item)
                } : null,
                className: item.subFilter.length === 0 ? "filter-menu-item" : "filter-sub-menu",
            })
        })
        return filterItems;
    }

    /**
     * Renders filters.
     * @param filterItem The filter item.
     */
    renderFilterItems = (filterItem: IDiscoveryFilter) => {
        let items: any[] = [];
        items.push({
            content: filterItem.value,
            className: "filter-menu-heading",
        })
        filterItem.subFilter.map((subFilter: IDiscoveryFilter) => {
            items.push({
                key: subFilter.key,
                menuOpen: true,
                content: this.renderFilterMenuItem(subFilter),
                menu: subFilter.subFilter.length !== 0 ? {
                    items: this.renderSubFilterItems(subFilter.subFilter, subFilter)
                } : null,
                className: subFilter.subFilter.length === 0 ? "filter-menu-item" : "filter-sub-menu",
            })
        })
        return (
            <MenuButton
                trigger={<Button icon={<Image src={this.getFilterImage(filterItem)} styles={{ width: "2rem", cursor: "pointer" }} />} iconOnly text title={filterItem.value} />}
                menu={items}
                className="filter-menu-item-container"
            />
        )
    }

    /**
     * Returns the filter icon.
     * @param filterItem Filter item data.
     */
    getFilterImage = (filterItem: IDiscoveryFilter) => {
        var filter = this.state.initialFilterData.find(filter => filter.filterId === filterItem.key);
        if (filter) {
            return Constants.getArtifactsPath + filter.toolbarIcon;
        }
        return "";
    }

    /**
     * Sets the selected filter item.
     * @param event The event.
     * @param v The v.
     * @param item The filter item.
     */
    setSelectedFilterItem = (event: any, v: any, item: IDiscoveryFilter) => {
        let existingData = cloneDeep(this.state.initialFilterData);
        let existingSelectedFilters = cloneDeep(this.state.selectedFilters);
        let index = existingData.findIndex(filter => filter.filterId === item.key);
        let selectedFilter: IDiscoveryTreeFilter = existingData.find(filter => filter.filterId === item.key);

        if (selectedFilter && index !== -1) {
            if (selectedFilter.dbValue[0] === dbValueOfAllFilterItem) {
                let filterItem = existingData.find(x => x.filterId === item.key);
                if (filterItem) {
                    var result = this.setSelectedSubFilterItem(filterItem, existingData, existingSelectedFilters, v.checked);
                    existingData = result.existingData;
                    existingSelectedFilters = result.existingSelectedFilters;
                }
            }
            else {
                let subFilters = existingData.filter(x => x.parentId === existingData[index].filterId && x.isChecked !== v.checked);
                if (subFilters.length) {
                    existingData[index].isChecked = v.checked;
                    if (existingData[index].enabled === true) {
                        existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                    }
                    subFilters.forEach(subFilter => {
                        let subFilterItems = existingData.filter(x => x.parentId === subFilter.filterId && x.isChecked !== v.checked);
                        if (subFilterItems.length) {
                            let index = existingData.findIndex(filter => filter.filterId === subFilter.filterId);
                            if (index !== -1) {
                                existingData[index].isChecked = v.checked;
                                if (existingData[index].enabled === true) {
                                    existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                                }
                            }
                            subFilterItems.forEach(subFilterItem => {
                                var result = this.setSelectedSubFilterItem(subFilterItem, existingData, existingSelectedFilters, v.checked);
                                existingData = result.existingData;
                                existingSelectedFilters = result.existingSelectedFilters;
                            })
                        }
                        else {
                            let index = existingData.findIndex(filter => filter.filterId === subFilter.filterId);
                            if (index !== -1) {
                                existingData[index].isChecked = v.checked;
                                if (existingData[index].enabled === true) {
                                    existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                                }
                            }
                        }
                    })
                }
                else {
                    existingData[index].isChecked = v.checked;
                    if (existingData[index].enabled === true) {
                        existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                    }
                }
            }
        }

        if (existingData[index].dbValue[0] !== dbValueOfAllFilterItem) {
            var result = this.selectDeselectParentFilterItem(existingData[index], existingData, existingSelectedFilters);
            existingData = result.existingData;
            existingSelectedFilters = result.existingSelectedFilters;
        }

        this.setState({ initialFilterData: existingData }, () => {
            this.getFilters();
            this.getFilterPillsData();
        });
        this.setState({ selectedFilters: existingSelectedFilters });
    }

    /**
     * Sets the selected sub filter item.
     * @param filterItem The filter item.
     * @param existingData The existing filter data.
     * @param existingSelectedFilters The existing selected filters.
     * @param isChecked The boolean value indicating whether the filter item is selected or not.
     */
    setSelectedSubFilterItem = (filterItem: IDiscoveryTreeFilter, existingData: IDiscoveryTreeFilter[], existingSelectedFilters: any, isChecked: boolean) => {
        if (filterItem) {
            let subFilters = existingData.filter(x => x.parentId === filterItem?.parentId && x.isChecked !== isChecked);
            if (subFilters.length) {
                subFilters.forEach(subFilter => {
                    let subFilterItems = existingData.filter(x => x.parentId === subFilter.filterId && x.isChecked !== isChecked);
                    if (subFilterItems.length) {
                        let index = existingData.findIndex(filter => filter.filterId === subFilter.filterId);
                        if (index !== -1) {
                            existingData[index].isChecked = isChecked;
                            if (existingData[index].enabled === true) {
                                existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                            }
                        }
                        subFilterItems.forEach(subFilterItem => {
                            var result = this.setSelectedSubFilterItem(subFilterItem, existingData, existingSelectedFilters, isChecked);
                            existingData = result.existingData;
                            existingSelectedFilters = result.existingSelectedFilters;
                        })
                    }
                    else {
                        let index = existingData.findIndex(filter => filter.filterId === subFilter.filterId);
                        if (index !== -1) {
                            existingData[index].isChecked = isChecked;
                            if (existingData[index].enabled === true) {
                                existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                            }
                        }
                    }
                })
            }
            else {
                let index = existingData.findIndex(filter => filter.filterId === filterItem!.filterId);
                if (index !== -1) {
                    existingData[index].isChecked = isChecked;
                    if (existingData[index].enabled === true) {
                        existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                    }
                }
            }
        }
        return { existingData: existingData, existingSelectedFilters: existingSelectedFilters };
    }

    /**
     * Deselects the selected filter item.
     * @param event The event.
     * @param v The v.
     * @param item The filter item.
     */
    deselectHiddenFilterItem = (filterData: IDiscoveryTreeFilter[]) => {
        let existingData = filterData;
        let existingSelectedFilters = cloneDeep(this.state.selectedFilters);
        let filterGroups = existingData.filter(x => x.parentId === FilterGroupParentId);
        filterGroups.forEach((group: any) => {
            let filtersValues = existingData.filter(x => x.parentId === group.filterId && !x.isVisible);
            filtersValues.forEach(filterValue => {
                var result = this.deselectHiddenSubFilterItem(filterValue, existingData, existingSelectedFilters);
                existingData = result.existingData;
                existingSelectedFilters = result.existingSelectedFilters;
            });
        });
        this.setState({ initialFilterData: existingData }, () => {
            this.getFilters();
            this.getFilterPillsData();
        });

        this.setState({ selectedFilters: existingSelectedFilters }, () => this.searchAndFilterAsync(this.state.selectedFilters));
    }

    /**
     * Deselects the selected sub filter item.
     * @param item The filter item.
     * @param existingData The existing filter data.
     */
    deselectHiddenSubFilterItem = (item: IDiscoveryTreeFilter, existingData: IDiscoveryTreeFilter[], existingSelectedFilters: any) => {
        if (item) {
            let subFilters = existingData.filter(x => x.parentId === item?.filterId);
            if (subFilters.length) {
                subFilters.forEach(subFilter => {
                    let subFilterItems = existingData.filter(x => x.parentId === subFilter.filterId);
                    if (subFilterItems.length) {
                        let index = existingData.findIndex(filter => filter.filterId === subFilter.filterId);
                        if (index !== -1) {
                            existingData[index].isChecked = false;
                            if (existingData[index].enabled === true) {
                                existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                            }
                        }
                        subFilterItems.forEach(subFilterItem => {
                            let index = existingData.findIndex(filter => filter.filterId === subFilterItem.filterId);
                            if (index !== -1) {
                                existingData[index].isChecked = false;
                                if (existingData[index].enabled === true) {
                                    existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                                }
                            }
                            var result = this.deselectHiddenSubFilterItem(subFilterItem, existingData, existingSelectedFilters);
                            existingData = result.existingData;
                            existingSelectedFilters = result.existingSelectedFilters;
                        })
                    }
                    else {
                        let index = existingData.findIndex(filter => filter.filterId === subFilter.filterId);
                        if (index !== -1) {
                            existingData[index].isChecked = false;
                            if (existingData[index].enabled === true) {
                                existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                            }
                        }
                    }
                })
            }
            else {
                let index = existingData.findIndex(filter => filter.filterId === item!.filterId);
                if (index !== -1) {
                    existingData[index].isChecked = false;
                    if (existingData[index].enabled === true) {
                        existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                    }
                }
            }
        }
        return { existingData: existingData, existingSelectedFilters: existingSelectedFilters };
    }

    /**
     * Selects or deselects parent filter item.
     * @param filterItem The filter item.
     * @param existingData The existing filter data.
     */
    selectDeselectParentFilterItem = (filterItem: IDiscoveryTreeFilter, existingData: IDiscoveryTreeFilter[], existingSelectedFilters: any) => {
        let isUnselectedFilter = existingData.some(filter => filter.parentId === filterItem.parentId && filter.isChecked === false);
        if (isUnselectedFilter) {
            let parentIndex = existingData.findIndex(filter => filter.filterId === filterItem.parentId && filter.dbValue[0] !== 0);
            if (parentIndex !== -1) {
                existingData[parentIndex].isChecked = false;
                if (existingData[parentIndex].enabled === true) {
                    existingSelectedFilters = this.prepareSelectedFiltersList(existingData[parentIndex], existingData[parentIndex].isChecked, existingSelectedFilters);
                }
                var result = this.selectDeselectParentFilterItem(existingData[parentIndex], existingData, existingSelectedFilters);
                existingData = result.existingData;
                existingSelectedFilters = result.existingSelectedFilters;
            }
            else {
                let isUnselected = existingData.some(filter => filter.parentId === filterItem.parentId && filter.isChecked === false && filter.dbValue[0] !== dbValueOfAllFilterItem);
                if (isUnselected) {
                    let indexOfAll = existingData.findIndex(filter => filter.parentId === filterItem.parentId && filter.dbValue[0] === dbValueOfAllFilterItem);
                    if (indexOfAll !== -1) {
                        existingData[indexOfAll].isChecked = false;
                        if (existingData[indexOfAll].enabled === true) {
                            existingSelectedFilters = this.prepareSelectedFiltersList(existingData[indexOfAll], existingData[indexOfAll].isChecked, existingSelectedFilters);
                        }
                    }
                }
                else {
                    let indexOfAll = existingData.findIndex(filter => filter.parentId === filterItem.parentId && filter.dbValue[0] === dbValueOfAllFilterItem);
                    if (indexOfAll !== -1) {
                        existingData[indexOfAll].isChecked = true;
                        if (existingData[indexOfAll].enabled === true) {
                            existingSelectedFilters = this.prepareSelectedFiltersList(existingData[indexOfAll], existingData[indexOfAll].isChecked, existingSelectedFilters);
                        }
                    }
                }
            }
        }
        else {
            let parentIndex = existingData.findIndex(filter => filter.filterId === filterItem.parentId && filter.dbValue[0] !== 0);
            if (parentIndex !== -1) {
                existingData[parentIndex].isChecked = true;
                if (existingData[parentIndex].enabled === true) {
                    existingSelectedFilters = this.prepareSelectedFiltersList(existingData[parentIndex], existingData[parentIndex].isChecked, existingSelectedFilters);
                }
                var result = this.selectDeselectParentFilterItem(existingData[parentIndex], existingData, existingSelectedFilters);
                existingData = result.existingData;
                existingSelectedFilters = result.existingSelectedFilters;
            }
            else {
                let isUnselected = existingData.some(filter => filter.parentId === filterItem.parentId && filter.isChecked === false && filter.dbValue[0] !== dbValueOfAllFilterItem);
                if (isUnselected) {
                    let indexOfAll = existingData.findIndex(filter => filter.parentId === filterItem.parentId && filter.dbValue[0] === dbValueOfAllFilterItem);
                    if (indexOfAll !== -1) {
                        existingData[indexOfAll].isChecked = false;
                        if (existingData[indexOfAll].enabled === true) {
                            existingSelectedFilters = this.prepareSelectedFiltersList(existingData[indexOfAll], existingData[indexOfAll].isChecked, existingSelectedFilters);
                        }
                    }
                }
                else {
                    let indexOfAll = existingData.findIndex(filter => filter.parentId === filterItem.parentId && filter.dbValue[0] === dbValueOfAllFilterItem);
                    if (indexOfAll !== -1) {
                        existingData[indexOfAll].isChecked = true;
                        if (existingData[indexOfAll].enabled === true) {
                            existingSelectedFilters = this.prepareSelectedFiltersList(existingData[indexOfAll], existingData[indexOfAll].isChecked, existingSelectedFilters);
                        }
                    }
                }
            }
        }

        return { existingData: existingData, existingSelectedFilters: existingSelectedFilters };
    }

    /**
     * Prepares the list of selected filters.
     * @param item The filter Item.
     * @param isChecked Indicates whether filter item is checked or not.
     * @param existingSelectedFilters The existing selected Filters.
     */
    prepareSelectedFiltersList = (item: IDiscoveryTreeFilter, isChecked: boolean, existingSelectedFilters: IDiscoveryTreeSelectedFilters[]) => {
        let selectedFilters = existingSelectedFilters;
        let superNodeId = this.getSuperFilterIdForSelectedFilter(item.filterId);

        let filter = selectedFilters.find((filter: IDiscoveryTreeSelectedFilters) =>
            filter.type === superNodeId);

        if (filter) {
            let filterItem = filter.filters?.find((x: IDiscoveryTreeFilter) => x.filterId === item.filterId);

            if (isChecked) {
                if (!filterItem) {
                    let filters = filter.filters ?? [];
                    filters.push(item);
                    filter.filters = filters;
                }
            }
            else {
                if (filterItem) {
                    let filters = filter.filters.filter((x: IDiscoveryTreeFilter) => x.filterId !== item.filterId);
                    filter.filters = filters;

                    if (filters.length === 0) {
                        selectedFilters = selectedFilters.filter((x: IDiscoveryTreeSelectedFilters) => x.type !== filter!.type);
                    }
                }
            }
        }
        else {
            if (isChecked) {
                selectedFilters.push({
                    type: superNodeId,
                    filters: [item]
                } as IDiscoveryTreeSelectedFilters);
            }
        }

        return selectedFilters;
    }

    /**
     * Returns super filter Ids of selected filter.
     * @param selectedItemFilterId Selected filter item Id.
     */
    getSuperFilterIdForSelectedFilter = (selectedItemFilterId: number) => {
        let filters = cloneDeep(this.state.initialFilterData);
        let parentNodes = this.getParentNodesForFilters(selectedItemFilterId, filters);
        let filterGroupNode = parentNodes.find(x => x.parentId === FilterGroupParentId);
        let superNode = parentNodes.find(x => x.parentId === filterGroupNode.filterId);
        return superNode.filterId;
    }

    /**
     * Gets the parent nodes for filters.
     * @param filterId The filter Id.
     * @param filters The filters.
     */
    getParentNodesForFilters = (filterId, filters) => {
        var leafNodeData = filters.find(x => x.filterId === filterId);

        if (leafNodeData) {
            return [].concat(leafNodeData, this.getParentNodesForFilters(leafNodeData.parentId, filters));
        }

        return [];
    }

    // Gets discovery tree taxonomy.
    getDiscoveryTreeTaxonomyAsync = async () => {
        this.setState({ isLoading: true }, this.getFilterPillsData);
        let response = await getTaxonomyAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let treeTaxonomy = response.data as ITaxonomyElement[];

            let taxonomyNodeIds: string[] = treeTaxonomy.map((taxonomyElement: ITaxonomyElement) => taxonomyElement.taxonomyId);

            let parentIds = treeTaxonomy.filter(x => x.parentId).map(y => y.parentId);

            let leafNodes = treeTaxonomy.filter(x => parentIds.indexOf(Number(x.taxonomyId)) === -1);
            let leafNodeIds = leafNodes.map(y => y.taxonomyId);

            for (let i = 0; i < leafNodes.length; i++) {
                treeTaxonomy.push({
                    taxonomyId: leafNodes[i].taxonomyId + "_loading",
                    parentId: Number(leafNodes[i].taxonomyId),
                    title: "Loading...",
                    type: leafNodes[i].nodeTypeId,
                    nodeTypeId: leafNodes[i].nodeTypeId,
                } as ITaxonomyElement);
            }

            this.originalTaxonomyTree = treeTaxonomy;

            this.setState({
                taxonomyTree: treeTaxonomy,
                taxonomyFolderNodeIds: taxonomyNodeIds,
                leafNodeIds: leafNodeIds,
                isLoading: false
            }, this.getFilterPillsData);
        }
        else {
            this.setState({
                status: { id: this.state.status.id + 1, message: this.localize("failedToLoadTaxonomyError"), type: ActivityStatus.Error },
                isLoading: false
            }, this.getFilterPillsData);
        }
    }

    /**
     * Handles search and filter taxonomy.
     * @param selectedFilters The selected filters.
     */
    searchAndFilterAsync = async (selectedFilters: IDiscoveryTreeSelectedFilters[]) => {
        if (!selectedFilters?.length && !this.state.searchString && !this.state.selectedKeywords.length) {
            this.setState((prevState: IDiscoveryHomeState) => ({
                taxonomyTree: [],
                nodesExpandedAtLeastOnce: [],
                selectedTreeItem: undefined,
                statusBar: [],
                taxonomyKey: prevState.taxonomyKey + 1,
                flatListData: []
            }));

            return;
        }

        this.setState({ isLoading: true }, this.getFilterPillsData);

        let selectedKeywordIds: number[] = [];
        this.state.selectedKeywords.forEach((keyword: IKeyword) => {
            selectedKeywordIds.push(Number(keyword.keywordId))
        });

        let searchAndFilterOptions: IDiscoveryTreeSearchAndFilter = {
            searchStrings: this.state.searchString ? [this.state.searchString]: [],
            searchKeywords: selectedKeywordIds,
            selectedFilters: selectedFilters
        }

        let response = await searchAndFilterDiscoveryTreeResourcesAsync(searchAndFilterOptions, this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let data = response.data as ITreeNodeDataElement;

            this.prepareDiscoveryTreeAfterSearchAndFilter(data);
            let flatListData = this.getFlatListData(data);

            this.setState({ flatListData, isLoading: false }, this.getFilterPillsData);
        }
        else {
            this.setState((prevState: IDiscoveryHomeState) => ({
                taxonomyTree: [],
                nodesExpandedAtLeastOnce: [],
                selectedTreeItem: undefined,
                statusBar: [],
                taxonomyKey: prevState.taxonomyKey + 1,
                flatListData: [],
                isLoading: false,
                status: { id: prevState.status.id + 1, message: this.localize("failedToLoadTreeNodeData"), type: ActivityStatus.Error }
            }), this.getFilterPillsData);
        }
    }

    /**
     * Prepares the discovery tre after filter and search.
     * @param data The searched and filtered data.
     */
    prepareDiscoveryTreeAfterSearchAndFilter = (data: ITreeNodeDataElement) => {
        let tree = cloneDeep(this.originalTaxonomyTree) as ITaxonomyElement[];

        if (!tree?.length
            || (!data.researchProjects?.length
                && !data.researchProposals?.length
                && !data.researchRequests?.length
                && !data.sponsors?.length
                && !data.partners?.length
                && !data.cois?.length
                && !data.newsArticles?.length
                && !data.events?.length
                && !data.users?.length
                && !data.athenaInfoResources?.length
                && !data.athenaTools?.length)) {
            this.setState({
                taxonomyTree: [],
                nodesExpandedAtLeastOnce: [],
                selectedTreeItem: undefined,
                statusBar: []
            });

            return;
        }

        let nodesExpandedAtLeastOnce = tree.map((node: ITaxonomyElement) => node.taxonomyId);
        let updatedTree: ITaxonomyElement[] = [];
        let maxId: number = 0;
        let filledTaxonomyIds: string[] = [];

        for (let i = 0; i < tree.length; i++) {
            let treeElement = tree[i];
            let addParentNodes: boolean = false;

            if (data.researchProjects?.length > 0) {
                let researchProjectNodes = data.researchProjects
                    .filter((researchProject: IResearchProject) => researchProject.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((researchProject: IResearchProject) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: researchProject.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: researchProject.title,
                            type: researchProject.nodeTypeId,
                            tooltip: researchProject.abstract,
                            dataValue: researchProject,
                            nodeTypeId: researchProject.nodeTypeId,
                            date: this.getItemDate(researchProject.nodeTypeId, researchProject, "YYYY-MM-DD")
                        } as ITaxonomyElement;
                    });

                if (researchProjectNodes.length > 0) {
                    updatedTree.push(...researchProjectNodes);
                    addParentNodes = true;
                }
            }

            if (data.researchProposals?.length > 0) {
                let researchProposalNodes = data.researchProposals
                    .filter((researchProposal: IResearchProposal) => researchProposal.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((researchProposal: IResearchProposal) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: researchProposal.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: researchProposal.title,
                            type: researchProposal.nodeTypeId,
                            tooltip: researchProposal.description,
                            dataValue: researchProposal,
                            nodeTypeId: researchProposal.nodeTypeId,
                            date: this.getItemDate(researchProposal.nodeTypeId, researchProposal, "YYYY-MM-DD")
                        } as ITaxonomyElement
                    });

                if (researchProposalNodes.length > 0) {
                    updatedTree.push(...researchProposalNodes);
                    addParentNodes = true;
                }
            }

            if (data.researchRequests?.length > 0) {
                let researchRequestsNodes = data.researchRequests
                    .filter((researchRequest: IResearchRequest) => researchRequest.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((researchRequest: IResearchRequest) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: researchRequest.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: researchRequest.title,
                            type: researchRequest.nodeTypeId,
                            tooltip: researchRequest.description,
                            dataValue: researchRequest,
                            nodeTypeId: researchRequest.nodeTypeId,
                            date: this.getItemDate(researchRequest.nodeTypeId, researchRequest, "YYYY-MM-DD")
                        } as ITaxonomyElement;
                    });

                if (researchRequestsNodes.length > 0) {
                    updatedTree.push(...researchRequestsNodes);
                    addParentNodes = true;
                }
            }

            if (data.sponsors?.length > 0) {
                let sponsorsNodes = data.sponsors
                    .filter((sponsor: ISponsorDetails) => sponsor.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((sponsor: ISponsorDetails) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: sponsor.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: sponsor.title,
                            type: sponsor.nodeTypeId,
                            tooltip: sponsor.description,
                            dataValue: sponsor,
                            nodeTypeId: sponsor.nodeTypeId,
                        } as ITaxonomyElement;
                    });

                if (sponsorsNodes.length > 0) {
                    updatedTree.push(...sponsorsNodes);
                    addParentNodes = true;
                }
            }

            if (data.partners?.length > 0) {
                let partnersNodes = data.partners
                    .filter((partner: IPartnerDetails) => partner.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((partner: IPartnerDetails) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: partner.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: partner.title,
                            type: partner.nodeTypeId,
                            tooltip: partner.description,
                            dataValue: partner,
                            nodeTypeId: partner.nodeTypeId,
                        } as ITaxonomyElement;
                    });

                if (partnersNodes.length > 0) {
                    updatedTree.push(...partnersNodes);
                    addParentNodes = true;
                }
            }

            if (data.cois?.length > 0) {
                let coiNodes = data.cois
                    .filter((coi: ICoi) => coi.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((coi: ICoi) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: coi.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: coi.coiName,
                            type: coi.nodeTypeId,
                            tooltip: coi.coiDescription,
                            dataValue: coi,
                            nodeTypeId: coi.nodeTypeId,
                            date: this.getItemDate(coi.nodeTypeId, coi, "YYYY-MM-DD")
                        } as ITaxonomyElement;
                    });

                if (coiNodes.length > 0) {
                    updatedTree.push(...coiNodes);
                    addParentNodes = true;
                }
            }

            if (data.newsArticles?.length > 0) {
                let newsArticleNodes = data.newsArticles
                    .filter((newsArticle: INews) => newsArticle.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((newsArticle: INews) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: newsArticle.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: newsArticle.title,
                            type: newsArticle.nodeTypeId,
                            tooltip: newsArticle.body,
                            dataValue: newsArticle,
                            nodeTypeId: newsArticle.nodeTypeId,
                            date: this.getItemDate(newsArticle.nodeTypeId, newsArticle, "YYYY-MM-DD")
                        } as ITaxonomyElement;
                    });

                if (newsArticleNodes.length > 0) {
                    updatedTree.push(...newsArticleNodes);
                    addParentNodes = true;
                }
            }

            if (data.events?.length > 0) {
                let athenaEventsNodes = data.events
                    .filter((event: IAthenaEvent) => event.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((event: IAthenaEvent) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: event.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: event.title,
                            type: event.nodeTypeId,
                            tooltip: event.description,
                            dataValue: event,
                            nodeTypeId: event.nodeTypeId,
                            date: this.getItemDate(event.nodeTypeId, event, "YYYY-MM-DD")
                        } as ITaxonomyElement;
                    });

                if (athenaEventsNodes.length > 0) {
                    updatedTree.push(...athenaEventsNodes);
                    addParentNodes = true;
                }
            }

            if (data.users?.length > 0) {
                let usersNodes = data.users
                    .filter((user: IUserSettings) => user.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((user: IUserSettings) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: user.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: user.userDisplayName,
                            type: user.nodeTypeId,
                            tooltip: user.emailAddress,
                            dataValue: user,
                            nodeTypeId: user.nodeTypeId,
                        } as ITaxonomyElement;
                    });

                if (usersNodes.length > 0) {
                    updatedTree.push(...usersNodes);
                    addParentNodes = true;
                }
            }

            if (data.athenaInfoResources?.length > 0) {
                let infoResourcesNodes = data.athenaInfoResources
                    .filter((infoResource: IAthenaInfoResource) => infoResource.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((infoResource: IAthenaInfoResource) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: infoResource.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: infoResource.title,
                            type: infoResource.nodeTypeId,
                            tooltip: infoResource.description,
                            dataValue: infoResource,
                            nodeTypeId: infoResource.nodeTypeId,
                            date: this.getItemDate(infoResource.nodeTypeId, infoResource, "YYYY-MM-DD")
                        } as ITaxonomyElement;
                    });

                if (infoResourcesNodes.length > 0) {
                    updatedTree.push(...infoResourcesNodes);
                    addParentNodes = true;
                }
            }

            if (data.athenaTools?.length > 0) {
                let athenaToolsNodes = data.athenaTools
                    .filter((athenaTool: IAthenaTool) => athenaTool.keywords?.some((nodeKeyword: number) => treeElement.keywords?.some((treeKeyword: number) => nodeKeyword === treeKeyword)))
                    .map((athenaTool: IAthenaTool) => {
                        maxId = maxId + 1;

                        return {
                            taxonomyId: athenaTool.tableId + TableIdAndMaxIdSeparatorInFilteredDiscoveryTree + maxId,
                            parentId: Number(treeElement.taxonomyId),
                            title: athenaTool.title,
                            type: athenaTool.nodeTypeId,
                            tooltip: athenaTool.description,
                            dataValue: athenaTool,
                            nodeTypeId: athenaTool.nodeTypeId,
                        } as ITaxonomyElement;
                    });

                if (athenaToolsNodes.length > 0) {
                    updatedTree.push(...athenaToolsNodes);
                    addParentNodes = true;
                }
            }

            if (addParentNodes) {
                let discoveryTreeNodes = this.getParentNodes(treeElement.taxonomyId, tree);
                let parentNodes: any[] = discoveryTreeNodes.filter((treenode: any) =>
                    !filledTaxonomyIds.some((filledTaxonomyId: string) => treenode.taxonomyId === filledTaxonomyId));

                filledTaxonomyIds.push(...parentNodes.map((parentNode: any) => parentNode.taxonomyId));

                if (parentNodes && parentNodes.length > 0) {
                    updatedTree.push(...parentNodes);
                }
            }
        }

        this.setState({
            taxonomyTree: updatedTree,
            nodesExpandedAtLeastOnce,
            selectedTreeItem: undefined,
            statusBar: []
        });
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    handleTokenAccessFailure = (error: string) => {
        this.props.history.push("/signin");
    }

    /**
     * Gets the deep link of node selected.
     * @param deepLinkNodeId The node Id.
     * @param deepLinkNodeParentId The parent Id.
     * @param nodeTypeId The node type Id.
     */
    selectedDeepLinkNode = async (deepLinkNodeId: any, deepLinkNodeParentId: any, nodeTypeId: number) => {
        let treeTaxonomy = cloneDeep(this.state.taxonomyTree);

        if (nodeTypeId) {
            let nodeType: IDiscoveryTreeNodeType | undefined = this.state.nodeTypeData
                .find((nodeType: IDiscoveryTreeNodeType) => nodeType.nodeTypeId === nodeTypeId);

            if (nodeType) {
                var jsonFile = nodeType.jsonFile;
                if (DiscoveryNodeFileNames.AthenaTaxonomy === jsonFile) {
                    this.expandQuickAccessNode(deepLinkNodeId);
                }
                else {
                    let deepLinkParentNode = treeTaxonomy.find(x => x.taxonomyId === deepLinkNodeParentId);
                    if (deepLinkParentNode) {
                        await this.treeViewRef?.instance?.expandRow(parseInt(deepLinkNodeParentId));
                        await this.expandNodesUptoParentNode(parseInt(deepLinkNodeParentId), this.state.taxonomyTree);
                        let nodeToBeSelected = this.state.taxonomyTree.find(x => x.taxonomyId.toString() === deepLinkNodeId);
                        if (nodeToBeSelected) {
                            this.setStatusBarHierarchy(nodeToBeSelected);
                        }
                    }
                }
            }
        }
    }

    // Get all keywords.
    getAllKeywords = async () => {
        let response = await getAllKeywordsAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let keywords = response.data as IKeyword[];
            this.setState({ allKeywords: keywords, isLoadingAllKeywords: false });
        }
        else {
            this.setState({ isLoadingAllKeywords: false });
        }
    }

    /**
     * Handles context menu item click.
     * @param e The event.
     */
    contextMenuItemClick = async (e) => {
        if (e.itemData && e.itemData.id === 'display-details') {
            this.setState({
                closeDisplayDetails: false,
                selectedMenuItemId: 'display-details'
            });
        }
        else if (e.itemData && e.itemData.id === 'coi-home-page') {
            this.setState({ selectedMenuItemId: 'coi-home-page' });
            this.redirectToCoiHomePage(this.state.selectedTreeItem);
        }
        else if (e.itemData && e.itemData.id === 'follow-item') {
            this.setState({ selectedMenuItemId: 'follow-item' });
            this.followResource(this.state.selectedTreeItem);
        }
        else if (e.itemData && e.itemData.id === 'share-with-colleague') {
            this.setState({ selectedMenuItemId: 'share-with-colleague' });
            this.shareLink(this.state.selectedTreeItem);
        }
        else if (e.itemData && e.itemData.id === 'interested-users') {
            this.setState({
                interestedUsers: [],
                selectedMenuItemId: 'interested-users'
            });
            this.getInterestedUsers(this.state.selectedTreeItem);
        }
        else if (e.itemData && e.itemData.id === 'interested-sponsors') {
            this.setState({
                interestedSponsors: [],
                selectedMenuItemId: 'interested-sponsors'
            });
            this.getInterestedSponsors(this.state.selectedTreeItem);
        }
        else if (e.itemData && e.itemData.id === 'suggest-research-project') {
            this.setState({ selectedMenuItemId: 'suggest-research-project' });
            this.createNewResearchProject();
        }
        else if (e.itemData && e.itemData.id === 'rate-comment-item') {
            this.setState({
                closeDisplayDetails: false,
                selectedMenuItemId: 'rate-comment-item'
            });
        }
        else if (e.itemData && e.itemData.id === 'add-to-collection') {
            this.setState({ selectedMenuItemId: 'add-to-collection' });
            this.addToCollection(this.state.selectedTreeItem);
        }
        else if (e.itemData && e.itemData.id === 'add-to-quick-access-list') {
            this.setState({ selectedMenuItemId: 'add-to-quick-access-list' });
            this.addQuickAccessItem(this.state.selectedTreeItem);
        }
    }

    /**
     * Redirects to COI's home page.
     * @param selectedTreeItem The selected tree item.
     */
    redirectToCoiHomePage = (selectedTreeItem: any) => {
        var channelId = selectedTreeItem?.dataValue?.channelId;
        var teamId = selectedTreeItem?.dataValue?.teamId;
        if (teamId && channelId) {
            microsoftTeams.executeDeepLink("https://teams.microsoft.com/l/team/" + channelId + "/conversations?groupId=" + teamId);
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("teamNotFoundError"), type: ActivityStatus.Error } });
        }
    }

    /**
     * Generates link to share.
     * @param selectedTreeItem The selected tree item.
     */
    shareLink = (selectedTreeItem: any) => {
        var subEntityLabel = selectedTreeItem?.title;
        let taxonomyId = selectedTreeItem?.taxonomyId?.toString();
        let index = taxonomyId.indexOf('_');
        if (index !== -1) {
            taxonomyId = taxonomyId.substring(0, index);
        }
        var subEntityId = {
            taxonomyId: taxonomyId,
            parentId: selectedTreeItem?.parentId,
            nodeTypeId: selectedTreeItem?.nodeTypeId,
        }
        microsoftTeams.shareDeepLink({
            subEntityId: JSON.stringify(subEntityId), subEntityLabel: subEntityLabel
        });
    }

    /**
     * Adds node to collection.
     * @param selectedTreeItem The selected tree item.
     */
    addToCollection = (selectedTreeItem) => {
        let jsonFile;
        if (this.state.selectedTreeItem?.nodeTypeId) {
            let nodeType: IDiscoveryTreeNodeType | undefined = this.state.nodeTypeData
                .find((nodeType: IDiscoveryTreeNodeType) => nodeType.nodeTypeId === this.state.selectedTreeItem?.nodeTypeId);
            if (nodeType) {
                jsonFile = nodeType.jsonFile;
            }
        }

        microsoftTeams.tasks.startTask({
            title: this.localize("fillDetailsText"),
            height: Constants.taskModuleHeight,
            width: Constants.taskModuleWidth,
            url: getBaseUrl() + `/add-collection-item?itemTableId=${selectedTreeItem?.taxonomyId}&itemJsonFile=${jsonFile}`,
            fallbackUrl: getBaseUrl() + `/add-collection-item?itemTableId=${selectedTreeItem?.taxonomyId}&itemJsonFile=${jsonFile}`,
        });
    }

    // Opens the task module to create new research project.
    createNewResearchProject = () => {
        microsoftTeams.tasks.startTask({
            title: this.localize("fillDetailsText"),
            height: Constants.taskModuleHeight,
            width: Constants.taskModuleWidth,
            url: getBaseUrl() + `/new-research-proposal`,
            fallbackUrl: getBaseUrl() + `/new-research-proposal`,
        });
    }

    /**
     * Handles tree list node selection.
     * @param e The event.
     */
    onSelectionChanged = (e) => {
        const selectedData = e.selectedRowsData[0];
        if (selectedData) {
            this.setStatusBarHierarchy(selectedData);
        }
    }

    /**
     * Sets the status bar.
     * @param selectedNodeData Selected node data.
     */
    setStatusBarHierarchy = (selectedNodeData) => {
        if (!selectedNodeData) {
            return;
        }

        let parentIdsOfSelectedNode = this.getParentNodeIds(selectedNodeData.taxonomyId, this.state.taxonomyTree);
        parentIdsOfSelectedNode = parentIdsOfSelectedNode.reverse();

        let nodesTraversal = this.state.taxonomyTree.filter(x => parentIdsOfSelectedNode.indexOf(x.taxonomyId) > -1);
        if (selectedNodeData.nodeTypeId === TreeViewFolderNodeTypeValue) {
            nodesTraversal.push({ ...selectedNodeData });
        }
        this.setState({
            selectedTreeItem: selectedNodeData,
            statusBar: nodesTraversal
        });
    }

    /**
     * Adds menu items.
     * @param e The event.
     */
    addMenuItems = (e) => {
        if (e.target === 'content') {
            let data = e.row?.data;
            this.setState({ selectedTreeItem: data });
        }
        if (e.row?.data?.nodeTypeId) {
            let nodeType: IDiscoveryTreeNodeType | undefined = this.state.nodeTypeData
                .find((nodeType: IDiscoveryTreeNodeType) => nodeType.nodeTypeId === e.row?.data?.nodeTypeId);

            if (nodeType) {
                var jsonFile = nodeType.jsonFile;
                switch (jsonFile) {
                    case DiscoveryNodeFileNames.AthenaTaxonomy:
                        this.setState({ menuItems: this.researchAreaMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaResearchProjects:
                        this.setState({ menuItems: this.researchProjectMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaCommunities:
                        this.setState({ menuItems: this.coiMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaUsers:
                        this.setState({ menuItems: this.personMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaResearchRequests:
                        this.setState({ menuItems: this.researchRequestMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaResearchProposals:
                        this.setState({ menuItems: this.researchProposalMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaEvents:
                        this.setState({ menuItems: this.eventMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaPartners:
                        this.setState({ menuItems: this.partnerMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaSponsors:
                        this.setState({ menuItems: this.sponsorMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaNewsArticles:
                        this.setState({ menuItems: this.newsMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaSources:
                        this.setState({ menuItems: this.sourceInstitutionMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaInfoResources:
                        this.setState({ menuItems: this.informationTypesMenuItems });
                        break;
                    case DiscoveryNodeFileNames.AthenaTools:
                        this.setState({ menuItems: this.toolTypesMenuItems });
                        break;
                    default:
                        this.setState({ menuItems: [] });
                }
            }
            else {
                this.setState({ menuItems: [] });
            }
        }
        else {
            this.setState({ menuItems: [] });
        }
    }

    /**
     * Binds the node data.
     * @param nodeData The node data.
     */
    bindNodeTasks = async (nodeData: ITaxonomyElement) => {
        if (!nodeData || !nodeData.keywords?.length) {
            return;
        }

        let keywords = nodeData.keywords;

        this.setState({ isLoadingNodeData: true });

        let data = {} as ITreeNodeDataElement;
        let response = await getNodeDataAsync(keywords, this.handleTokenAccessFailure);

        let tree = cloneDeep(this.state.taxonomyTree) as ITaxonomyElement[];
        tree = tree.filter((node: ITaxonomyElement) => node.taxonomyId !== (nodeData.taxonomyId + "_loading"));

        this.setState({ isLoadingNodeData: false });

        let nodeToBeSelected = nodeData;

        let isLeafNode: boolean = this.state.leafNodeIds.indexOf(nodeData.taxonomyId) > -1;

        if (!response || response.status !== StatusCodes.OK || !response.data) {
            if (isLeafNode) {
                tree.push({
                    taxonomyId: nodeData.taxonomyId + "_nodata",
                    parentId: Number(nodeData.taxonomyId),
                    title: this.localize("discoveryTreeNodeDataNotAvailable")
                } as ITaxonomyElement);

                this.setState({ taxonomyTree: tree });
            }

            this.setState({
                selectedTreeItem: nodeToBeSelected,
                status: { id: this.state.status.id + 1, message: this.localize("failedToLoadTreeNodeData"), type: ActivityStatus.Error }
            });
            return;
        }

        data = response.data as ITreeNodeDataElement;

        if (!data.researchProjects?.length
            && !data.researchRequests?.length
            && !data.sponsors?.length
            && !data.partners?.length
            && !data.researchProposals?.length
            && !data.cois?.length
            && !data.newsArticles?.length
            && !data.events?.length
            && !data.users?.length
            && !data.athenaInfoResources?.length
            && !data.athenaTools?.length) {
            if (isLeafNode) {
                tree.push({
                    taxonomyId: nodeData.taxonomyId + "_nodata",
                    parentId: Number(nodeData.taxonomyId),
                    title: this.localize("discoveryTreeNodeDataNotAvailable")
                } as ITaxonomyElement);

                this.setState({ taxonomyTree: tree });
            }

            this.setState({
                selectedTreeItem: nodeToBeSelected
            });
            return;
        }

        let flatListData = cloneDeep(this.state.flatListData) as ITaxonomyElement[];

        let researchProjectsNodes: ITaxonomyElement[] = data.researchProjects.map((researchProject: IResearchProject) => {
            let nodeToAdd = {
                taxonomyId: researchProject.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: researchProject.title,
                type: researchProject.nodeTypeId,
                tooltip: researchProject.abstract,
                dataValue: researchProject,
                nodeTypeId: researchProject.nodeTypeId,
                date: this.getItemDate(researchProject.nodeTypeId, researchProject, "YYYY-MM-DD")
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...researchProjectsNodes);

        let researchRequestsNodes: ITaxonomyElement[] = data.researchRequests.map((researchRequest: IResearchRequest) => {
            let nodeToAdd = {
                taxonomyId: researchRequest.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: researchRequest.title,
                type: researchRequest.nodeTypeId,
                tooltip: researchRequest.details,
                dataValue: researchRequest,
                nodeTypeId: researchRequest.nodeTypeId,
                date: this.getItemDate(researchRequest.nodeTypeId, researchRequest, "YYYY-MM-DD")
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...researchRequestsNodes);

        let sponsorsNodes: ITaxonomyElement[] = data.sponsors.map((sponsor: ISponsorDetails) => {
            let nodeToAdd = {
                taxonomyId: sponsor.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: sponsor.firstName + " " + sponsor.lastName,
                type: sponsor.nodeTypeId,
                tooltip: sponsor.description,
                dataValue: sponsor,
                nodeTypeId: sponsor.nodeTypeId,
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...sponsorsNodes);

        let partnerNodes: ITaxonomyElement[] = data.partners.map((partner: IPartnerDetails) => {
            let nodeToAdd = {
                taxonomyId: partner.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: partner.firstName + " " + partner.lastName,
                type: partner.nodeTypeId,
                tooltip: partner.description,
                dataValue: partner,
                nodeTypeId: partner.nodeTypeId,
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...partnerNodes);

        let researchProposalNodes: ITaxonomyElement[] = data.researchProposals.map((researchProposal: IResearchProposal) => {
            let nodeToAdd = {
                taxonomyId: researchProposal.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: researchProposal.title,
                type: researchProposal.nodeTypeId,
                tooltip: researchProposal.description,
                dataValue: researchProposal,
                nodeTypeId: researchProposal.nodeTypeId,
                date: this.getItemDate(researchProposal.nodeTypeId, researchProposal, "YYYY-MM-DD")
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...researchProposalNodes);

        let coiNodes: ITaxonomyElement[] = data.cois.map((coi: ICoi) => {
            let nodeToAdd = {
                taxonomyId: coi.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: coi.coiName,
                type: coi.nodeTypeId,
                tooltip: coi.coiDescription,
                dataValue: coi,
                nodeTypeId: coi.nodeTypeId,
                date: this.getItemDate(coi.nodeTypeId, coi, "YYYY-MM-DD")
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...coiNodes);

        let newsArticleNodes: ITaxonomyElement[] = data.newsArticles.map((newsArticle: INews) => {
            let nodeToAdd = {
                taxonomyId: newsArticle.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: newsArticle.title,
                type: newsArticle.nodeTypeId,
                tooltip: newsArticle.body,
                dataValue: newsArticle,
                nodeTypeId: newsArticle.nodeTypeId,
                date: this.getItemDate(newsArticle.nodeTypeId, newsArticle, "YYYY-MM-DD")
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...newsArticleNodes);

        let eventNodes: ITaxonomyElement[] = data.events.map((event: IAthenaEvent) => {
            let nodeToAdd = {
                taxonomyId: event.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: event.title,
                type: event.nodeTypeId,
                tooltip: event.description,
                dataValue: event,
                nodeTypeId: event.nodeTypeId,
                date: this.getItemDate(event.nodeTypeId, event, "YYYY-MM-DD")
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...eventNodes);

        let userNodes: ITaxonomyElement[] = data.users.map((user: IUserSettings) => {
            let nodeToAdd = {
                taxonomyId: user.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: user.userDisplayName,
                type: user.nodeTypeId,
                tooltip: user.emailAddress,
                dataValue: user,
                nodeTypeId: user.nodeTypeId,
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...userNodes);

        let infoResourcesNodes: ITaxonomyElement[] = data.athenaInfoResources.map((infoResource: IAthenaInfoResource) => {
            let nodeToAdd = {
                taxonomyId: infoResource.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: infoResource.title,
                type: infoResource.nodeTypeId,
                tooltip: infoResource.description,
                dataValue: infoResource,
                nodeTypeId: infoResource.nodeTypeId,
                date: this.getItemDate(infoResource.nodeTypeId, infoResource, "YYYY-MM-DD")
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...infoResourcesNodes);

        let athenaToolNodes: ITaxonomyElement[] = data.athenaTools.map((athenaTool: IAthenaTool) => {
            let nodeToAdd = {
                taxonomyId: athenaTool.tableId,
                parentId: Number(nodeData.taxonomyId),
                title: athenaTool.title,
                type: athenaTool.nodeTypeId,
                tooltip: athenaTool.description,
                dataValue: athenaTool,
                nodeTypeId: athenaTool.nodeTypeId,
            } as ITaxonomyElement;

            let isFlatListHasData: boolean = flatListData.some((x: ITaxonomyElement) => x.taxonomyId === nodeToAdd.taxonomyId);

            if (!isFlatListHasData) {
                flatListData.push(nodeToAdd);
            }

            return nodeToAdd;
        });

        tree.push(...athenaToolNodes);

        this.setState({
            taxonomyTree: tree,
            selectedTreeItem: nodeToBeSelected,
            flatListData
        });
    }

    /**
     * Expands node till the parent node.
     * @param nodeId The node Id.
     * @param taxonomy The tree taxonomy.
     */
    expandNodesUptoParentNode = async (nodeId: any, taxonomy: any) => {
        let parentNodeIds = this.getParentNodeIds(nodeId, taxonomy);
        parentNodeIds = parentNodeIds.reverse();

        for (let i = 0; i < parentNodeIds.length; i++) {
            await this.treeViewRef?.instance?.expandRow(parentNodeIds[i]);
        }
    }

    /**
     * Gets the parent node Ids.
     * @param childNodeId The child node.
     * @param taxonomy The tree taxonomy.
     */
    getParentNodeIds = (childNodeId, taxonomy) => {
        var leafNodeData = taxonomy.find(x => x.taxonomyId === childNodeId);

        if (leafNodeData && leafNodeData.parentId) {
            return [].concat(leafNodeData.parentId, this.getParentNodeIds(leafNodeData.parentId, taxonomy));
        }

        return [];
    }

    /**
     * Recursive function to get parent nodes.
     * @param childNodeId The child node Id.
     * @param taxonomy The taxonomy.
     */
    getParentNodes = (childNodeId, taxonomy) => {
        var leafNodeData = taxonomy.find(x => x.taxonomyId === childNodeId);

        if (leafNodeData && leafNodeData.parentId !== undefined && leafNodeData.parentId !== null) {
            return [].concat(leafNodeData, this.getParentNodes(leafNodeData.parentId, taxonomy));
        }

        return [];
    }

    // Handles close button click of display details.
    onDisplayDetailsCloseIconClick = () => {
        this.setState({
            closeDisplayDetails: true,
            interestedUsers: [],
            interestedSponsors: [],
            selectedMenuItemId: ""
        });
    }

    // Saves the selected filters in database.
    saveFilterQueryAsync = async () => {
        this.setState({ isSavingQuery: true });

        var selectedFilterIds = this.state.initialFilterData
            .filter((filter: IDiscoveryTreeFilter) => filter.isChecked === true && !filter.isVisible)
            .map((filter: IDiscoveryTreeFilter) => { return filter.filterId });

        var selectedConfigureFilterIds = this.state.initialFilterData
            .filter((filter: IDiscoveryTreeFilter) => filter.isVisible === true)
            .map((filter: IDiscoveryTreeFilter) => { return filter.filterId });

        let discoveryTreePersistentData: IDiscoveryTreePersistentData = {
            selectedFilterIds,
            selectedConfigureFilterIds: selectedConfigureFilterIds
        }

        let response = await saveDiscoveryTreePersistentDataAsync(discoveryTreePersistentData, this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("saveFilterQuerySuccessMessage"), type: ActivityStatus.Success } });
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToSaveFilterQueryErrorMessage"), type: ActivityStatus.Error } });
        }

        this.setState({ isSavingQuery: false });
    }

    // Gets the persistent data of logged-in user.
    getPersistentDataAsync = async () => {
        let response = await getUserPersistentDataAsync(this.handleTokenAccessFailure);

        if (response && (response.status === StatusCodes.OK || response.status === StatusCodes.NO_CONTENT)) {
            return response.data ? response.data : null;
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error } });
            return null;
        }
    }

    // Event handler called when click on button to clear applied filters.
    // This will clear all applied filters and search query and reset the discovery tree.
    onClearAppliedFiltersAndSearchQuery = () => {
        this.setState({ isClearingAppliedFiltersAndSearchQuery: true });

        let initialFilterData: IDiscoveryTreeFilter[] = cloneDeep(this.state.initialFilterData);
        let selectedFilters: IDiscoveryTreeFilter[] = initialFilterData.filter((data: IDiscoveryTreeFilter) => data.isChecked === true);

        for (let i = 0; i < selectedFilters.length; i++) {
            selectedFilters[i].isChecked = false;
        }

        this.setState({
            initialFilterData,
            selectedFilters: [],
            searchString: "",
            isClearingAppliedFiltersAndSearchQuery: false,
            selectedKeywords: []
        }, () => {
            this.getFilters();
            this.getFilterPillsData();
            this.searchAndFilterAsync([]);
        });
    }

    /**
     * Returns the title of research importance.
     * @param importanceId The importance Id.
     */
    getResearchImportanceTitle = (importanceId: number) => {
        var reserachImportance = this.state.researchImportanceData.find((researchImportance: IAthenaResearchImportance) => researchImportance.importanceId === importanceId);
        if (reserachImportance) {
            return reserachImportance.title;
        }
        return "NA";
    }

    /**
     * Returns the title of research priority.
     * @param priorityId The priority Id.
     */
    getResearchPriorityTitle = (priorityId: number) => {
        var reserachPriority = this.state.researchPriorities.find((reserachPriority: IAthenaResearchPriority) => reserachPriority.priorityId === priorityId);
        if (reserachPriority) {
            return reserachPriority.title;
        }
        return "NA";
    }

    // Renders research request details.
    renderResearchRequestDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsTitle')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.title ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsDescription')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.description ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsTaskModule')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.details ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('priorityTitle')}:`} weight="semibold" />
                    <Text content={this.getResearchPriorityTitle(this.state.selectedTreeItem?.dataValue?.priority)} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('lastUpdatedTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.lastUpdate ? moment(this.state.selectedTreeItem?.dataValue?.lastUpdate).format("DD-MMM-YYYY hh:mm A") : "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('focusQuestion1Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.focusQuestion1 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('focusQuestion2Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.focusQuestion2 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('focusQuestion3Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.focusQuestion3 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('focusQuestion4Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.focusQuestion4 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('focusQuestion5Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.focusQuestion5 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('topicTypeTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.topicType ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('startDateTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.startDate ? moment(this.state.selectedTreeItem?.dataValue?.startDate).format("DD-MMM-YYYY hh:mm A") : "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('completionTimeTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.completionTime ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('endorsementsTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.endorsements ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('potentialFundingTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.potentialFunding ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('desiredCurriculum1Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.desiredCurriculum1 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('desiredCurriculum2Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.desiredCurriculum2 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('desiredCurriculum3Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.desiredCurriculum3 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('desiredCurriculum4Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.desiredCurriculum4 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('desiredCurriculum5Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.desiredCurriculum5 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('erbTrbOrgTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.erbTrbOrg ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('importanceTitle')}:`} weight="semibold" />
                    <Text content={this.getResearchImportanceTitle(this.state.selectedTreeItem?.dataValue?.importance)} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('sponsorsTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.sponsors ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('statusTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.status ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('fiscalYearTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.fiscalYear ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('topicNotesTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.topicNotes ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('createdDateTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.createDate ? moment(this.state.selectedTreeItem?.createDate?.createDate).format("DD-MMM-YYYY hh:mm A") : "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('irefTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.irefTitle ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('completionDateTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.completionDate ? moment(this.state.selectedTreeItem?.createDate?.completionDate).format("DD-MMM-YYYY hh:mm A") : "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('contributingStudentsCountTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.contributingStudentsCount ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('notesByUserTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.notesByUser ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    /**
     * Display keywords associated with selected item on discovery tree.
     * @param keywordIds The selected item's keyword Ids.
     */
    renderKeywordsInDisplayDetailsSection = (keywordIds: number[] | undefined) => {
        if (this.state.isLoadingAllKeywords) {
            return <Text content={this.localize("loadingLabel")} />;
        }

        if (!this.state.allKeywords?.length || !keywordIds?.length) {
            return <Text content="NA" />;
        }

        let keywordIdsStringArray = keywordIds.map(String);

        let keywordsString = this.state.allKeywords
            .filter((keyword: IKeyword) => keywordIdsStringArray.some((keywordId: string) => keyword.keywordId === keywordId))
            .map((keyword: IKeyword) => keyword.title)
            .join(", ");

        return <Text content={keywordsString?.trim().length > 0 ? keywordsString : "NA"} />;
    }

    // Renders research proposal details.
    renderResearchProposalDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsTitle')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.title ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsDescription')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.description ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsTaskModule')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.details ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('priorityTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.priority ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('statusText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.status ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('topicTypeTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.topicType ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('focusQuestion1Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.focusQuestion1 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('focusQuestion2Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.focusQuestion2 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('focusQuestion3Title')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.focusQuestion3 ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('objectivesTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.objectives ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('planTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.plan ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('deliverablesTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.deliverables ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('budgetTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.budget ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('startDateTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.startDate ? moment(this.state.selectedTreeItem?.dataValue?.startDate).format("DD-MMM-YYYY hh:mm A") : "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('completionTimeTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.completionTime ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('endorsementsTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.endorsements ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('potentialFundingTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.potentialFunding ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('submittedByTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.submittedBy ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    // Renders event details.
    renderEventDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                {
                    this.state.selectedTreeItem?.dataValue?.webSite &&
                    <Flex column gap="gap.small">
                        <a href={this.state.selectedTreeItem?.dataValue?.webSite} target="_blank">{this.localize("openLinkText")}</a>
                    </Flex>
                }
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsTitle')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.title ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsDescription')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.description ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('dateOfEventTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.dateOfEvent ? moment(this.state.selectedTreeItem?.dataValue?.dateOfEvent).format("DD-MMM-YYYY hh:mm A") : "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('organizationText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.organization ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('locationTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.location ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('otherContactInfoText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.otherContactInfo ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    // Renders sponsor's details.
    renderSponsorDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('firstNameText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.firstName ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('lastNameText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.lastName ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('emailText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.emailAddress ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsTitle')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.title ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsDescription')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.description ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('organizationText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.organization ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('serviceText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.service ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('phoneText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.phone ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('otherContactInfoText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.otherContact ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    // Renders partner's details.
    renderPartnerDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('firstNameText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.firstName ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('lastNameText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.lastName ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsTitle')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.title ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsDescription')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.description ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('organizationText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.organization ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('phoneText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.phone ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('otherContactInfoText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.otherContact ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('projectTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.projects ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    /**
     * Returns news source title.
     * @param newsSourceId The news source Id.
     */
    getNewsSourceTitle = (newsSourceId: number) => {
        var newsSource = this.state.newsSources.find((newsSource: IAthenaNewsSource) => newsSource.newsSourceId === newsSourceId);
        if (newsSource) {
            return newsSource.title;
        }
        return "NA";
    }

    // Renders details of research news.
    renderNewsDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                {
                    this.state.selectedTreeItem?.dataValue?.externalLink &&
                    <Flex column gap="gap.small">
                        <a href={this.state.selectedTreeItem?.dataValue?.externalLink} target="_blank">{this.localize("openLinkText")}</a>
                    </Flex>
                }
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsTitle')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.title ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={this.localize('sourceTitle')} weight="semibold" />
                    <Text content={this.getNewsSourceTitle(this.state.selectedTreeItem?.dataValue?.newsSourceId)} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsAbstract')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.abstract ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('publishedDateTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.publishedDate ? moment(this.state.selectedTreeItem?.dataValue?.publishedDate).format("DD-MMM-YYYY hh:mm A") : "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    /**
     * Gets the display details item.
     * @param header The header.
     * @param value The value.
     */
    getDisplayDetailsItem = (header: string, value: any | undefined | null) => {
        return <Flex column gap="gap.small">
            <Text content={header} weight="semibold" />
            <Text content={value ?? "NA"} />
        </Flex>;
    }

    // Renders details of research project.
    renderResearchProjectDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                {
                    this.state.selectedTreeItem?.dataValue?.files &&
                    <Flex column gap="gap.small">
                        <a href={this.state.selectedTreeItem?.dataValue?.files} target="_blank">{this.localize("openLinkText")}</a>
                    </Flex>
                }
                {this.getDisplayDetailsItem(this.localize('requestDetailsTitle'), this.state.selectedTreeItem?.dataValue?.title)}
                {this.getDisplayDetailsItem(`${this.localize('requestDetailsAbstract')}:`, this.state.selectedTreeItem?.dataValue?.abstract)}
                {this.getDisplayDetailsItem(this.localize('requestDetailsStatus'), this.state.selectedTreeItem?.dataValue?.status)}
                {this.getDisplayDetailsItem(`${this.localize('priorityTitle')}:`, this.getResearchPriorityTitle(this.state.selectedTreeItem?.dataValue?.priority))}
                {this.getDisplayDetailsItem(`${this.localize('importanceTitle')}:`, this.getResearchImportanceTitle(this.state.selectedTreeItem?.dataValue?.importance))}
                {this.getDisplayDetailsItem(`${this.localize('statusDescriptionLabel')}:`, this.state.selectedTreeItem?.dataValue?.statusDescription)}
                {this.getDisplayDetailsItem(`${this.localize('requestDetailsAuthors')}:`, this.state.selectedTreeItem?.dataValue?.authors)}
                {this.getDisplayDetailsItem(`${this.localize('authorsOrgLabel')}:`, this.state.selectedTreeItem?.dataValue?.authorsOrg)}
                {this.getDisplayDetailsItem(`${this.localize('requestDetailsResearchDept')}:`, this.state.selectedTreeItem?.dataValue?.researchDept)}
                {this.getDisplayDetailsItem(`${this.localize('requestDetailsAdvisors')}:`, this.state.selectedTreeItem?.dataValue?.advisors)}
                {this.getDisplayDetailsItem(`${this.localize('infoResourceDisplayDetailsPublisherLabel')}:`, this.state.selectedTreeItem?.dataValue?.publisher)}
                {this.getDisplayDetailsItem(`${this.localize('secondReadersLabel')}:`, this.state.selectedTreeItem?.dataValue?.secondReaders)}
                {this.getDisplayDetailsItem(`${this.localize('reviewerNotesLabel')}:`, this.state.selectedTreeItem?.dataValue?.reviewerNotes)}
                {this.getDisplayDetailsItem(`${this.localize('degreeProgramLabel')}:`, this.state.selectedTreeItem?.dataValue?.degreeProgram)}
                {this.getDisplayDetailsItem(`${this.localize('degreeLevelLabel')}:`, this.state.selectedTreeItem?.dataValue?.degreeLevel)}
                {this.getDisplayDetailsItem(`${this.localize('degreeTitlesLabel')}:`, this.state.selectedTreeItem?.dataValue?.degreeTitles)}
                {this.getDisplayDetailsItem(`${this.localize('recognitionLabel')}:`, this.state.selectedTreeItem?.dataValue?.recognition)}
                {this.getDisplayDetailsItem(`${this.localize('originatingRequestLabel')}:`, this.state.selectedTreeItem?.dataValue?.originatingRequest)}
                {this.getDisplayDetailsItem(`${this.localize('dateStartedLabel')}:`, this.state.selectedTreeItem?.dataValue?.dateStarted ? moment(this.state.selectedTreeItem?.dataValue?.dateStarted).format("DD-MMM-YYYY hh:mm A") : "NA")}
                {this.getDisplayDetailsItem(`${this.localize('dateCompletedLabel')}:`, this.state.selectedTreeItem?.dataValue?.dateCompleted ? moment(this.state.selectedTreeItem?.dataValue?.dateCompleted).format("DD-MMM-YYYY hh:mm A") : "NA")}
                {this.getDisplayDetailsItem(`${this.localize('useRightsLabel')}:`, this.state.selectedTreeItem?.dataValue?.useRights)}
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    // Renders COI details.
    renderCoiDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                {
                    this.state.selectedTreeItem?.dataValue?.webSite &&
                    <Flex column gap="gap.small">
                        <a href={this.state.selectedTreeItem?.dataValue?.webSite} target="_blank">{this.localize("openLinkText")}</a>
                    </Flex>
                }
                <Flex column gap="gap.small">
                    <Text content={this.localize('coiNameLabel')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.coiName ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('descriptionText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.coiDescription ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsStatus')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem.dataValue?.status ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('createdOnText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.createdOn ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('createdByText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.createdByName ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('organizationText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.organization ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    // Renders user details.
    renderUserDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                {
                    this.state.selectedTreeItem?.dataValue?.webSite &&
                    <Flex column gap="gap.small">
                        <a href={this.state.selectedTreeItem?.dataValue?.webSite} target="_blank">{this.localize("openLinkText")}</a>
                    </Flex>
                }
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('nameTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.userDisplayName ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('emailText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.emailAddress ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('otherContactInfoText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem.dataValue?.otherContact ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('serviceText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.service ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('titleRankText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.rank ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('payGradeText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.payGrade ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('specialtyText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.specialty ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('jobTitleText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.jobTitle ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('currentOrganizationText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.currentOrganization ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('underGraduateDegreeText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.underGraduateDegree ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('gradSchoolText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.gradSchool ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('graduateDegreeProgramText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.graduateDegreeProgram ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('deptofStudyText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.deptOfStudy ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsAdvisors')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.advisors ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('dateAtPostText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.dateAtPost ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('rotationDateText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.rotationDate ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('dateOfRankTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.dateOfRank ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    // Renders Athena info resource details.
    renderAthenaInfoResourceDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                {
                    this.state.selectedTreeItem?.dataValue?.website &&
                    <Flex column gap="gap.small">
                        <a href={this.state.selectedTreeItem?.dataValue?.website} target="_blank">{this.localize("openLinkText")}</a>
                    </Flex>
                }
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsTitle')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.title ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsDescription')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.description ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('publishedDateTitle')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.publishedDate ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('infoResourceDisplayDetailsPublisherLabel')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.publisher ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('infoResourceDisplayDetailsProvenanceLabel')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.provenance ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('infoResourceDisplayDetailsCollectionLabel')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.collection ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('infoResourceDisplayDetailsSourceOrgLabel')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.sourceOrg ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('infoResourceDisplayDetailsSourceGroupLabel')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.sourceGroup ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('infoResourceDisplayDetailsUsageLicensingLabel')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.usageLicensing ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('infoResourceDisplayDetailsAvgUserRatingLabel')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.avgUserRating} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    // Renders Athena tool details.
    renderAthenaToolDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                {
                    this.state.selectedTreeItem?.dataValue?.website &&
                    <Flex column gap="gap.small">
                        <a href={this.state.selectedTreeItem?.dataValue?.website} target="_blank">{this.localize("openLinkText")}</a>
                    </Flex>
                }
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsTitle')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.title ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsDescription')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.description ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('toolsManufacturer')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.manufacturer ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('toolsUsageLicensing')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.usageLicensing ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('toolsUserComments')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.userComments ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('toolsDisplayDetailsAvgUserRatingLabel')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.dataValue?.avgUserRating} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.dataValue?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    // Renders default details.
    renderDefaultDisplayDetails = () => {
        return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
            <Flex>
                <Text content={this.localize('requestDetailsTaskModule')} weight="semibold" size="large" />
                <Flex.Item push>
                    <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                </Flex.Item>
            </Flex>
            <Flex column gap="gap.medium">
                <Flex column gap="gap.small">
                    <Text content={this.localize('requestDetailsTitle')} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.title ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('typeText')}:`} weight="semibold" />
                    <Text content={this.getItemType(this.state.selectedTreeItem?.nodeTypeId)} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('statusText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.status ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('organizationText')}:`} weight="semibold" />
                    <Text content={this.state.selectedTreeItem?.organization ?? "NA"} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={`${this.localize('requestDetailsKeywords')}`} weight="semibold" />
                    {this.renderKeywordsInDisplayDetailsSection(this.state.selectedTreeItem?.keywords)}
                </Flex>
            </Flex>
        </Flex>;
    }

    /**
     * Renders details based on node type Id.
     * @param nodeTypeId The node type Id.
     */
    renderDetailsByNodeType = (nodeTypeId: number | undefined) => {
        if (nodeTypeId) {
            let nodeType: IDiscoveryTreeNodeType | undefined = this.state.nodeTypeData
                .find((nodeType: IDiscoveryTreeNodeType) => nodeType.nodeTypeId === nodeTypeId);

            if (nodeType) {
                var jsonFile = nodeType.jsonFile;
                switch (jsonFile) {
                    case DiscoveryNodeFileNames.AthenaResearchProjects:
                        return this.renderResearchProjectDetails();
                    case DiscoveryNodeFileNames.AthenaCommunities:
                        return this.renderCoiDetails();
                    case DiscoveryNodeFileNames.AthenaUsers:
                        return this.renderUserDetails();
                    case DiscoveryNodeFileNames.AthenaResearchRequests:
                        return this.renderResearchRequestDetails();
                    case DiscoveryNodeFileNames.AthenaResearchProposals:
                        return this.renderResearchProposalDetails();
                    case DiscoveryNodeFileNames.AthenaPartners:
                        return this.renderPartnerDetails();
                    case DiscoveryNodeFileNames.AthenaSponsors:
                        return this.renderSponsorDetails();
                    case DiscoveryNodeFileNames.AthenaEvents:
                        return this.renderEventDetails();
                    case DiscoveryNodeFileNames.AthenaNewsArticles:
                        return this.renderNewsDetails();
                    case DiscoveryNodeFileNames.AthenaInfoResources:
                        return this.renderAthenaInfoResourceDetails();
                    case DiscoveryNodeFileNames.AthenaTools:
                        return this.renderAthenaToolDetails();
                    default:
                        return this.renderDefaultDisplayDetails();
                }
            }
        }
    }

    // Renders node details.
    renderDetails = () => {
        if (this.state.selectedMenuItemId === 'rate-comment-item') {
            let jsonFile;
            if (this.state.selectedTreeItem?.nodeTypeId) {
                let nodeType: IDiscoveryTreeNodeType | undefined = this.state.nodeTypeData
                    .find((nodeType: IDiscoveryTreeNodeType) => nodeType.nodeTypeId === this.state.selectedTreeItem?.nodeTypeId);
                if (nodeType) {
                    jsonFile = nodeType.jsonFile;
                }
            }

            return <RateOrComment itemTableId={this.state.selectedTreeItem?.dataValue?.tableId} itemJsonFile={jsonFile} onDisplayDetailsCloseIconClick={this.onDisplayDetailsCloseIconClick} />;
        }
        else if (this.state.selectedMenuItemId === 'display-details') {
            return this.renderDetailsByNodeType(this.state.selectedTreeItem?.nodeTypeId);
        }
        else if (this.state.selectedMenuItemId === 'interested-users') {
            return this.renderInterestedUsers();
        }
        else if (this.state.selectedMenuItemId === 'interested-sponsors') {
            return this.renderInterestedSponsors();
        }

        return "";
    }

    // Renders details of interested users.
    renderInterestedUsers = () => {
        if (this.state.interestedUsers.length > 0) {
            return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
                <Flex>
                    <Text content={this.localize("interestedUsersTitle")} weight="bold" />
                    <Flex.Item push>
                        <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                    </Flex.Item>
                </Flex>
                {
                    this.state.interestedUsers.map(user => {
                        return <Flex gap="gap.small" vAlign="center">
                            <ProfilePic profilePhoto={user.profileImage} userName={user.displayName} />
                            <Text content={user.displayName} />
                            <Flex.Item push>
                                <ChatIcon outline size="medium" className="icon-pointer" onClick={() => microsoftTeams.executeDeepLink("https://teams.microsoft.com/l/chat/0/0?users=" + user.mail)} />
                            </Flex.Item>
                        </Flex>;
                    })
                }
            </Flex>;
        }

    }

    /**
     * Loads the interested users data based on keywords.
     * @param selectedTreeItem The selected tree item.
     */
    getInterestedUsers = async (selectedTreeItem) => {
        this.setState({ isLoadingNodeData: true });
        let keywords = selectedTreeItem?.keywords ?? selectedTreeItem?.dataValue?.keywords;
        let response = await getInterestedUsersDataAsync(keywords, this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let interestedUsersData = response.data as IUserDetails[];

            if (interestedUsersData.length === 0) {
                this.setState({ status: { id: this.state.status.id + 1, message: this.localize("interestedUsersDoesNotExistMsg"), type: ActivityStatus.Success } });
            }

            this.setState({
                closeDisplayDetails: false,
                interestedUsers: interestedUsersData
            });
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToLoadInterestedUsersError"), type: ActivityStatus.Error } });
        }
        this.setState({ isLoadingNodeData: false });
    }

    // Renders interested sponsors.
    renderInterestedSponsors = () => {
        if (this.state.interestedSponsors.length > 0) {
            return <Flex padding="padding.medium" gap="gap.medium" column className="discovery-details-pane">
                <Flex>
                    <Text content={this.localize("interestedSponsorsTitle")} weight="bold" />
                    <Flex.Item push>
                        <CloseIcon onClick={this.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                    </Flex.Item>
                </Flex>
                {
                    this.state.interestedSponsors.map(sponsor => {
                        return <Flex gap="gap.small" vAlign="center">
                            <ProfilePic profilePhoto={sponsor.profileImage} userName={sponsor.firstName + " " + sponsor.lastName} />
                            <Text content={sponsor.firstName + " " + sponsor.lastName} />
                            <Flex.Item push>
                                <ChatIcon outline size="medium" className="icon-pointer" />
                            </Flex.Item>
                        </Flex>;
                    })
                }
            </Flex>;
        }
    }

    /**
     * Loads the interested aponosors data based on keywords.
     * @param selectedTreeItem The selected tree item.
     */
    getInterestedSponsors = async (selectedTreeItem) => {
        this.setState({ isLoadingNodeData: true });
        let keywords = selectedTreeItem?.keywords ?? selectedTreeItem?.dataValue?.keywords;
        let response = await getInterestedSponsorsDataAsync(keywords, this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let interestedSponsorsData = response.data as ISponsorDetails[];

            if (interestedSponsorsData.length === 0) {
                this.setState({ status: { id: this.state.status.id + 1, message: this.localize("interestedSponsorsDoesNotExistMsg"), type: ActivityStatus.Success } });
            }

            this.setState({
                closeDisplayDetails: false,
                interestedSponsors: interestedSponsorsData
            });
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("failedToInterestedSponsorsError"), type: ActivityStatus.Error } });
        }
        this.setState({ isLoadingNodeData: false });
    }

    /**
     * Follows a resource.
     * @param selectedTreeItem The selected tree item.
     */
    followResource = async (selectedTreeItem) => {
        this.setState({ isLoadingNodeData: true });
        let keywords = selectedTreeItem?.keywords ?? selectedTreeItem?.dataValue?.keywords;

        if (!keywords?.length) {
            this.setState({
                status: { id: this.state.status.id + 1, message: this.localize("followItemEmptyKeywordsErrorMessage"), type: ActivityStatus.Error },
                isLoadingNodeData: false
            });
            return;
        }

        let response = await followResourceAsync(keywords, this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("followKeywordsSuccessMessage"), type: ActivityStatus.Success } });
        }
        else if (response && response.status === StatusCodes.NOT_FOUND) {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("appNotInstalledErrorMessage"), type: ActivityStatus.Error } });
        }
        else {
            this.setState({ status: { id: this.state.status.id + 1, message: this.localize("followKeywordsErrorMessage"), type: ActivityStatus.Error } });
        }
        this.setState({ isLoadingNodeData: false });
    }

    /**
     * Handles collapsing of rows.
     * @param e The event.
     */
    onRowCollapsed = async (e) => {
        let selectedNodeData = this.state.taxonomyTree.find(x => x.taxonomyId === e.key);

        this.setStatusBarHierarchy(selectedNodeData);
    }

    /**
     * handles expansion of rows.
     * @param eventDetails The event details.
     */
    onRowExpanded = async (eventDetails: any) => {
        let selectedNodeData = this.state.taxonomyTree.find(x => x.taxonomyId === eventDetails.key);

        if (selectedNodeData) {
            this.setStatusBarHierarchy(selectedNodeData);

            let hasNodeIdInNodesExpandedAtleastOnce = this.state.nodesExpandedAtLeastOnce
                .some((nodeId: string) => selectedNodeData?.taxonomyId === nodeId);

            if (selectedNodeData?.taxonomyId && !hasNodeIdInNodesExpandedAtleastOnce) {
                let nodesExpandedAtLeastOnceList = cloneDeep(this.state.nodesExpandedAtLeastOnce);
                nodesExpandedAtLeastOnceList.push(selectedNodeData?.taxonomyId?.toString());
                this.setState({ nodesExpandedAtLeastOnce: nodesExpandedAtLeastOnceList }, () => {
                    let isTaxonomyFolderNode: boolean = this.state.taxonomyFolderNodeIds
                        .some((taxonomyFolderNodeId: string) => taxonomyFolderNodeId === selectedNodeData?.taxonomyId);

                    if (selectedNodeData && isTaxonomyFolderNode && !hasNodeIdInNodesExpandedAtleastOnce) {
                        this.bindNodeTasks(selectedNodeData);
                    }
                });
            }
        }
    }

    // Renders status bar of selected nodes.
    renderStatusBar = () => {
        let statusBarItems = this.state.statusBar.map((x, index) => {
            if (index > 0)
                return <React.Fragment>
                    <ChevronEndMediumIcon />
                    <FluentUIButton text content={<Flex gap="gap.small">
                        <Image src={getResourceImagePath(this.state.nodeTypeData, x.nodeTypeId)} styles={{ width: "2.2rem", cursor: "pointer" }} />
                        <Text content={x.title} />
                    </Flex>} title={x.title} onClick={() => x.nodeTypeId !== TreeViewFolderNodeTypeValue ? void 0 : this.onStatusBarItemClick(x.taxonomyId)} />
                </React.Fragment>

            return <FluentUIButton text content={<Flex gap="gap.small">
                <Image src={getResourceImagePath(this.state.nodeTypeData, x.nodeTypeId)} styles={{ width: "2.2rem", cursor: "pointer" }} />
                <Text content={x.title} />
            </Flex>} title={x.title} onClick={() => x.nodeTypeId !== TreeViewFolderNodeTypeValue ? void 0 : this.onStatusBarItemClick(x.taxonomyId)} />
        });

        return <Flex vAlign="center" gap="gap.smaller" wrap>{statusBarItems}</Flex>
    }

    /**
     * Handles click on status bar item.
     * @param nodeId Node id od selected node.
     */
    onStatusBarItemClick = (nodeId) => {
        let existingTasks = cloneDeep(this.state.taxonomyTree);
        let nodeData = existingTasks.find(x => x.taxonomyId === nodeId);

        this.setStatusBarHierarchy(nodeData);
    }

    /**
     * Renders taxonmy title cell.
     * @param data The node data.
     */
    renderNameCell = (data) => {
        if (data.data.title) {
            return <Flex vAlign="center" gap="gap.smaller">
                <Image src={getResourceImagePath(this.state.nodeTypeData, data.data.nodeTypeId)} styles={{ width: "2rem", height: "1.6rem" }} />
                <Text content={data.data.title} title={data.data.tooltip} />
            </Flex>;
        }

        return <Text content={data.data.title} />;
    }

    // Renders title's column header.
    renderNameHeader = () => {
        return this.state.viewAsCheckedValue === ViewAsTreeViewId ? <Text content={this.localize('discoveryTreeAthenaTaxonomyColumnHeader')} weight="bold" /> : <Text content={this.localize('discoveryTreeTitleColumnHeader')} weight="bold" />;
    }

    // Renders type's column header.
    renderTypeHeader = () => {
        return <Flex vAlign="center">
            <Text content={this.localize('typeText')} weight="bold" />
        </Flex>;
    }

    /**
     * Renders type's column cell.
     * @param data The node data.
     */
    renderTypeCell = (data) => {
        return <Text content={data.data.title !== this.localize("discoveryTreeNodeDataNotAvailable") ? this.getItemType(data.data.nodeTypeId) : '-'} />
    }

    /**
     * Renders date's column cell.
     * @param data The node data.
     */
    renderDateCell = (data) => {
        return <Text content={data.data.title !== this.localize("discoveryTreeNodeDataNotAvailable") ? this.getItemDate(data.data.nodeTypeId, data.data.dataValue, "DD-MMM-YYYY") ?? "NA" : '-'} />
    }

    // Renders date's column header.
    renderDateHeader = () => {
        return <Text content={this.localize("dateText")} weight="bold" />;
    }

    /**
    * Returns date of node item. 
    * @param nodeTypeId The node type Id.
    * @param data The node data.
    * @param dateFormat The date format.
    */
    getItemDate = (nodeTypeId: number | undefined, data: any, dateFormat: string) => {
        if (nodeTypeId) {
            let nodeType: IDiscoveryTreeNodeType | undefined = this.state.nodeTypeData
                .find((nodeType: IDiscoveryTreeNodeType) => nodeType.nodeTypeId === nodeTypeId);

            if (nodeType) {
                var jsonFile = nodeType.jsonFile;
                var dateFieldName = nodeType.dateFieldName;
                if (dateFieldName !== "NA") {
                    dateFieldName = dateFieldName.charAt(0).toLowerCase() + dateFieldName.slice(1);
                    if (jsonFile === DiscoveryNodeFileNames.AthenaNewsArticles) {
                        return data[dateFieldName] ? moment(data[dateFieldName]).format(`${dateFormat} hh:mm A`) : undefined;
                    }
                    else if (jsonFile === DiscoveryNodeFileNames.AthenaCommunities) {
                        return data?.createdOn ? moment(data.createdOn).format(dateFormat) : undefined;
                    }
                    return data[dateFieldName] ? moment(data[dateFieldName]).format(dateFormat) : undefined;
                }
            }
        }
    }

    /**
     * Returns type of node item.
     * @param nodeTypeId The node type Id.
     */
    getItemType = (nodeTypeId: number | undefined) => {
        if (nodeTypeId) {
            let nodeType: IDiscoveryTreeNodeType | undefined = this.state.nodeTypeData
                .find((nodeType: IDiscoveryTreeNodeType) => nodeType.nodeTypeId === nodeTypeId);

            if (nodeType) {
                return nodeType.title;
            }
        }

        return "NA";
    }

    // Opens filter menu.
    handleFilterButtonCLick = () => {
        this.setState({ isFilterOpen: !this.state.isFilterOpen });
    }

    // Closes the filter dropdown if clicked outside.
    handleFilterOutsideClick = () => {
        this.setState({ isFilterOpen: !this.state.isFilterOpen });
    }

    // Closes the filter dropdown on close button click.
    handleCloseButtonClick = () => {
        this.setState({ isFilterOpen: false });
    }

    /**
     * Handles the apply button click of configure filter.
     * @param filterData The filter data.
     */
    handleApplyButtonClick = (filterData: IDiscoveryTreeFilter[]) => {
        this.deselectHiddenFilterItem(filterData);
    }

    /**
     * Prepares data to display resources in flat list.
     * @param filteredAndSearchedData The filtered and searched data.
     */
    getFlatListData = (filteredAndSearchedData: ITreeNodeDataElement) => {
        if (!filteredAndSearchedData) {
            return [];
        }

        let flatListData: ITaxonomyElement[] = [];

        // Research projects.
        if (filteredAndSearchedData.researchProjects?.length > 0) {
            let researchProjectsData = filteredAndSearchedData.researchProjects
                .map((researchProject: IResearchProject) => {
                    return {
                        taxonomyId: researchProject.tableId,
                        title: researchProject.title,
                        type: researchProject.nodeTypeId,
                        tooltip: researchProject.abstract,
                        dataValue: researchProject,
                        nodeTypeId: researchProject.nodeTypeId,
                        date: this.getItemDate(researchProject.nodeTypeId, researchProject, "YYYY-MM-DD")
                    } as ITaxonomyElement;
                });

            flatListData.push(...researchProjectsData);
        }

        // Research requests.
        if (filteredAndSearchedData.researchRequests?.length > 0) {
            let researchRequestsData = filteredAndSearchedData.researchRequests
                .map((researchRequest: IResearchRequest) => {
                    return {
                        taxonomyId: researchRequest.tableId,
                        title: researchRequest.title,
                        type: researchRequest.nodeTypeId,
                        tooltip: researchRequest.description,
                        dataValue: researchRequest,
                        nodeTypeId: researchRequest.nodeTypeId,
                        date: this.getItemDate(researchRequest.nodeTypeId, researchRequest, "YYYY-MM-DD")
                    } as ITaxonomyElement;
                });

            flatListData.push(...researchRequestsData);
        }

        // Research proposals.
        if (filteredAndSearchedData.researchProposals?.length > 0) {
            let researchProposalsData = filteredAndSearchedData.researchProposals
                .map((researchProposal: IResearchProposal) => {
                    return {
                        taxonomyId: researchProposal.tableId,
                        title: researchProposal.title,
                        type: researchProposal.nodeTypeId,
                        tooltip: researchProposal.description,
                        dataValue: researchProposal,
                        nodeTypeId: researchProposal.nodeTypeId,
                        date: this.getItemDate(researchProposal.nodeTypeId, researchProposal, "YYYY-MM-DD")
                    } as ITaxonomyElement
                });

            flatListData.push(...researchProposalsData);
        }

        // Athena partners.
        if (filteredAndSearchedData.partners?.length > 0) {
            let athenaPartnersData = filteredAndSearchedData.partners
                .map((partner: IPartnerDetails) => {
                    return {
                        taxonomyId: partner.tableId,
                        title: partner.title,
                        type: partner.nodeTypeId,
                        tooltip: partner.description,
                        dataValue: partner,
                        nodeTypeId: partner.nodeTypeId,
                    } as ITaxonomyElement;
                });

            flatListData.push(...athenaPartnersData);
        }

        // Athena events.
        if (filteredAndSearchedData.events?.length > 0) {
            let athenaEventsData = filteredAndSearchedData.events
                .map((event: IAthenaEvent) => {
                    return {
                        taxonomyId: event.tableId,
                        title: event.title,
                        type: event.nodeTypeId,
                        tooltip: event.description,
                        dataValue: event,
                        nodeTypeId: event.nodeTypeId,
                        date: this.getItemDate(event.nodeTypeId, event, "YYYY-MM-DD")
                    } as ITaxonomyElement;
                });

            flatListData.push(...athenaEventsData);
        }

        // Athena communities.
        if (filteredAndSearchedData.cois?.length > 0) {
            let athenaCommunitiesData = filteredAndSearchedData.cois
                .map((coi: ICoi) => {
                    return {
                        taxonomyId: coi.tableId,
                        title: coi.coiName,
                        type: coi.nodeTypeId,
                        tooltip: coi.coiDescription,
                        dataValue: coi,
                        nodeTypeId: coi.nodeTypeId,
                        date: this.getItemDate(coi.nodeTypeId, coi, "YYYY-MM-DD")
                    } as ITaxonomyElement;
                });

            flatListData.push(...athenaCommunitiesData);
        }

        // News articles.
        if (filteredAndSearchedData.newsArticles?.length > 0) {
            let newsArticlesData = filteredAndSearchedData.newsArticles
                .map((newsArticle: INews) => {
                    return {
                        taxonomyId: newsArticle.tableId,
                        title: newsArticle.title,
                        type: newsArticle.nodeTypeId,
                        tooltip: newsArticle.body,
                        dataValue: newsArticle,
                        nodeTypeId: newsArticle.nodeTypeId,
                        date: this.getItemDate(newsArticle.nodeTypeId, newsArticle, "YYYY-MM-DD")
                    } as ITaxonomyElement;
                });

            flatListData.push(...newsArticlesData);
        }

        // Sponsors.
        if (filteredAndSearchedData.sponsors?.length > 0) {
            let sponsorsData = filteredAndSearchedData.sponsors
                .map((sponsor: ISponsorDetails) => {
                    return {
                        taxonomyId: sponsor.tableId,
                        title: sponsor.title,
                        type: sponsor.nodeTypeId,
                        tooltip: sponsor.description,
                        dataValue: sponsor,
                        nodeTypeId: sponsor.nodeTypeId,
                    } as ITaxonomyElement;
                });

            flatListData.push(...sponsorsData);
        }

        // Users.
        if (filteredAndSearchedData.users?.length > 0) {
            let usersData = filteredAndSearchedData.users
                .map((user: IUserSettings) => {
                    return {
                        taxonomyId: user.tableId,
                        title: user.userDisplayName,
                        type: user.nodeTypeId,
                        tooltip: user.emailAddress,
                        dataValue: user,
                        nodeTypeId: user.nodeTypeId,
                    } as ITaxonomyElement;
                });

            flatListData.push(...usersData);
        }

        // Athena info resources.
        if (filteredAndSearchedData.athenaInfoResources?.length > 0) {
            let athenaInfoResourcesData = filteredAndSearchedData.athenaInfoResources
                .map((infoResource: IAthenaInfoResource) => {
                    return {
                        taxonomyId: infoResource.tableId,
                        title: infoResource.title,
                        type: infoResource.nodeTypeId,
                        tooltip: infoResource.description,
                        dataValue: infoResource,
                        nodeTypeId: infoResource.nodeTypeId,
                        date: this.getItemDate(infoResource.nodeTypeId, infoResource, "YYYY-MM-DD")
                    } as ITaxonomyElement;
                });

            flatListData.push(...athenaInfoResourcesData);
        }

        // Athena tools.
        if (filteredAndSearchedData.athenaTools?.length > 0) {
            let athenaToolsData = filteredAndSearchedData.athenaTools
                .map((athenaTool: IAthenaTool) => {
                    return {
                        taxonomyId: athenaTool.tableId,
                        title: athenaTool.title,
                        type: athenaTool.nodeTypeId,
                        tooltip: athenaTool.description,
                        dataValue: athenaTool,
                        nodeTypeId: athenaTool.nodeTypeId,
                    } as ITaxonomyElement;
                });

            flatListData.push(...athenaToolsData);
        }

        return flatListData;
    }

    /**
     * Event handler called when 'View as' get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onViewAsChanged = (eventDetails: any, eventData: any) => {
        this.setState({ viewAsCheckedValue: eventData.value });
    }

    /**
     * Deselects the selected filter item pill.
     * @param filterItem The filter item.
     */
    deselectFilterPill = (filterItem: IDiscoveryTreeFilter) => {
        let existingData = cloneDeep(this.state.initialFilterData);
        let existingSelectedFilters = cloneDeep(this.state.selectedFilters);
        let index = existingData.findIndex(filter => filter.filterId === filterItem.filterId);
        if (filterItem && index !== -1) {
            if (filterItem.dbValue[0] === dbValueOfAllFilterItem) {
                var result = this.setSelectedSubFilterItem(filterItem, existingData, existingSelectedFilters, false);
                existingData = result.existingData;
                existingSelectedFilters = result.existingSelectedFilters;
            }
            else {
                let subFilters = existingData.filter(x => x.parentId === existingData[index].filterId && x.isChecked !== false);
                if (subFilters.length) {
                    existingData[index].isChecked = false;
                    if (existingData[index].enabled === true) {
                        existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                    }
                    subFilters.forEach(subFilter => {
                        let subFilterItems = existingData.filter(x => x.parentId === subFilter.filterId && x.isChecked !== false);
                        if (subFilterItems.length) {
                            let index = existingData.findIndex(filter => filter.filterId === subFilter.filterId);
                            if (index !== -1) {
                                existingData[index].isChecked = false;
                                if (existingData[index].enabled === true) {
                                    existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                                }
                            }
                            subFilterItems.forEach(subFilterItem => {
                                var result = this.setSelectedSubFilterItem(subFilterItem, existingData, existingSelectedFilters, false);
                                existingData = result.existingData;
                                existingSelectedFilters = result.existingSelectedFilters;
                            })
                        }
                        else {
                            let index = existingData.findIndex(filter => filter.filterId === subFilter.filterId);
                            if (index !== -1) {
                                existingData[index].isChecked = false;
                                if (existingData[index].enabled === true) {
                                    existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                                }
                            }
                        }
                    })
                }
                else {
                    existingData[index].isChecked = false;
                    if (existingData[index].enabled === true) {
                        existingSelectedFilters = this.prepareSelectedFiltersList(existingData[index], existingData[index].isChecked, existingSelectedFilters);
                    }
                }
            }
        }

        if (existingData[index].dbValue[0] !== dbValueOfAllFilterItem) {
            var result = this.selectDeselectParentFilterItem(existingData[index], existingData, existingSelectedFilters);
            existingData = result.existingData;
            existingSelectedFilters = result.existingSelectedFilters;
        }

        this.setState({ initialFilterData: existingData }, () => {
            this.getFilters();
            this.getFilterPillsData();
        });
        this.setState({ selectedFilters: existingSelectedFilters });
    }

    // Returns the title of selected filter to be shown in pill.
    getFilterPillsTitle = (filterItem: IDiscoveryTreeFilter) => {
        let parentFilterId = this.getSuperFilterIdForSelectedFilter(filterItem.filterId);
        let parentFilter = this.state.initialFilterData.find(filter => filter.filterId === parentFilterId);
        if (parentFilter) {
            return `${parentFilter.title} - ${filterItem.title}`;
        }
    }

    /**
     * Deselects the selected keyword.
     * @param selectedKeyword The selected keywords.
     */
    deselectSelectedKeyword = (selectedKeyword: IKeyword) => {
        let existingSelectedKeywords = cloneDeep(this.state.selectedKeywords);
        let index = existingSelectedKeywords.findIndex((keyword: IKeyword) => keyword.keywordId === selectedKeyword.keywordId);
        if (index !== -1) {
            existingSelectedKeywords.splice(index, 1);
            this.setState({ selectedKeywords: existingSelectedKeywords }, this.getFilterPillsData);
        }
    }

    // Clears search string.
    clearSearchString = () => {
        this.setState({
            searchString: "",
        }, this.getFilterPillsData);
    }

    // Returns the filter Pills Data.
    getFilterPillsData = () => {
        let itemData: any[] = [];

        if (this.state.searchString) {
            itemData.push({
                title: this.state.searchString,
                kind: 'custom',
                fitted: 'horizontally',
                content: <Pill
                    actionable
                    onDismiss={this.clearSearchString}
                    disabled={this.state.isLoading}
                >
                    {`"${this.state.searchString}"`}
                </Pill>,
                className: "toolbar-filter-item"
            })
        }

        this.state.selectedKeywords.map((selectedKeyword: IKeyword) => {
            itemData.push({
                key: selectedKeyword.keywordId,
                title: selectedKeyword.title,
                kind: 'custom',
                fitted: 'horizontally',
                content: <Pill
                    actionable
                    onDismiss={() => { this.deselectSelectedKeyword(selectedKeyword) }}
                    disabled={this.state.isLoading}
                >
                    {`<${selectedKeyword.title}>`}
                </Pill>,
                className: "toolbar-filter-item"
            });
        });

        this.state.initialFilterData.filter((filter: IDiscoveryTreeFilter) => filter.isChecked === true).map((filter: IDiscoveryTreeFilter) => {
            let parentFilter = this.state.initialFilterData.find(filterItem => filterItem.filterId === filter.parentId);
            if (parentFilter && parentFilter.parentId !== FilterGroupParentId) {
                let filterTitle = this.getFilterPillsTitle(filter);
                itemData.push({
                    key: filter.filterId,
                    title: filterTitle,
                    kind: 'custom',
                    fitted: 'horizontally',
                    content: <Pill
                        actionable
                        onDismiss={() => { this.deselectFilterPill(filter) }}
                        disabled={this.state.isLoading}
                    >
                        {filter.dbValue[0] === dbValueOfAllFilterItem ? filterTitle : filter.title}
                    </Pill>,
                    className: "toolbar-filter-item"
                });
            }
        });

        this.setState({ filterPillsData: itemData });
    }

    // Event handler called when click on search button.
    onSearchButtonClick = () => {
        this.searchAndFilterAsync(this.state.selectedFilters);
        this.getFilterPillsData();
    }

    /**
     * Handles the selection of item from keywords dropdown items.
     * @param event The event.
     * @param dropdownProps The dropdown props.
     */
    handleKeywordDropdownChange = (event: any, dropdownProps?: any) => {
        if (dropdownProps.value) {
            let existingSelectedKeywords = cloneDeep(this.state.selectedKeywords);
            let isPresent = existingSelectedKeywords.some((keyword: IKeyword) => keyword.keywordId === dropdownProps.value!.key);
            if (!isPresent) {
                existingSelectedKeywords.push({ keywordId: dropdownProps.value!.key, title: dropdownProps.value!.header });
                this.setState({ selectedKeywords: existingSelectedKeywords }, this.getFilterPillsData);
            }
            this.setState({ searchString: "" });
        }
    }

    /**
     * Intiates the search when query string of dropdown changes.
     * @param searchQuery The search query.
     */
    initiateSearch = (searchQuery: string) => {
        this.setState({ searchString: searchQuery });
        var result: IKeyword[] = [];
        if (searchQuery) {
            result = this.state.allKeywords.filter((keyword: IKeyword) => keyword.title.toLowerCase().includes(searchQuery.toLowerCase()));
        }
        else {
            result = this.state.allKeywords;
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
        this.setState({ searchResults });
    }

    render() {
        return (
            <Flex className="discovery-tab" column fill>
                <StatusBar status={this.state.status} isMobile={false} />
                <AthenaSplash description={this.localize("discoveryTabDescription")} heading={this.localize("discoveryTabHeading")} />
                <Flex column fill>
                    <Flex column className="discovery-header-container">
                        <Flex gap="gap.small" hAlign="end" vAlign="center">
                            <Flex gap="gap.smaller">
                                {
                                    this.state.filterData.map((group: IDiscoveryGroup, index: number) => {
                                        return (
                                            <Flex className="filter-group-container" key={index}>
                                                {
                                                    group.filters.map((filterItem: IDiscoveryFilter, index: number) => {
                                                        return (
                                                            <Flex key={index}>
                                                                {
                                                                    this.renderFilterItems(filterItem)
                                                                }
                                                            </Flex>
                                                        )
                                                    })
                                                }
                                            </Flex>
                                        )
                                    })
                                }
                            </Flex>
                            <Flex gap="gap.small" vAlign="center">
                                <Button icon={<SettingsIcon />} iconOnly disabled={this.state.isLoading || this.state.isSavingQuery} title={this.localize("userSettingsHeading")} className="icon-pointer" onClick={this.handleFilterButtonCLick} />
                                {
                                    this.state.isFilterOpen &&
                                    <DiscoveryFilter handleClickOutside={this.handleFilterOutsideClick} handleCloseButtonClick={this.handleCloseButtonClick} initialFilterData={this.state.initialFilterData} handleApplyButtonClick={this.handleApplyButtonClick} />
                                }
                                <Button iconOnly icon={<SaveIcon size="large" />} loading={this.state.isSavingQuery} disabled={this.state.isLoading || this.state.isSavingQuery} title={this.localize("discoveryTreeSaveQueryButtonTitle")} onClick={this.saveFilterQueryAsync} />
                                <MenuButton trigger={<Button iconOnly icon={<CloseIcon />} loading={this.state.isClearingAppliedFiltersAndSearchQuery} disabled={this.state.isLoading || this.state.isSavingQuery || this.state.isClearingAppliedFiltersAndSearchQuery} />} title={this.localize("discoveryTreeClearAndResetButtonText")} menu={this.clearFilterMenuItems} on="hover" />
                                <Popup
                                    trigger={<Button iconOnly icon={<EyeIcon />} loading={this.state.isClearingAppliedFiltersAndSearchQuery} disabled={this.state.isLoading || this.state.isSavingQuery || this.state.isClearingAppliedFiltersAndSearchQuery} />}
                                    content={<RadioGroup
                                        vertical
                                        items={this.viewAsRadioGroupItems}
                                        checkedValue={this.state.viewAsCheckedValue}
                                        onCheckedValueChange={this.onViewAsChanged}
                                    />}
                                    on="hover"
                                />
                                <MenuButton className="quick-access-menu-container" trigger={<Button iconOnly icon={<StarIcon />} title={this.localize('quickAccessListTitle')} disabled={this.state.isLoading || this.state.isUpdatingQuickAccessList} loading={this.state.isUpdatingQuickAccessList} />} menu={this.state.quickAccessListItems} />
                            </Flex>
                            <Flex gap="gap.smaller" vAlign="center">
                                <Dropdown
                                    search
                                    inverted
                                    placeholder={this.localize("findText")}
                                    items={this.state.searchResults}
                                    toggleIndicator={<SearchIcon />}
                                    onSearchQueryChange={(e, { searchQuery }) => {
                                        this.initiateSearch(searchQuery!);
                                    }}
                                    onChange={this.handleKeywordDropdownChange}
                                    noResultsMessage={this.localize("noKeywordsFoundDropdownMessage")}
                                    className="discovery-search-box"
                                    loading={this.state.isLoading}
                                    loadingMessage={this.localize("loadingLabel")}
                                    value={this.state.searchString}
                                    disabled={this.state.isLoading}
                                />
                                <Button content={this.localize("searchBtnTxt")} className="athena-button icon-pointer" onClick={this.onSearchButtonClick} disabled={this.state.isLoading} />
                            </Flex>
                        </Flex>
                    </Flex>
                    {
                        this.state.isLoading ?
                            <ContentLoader />
                            :
                            <Flex styles={{ height: "72vh" }}>
                                <Flex column padding="padding.medium" styles={{ overflowY: "hidden" }}>
                                    {this.state.viewAsCheckedValue === ViewAsTreeViewId && this.renderStatusBar()}
                                    <TreeList
                                        key={`taxonomy-key-${this.state.taxonomyKey}`}
                                        ref={node => { this.treeViewRef = node; }}
                                        onContextMenuPreparing={this.addMenuItems}
                                        dataSource={this.state.viewAsCheckedValue === ViewAsFlatListId ? this.state.flatListData : this.state.taxonomyTree}
                                        showBorders={false}
                                        columnAutoWidth={true}
                                        wordWrapEnabled={true}
                                        onSelectionChanged={this.onSelectionChanged}
                                        keyExpr="taxonomyId"
                                        parentIdExpr={this.state.viewAsCheckedValue === ViewAsFlatListId ? "invalidParentId" : "parentId"}
                                        id="taxonomy"
                                        allowColumnResizing={true}
                                        rootValue={TreeViewRootFolderValue}
                                        height={this.state.statusBar.length > 0 ? "88%" : "100%"}
                                        onRowExpanded={this.onRowExpanded}
                                        onRowCollapsed={this.onRowCollapsed}
                                        selectedRowKeys={this.state.selectedTreeItem ? [this.state.selectedTreeItem.taxonomyId] : []}
                                    >
                                        <Selection mode="single" />
                                        <Sorting showSortIndexes={false} />
                                        <Column
                                            dataField="title"
                                            allowSorting={true}
                                            defaultSortOrder="asc"
                                            sortIndex={TitleColumnSortIndex}
                                            width="60%"
                                            cellRender={this.renderNameCell}
                                            headerCellRender={this.renderNameHeader}
                                        >
                                        </Column>
                                        <Column
                                            dataField="type"
                                            allowSorting={true}
                                            defaultSortOrder="desc"
                                            sortIndex={TypeColumnSortIndex}
                                            width="25%"
                                            cellRender={this.renderTypeCell}
                                            headerCellRender={this.renderTypeHeader}
                                            alignment="left"
                                        >
                                        </Column>
                                        <Column
                                            dataField="date"
                                            defaultSortOrder="desc"
                                            width="25%"
                                            sortIndex={DateColumnSortIndex}
                                            cellRender={this.renderDateCell}
                                            headerCellRender={this.renderDateHeader}
                                        >
                                        </Column>
                                        <ColumnChooser enabled={false} mode='select' />
                                    </TreeList>
                                </Flex>
                                <Flex>
                                    {!this.state.closeDisplayDetails && this.renderDetails()}
                                </Flex>
                                <ContextMenu
                                    ref={element => { this.contextMenuRef = element; }}
                                    dataSource={this.state.menuItems}
                                    onItemClick={this.contextMenuItemClick}
                                    target="#taxonomy .dx-data-row"
                                />
                                <Dialog
                                    design={{ width: "18rem", height: "12rem" }}
                                    content={<Flex hAlign="center" vAlign="center"><Loader label={this.localize("loadingLabel")} /></Flex>}
                                    open={this.state.isLoadingNodeData}
                                />
                            </Flex>
                    }
                    {
                        this.state.filterPillsData.length > 0 &&
                        <Flex.Item push>
                            <Toolbar
                                items={this.state.filterPillsData}
                                overflow
                                overflowOpen={this.state.overflowOpen}
                                overflowItem={{
                                    title: this.localize("moreLabel"),
                                }}
                                onOverflowOpenChange={(e, toolbarProps) => {
                                    if (toolbarProps) {
                                        this.setState({ overflowOpen: toolbarProps.overflowOpen! })
                                    }
                                }}
                                getOverflowItems={startIndex => this.state.filterPillsData.slice(startIndex)}
                                className="discovery-toolbar"
                            />
                        </Flex.Item>
                    }
                </Flex>
            </Flex>
        );
    }
}

export default withTranslation()(withRouter(DiscoveryHome));