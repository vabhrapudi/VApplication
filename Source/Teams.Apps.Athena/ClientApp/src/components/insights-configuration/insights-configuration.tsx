// <copyright file="insights-configuration.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { Flex, Segment, Button, AddIcon, EditIcon, Dialog, SearchIcon, TrashCanIcon, Input, Table, Checkbox, Skeleton } from "@fluentui/react-northstar";
import StatusBar from "../common/status-bar/status-bar";
import AthenaSplash from "../athena-splash/athena-splash";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { useTranslation } from 'react-i18next';
import Constants from "../../constants/constants";
import { cloneDeep } from "lodash";
import { StatusCodes } from "http-status-codes";
import IPriority from "../../models/priority";
import { deletePrioritiesAsync, getAllPrioritiesAsync, getPriorityTypesAsync } from "../../api/priority-api";
import IPriorityType from "../../models/priority-type";
import IKeyword from "../../models/keyword";
import { getAllKeywordsAsync } from "../../api/keyword-api";

import "./insights-configuration.scss";

interface IInsightsConfigurationProps extends RouteComponentProps {
}

const InsightsConfiguration: React.FunctionComponent<IInsightsConfigurationProps> = (props: IInsightsConfigurationProps) => {
    const localize = useTranslation().t;

    React.useEffect(() => {
        microsoftTeams.initialize();
        getKeywords();
        getPriorityTypes();
        getPrioritiesAsync();
    }, []);

    const [status, setStatus] = React.useState({ id: 0, message: "", type: ActivityStatus.None } as IStatusBar);
    const [isEditButtonEnabled, setEditButtonEnabled] = React.useState(false);
    const [isDeletingPriorities, setDeletingPriorities] = React.useState(false);
    const [isDeleteButtonEnabled, setDeleteButtonEnabled] = React.useState(false);
    const [isLoadingPriorities, setLoadingPriorities] = React.useState(true);
    const [priorities, setPriorities] = React.useState([] as IPriority[]);
    const [isHeaderCheckboxChecked, setHeaderCheckboxChecked] = React.useState(false);
    const [filteredPriorities, setFilteredPriorities] = React.useState([] as IPriority[]);
    const [searchString, setSearchString] = React.useState("");
    const [priorityTypes, setPriorityTypes] = React.useState([] as IPriorityType[]);
    const [isLoadingPriorityTypes, setLoadingPriorityTypes] = React.useState(true);
    const [keywords, setKeywords] = React.useState([] as IKeyword[]);
    const [isLoadingKeywords, setLoadingKeywords] = React.useState(true);

    // Gets the priorities.
    const getPrioritiesAsync = async () => {
        setLoadingPriorities(true);
        setHeaderCheckboxChecked(false);
        setEditButtonEnabled(false);
        setDeleteButtonEnabled(false);
        setSearchString("");

        var response = await getAllPrioritiesAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setPriorities(response.data as IPriority[]);
            setLoadingPriorities(false);
        }
        else {
            setLoadingPriorities(false);
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }
    }

    // Gets priority types.
    const getPriorityTypes = async () => {
        let response = await getPriorityTypesAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setPriorityTypes(response.data as IPriorityType[]);
        }
        else {
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }

        setLoadingPriorityTypes(false);
    }

    // Gets the keywords.
    const getKeywords = async () => {
        let response = await getAllKeywordsAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setKeywords(response.data as IKeyword[]);
        }
        else {
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }

        setLoadingKeywords(false);
    }

    /**
     * Opens a task module to edit a priority.
     * @param priorityId The priority Id to be edited.
     */
    const editPriority = (priorityId: string) => {
        microsoftTeams.tasks.startTask({
            title: localize("insightsConfigEditPriorityTaskModuleTitle"),
            width: 600,
            height: 600,
            url: `${window.location.origin}/new-priority?${Constants.UrlParamInsightsConfigPriorityId}=${priorityId}`
        }, (error: string, result: any) => {
            if (result?.type === "updated") {
                setStatus(prevState => ({ id: prevState.id + 1, message: localize("insightsConfigPriorityUpdatedMessage"), type: ActivityStatus.Success }));

                getPrioritiesAsync();
                return;
            }
        });
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    // Gets the priority type title.
    const getPriorityType = (priorityTypeId: number) => {
        if (isLoadingPriorityTypes) {
            return localize("loadingLabel");
        }

        let priorityType: IPriorityType | undefined = priorityTypes
            .find((priorityType: IPriorityType) => priorityType.id === priorityTypeId);

        if (priorityType) {
            return priorityType.title;
        }

        return "NA";
    }

    // Gets the keywords' string.
    const getKeywordsString = (keywordIds: number[]) => {
        if (isLoadingKeywords) {
            return localize("loadingLabel");
        }

        let keywordIdsStringArray = keywordIds.map(String);

        let keywordsTitleArray: string[] = keywords
            .filter((keyword: IKeyword) => keywordIdsStringArray.includes(keyword.keywordId))
            .map((keyword: IKeyword) => keyword.title);

        return keywordsTitleArray.length ? keywordsTitleArray.join(", ") : "NA";
    }

    // Event handler called when creating new priority.
    const onNewPriorityButtonClick = () => {
        microsoftTeams.tasks.startTask({
            title: localize("fillDetailsText"),
            width: 600,
            height: 600,
            url: `${window.location.origin}/new-priority`
        }, (error: string, result: any) => {
            if (result?.type === "created") {
                setStatus(prevState => ({ id: prevState.id + 1, message: localize("insightsConfigNewPriorityCreatedMessage"), type: ActivityStatus.Success }));
                getPrioritiesAsync();
            }
        });
    }

    // Event handler called when click on edit button.
    const onEditButtonClick = () => {
        let priorityToEdit = priorities
            .find((priority: any) => priority.isChecked === true);

        if (priorityToEdit?.id) {
            editPriority(priorityToEdit.id);
        }
    }

    // Event handler called when delete priority action get confirmed.
    const onConfirmDeletePriorityAsync = async () => {
        setDeletingPriorities(true);

        let priorityIdsToDelete: string[] = priorities
            .filter((priority: IPriority) => priority.isChecked === true)
            .map((priority: IPriority) => priority.id);

        // API call here.
        var response = await deletePrioritiesAsync(priorityIdsToDelete, handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setDeletingPriorities(false);
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("insightsConfigPriorityDeletedMessage"), type: ActivityStatus.Success }));

            getPrioritiesAsync();
        }
        else {
            setDeletingPriorities(false);
            setStatus(prevState => ({ id: prevState.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error }));
        }
    }

    /**
     * Event handler called when search string gets changed.
     * @param searchString The current searched string.
     */
    const onSearchStringChange = (searchString: any) => {
        let prioritiesData: any[] = cloneDeep(priorities);

        let filteredPriorities = prioritiesData.filter((priority: IPriority) => priority.title.toLowerCase().includes(searchString.toLowerCase())
            || priority.description.toLowerCase().includes(searchString.toLowerCase()));

        let totalSelectedPriorities: number = -1;

        if (searchString.trim()) {
            if (filteredPriorities.length) {
                totalSelectedPriorities = filteredPriorities.filter((priority: IPriority) => priority.isChecked).length;
            }
        }
        else {
            totalSelectedPriorities = prioritiesData.filter((priority: IPriority) => priority.isChecked).length;
        }

        setFilteredPriorities(filteredPriorities);
        setEditButtonEnabled(totalSelectedPriorities === 1);
        setDeleteButtonEnabled(totalSelectedPriorities > 0);
        setSearchString(searchString);
        setHeaderCheckboxChecked(searchString.trim() ? totalSelectedPriorities === filteredPriorities.length : totalSelectedPriorities === prioritiesData.length);
    }

    /**
     * Event handler called when click on table header checkbox.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    const onHeaderCheckboxCheckedChange = (eventDetails: any, eventData: any) => {
        let prioritiesData: any[] = cloneDeep(priorities);
        let filteredPrioritiesData: any[] = cloneDeep(filteredPriorities);

        let updatedPriorities: IPriority[] = prioritiesData;
        let updatedFilteredPriorities: IPriority[] = [];

        if (searchString.trim()) {
            updatedFilteredPriorities = filteredPrioritiesData.map((priority: IPriority) => {
                return {
                    ...priority,
                    isChecked: eventData.checked
                }
            });

            if (updatedFilteredPriorities.length) {
                for (let i = 0; i < updatedPriorities.length; i++) {
                    let hasPriority: boolean = updatedFilteredPriorities
                        .some((priority: IPriority) => updatedPriorities[i].id === priority.id);

                    if (hasPriority) {
                        updatedPriorities[i].isChecked = eventData.checked;
                    }
                }
            }
        }
        else {
            updatedPriorities = prioritiesData.map((priority: IPriority) => {
                return {
                    ...priority,
                    isChecked: eventData.checked
                }
            });
        }

        setPriorities(updatedPriorities);
        setFilteredPriorities(updatedFilteredPriorities);
        setDeleteButtonEnabled(eventData.checked);
        setEditButtonEnabled(searchString.trim() ? updatedFilteredPriorities.length === 1 && eventData.checked : updatedPriorities.length === 1 && eventData.checked)
        setHeaderCheckboxChecked(eventData.checked);
    }

    /**
     * Event handler called when click on table row.
     * @param priorityId The priority Id to be edited.
     */
    const onTableRowClick = (priorityId: string) => {
        editPriority(priorityId);
    }

    /**
     * Event handler called when row checkbox checked change.
     * @param eventData The event data.
     * @param priorityId The priority Id.
     */
    const onRowCheckboxCheckedChange = (eventData: any, priorityId: string) => {
        let prioritiesData: IPriority[] = cloneDeep(priorities);
        let filteredPrioritiesData: IPriority[] = cloneDeep(filteredPriorities);

        let priorityToUpdate = prioritiesData.find((priority: IPriority) => priority.id === priorityId);
        let filteredPriorityToUpdate = filteredPrioritiesData.find((priority: IPriority) => priority.id === priorityId);

        if (priorityToUpdate) {
            priorityToUpdate.isChecked = eventData.checked;
        }

        if (filteredPriorityToUpdate) {
            filteredPriorityToUpdate.isChecked = eventData.checked;
        }

        let totalSelectedPriorities: number = -1;

        if (searchString.trim()) {
            totalSelectedPriorities = filteredPrioritiesData.filter((priority: IPriority) => priority.isChecked === true).length;
        }
        else {
            totalSelectedPriorities = prioritiesData.filter((priority: IPriority) => priority.isChecked === true).length;
        }

        setPriorities(prioritiesData);
        setFilteredPriorities(filteredPrioritiesData);
        setEditButtonEnabled(totalSelectedPriorities === 1);
        setDeleteButtonEnabled(totalSelectedPriorities > 0);
        setHeaderCheckboxChecked(searchString.trim() ? totalSelectedPriorities === filteredPrioritiesData.length : totalSelectedPriorities === prioritiesData.length);
    }

    // Gets the table header.
    const getTableHeader = () => {
        return <Table.Row className="header" design={{ minHeight: "4.6rem" }} header>
            <Table.Cell design={{ minWidth: "4rem" }} content={<Checkbox disabled={!priorities.length || isLoadingPriorities || isDeletingPriorities || (searchString.trim().length > 0 && !filteredPriorities.length)} checked={isHeaderCheckboxChecked} onChange={onHeaderCheckboxCheckedChange} />} />
            <Table.Cell design={{ minWidth: "28%" }} content={localize("myRequestsTitleColumn")} title={localize("myRequestsTitleColumn")} truncateContent={true} />
            <Table.Cell design={{ minWidth: "28%" }} content={localize("descriptionText")} title={localize("descriptionText")} truncateContent={true} />
            <Table.Cell design={{ minWidth: "12%" }} content={localize("typeText")} title={localize("typeText")} truncateContent={true} />
            <Table.Cell design={{ minWidth: "28%" }} content={localize("myRequestsKeywordsColumn")} title={localize("myRequestsKeywordsColumn")} truncateContent={true} />
        </Table.Row>;
    }

    // Gets the table rows.
    const getTableRows = () => {
        console.log("computing table rows");

        if (isLoadingPriorities) {
            let tableRowSkeleton = <Table.Row className="row" design={{ minHeight: "4.6rem" }}>
                <Table.Cell design={{ minWidth: "4rem" }} content={<Skeleton.Line width="2rem" height="1.4rem" />} />
                <Table.Cell design={{ minWidth: "28%" }} content={<Skeleton.Line width="26rem" height="1.4rem" />} />
                <Table.Cell design={{ minWidth: "28%" }} content={<Skeleton.Line width="30rem" height="1.4rem" />} />
                <Table.Cell design={{ minWidth: "12%" }} content={<Skeleton.Line width="10rem" height="1.4rem" />} />
                <Table.Cell design={{ minWidth: "28%" }} content={<Skeleton.Line width="30rem" height="1.4rem" />} />
            </Table.Row>;

            return <Skeleton animation="wave">
                {getTableHeader()}
                {tableRowSkeleton}
                {tableRowSkeleton}
                {tableRowSkeleton}
                {tableRowSkeleton}
                {tableRowSkeleton}
            </Skeleton>;
        }

        if (searchString.trim()) {
            if (!filteredPriorities?.length) {
                return [getTableHeader(), <Table.Row className="row" design={{ minHeight: "4.6rem" }}>
                    <Table.Cell content={localize("noSearchResultMessage")} />
                </Table.Row>];
            }

            return [getTableHeader(), ...filteredPriorities.map((priority: IPriority) => {
                let keywords: string = getKeywordsString(priority.keywords);

                return <Table.Row key={`insights-priority-${priority.id}`} className="row" design={{ minHeight: "4.6rem" }} onClick={() => onTableRowClick(priority.id)}>
                    <Table.Cell design={{ minWidth: "4rem" }} content={<Checkbox checked={priority.isChecked} onChange={(eventDetails: any, eventData: any) => onRowCheckboxCheckedChange(eventData, priority.id)} />} onClick={(eventDetails: any) => eventDetails.stopPropagation()} />
                    <Table.Cell design={{ minWidth: "28%" }} content={priority.title} title={priority.title} truncateContent={true} />
                    <Table.Cell design={{ minWidth: "28%" }} content={priority.description} title={priority.description} truncateContent={true} />
                    <Table.Cell design={{ minWidth: "12%" }} content={getPriorityType(priority.type)} truncateContent={true} />
                    <Table.Cell design={{ minWidth: "28%" }} content={keywords} title={keywords} truncateContent={true} />
                </Table.Row>;
            })];
        }

        return [getTableHeader(), ...priorities.map((priority: IPriority) => {
            let keywords: string = getKeywordsString(priority.keywords);

            return <Table.Row key={`insights-priority-${priority.id}`} className="row" design={{ minHeight: "4.6rem" }} onClick={() => onTableRowClick(priority.id)}>
                <Table.Cell design={{ minWidth: "4rem" }} content={<Checkbox checked={priority.isChecked} onChange={(eventDetails: any, eventData: any) => onRowCheckboxCheckedChange(eventData, priority.id)} />} onClick={(eventDetails: any) => eventDetails.stopPropagation()} />
                <Table.Cell design={{ minWidth: "28%" }} content={priority.title} title={priority.title} truncateContent={true} />
                <Table.Cell design={{ minWidth: "28%" }} content={priority.description} title={priority.description} truncateContent={true} />
                <Table.Cell design={{ minWidth: "12%" }} content={getPriorityType(priority.type)} truncateContent={true} />
                <Table.Cell design={{ minWidth: "28%" }} content={keywords} title={keywords} truncateContent={true} />
            </Table.Row>;
        })];
    };

    return (
        <Flex className="insights-configuaration" column>
            <StatusBar status={status} isMobile={false} />
            <AthenaSplash description={localize("insightsConfigurationAthenaSplashDescription")} heading={localize("insightsConfigurationAthenaSplashHeading")} />
            <Segment
                className="menu-box"
                content={<Flex vAlign="center">
                    <Button text icon={<AddIcon />} content={localize("insightsConfigurationNewPriorityButtonContent")} onClick={onNewPriorityButtonClick} />
                    <Button text icon={<EditIcon />} disabled={!isEditButtonEnabled || isDeletingPriorities} content={localize("editButtonContent")} onClick={onEditButtonClick} />
                    <Dialog
                        cancelButton={localize("noLabel")}
                        confirmButton={localize("yesLabel")}
                        content={localize("insightsConfigDeleteConfirmationDialogContent")}
                        header={localize("insightsConfigDeleteConfirmationDialogTitle")}
                        trigger={<Button text icon={<TrashCanIcon />} loading={isDeletingPriorities} disabled={!isDeleteButtonEnabled || isDeletingPriorities} content={localize("deleteButtonContent")} />}
                        onConfirm={onConfirmDeletePriorityAsync}
                    />
                    <Flex.Item push>
                        <Input className="search-box" inverted icon={<SearchIcon />} placeholder={localize("myRequestsFindInputPlaceholder")} disabled={isLoadingPriorities} onChange={(eventDetails: any, eventData: any) => onSearchStringChange(eventData.value)} />
                    </Flex.Item>
                </Flex>
                }
            />
            <Table className="insights-configuration-table">
                {getTableRows()}
            </Table>
        </Flex>
    );
}

export default withRouter(InsightsConfiguration);