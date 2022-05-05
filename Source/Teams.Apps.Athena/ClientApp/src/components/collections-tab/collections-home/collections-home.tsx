// <copyright file="collections-home.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Flex, Button, Text, Input, Image, SearchIcon, AddIcon, Loader, ArrowLeftIcon } from "@fluentui/react-northstar";
import { useTranslation } from 'react-i18next';
import * as microsoftTeams from "@microsoft/teams-js";
import { getBaseUrl } from "../../../configVariables";
import { IMyCollections } from "../../../models/my-collections";
import CollectionsList from "../collections-list/collections-list";
import CollectionsTable from "../collections-table/collections-table";
import StatusBar from "../../common/status-bar/status-bar";
import IStatusBar from "../../../models/status-bar";
import { ActivityStatus } from "../../../models/activity-status";
import { getAllCollectionsDataAsync } from "../../../api/my-collections-api";
import Constants from "../../../constants/constants";
import { Col, Row } from "react-bootstrap";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { StatusCodes } from "http-status-codes";
import AthenaSplash from "../../athena-splash/athena-splash";

import "./collections-home.scss";

interface ICollectionsHomeProps extends RouteComponentProps {
}

const CollectionsHome: React.FunctionComponent<ICollectionsHomeProps> = (props: ICollectionsHomeProps) => {

    const localize = useTranslation().t;
    const [searchText, setSearchText] = React.useState<string>("");
    const [collectionsData, setCollectionsData] = React.useState<IMyCollections[]>([]);
    const [filterdCollectionsData, setFilteredCollectionsData] = React.useState<IMyCollections[]>([]);
    const [isLoading, setIsLoading] = React.useState<boolean>(true);
    const [status, setStatus] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });
    const [selectedCollectionId, setSelectedCollectionId] = React.useState<string>("");
    const [selectedCollectionName, setSelectedCollectionName] = React.useState<string>("");
    const [isDeleting, setIsDeleting] = React.useState<boolean>(false);

    React.useEffect(() => {
        microsoftTeams.initialize();
        getCollectionsData();
    }, []);

    /**
     * Handles page loading on clicking delete collection button
     * @param isDeleting
     */
    const handleIsDeletingLoader = (isDeleting: boolean) => {
        setIsDeleting(isDeleting);
    }

    // Gets the collection Data from api
    const getCollectionsData = async () => {
        var response = await getAllCollectionsDataAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setIsLoading(false);
            setCollectionsData(response.data);
            setFilteredCollectionsData(response.data);
        }
        else {
            setIsLoading(false);
            setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
        }
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    // Opens the task module to add new collection
    const onNewCollectionBtnClick = () => {
        microsoftTeams.tasks.startTask({
            title: localize("fillDetailsText"),
            height: Constants.taskModuleHeight,
            width: Constants.taskModuleWidth,
            url: getBaseUrl() + `/new-collection`,
            fallbackUrl: getBaseUrl() + `/new-collection`,
        }, createCollectionSubmitHandler);
    }

    /**
     * Submit handler of create collection
     * @param err
     * @param details
     */
    const createCollectionSubmitHandler = async (err: any, details: any) => {
        if (details != null) {
            var existingData = collectionsData.map((collection: IMyCollections) => {
                return {
                    ...collection
                }
            });
            existingData.unshift(details);
            existingData.concat(details);
            setCollectionsData(existingData);
            setFilteredCollectionsData(existingData);
        }
    };

    /**
     * Updates the collection
     * @param err
     * @param details
     */
    const handleUpdateCollection = async (collection: IMyCollections) => {
        var existingData = collectionsData.map((existingCollection: IMyCollections) => {
            return {
                ...existingCollection
            }
        });
        var index = existingData.findIndex((existingCollection: IMyCollections) => existingCollection.collectionId === collection.collectionId);
        if (index !== -1) {
            existingData.splice(index, 1);
        }
        existingData.unshift(collection);
        existingData.concat(collection);
        setCollectionsData(existingData);
        setFilteredCollectionsData(existingData);
    };

    /**
     * Gets filtered data based on search string and selected keywords
     * @param searchText
     */
    const getFilteredCollectionData = (searchText: string) => {
        if (searchText) {
            setFilteredCollectionsData(collectionsData.filter((collection: IMyCollections) => (collection.name?.toUpperCase().indexOf(searchText.toUpperCase()) !== -1)));
        }
        else {
            setFilteredCollectionsData(collectionsData);
        }
    }

    /**
     * Method to set search text given in the search box.
     * @param event
     */
    const handleSearchInputChange = (event: any) => {
        setSearchText(event.target.value);
        getFilteredCollectionData(event.target.value);
    }

    /**
     * Handles the click on collection item.
     * @param collection The selected collection.
     */
    const handleCollectionItemClick = (collection: IMyCollections) => {
        setSelectedCollectionId(collection.collectionId!);
        setSelectedCollectionName(collection.name);
    }

    // Handles back button click.
    const handleBackButtonClick = () => {
        setSelectedCollectionId("");
        setSelectedCollectionName("");
    }

    /**
     * Handles the click on collection item
     * @param collectionId
     * @param deleteStatus
     */
    const onDeleteCollectionClick = (collectionId: string, deleteStatus: IStatusBar) => {
        if (deleteStatus.type === ActivityStatus.Success) {
            setStatus({ id: status.id + 1, message: deleteStatus.message, type: ActivityStatus.Success });
            var existingData = collectionsData.map((collection: IMyCollections) => {
                return {
                    ...collection
                }
            });
            var index = existingData.findIndex((collection: IMyCollections) => collection.collectionId === collectionId);
            if (index !== -1) {
                existingData.splice(index, 1);
                setCollectionsData(existingData);
                setFilteredCollectionsData(existingData);
            }
        }
        else if (deleteStatus.type === ActivityStatus.Error) {
            setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
        }
    }

    return (
        <Flex column className="collections-main-container">
            <StatusBar status={status} isMobile={false} />
            <AthenaSplash description={localize("myCollectionTabDescription")} heading={localize("myCollectionsText")} />
            <Flex className="collections-header-container">
                {selectedCollectionId !== "" ? <Button text content={localize("backButtonContent")} icon={<ArrowLeftIcon />} onClick={handleBackButtonClick} /> : null}
                {
                    selectedCollectionName &&
                    <Flex.Item push>
                        <Text content={selectedCollectionName} weight="bold" className="collections-name-text" />
                    </Flex.Item>
                }
                {selectedCollectionId === "" ? <Button text content={localize("newCollectionText")} icon={<AddIcon size="medium" />} iconOnly className="icon-pointer" onClick={onNewCollectionBtnClick} /> : null}
                <Flex.Item push>
                    <Input inverted icon={<SearchIcon />} className="collections-search-box" placeholder={localize("findText")} onChange={handleSearchInputChange} />
                </Flex.Item>
            </Flex>
            <div className="collection-card-container">
                {
                    isLoading ?
                        <Loader hidden={!isLoading} className="page-loader" />
                        :
                        <>
                            {
                                selectedCollectionId !== ""
                                    ?
                                    <CollectionsTable collectionId={selectedCollectionId} searchString={searchText} />
                                    :
                                    <>
                                        {
                                            filterdCollectionsData.length > 0 ?
                                                        <Row>
                                                                <Flex gap="gap.medium" className="card-container" wrap>
                                                                    <Loader hidden={!isDeleting} className="page-loader" />
                                                                    {
                                                                        filterdCollectionsData.map((collectionItem: IMyCollections) => {
                                                                            return (
                                                                                <Col lg={3} sm={6} md={4}>
                                                                                    <CollectionsList collection={collectionItem} handleCollectionItemClick={handleCollectionItemClick} onDeleteCollectionClick={onDeleteCollectionClick} handleUpdateCollection={handleUpdateCollection} handleIsDeletingLoader={handleIsDeletingLoader} />
                                                                                </Col>
                                                                            )
                                                                        })
                                                                    }
                                                                </Flex>
                                                        </Row>

                                                
                                                :
                                                <Flex column gap="gap.medium" padding="padding.medium" hAlign="center" vAlign="center">
                                                    <Image src="Artifacts/Image6.png" className="no-result-image" />
                                                    <Text content={localize("noMyCollectionsResultErrorMsg")} className="no-result-message" />
                                                </Flex>
                                        }
                                    </>
                            }
                        </>
                }
            </div>
        </Flex>
    )
};

export default withRouter(CollectionsHome);