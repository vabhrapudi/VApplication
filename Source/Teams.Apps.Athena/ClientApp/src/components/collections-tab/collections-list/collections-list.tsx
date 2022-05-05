// <copyright file="collections-list.tsx " company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Flex, Text, MoreIcon, EditIcon, TrashCanIcon, MenuButton, Button, Card } from "@fluentui/react-northstar";
import { useTranslation } from 'react-i18next';
import { IMyCollections } from "../../../models/my-collections";
import * as microsoftTeams from "@microsoft/teams-js";
import { getBaseUrl } from "../../../configVariables";
import CardImage from "../../common/card-image/card-image";
import { deleteCollectionAsync } from "../../../api/my-collections-api";
import { ActivityStatus } from "../../../models/activity-status";
import IStatusBar from "../../../models/status-bar";
import Constants from "../../../constants/constants";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { StatusCodes } from "http-status-codes";

import "./collections-list.scss";

interface ICollectionListProps extends RouteComponentProps {
    collection: IMyCollections;
    handleCollectionItemClick: (collection: IMyCollections) => void;
    onDeleteCollectionClick: (collectionId: string, status: IStatusBar) => void;
    handleUpdateCollection: (collection: IMyCollections) => void;
    handleIsDeletingLoader: (isDeleting: boolean) => void;
}

const CollectionsList: React.FunctionComponent<ICollectionListProps> = (props: ICollectionListProps) => {

    const localize = useTranslation().t;
    const menuItems = [
        {
            icon: (
                <EditIcon
                    {...{
                        outline: true,
                        size: "small",
                    }}
                />
            ),
            key: '1',
            content: localize("editText"),
        },
        {
            icon: (
                <TrashCanIcon
                    {...{
                        outline: true,
                        size: "small",
                    }}
                />
            ),
            key: '2',
            content: localize("deleteText"),
        }
    ]

    // Opens the task module to edit collection
    const onEditBtnClick = () => {
        microsoftTeams.tasks.startTask({
            title: localize("fillDetailsText"),
            height: Constants.taskModuleHeight,
            width: Constants.taskModuleWidth,
            url: getBaseUrl() + `/new-collection?collectionItemId=${props.collection.collectionId}`,
            fallbackUrl: getBaseUrl() + `/new-collection?collectionItemId=${props.collection.collectionId}`,
        }, updateCollection);
    }

    /**
     * Updates the collection
     * @param err
     * @param details
     */
    const updateCollection = async (err: any, details: any) => {
        if (details != null) {
            props.handleUpdateCollection(details);
        }
    };

    // Deletes the collection
    const onDeleteBtnClick = async () => {
        props.handleIsDeletingLoader(true);
        var response = await deleteCollectionAsync(props.collection.collectionId!, handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            props.onDeleteCollectionClick(props.collection.collectionId!, { id: 1, message: localize("deleteCollectionSuccessMessage"), type: ActivityStatus.Success });
            props.handleIsDeletingLoader(false);
        }
        else {
            props.onDeleteCollectionClick(props.collection.collectionId!, { id: 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
            props.handleIsDeletingLoader(false);
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
     * Handles the selection of menu items
     * @param event
     * @param menuItemProps
     */
    const handleMenuItemClick = (event: any, menuItemProps: any) => {
        if (menuItemProps.index === 0) {
            onEditBtnClick();
        }
        else if (menuItemProps.index === 1) {
            onDeleteBtnClick();
        }
    }

    // Handles selection of collection
    const handleCollectionItemClick = () => {
        props.handleCollectionItemClick(props.collection);
    }
   
    return (
        <Card className="card">
            <Card.Header onClick={handleCollectionItemClick} className="icon-pointer">
                <CardImage imageSrc={props.collection.imageURL} className="card-image" />
            </Card.Header>
            <Card.Body onClick={handleCollectionItemClick} className="card-sub-container">
                <Flex gap="gap.small" column>
                    <Text content={props.collection.name} weight="bold" />
                    <Text content={props.collection.description} className="card-description" size="small" />
                </Flex>
            </Card.Body>
            <Card.Footer className="card-footer-container">
                <Flex hAlign="end">
                    <MenuButton menu={menuItems} trigger={<Button icon={<MoreIcon />} iconOnly text />} className="icon-pointer" onMenuItemClick={handleMenuItemClick} />
                </Flex>
            </Card.Footer>
        </Card>
    )
};

export default withRouter(CollectionsList);