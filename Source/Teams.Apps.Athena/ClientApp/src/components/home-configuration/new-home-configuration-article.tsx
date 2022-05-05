// <copyright file="new-home-configuration-article.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { withTranslation, WithTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { Flex, FormInput, Text, Button } from "@fluentui/react-northstar";
import StatusBar from "../../components/common/status-bar/status-bar";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import { IsValidUrl } from "../../helpers/url-helper";
import Constants from "../../constants/constants";
import ContentLoader from "../../components/common/loader/loader";
import IHomeConfigurationArticle from "../../models/home-configuration-article";
import { createHomeConfigurationArticleAsync, deleteHomeConfigurationArticlesAsync, getHomeConfigurationArticleAsync, updateHomeConfigurationArticleAsync } from "../../api/home-configurations-api";
import { StatusCodes } from "http-status-codes";
import { Editor } from 'react-draft-wysiwyg';
import { EditorState, convertToRaw, ContentState } from 'draft-js';
import draftToHtml from 'draftjs-to-html';
import htmlToDraft from 'html-to-draftjs';

import "./new-home-configuration-article.scss";
import '../../../node_modules/react-draft-wysiwyg/dist/react-draft-wysiwyg.css';

interface INewHomeConfigurationArticleProps extends WithTranslation, RouteComponentProps {
}

interface INewHomeConfigurationArticleState {
    article: IHomeConfigurationArticle;
    status: IStatusBar;
    isSubmitting: boolean;
    isLoadingDetails: boolean;
    isDeleteArticleConfirmationScreenActivated: boolean;
    descriptionEditorState: any;
}

const TitleMaxLength: number = 75;

class NewHomeConfigurationArticle extends React.Component<INewHomeConfigurationArticleProps, INewHomeConfigurationArticleState> {
    readonly localize: TFunction;
    readonly itemIdToEditOrDelete: string | null;

    teamId: string = "";

    constructor(props) {
        super(props);

        this.localize = this.props.t;

        let urlParams = new URLSearchParams(window.location.search);
        this.itemIdToEditOrDelete = urlParams.get(Constants.UrlParamHomeConfigArticleId);

        this.state = {
            article: { title: "", description: "", imageUrl: "" } as IHomeConfigurationArticle,
            status: { id: 0, message: "", type: ActivityStatus.None },
            isSubmitting: false,
            isLoadingDetails: this.itemIdToEditOrDelete ? true : false,
            isDeleteArticleConfirmationScreenActivated: false,
            descriptionEditorState: EditorState.createEmpty()
        }
    }

    async componentDidMount() {
        microsoftTeams.initialize();
        microsoftTeams.getContext(async (context: microsoftTeams.Context) => {
            this.teamId = context.groupId!;

            if (this.itemIdToEditOrDelete) {
                this.getConfigurationItemAsync();
            }
        });
    }

    // Gets the details of article to be edited or deleted.
    getConfigurationItemAsync = async () => {
        var response = await getHomeConfigurationArticleAsync(this.teamId, this.itemIdToEditOrDelete!, this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            const contentBlock = response.data?.description ? htmlToDraft(response.data.description) : null;

            if (contentBlock) {
                const contentState = ContentState.createFromBlockArray(contentBlock.contentBlocks);
                const editorState = EditorState.createWithContent(contentState);

                this.setState({
                    descriptionEditorState: editorState,
                    article: response.data,
                    isLoadingDetails: false
                })
            }
            else {
                this.setState({ article: response.data, isLoadingDetails: false });
            }
        }
        else {
            this.setState((prevState: INewHomeConfigurationArticleState) => ({
                isLoadingDetails: false,
                status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
            }));
        }
    }

    // Validates all form field. Returns 'true' if all required fields filled and valid.
    validateFields = (): boolean => {
        if (!this.state.article.title.trim()) {
            this.setState((prevState: INewHomeConfigurationArticleState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("newHomeConfigItemFormFieldsRequiredError"), type: ActivityStatus.Error }
            }));

