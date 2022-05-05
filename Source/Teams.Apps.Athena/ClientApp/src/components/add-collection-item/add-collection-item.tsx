// <copyright file="add-collection-item.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Flex, RadioGroup, Text, Button, Image } from '@fluentui/react-northstar';
import { useTranslation } from 'react-i18next';
import StatusBar from "../common/status-bar/status-bar";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import * as microsoftTeams from "@microsoft/teams-js";
import { StatusCodes } from "http-status-codes";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { getAllCollectionsDataAsync, updateCollectionDataAsync, getCollectionItemDetailsAsync } from "../../api/my-collections-api";
import { IMyCollections, Item } from "../../models/my-collections";
import NewCollection from "../collections-tab/new-collection/new-collection";
import ContentLoader from "../common/loader/loader";
import { DiscoveryNodeFileNames } from "../../models/discovery-tree-node-file-names";
import { ItemType } from "../../models/discovery-tree-item-type";

interface IAddCollectionItemProps extends RouteComponentProps {
}

const AddCollectionItem: React.FunctionComponent<IAddCollectionItemProps> = (props: IAddCollectionItemProps) => {

    const localize = useTranslation().t;
    const params = new URLSearchParams(window.location.search);
    const [status, setStatus] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });
    const [isLoading, setIsLoading] = React.useState<boolean>(false);
    const [itemTableId, setItemTableId] = React.useState<string>("");
    const [itemJsonFile, setItemJsonFile] = React.useState<string>("");
    const [isPageLoading, setIsPageLoading] = React.useState<boolean>(true);
    const [collectionsData, setCollectionsData] = React.useState<IMyCollections[]>([]);
    const [selectedCollection, setSelectedCollection] = React.useState<IMyCollections | undefined>(undefined);
    const [isNewCollectionClicked, setIsNewCollectionClicked] = React.useState<boolean>(false);

    React.useEffect(() => {
        let itemTableId = params.get('itemTableId') ?? "";
        let index = itemTableId.indexOf('_');
        if (index !== -1) {
            itemTableId = itemTableId.substring(0, index);
        }
        let itemJsonFile = params.get('itemJsonFile') ?? "";
        setItemTableId(itemTableId);
        setItemJsonFile(itemJsonFile);
        getCollectionData();
    }, []);

    const getCollectionData = async () => {
        var response = await getAllCollectionsDataAsync(handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setCollectionsData(response.data);
        }
        else {
            setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
        }
        setIsPageLoading(false);
    }

    const handleAddToCollectionBtnClick = async (collection: IMyCollections | undefined) => {
        setIsLoading(true);

        var existingCollection = collection ?? selectedCollection;

        let apiResponse = await getCollectionItemDetailsAsync(existingCollection!.collectionId!, handleTokenAccessFailure);
        if (apiResponse && apiResponse.status === StatusCodes.OK) {
            var existingCollectionItem: Item[] = apiResponse.data.map(collectionItem => {
                return ({
                    itemId: collectionItem.collectionItemId,
                    itemType: collectionItem.collectionItemType
                })
            });

            let itemType: number = 0;
            if (itemJsonFile === DiscoveryNodeFileNames.AthenaResearchProjects) {
                itemType = ItemType.ResearchProject;
            }
            else if (itemJsonFile === DiscoveryNodeFileNames.AthenaNewsArticles) {
                itemType = ItemType.News;
            }
            else if (itemJsonFile === DiscoveryNodeFileNames.AthenaCommunities) {
                itemType = ItemType.COI;
            }
            else if (itemJsonFile === DiscoveryNodeFileNames.AthenaResearchRequests) {
                itemType = ItemType.ResearchRequest;
            }
            else if (itemJsonFile === DiscoveryNodeFileNames.AthenaResearchProposals) {
                itemType = ItemType.ResearchProposal;
            }
            else if (itemJsonFile === DiscoveryNodeFileNames.AthenaEvents) {
                itemType = ItemType.Event;
            }
            else if (itemJsonFile === DiscoveryNodeFileNames.AthenaPartners) {
                itemType = ItemType.Partner;
            }
            else if (itemJsonFile === DiscoveryNodeFileNames.AthenaSponsors) {
                itemType = ItemType.Sponsor;
            }
            else if (itemJsonFile === DiscoveryNodeFileNames.AthenaSources) {
                itemType = ItemType.Source;
            }
            else if (itemJsonFile === DiscoveryNodeFileNames.AthenaInfoResources) {
                itemType = ItemType.Information;
            }
            else if (itemJsonFile === DiscoveryNodeFileNames.AthenaTools) {
                itemType = ItemType.Tool;
            }

            if (itemType) {
                var item: Item = {
                    itemId: itemTableId,
                    itemType: itemType
                };

                var index = existingCollectionItem.findIndex(collectionItem => collectionItem.itemId === item.itemId);
                if (index === -1) {
                    existingCollectionItem.push(item);
                }

                var updatedCollectionData: IMyCollections = {
                    collectionId: existingCollection!.collectionId,
                    name: existingCollection!.name,
                    description: existingCollection!.description,
                    imageURL: existingCollection!.imageURL,
                    items: existingCollectionItem
                }

                var response = await updateCollectionDataAsync(existingCollection!.collectionId!, updatedCollectionData, handleTokenAccessFailure);
                if (response && response.status === StatusCodes.OK) {
                    setStatus({ id: status.id + 1, message: localize("addedToCollectionSuccessMessage"), type: ActivityStatus.Success });
                    microsoftTeams.tasks.submitTask();
                }
                else {
                    setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
                }
            }
        }
        else {
            setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
        }
        
        setIsLoading(false);
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    // Sets lists of existing collections
    const getItems = () => {
        var items: any[] = [];
        collectionsData.map(collection => {
            items.push({
                name: collection.name,
                key: collection.collectionId,
                label: collection.name,
                value: collection.name,
            })
        })
        return items;
    }

    // Redirects the task module to add new collection
    const onNewCollectionBtnClick = () => {
        setSelectedCollection(undefined);
        setIsNewCollectionClicked(true);
    }

    const handleChange = (e, props) => {
        var value = props.value;
        var selectedCollection = collectionsData.find(collection => collection.name === value);
        setSelectedCollection(selectedCollection);
    }

    const handleBackButtonClick = () => {
        setIsNewCollectionClicked(false);
    }

    const handleCreatedCollection = (collection: IMyCollections) => {
        handleAddToCollectionBtnClick(collection);
    }

    return (
        <>
            {
                isPageLoading ?
                    <ContentLoader />
                    :
                    <Flex fill column gap="gap.medium" className="task-module-container">
                        <StatusBar status={status} isMobile={false} />
                        {
                            isNewCollectionClicked ?
                                <NewCollection handleBackButtonClick={handleBackButtonClick} handleCreatedCollection={handleCreatedCollection} displayBackButton={true} />
                                :
                                <>
                                    <Flex fill column gap="gap.medium" className="form-fields">
                                        <Flex column gap="gap.medium">
                                            <Text content={localize("addToCollectionPageContent")} weight="bold" />
                                            {
                                                collectionsData.length > 0 ?
                                                    <RadioGroup
                                                        vertical
                                                        items={getItems()}
                                                        onCheckedValueChange={handleChange}
                                                    />
                                                    :
                                                    <Flex column gap="gap.medium" padding="padding.medium" hAlign="center" vAlign="center">
                                                        <Image src="Artifacts/Image6.png" className="no-result-image" />
                                                        <Text content={localize("noMyCollectionsResultErrorMsg")} className="no-result-message" />
                                                    </Flex>
                                            }
                                        </Flex>
                                    </Flex>
                                    <Flex>
                                        <Flex.Item push>
                                            <Flex gap="gap.medium">
                                                <Button className="athena-button" content={localize("newCollectionText")} onClick={onNewCollectionBtnClick} />
                                                <Button className="athena-button" content={localize("addToCollectionMenuItemText")} onClick={() => handleAddToCollectionBtnClick(undefined)} loading={isLoading} disabled={!selectedCollection || isLoading} />
                                            </Flex>
                                        </Flex.Item>
                                    </Flex>
                                </>
                        }
                    </Flex>                           
            }
        </>
    )
}

export default withRouter(AddCollectionItem);