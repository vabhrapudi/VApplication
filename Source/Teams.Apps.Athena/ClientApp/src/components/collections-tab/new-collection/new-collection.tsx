// <copyright file="new-collections.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Flex, Input, Text, Button, TextArea, FormLabel, FormField, Table, CloseIcon } from '@fluentui/react-northstar';
import { useTranslation } from 'react-i18next';
import StatusBar from "../../common/status-bar/status-bar";
import IStatusBar from "../../../models/status-bar";
import { ActivityStatus } from "../../../models/activity-status";
import { IMyCollections, IMyCollectionsItem, Item } from "../../../models/my-collections";
import { createCollectionAsync, getCollectionByIdAsync, getCollectionItemDetailsAsync, updateCollectionDataAsync } from "../../../api/my-collections-api";
import * as microsoftTeams from "@microsoft/teams-js";
import { IsValidUrl } from "../../../helpers/url-helper";
import { StatusCodes } from "http-status-codes";
import { withRouter, RouteComponentProps } from "react-router-dom";
import ContentLoader from "../../common/loader/loader";

import "./new-collection.scss";

interface INewsCollectionProps extends RouteComponentProps {
    handleBackButtonClick: () => void;
    handleCreatedCollection: (collection: IMyCollections) => void;
    displayBackButton: boolean;
}

