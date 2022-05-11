// <copyright file="collections-table.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Table, Flex, Text, MenuButton, TrashCanIcon, MoreIcon, Button, Loader, Image, Avatar } from "@fluentui/react-northstar";
import { useTranslation } from 'react-i18next';
import { IMyCollectionsItem, Item } from "../../../models/my-collections";
import { getCollectionByIdAsync, getCollectionItemDetailsAsync, updateCollectionDataAsync } from "../../../api/my-collections-api";
import StatusBar from "../../common/status-bar/status-bar";
import IStatusBar from "../../../models/status-bar";
import { ActivityStatus } from "../../../models/activity-status";
import moment from "moment";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { StatusCodes } from "http-status-codes";

import "./collections-table.scss";

interface ICollectionTableProps extends RouteComponentProps {
    collectionId: string;
    searchString: string;
}

const CollectionsTable: React.FunctionComponent<ICollectionTableProps> = (props: ICollectionTableProps) => {

    const localize = useTranslation().t;
    const tableResearchColumnDesign = { minWidth: "35vw", maxWidth: "35vw" };
    const tableCreatedColumnDesign = { minWidth: "20vw", maxWidth: "20vw" };
    const tableCategoryColumnDesign = { minWidth: "18vw", maxWidth: "18vw" };
    const [tableRows, setTableRows] = React.useState<any[]>([]);
    const [status, setStatus] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });
    const [isLoading, setIsLoading] = React.useState<boolean>(true);
    const [isPageLoading, setIsPageLoading] = React.useState<boolean>(true);
    const [collectionItemDetails, setCollectionItemDetails] = React.useState<IMyCollectionsItem[]>([]);
    const [filteredCollectionItems, setFilteredCollectionItems] = React.useState<IMyCollectionsItem[]>([]);

    React.useEffect(() => {
        getCollectionItemDetails();
    }, [props.collectionId]);

    React.useEffect(() => {
        getTableRows(filteredCollectionItems);
    }, [filteredCollectionItems]);

    React.useEffect(() => {
        getFilteredCollectionItems(props.searchString);
    }, [props.searchString]);

    /**
     * Filters the collection items based on searched text and selected keywords
     * @param searchText
     */
    const getFilteredCollectionItems = (searchText: string) => {
        setIsLoading(true);
        if (searchText) {
            setFilteredCollectionItems(collectionItemDetails.filter((collection: IMyCollectionsItem) => (collection.collectionItemName?.toUpperCase().indexOf(searchText.toUpperCase()) !== -1)));
        }
        else {
            setFilteredCollectionItems(collectionItemDetails);
        }
        setIsLoading(false);
    }

    // Gets collection details of collection id
    const getCollectionItemDetails = async () => {
        var response = await getCollectionItemDetailsAsync(props.collectionId, handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setIsPageLoading(false);
            setIsLoading(false);
            setCollectionItemDetails(response.data);
            setFilteredCollectionItems(response.data);
        }
        else {
            setIsPageLoading(false);
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

    // Table header
    const tableHeader = {
        key: 'header',
        items: [
            {
                content:
                    <Flex gap="gap.small">
                        <Text content={localize("titleText")} />
                    </Flex>,
                design: tableResearchColumnDesign
            },
            {
                content: <Flex gap="gap.small">
                    <Text content={localize("sourceTitle")} />
                </Flex>,
                design: tableCreatedColumnDesign
            },
            {
                content: <Flex gap="gap.small">
                    <Text content={localize("createdOnText")} />
                </Flex>,
                design: tableCreatedColumnDesign
            },
            {
                content: <Flex gap="gap.small">
                    <Text content={localize("resourceTypeText")} />
                </Flex>,
                design: tableCategoryColumnDesign
            }
        ],
    };

    // Menu Items
    const menuItems = [
        {
            key: '1',
            content: localize("removeFromHistoryText"),
            icon: <TrashCanIcon outline />,
        }
    ];

     /**
     * Handles the selection of menu items
     * @param event
     * @param menuItemProps
     */
    const handleMenuItemClick = (index: number) => {
        removeCollectionItem(index);
    }

    const removeCollectionItem = async (index: number) => {
        setIsLoading(true);
        var collectionResponse = await getCollectionByIdAsync(props.collectionId, handleTokenAccessFailure);

        if (!collectionResponse || collectionResponse.status !== StatusCodes.OK) {
            setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
            return;
        }

        var existingCollection = collectionResponse.data;
        var updatedCollectionItems: Item[] = [];
        var existingCollectionItem = collectionItemDetails.map((collectionItem: IMyCollectionsItem) => {
            return {
                ...collectionItem
            }
        });
        existingCollectionItem.splice(index, 1);
        existingCollectionItem.map((collectionItem: IMyCollectionsItem) => {
            updatedCollectionItems.push({
                itemId: collectionItem.collectionItemId,
                itemType: collectionItem.collectionItemType
            })
        });
        var updatedCollectionData = { ...existingCollection, items: updatedCollectionItems };
        var response = await updateCollectionDataAsync(props.collectionId, updatedCollectionData, handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            setStatus({ id: status.id + 1, message: localize("deleteCollectionSuccessMessage"), type: ActivityStatus.Success });
            setCollectionItemDetails(existingCollectionItem);
            setFilteredCollectionItems(existingCollectionItem);
        }
        else {
            setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
            return;
        }

        setIsLoading(false);
    }

    /**
     * Sets table row
     * @param rowData
     */
    const getTableRows = (rowData: IMyCollectionsItem[]) => {
        setTableRows(rowData.map((collectionItem, index: number) => ({
            key: collectionItem.collectionId,
            items:
                [
                    {
                        content:
                            <>
                            {
                                collectionItem.externalLink ?
                                    <a href={collectionItem.externalLink} target="_blank"><Text content={collectionItem.collectionItemName} title={collectionItem.collectionItemName} className="collection-title" /></a>
                                    :
                                    <Text content={collectionItem.collectionItemName} title={collectionItem.collectionItemName} className="collection-title" />
                            }
                            </>,
                        design: tableResearchColumnDesign
                    },
                    {
                        content: <Text className="collection-title" content={collectionItem.source ?? "NA"} />,
                        design: tableCreatedColumnDesign
                    },
                    {
                        content: <Text content={collectionItem.createdOn ? moment(collectionItem.createdOn).format("DD-MMM-YYYY hh:mm A") : "NA"} />,
                        design: tableCreatedColumnDesign
                    },
                    {
                        content: <Text content={collectionItem.category} />,
                        design: tableCategoryColumnDesign
                    },
                    {
                        content: <MenuButton menu={menuItems} trigger={<Button icon={<MoreIcon />} iconOnly text />} className="icon-pointer" onMenuItemClick={() => handleMenuItemClick(index)} />
                    }
                ]
        })));
    }

    return (
        <>
            <StatusBar status={status} isMobile={false} />
            <Loader hidden={!isLoading} className="page-loader" />
            {
                isPageLoading ?
                    <Loader className="page-loader" />
                    :
                    <>
                        {
                            filteredCollectionItems.length > 0 ?
                                <Table
                                    header={tableHeader}
                                    rows={tableRows}
                                    className="collections-table-style"
                                />
                                :
                                <Flex column gap="gap.medium" padding="padding.medium" hAlign="center" vAlign="center">
                                    <Image src="Artifacts/Image6.png" className="no-result-image" />
                                    <Text content={localize("noCollectionItemsResultErrorMsg")} className="no-result-message" />
                                </Flex>
                        }
                    </>
            }
        </>
    )
};

export default withRouter(CollectionsTable);