            return false;
        }

        let isDescriptionHasScriptTag = this.state.article.description?.length > 0
            && this.state.article.description.toLowerCase().includes("script");

        if (isDescriptionHasScriptTag) {
            this.setState((prevState: INewHomeConfigurationArticleState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("scriptTagIsNotAllowedError"), type: ActivityStatus.Error }
            }));

            return false;
        }

        if (!this.state.article.imageUrl.trim() || !IsValidUrl(this.state.article.imageUrl.trim())) {
            this.setState((prevState: INewHomeConfigurationArticleState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("requestImageUrlRequiredError"), type: ActivityStatus.Error }
            }));

            return false;
        }

        return true;
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    handleTokenAccessFailure = (error: string) => {
        this.props.history.push("/signin");
    }

    /**
     * Event handler called when title get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onTitleChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        this.setState((prevState: INewHomeConfigurationArticleState) => ({
            article: { ...prevState.article, title: eventData.value }
        }));
    }

    /**
     * Event handler called when edit state gets changed.
     * @param editorState The editor state.
     */
    onDescriptionEditorStateChange = (editorState: any) => {
        this.setState((prevState: INewHomeConfigurationArticleState) => ({
            descriptionEditorState: editorState,
            article: { ...prevState.article, description: draftToHtml(convertToRaw(editorState.getCurrentContent())) }
        }));
    }

    /**
     * Event handler called when description get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onImageUrlChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        this.setState((prevState: INewHomeConfigurationArticleState) => ({
            article: { ...prevState.article, imageUrl: eventData.value }
        }));
    }

    // Event handler called when submit a configuration item details.
    onSubmit = async () => {
        let isAllFieldsValid: boolean = this.validateFields();

        if (!isAllFieldsValid) {
            return;
        }

        this.setState({ isSubmitting: true });

        if (this.itemIdToEditOrDelete) {
            var response = await updateHomeConfigurationArticleAsync(this.teamId, this.state.article, this.handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                microsoftTeams.tasks.submitTask({ type: "update" });
            }
            else {
                this.setState((prevState: INewHomeConfigurationArticleState) => ({
                    isSubmitting: false,
                    status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
                }));
            }
        }
        else {
            var response = await createHomeConfigurationArticleAsync(this.teamId, this.state.article, this.handleTokenAccessFailure);

            if (response && response.status === StatusCodes.CREATED) {
                microsoftTeams.tasks.submitTask({ status: ActivityStatus.Success });
            }
            else {
                this.setState((prevState: INewHomeConfigurationArticleState) => ({
                    isSubmitting: false,
                    status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
                }));
            }
        }
    }

    // Event hanlder called when 'Delete' button gets clicked.
    onDeleteButtonClick = () => {
        this.setState({ isDeleteArticleConfirmationScreenActivated: true });
    }

    // Event handler called when 'Back' button clicked on delete confirmation screen.
    onBackButtonClick = () => {
        this.setState({ isDeleteArticleConfirmationScreenActivated: false });
    }

    // Event handler called when delete item is confirmed.
    onConfirmDelete = async () => {
        this.setState({ isSubmitting: true });

        var response = await deleteHomeConfigurationArticlesAsync(this.teamId, [this.itemIdToEditOrDelete!], this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            microsoftTeams.tasks.submitTask({ type: "delete" });
        }
        else {
            this.setState((prevState: INewHomeConfigurationArticleState) => ({
                isSubmitting: false,
                status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
            }));
        }
    }

    // Returns the confirmation screen when item needs to be deleted.
    getDeleteArticleConfirmationScreen = () => {
        return <Flex className="overflow-y" column gap="gap.medium">
            <Text content={this.localize("newHomeConfigItemDeleteConfirmationMessage")} weight="semibold" />
        </Flex>;
    }

    // Gets the screen to create or edit the item details.
    getFillAndUpdateDetailsScreen = () => {
        return <Flex className="overflow-y" column gap="gap.medium">
            <FormInput
                label={this.localize("titleText")}
                placeholder={this.localize("newRequestNewsTitleInputPlaceholder")}
                maxLength={TitleMaxLength}
                onChange={this.onTitleChange}
                value={this.state.article.title}
                showSuccessIndicator={false}
                disabled={this.state.isSubmitting}
                required
                fluid />
            <Flex column>
                <Text content={this.localize("newRequestDescriptionInputLabel")} />
                <Editor
                    wrapperClassName="description-editor-wrapper"
                    editorClassName="editor-class"
                    toolbarClassName="toolbar-class"
                    editorState={this.state.descriptionEditorState}
                    toolbar={{
                        options: ['inline', 'link', 'remove', 'history']
                    }}
                    onEditorStateChange={this.onDescriptionEditorStateChange}
                />
            </Flex>
            <FormInput
                label={this.localize("newRequestImageUrlInputLabel")}
                placeholder={this.localize("newRequestImageUrlInputPlaceholder")}
                onChange={this.onImageUrlChange}
                value={this.state.article.imageUrl}
                showSuccessIndicator={false}
                disabled={this.state.isSubmitting}
                required
                fluid />
        </Flex>;
    }

    // Renders the action buttons.
    renderActionButtons = () => {
        if (this.state.isDeleteArticleConfirmationScreenActivated) {
            return <Flex.Item push>
                <Flex gap="gap.small">
                    <Flex.Item push>
                        <Button
                            content={this.localize("backButtonText")}
                            disabled={this.state.isSubmitting}
                            onClick={this.onBackButtonClick} />
                    </Flex.Item>
                    <Button
                        className="athena-button"
                        content={this.localize("okButtonContent")}
                        disabled={this.state.isSubmitting}
                        loading={this.state.isSubmitting}
                        onClick={this.onConfirmDelete} />
                </Flex>
            </Flex.Item>;
        }

        if (this.itemIdToEditOrDelete) {
            return <Flex.Item push>
                <Flex gap="gap.small">
                    <Flex.Item push>
                        <Button
                            content={this.localize("deleteButtonContent")}
                            disabled={this.state.isSubmitting}
                            onClick={this.onDeleteButtonClick} />
                    </Flex.Item>
                    <Button
                        className="athena-button"
                        content={this.itemIdToEditOrDelete ? this.localize("updateBtnText") : this.localize("submitButtonContent")}
                        disabled={this.state.isSubmitting}
                        loading={this.state.isSubmitting}
                        onClick={this.onSubmit} />
                </Flex>
            </Flex.Item>;
        }

        return <Flex.Item push>
            <Flex>
                <Flex.Item push>
                    <Button
                        className="athena-button"
                        content={this.itemIdToEditOrDelete ? this.localize("updateBtnText") : this.localize("submitButtonContent")}
                        disabled={this.state.isSubmitting}
                        loading={this.state.isSubmitting}
                        onClick={this.onSubmit} />
                </Flex.Item>
            </Flex>
        </Flex.Item>;
    }

    // Renders component.
    render() {
        if (this.state.isLoadingDetails) {
            return <ContentLoader className="task-module-container" />;
        }

        return (
            <Flex className="task-module-container new-home-configuration-article" column fill gap="gap.medium">
                <StatusBar status={this.state.status} isMobile={false} />
                { this.state.isDeleteArticleConfirmationScreenActivated ? this.getDeleteArticleConfirmationScreen() : this.getFillAndUpdateDetailsScreen() }
                { this.renderActionButtons() }
            </Flex>
        );
    }
}

export default withTranslation()(withRouter(NewHomeConfigurationArticle));