const NewCollection: React.FunctionComponent<INewsCollectionProps> = (props: INewsCollectionProps) => {

    const localize = useTranslation().t;
    const params = new URLSearchParams(window.location.search);
    const tableResearchColumnDesign = { minWidth: "80vw", maxWidth: "80vw" };
    const [title, setTitle] = React.useState<string>("");
    const [description, setDescription] = React.useState<string>("");
    const [imageUrl, setImageUrl] = React.useState<string>("");
    const [titleError, setTitleError] = React.useState<string>("");
    const [descriptionError, setDescriptionError] = React.useState<string>("");
    const [imageUrlError, setImageUrlError] = React.useState<string>("");
    const [collectionId, setCollectionId] = React.useState<string>("");
    const [tableRows, setTableRows] = React.useState<any[]>([]);
    const [collectionItemDetails, setCollectionItemDetails] = React.useState<IMyCollectionsItem[]>([]);
    const [status, setStatus] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });
    const [isLoading, setIsLoading] = React.useState<boolean>(false);
    const [isPageLoading, setIsPageLoading] = React.useState<boolean>(true);

    const tableHeader = {
        key: 'header',
        items: [
            {
                content:
                    <Flex gap="gap.small">
                        <Text content={localize("researchTopicText")} />
                    </Flex>,
                design: tableResearchColumnDesign
            },
            {
                content: <Flex gap="gap.small">
                    <Text content={localize("actionBtnText")} />
                </Flex>
            }
        ],
    };

    React.useEffect(() => {
        setIsPageLoading(false);
        let collectionId = params.get('collectionItemId') ?? "";
        setCollectionId(collectionId!);
    }, []);

    React.useEffect(() => {
        getCollectionDetails();
        getCollectionItemDetails();
    }, [collectionId]);

    React.useEffect(() => {
        getTableRows();
    }, [collectionItemDetails]);

    // Fetches collection item details
    const getCollectionItemDetails = async () => {
        if (collectionId) {
            var response = await getCollectionItemDetailsAsync(collectionId, handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                setIsPageLoading(false);
                setCollectionItemDetails(response.data);
            }
            else {
                setIsPageLoading(false);
                setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
            }
        }
    }

    // Fetches collection deatils by collection id
    const getCollectionDetails = async () => {
        if (collectionId) {
            var response = await getCollectionByIdAsync(collectionId, handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                setIsPageLoading(false);
                var collectionData = response.data;
                setTitle(collectionData.name);
                setDescription(collectionData.description);
                setImageUrl(collectionData.imageURL);
            }
            else {
                setIsPageLoading(false);
                setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
            }
        }
    }

    // Sets table rows
    const getTableRows = () => {
        setTableRows(collectionItemDetails.map((collectionItem: IMyCollectionsItem, index: number) => ({
            key: collectionItem.collectionId,
            items:
                [
                    {
                        content: <Text content={collectionItem.collectionItemName} />,
                        design: tableResearchColumnDesign
                    },
                    {
                        content: <CloseIcon outline className="icon-pointer" onClick={()  => removeCollectionItem(index)} />
                    }
                ]
        })));
    }

    /**
     * Removes the collection item
     * @param index The index.
     */
    const removeCollectionItem = (index: number) => {
        var existingCollectionItem = collectionItemDetails.map((collectionItem: IMyCollectionsItem) => {
            return {
                ...collectionItem
            }
        });
        existingCollectionItem.splice(index, 1);
        setCollectionItemDetails(existingCollectionItem);
    }

    /**
     * Sets the collection's title 
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onTitleChange = async (event: any, inputProps: any) => {
        setTitle(inputProps.value);
        if (inputProps.value) {
            setTitleError("");
        }
    }

    /**
     * Sets the collection's description
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onDescriptionChange = async (event: any, inputProps: any) => {
        setDescription(inputProps.value);
        if (inputProps.value) {
            setDescriptionError("");
        }
    }

    /**
     * Sets the collection's image url
     * @param event The event details.
     * @param inputProps The input props data.
     */
    const onImageUrlChange = async (event: any, inputProps: any) => {
        setImageUrlError("");
        setImageUrl(inputProps.value);
    }

    // Handles create button click
    const handleCreateBtnClick = async () => {
        let inputsValidated = validateInputs();
        if (inputsValidated) {
            setIsLoading(true);
            var myCollection: IMyCollections = {
                name: title,
                description: description,
                imageURL: imageUrl
            }
            var response = await createCollectionAsync(myCollection, handleTokenAccessFailure);
            if (response && response.status === StatusCodes.CREATED) {
                if (props.displayBackButton) {
                    props.handleCreatedCollection(response.data);
                }
                else {
                    setStatus({ id: status.id + 1, message: localize("submitSuccessMessage"), type: ActivityStatus.Success });
                    setIsLoading(false);
                    microsoftTeams.tasks.submitTask(response.data);
                }
            }
            else if (response && response.status === StatusCodes.CONFLICT) {
                setStatus({ id: status.id + 1, message: localize("collectionWithTitleExists"), type: ActivityStatus.Error });
                setIsLoading(false);
            }
            else {
                if (response && typeof response.data === 'string') {
                    setStatus({ id: status.id + 1, message: response.data, type: ActivityStatus.Error });
                }
                else {
                    setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
                }
                setIsLoading(false);
            }
        }
        else {
            return;
        }
    }

    // Handles create button click
    const handleUpdateBtnClick = async () => {
        let inputsValidated = validateInputs();
        if (inputsValidated) {
            setIsLoading(true);

            var existingCollectionItem = collectionItemDetails.map((collectionItem: IMyCollectionsItem) => { return { ...collectionItem } });
            var updatedCollectionItems: Item[] = [];

            existingCollectionItem.map((collectionItem: IMyCollectionsItem) => {
                updatedCollectionItems.push({
                    itemId: collectionItem.collectionItemId,
                    itemType: collectionItem.collectionItemType
                })
            });

            var updatedCollection: IMyCollections = {
                collectionId: collectionId,
                name: title,
                description: description,
                imageURL: imageUrl,
                items: updatedCollectionItems
            }

            var response = await updateCollectionDataAsync(collectionId, updatedCollection, handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                setStatus({ id: status.id + 1, message: localize("submitSuccessMessage"), type: ActivityStatus.Success });
                setIsLoading(false);
                microsoftTeams.tasks.submitTask(response.data);
            }
            else if (response && response.status === StatusCodes.CONFLICT) {
                setStatus({ id: status.id + 1, message: localize("collectionWithTitleExists"), type: ActivityStatus.Success });
                setIsLoading(false);
            }
            else {
                setStatus({ id: status.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
                setIsLoading(false);
            }
        }
        else {
            return;
        }
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    // Validates all the values in the input fields 
    const validateInputs = () => {
        let isValid = true;
        if (!title) {
            setTitleError(localize("enterTitleErrorMsg"));
            isValid = false;
        }
        if (!description) {
            setDescriptionError(localize("addDescriptionErrorMessage"));
            isValid = false;
        }
        if (!imageUrl) {
            setImageUrlError(localize("enterImageUrlErrorMsg"));
            isValid = false;
        }
        if (imageUrl) {
            var isValidUrl = IsValidUrl(imageUrl);
            if (!isValidUrl) {
                setImageUrlError(localize("invalidImageUrlMsg"));
                isValid = false;
            }
        }
        return isValid;
    }

    return (
         <>
            {
                isPageLoading ?
                    <ContentLoader />
                    :
                    <Flex fill column gap="gap.medium" className={props.displayBackButton ? "" : "task-module-container"}>
                        <Flex fill column gap="gap.medium" className="form-fields">
                            <StatusBar status={status} isMobile={false} />
                            <Flex gap="gap.small" column>
                                <FormLabel>
                                    <Text content={"*" + localize("collectionTitleText")} />
                                </FormLabel>
                                <FormField>
                                    <Input fluid placeholder={localize("titlePlaceholderTxt")} value={title} onChange={onTitleChange} maxLength={75} />
                                </FormField>
                                {
                                    titleError &&
                                    <Text content={titleError} error />
                                }
                            </Flex>
                            <Flex gap="gap.small" column>
                                <FormLabel>
                                    <Text content={"*" + localize("descriptionText")} />
                                </FormLabel>
                                <FormField>
                                    <TextArea placeholder={localize("typeHerePlaceholderTxt")} value={description} onChange={onDescriptionChange} maxLength={500} className="desc-text-area" />
                                </FormField>
                                {
                                    descriptionError &&
                                    <Text content={descriptionError} error />
                                }
                            </Flex>
                            <Flex gap="gap.small" column>
                                <FormLabel>
                                    <Text content={"*" + localize("imageUrlText")} />
                                </FormLabel>
                                <FormField>
                                    <Input fluid placeholder={localize("imageUrlPlaceholderTxt")} value={imageUrl} onChange={onImageUrlChange} />
                                </FormField>
                                {
                                    imageUrlError &&
                                    <Text content={imageUrlError} error />
                                }
                            </Flex>
                            {
                                collectionItemDetails.length > 0 &&
                                <Flex gap="gap.small" column>
                                    <Text content={localize("researchTopicText")} weight="bold" />
                                    <Table header={tableHeader} rows={tableRows} />
                                </Flex>
                            }
                        </Flex>
                        <Flex>
                            <Flex.Item push>
                                <Flex gap="gap.medium">
                                    {
                                        props.displayBackButton &&
                                        <Button className="athena-button" content={localize("backButtonContent")} onClick={props.handleBackButtonClick} />
                                    }
                                    <Button className="athena-button" content={!collectionId ? localize("createButtonText") : localize("updateButtonText")} onClick={!collectionId ? handleCreateBtnClick : handleUpdateBtnClick} loading={isLoading} disabled={isLoading} />
                                </Flex>
                            </Flex.Item>
                        </Flex>
                    </Flex>
            }
        </>
    )
}

export default withRouter(NewCollection);