// <copyright file="home-configuration.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { Segment, Button, Flex, AddIcon, Table, Checkbox, EditIcon, TrashCanIcon, Dialog, Input, SearchIcon, Skeleton, Menu, MenuItemProps, Text, FormInput } from "@fluentui/react-northstar";
import { withTranslation, WithTranslation } from "react-i18next";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { TFunction } from "i18next";
import StatusBar from "../common/status-bar/status-bar";
import AthenaSplash from "../athena-splash/athena-splash";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import IHomeConfigurationArticle from "../../models/home-configuration-article";
import { cloneDeep } from "lodash";
import Constants from "../../constants/constants";
import NoContent from "../../components/common/no-content/no-content";
import { createHomeStatusBarConfigurationAsync, deleteHomeConfigurationArticlesAsync, getHomeConfigurationArticlesAsync, getHomeStatusBarConfigurationAsync, updateHomeStatusBarConfigurationAsync } from "../../api/home-configurations-api";
import { StatusCodes } from "http-status-codes";
import IHomeStatusBarConfiguration from "../../models/home-status-bar-configuration";
import { IsValidUrl } from "../../helpers/url-helper";
import ContentLoader from "../common/loader/loader";
import showdown from "showdown";

import "./home-configuration.scss";

interface IHomeConfigurationState {
    status: IStatusBar;
    articles: IHomeConfigurationArticle[];
    isEditButtonEnabled: boolean;
    isDeleteButtonEnabled: boolean;
    isHeaderCheckboxChecked: boolean;
    isLoadingArticles: boolean;
    filteredArticles: IHomeConfigurationArticle[];
    searchString: string;
    isUnauthorizedUser: boolean;
    isDeletingArticles: boolean;
    menuAciveIndex: number;
    homeStatusBarConfiguration: IHomeStatusBarConfiguration;
    isSubmittingHomeStatusBarConfiguration: boolean;
    isLoadingHomeStatusBarConfiguration: boolean;
}

interface HomeConfigurationProps extends WithTranslation, RouteComponentProps {
}

const ConfigureNewToAthenaSectionMenuItemIndex: number = 0;
const ConfigureHomeStatusBarMenuItemIndex: number = 1;
const StatusBarMessageTextLength = 150;
const StatusBarLinkLabelLength = 50;
const MaxArticlesCanBeAddedForTeam = 5;

class HomeConfiguration extends React.Component<HomeConfigurationProps, IHomeConfigurationState> {
    readonly localize: TFunction;
    readonly menuItems: MenuItemProps[];
    readonly converter: any;

    teamId: string = "";

    constructor(props) {
        super(props);

        this.localize = this.props.t;
        this.converter = new showdown.Converter();

        this.menuItems = [
            { content: this.localize("homeConfigConfigureNewToAthenaSectionMenuItemLabel") },
            { content: this.localize("homeConfigConfigureStatusBarMenuItemLabel") }
        ]

        this.state = {
            status: { id: 0, message: "", type: ActivityStatus.None },
            articles: [],
            isEditButtonEnabled: false,
            isDeleteButtonEnabled: false,
            isHeaderCheckboxChecked: false,
            isLoadingArticles: true,
            filteredArticles: [],
            searchString: "",
            isUnauthorizedUser: false,
            isDeletingArticles: false,
            menuAciveIndex: ConfigureNewToAthenaSectionMenuItemIndex,
            homeStatusBarConfiguration: { message: "", linkLabel: "", url: "", isActive: false } as IHomeStatusBarConfiguration,
            isSubmittingHomeStatusBarConfiguration: false,
            isLoadingHomeStatusBarConfiguration: true
        }
    }

    componentDidMount() {
        microsoftTeams.initialize();
        microsoftTeams.getContext(async (context: microsoftTeams.Context) => {
            this.teamId = context.groupId!;
            this.getArticlesAsync();
            this.getHomeStatusBarConfigurationDetails();
        });
    }

    // Gets all articles configured for the Microsoft Teams's team.
    getArticlesAsync = async () => {
        this.setState({ isLoadingArticles: true });

        let response = await getHomeConfigurationArticlesAsync(this.teamId, this.handleTokenAccessFailure);

        if (response) {
            if (response.status === StatusCodes.OK) {
                this.setState({ articles: response.data, isLoadingArticles: false });
                return;
            }

            if (response.status === StatusCodes.UNAUTHORIZED || response.status === StatusCodes.FORBIDDEN) {
                this.setState({ isUnauthorizedUser: true, isLoadingArticles: false });
                return;
            }
        }

        this.setState((prevState: IHomeConfigurationState) => ({
            isLoadingArticles: false,
            status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
        }));
    }

    // Gets the home status bar configuration details.
    getHomeStatusBarConfigurationDetails = async () => {
        let response = await getHomeStatusBarConfigurationAsync(this.teamId, this.handleTokenAccessFailure);

        if (response) {
            if (response.status === StatusCodes.OK) {
                this.setState({ homeStatusBarConfiguration: response.data, isLoadingHomeStatusBarConfiguration: false });
                return;
            }

            if (response.status === StatusCodes.UNAUTHORIZED || response.status === StatusCodes.FORBIDDEN) {
                this.setState({ isUnauthorizedUser: true, isLoadingHomeStatusBarConfiguration: false });
                return;
            }
        }

        this.setState({ isLoadingHomeStatusBarConfiguration: false });
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    handleTokenAccessFailure = (error: string) => {
        this.props.history.push("/signin");
    }

    // Opens a task module to edit an article.
    editArticle = (articleId: string) => {
        microsoftTeams.tasks.startTask({
            title: this.localize("homeConfigEditArticleTaskModuleTitle"),
            width: 600,
            height: 600,
            url: `${window.location.origin}/new-home-configuration-article?${Constants.UrlParamHomeConfigArticleId}=${articleId}`
        }, (error: string, result: any) => {
            if (result?.type === "update") {
                this.setState((prevState: IHomeConfigurationState) => ({
                    isHeaderCheckboxChecked: false,
                    isEditButtonEnabled: false,
                    isDeleteButtonEnabled: false,
                    searchString: "",
                    status: { id: prevState.status.id + 1, message: this.localize("homeConfigArticleUpdatedMessage"), type: ActivityStatus.Success }
                }));

                this.getArticlesAsync();
                return;
            }

            if (result?.type === "delete") {
                this.setState((prevState: IHomeConfigurationState) => ({
                    isHeaderCheckboxChecked: false,
                    isEditButtonEnabled: false,
                    isDeleteButtonEnabled: false,
                    searchString: "",
                    status: { id: prevState.status.id + 1, message: this.localize("homeConfigArticlesDeletedMessage"), type: ActivityStatus.Success }
                }));

                this.getArticlesAsync();
            }
        });
    }

    // Validates status bar configuration fields.
    validateStatusBarConfigurationFields = (): boolean => {
        if (!this.state.homeStatusBarConfiguration.message?.trim()) {
            this.setState((prevState: IHomeConfigurationState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("homeStatusBarConfigMessageRequiredMessage"), type: ActivityStatus.Error }
            }));

            return false;
        }

        if (this.state.homeStatusBarConfiguration.url?.trim().length > 0 && !IsValidUrl(this.state.homeStatusBarConfiguration.url.trim())) {
            this.setState((prevState: IHomeConfigurationState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("homeStatusBarConfigUrlRequiredMessage"), type: ActivityStatus.Error }
            }));

            return false;
        }

        return true;
    }

    // Event handler called when click on new item button.
    onNewItemButtonClick = () => {
        if (this.state.articles.length >= MaxArticlesCanBeAddedForTeam) {
            this.setState((prevState: IHomeConfigurationState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("homeConfigMaxArticlesCanBeAddedMessage", { MaxArticlesCanBeAddedForTeam }), type: ActivityStatus.Error }
            }));

            return;
        }

        microsoftTeams.tasks.startTask({
            title: this.localize("fillDetailsText"),
            width: 600,
            height: 600,
            url: `${window.location.origin}/new-home-configuration-article`
        }, (error: string, result: any) => {
            if (result && result.status === ActivityStatus.Success) {
                this.setState((prevState: IHomeConfigurationState) => ({
                    status: { id: prevState.status.id + 1, message: this.localize("homeConfigArticleCreatedMessage"), type: ActivityStatus.Success }
                }));

                this.getArticlesAsync();
            }
        });
    }

    /**
     * Event handler called when click on table row.
     * @param articleId The Id of article on which clicked.
     */
    onArticleClick = (articleId: string) => {
        this.editArticle(articleId);
    }

    // Event handler called when edit button clicked.
    onEditButtonClick = () => {
        let articleToEdit = this.state.articles
            .find((article: IHomeConfigurationArticle) => article.isChecked === true);

        if (articleToEdit?.articleId) {
            this.editArticle(articleToEdit.articleId);
        }
    }

    /**
     * Event handler called when header checkbox checked change.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onHeaderCheckboxCheckedChange = (eventDetails: any, eventData: any) => {
        let articles: IHomeConfigurationArticle[] = cloneDeep(this.state.articles);
        let filteredArticles: IHomeConfigurationArticle[] = cloneDeep(this.state.filteredArticles);

        let updatedArticles: IHomeConfigurationArticle[] = articles;
        let updatedFilteredArticles: IHomeConfigurationArticle[] = [];

        if (this.state.searchString?.trim()) {
            updatedFilteredArticles = filteredArticles.map((article: IHomeConfigurationArticle) => {
                return {
                    ...article,
                    isChecked: eventData.checked
                }
            });

            if (updatedFilteredArticles.length) {
                for (let i = 0; i < updatedArticles.length; i++) {
                    let hasArticle: boolean = updatedFilteredArticles.some((article: IHomeConfigurationArticle) => updatedArticles[i].articleId === article.articleId);

                    if (hasArticle) {
                        updatedArticles[i].isChecked = eventData.checked;
                    }
                }
            }
        }
        else {
            updatedArticles = articles.map((article: IHomeConfigurationArticle) => {
                return {
                    ...article,
                    isChecked: eventData.checked
                }
            });
        }

        this.setState({
            articles: updatedArticles,
            filteredArticles: updatedFilteredArticles,
            isDeleteButtonEnabled: eventData.checked,
            isEditButtonEnabled: this.state.searchString?.trim() ? updatedFilteredArticles.length === 1 && eventData.checked : updatedArticles.length === 1 && eventData.checked,
            isHeaderCheckboxChecked: eventData.checked
        });
    }

    /**
     * Event handler called when row checkbox checked change.
     * @param eventData The event data.
     * @param articleId The Id of article which checked or unchecked.
     */
    onRowCheckboxCheckedChange = (eventData: any, articleId: string) => {
        let articles: IHomeConfigurationArticle[] = cloneDeep(this.state.articles);
        let filteredArticles: IHomeConfigurationArticle[] = cloneDeep(this.state.filteredArticles);

        let articleToUpdate = articles.find((article: IHomeConfigurationArticle) => article.articleId === articleId);
        let filteredArticleToUpdate = filteredArticles.find((article: IHomeConfigurationArticle) => article.articleId === articleId);

        if (articleToUpdate) {
            articleToUpdate.isChecked = eventData.checked;
        }

        if (filteredArticleToUpdate) {
            filteredArticleToUpdate.isChecked = eventData.checked;
        }

        let selectedArticlesCount: number = -1;

        if (this.state.searchString?.trim()) {
            selectedArticlesCount = filteredArticles.filter((article: IHomeConfigurationArticle) => article.isChecked === true).length;
        }
        else {
            selectedArticlesCount = articles.filter((article: IHomeConfigurationArticle) => article.isChecked === true).length;
        }

        this.setState({
            articles,
            filteredArticles,
            isEditButtonEnabled: selectedArticlesCount === 1,
            isDeleteButtonEnabled: selectedArticlesCount > 0,
            isHeaderCheckboxChecked: this.state.searchString.trim() ? selectedArticlesCount === filteredArticles.length : selectedArticlesCount === articles.length
        });
    }

    // Event handler called when delete articles action is confirmed.
    onConfirmDeleteArticlesAsync = async () => {
        this.setState({ isDeletingArticles: true });

        let articleIdsToDelete: string[] = this.state.articles
            .filter((article: IHomeConfigurationArticle) => article.isChecked === true)
            .map((article: IHomeConfigurationArticle) => article.articleId);

        var response = await deleteHomeConfigurationArticlesAsync(this.teamId, articleIdsToDelete, this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            this.setState((prevState: IHomeConfigurationState) => ({
                isDeletingArticles: false,
                isHeaderCheckboxChecked: false,
                isEditButtonEnabled: false,
                isDeleteButtonEnabled: false,
                searchString: "",
                status: { id: prevState.status.id + 1, message: this.localize("homeConfigArticlesDeletedMessage"), type: ActivityStatus.Success }
            }));

            this.getArticlesAsync();
        }
        else {
            this.setState((prevState: IHomeConfigurationState) => ({
                isDeletingArticles: false,
                status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
            }));
        }
    }

    /**
     * Event handler called when search string get changed.
     * @param searchString The search string.
     */
    onSearchStringChange = (searchString: string) => {
        let articles: IHomeConfigurationArticle[] = cloneDeep(this.state.articles);

        let filteredArticles = articles.filter((article: IHomeConfigurationArticle) => article.title.toLowerCase().includes(searchString.toLowerCase())
            || article.description.toLowerCase().includes(searchString.toLowerCase()));

        let selectedArticlesCount: number = -1;

        if (searchString.trim()) {
            if (filteredArticles.length) {
                selectedArticlesCount = filteredArticles.filter((article: IHomeConfigurationArticle) => article.isChecked).length;
            }
        }
        else {
            selectedArticlesCount = articles.filter((article: IHomeConfigurationArticle) => article.isChecked).length;
        }

        this.setState({
            filteredArticles,
            searchString,
            isEditButtonEnabled: selectedArticlesCount === 1,
            isDeleteButtonEnabled: selectedArticlesCount > 0,
            isHeaderCheckboxChecked: searchString.trim() ? selectedArticlesCount === filteredArticles.length : selectedArticlesCount === articles.length
        });
    }

    /**
     * The event handler called when active menu index get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onMenuActiveIndexChange = (eventDetails: any, eventData: any) => {
        this.setState({ menuAciveIndex: eventData.activeIndex })
    }

    /**
     * Event handler called when message get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onMessageChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        this.setState((prevState: IHomeConfigurationState) => ({
            homeStatusBarConfiguration: { ...prevState.homeStatusBarConfiguration, message: eventData.value }
        }));
    }

    /**
     * Event handler called when link label get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onLinkLabelChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        this.setState((prevState: IHomeConfigurationState) => ({
            homeStatusBarConfiguration: { ...prevState.homeStatusBarConfiguration, linkLabel: eventData.value }
        }));
    }

    /**
     * Event handler called when URL get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onUrlChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        this.setState((prevState: IHomeConfigurationState) => ({
            homeStatusBarConfiguration: { ...prevState.homeStatusBarConfiguration, url: eventData.value }
        }));
    }

    /**
     * Event handler called when 'Active' checkbox value gets changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onHomeStatusBarActiveChange = (eventDetails: any, eventData: any) => {
        this.setState((prevState: IHomeConfigurationState) => ({
            homeStatusBarConfiguration: { ...prevState.homeStatusBarConfiguration, isActive: eventData.checked }
        }));
    }

    // Event handler called when click on submit status bar configuration button.
    onSubmitStatusBarConfiguration = async () => {
        let isAllFieldsValid: boolean = this.validateStatusBarConfigurationFields();

        if (!isAllFieldsValid) {
            return;
        }

        this.setState({ isSubmittingHomeStatusBarConfiguration: true });

        if (this.state.homeStatusBarConfiguration.teamId) {
            let response = await updateHomeStatusBarConfigurationAsync(this.teamId, this.state.homeStatusBarConfiguration, this.handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                this.setState((prevState: IHomeConfigurationState) => ({
                    isSubmittingHomeStatusBarConfiguration: false,
                    status: { id: prevState.status.id + 1, message: this.localize("homeStatusBarConfigUpdatedMessage"), type: ActivityStatus.Success }
                }));

                return;
            }
        }
        else {
            let response = await createHomeStatusBarConfigurationAsync(this.teamId, this.state.homeStatusBarConfiguration, this.handleTokenAccessFailure);

            if (response && response.status === StatusCodes.CREATED) {
                this.setState((prevState: IHomeConfigurationState) => ({
                    isSubmittingHomeStatusBarConfiguration: false,
                    status: { id: prevState.status.id + 1, message: this.localize("homeStatusBarConfigCreatedMessage"), type: ActivityStatus.Success }
                }));

                return;
            }
        }

        this.setState((prevState: IHomeConfigurationState) => ({
            isSubmittingHomeStatusBarConfiguration: false,
            status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
        }));
    }

    // Event handler called when click on reset home status bar configuration button.
    onResetButtonClick = () => {
        this.setState((prevState: IHomeConfigurationState) => ({
            homeStatusBarConfiguration: { ...prevState.homeStatusBarConfiguration, message: "", linkLabel: "", url: "", isActive: false }
        }));
    }

    // Returns table header.
    getTableHeader = () => {
        return <Table.Row design={{ minHeight: "4.6rem" }} className="header" header>
            <Table.Cell design={{ minWidth: "4rem" }} content={<Checkbox disabled={!this.state.articles.length || this.state.isLoadingArticles || this.state.isDeletingArticles || (this.state.searchString?.trim().length > 0 && !this.state.filteredArticles.length)} checked={this.state.isHeaderCheckboxChecked} onChange={this.onHeaderCheckboxCheckedChange} />} />
            <Table.Cell design={{ minWidth: "32%" }} content={this.localize("myRequestsTitleColumn")} title={this.localize("myRequestsTitleColumn")} truncateContent={true} />
            <Table.Cell design={{ minWidth: "34%" }} content={this.localize("descriptionText")} title={this.localize("descriptionText")} truncateContent={true} />
            <Table.Cell design={{ minWidth: "30%" }} content={this.localize("homeConfigTableImageLinkColumnName")} title={this.localize("homeConfigTableImageLinkColumnName")} truncateContent={true} />
        </Table.Row>;
    }

    // Prepares and return tables rows array.
    getTableRows = () => {
        if (!this.state.articles.length) {
            return <Table.Row design={{ minHeight: "4.6rem" }} className="row">
                <Table.Cell content={this.localize("homeConfigArticlesNotAvailable")} />
            </Table.Row>;
        }

        if (this.state.searchString?.trim()) {
            if (!this.state.filteredArticles.length) {
                return <Table.Row design={{ minHeight: "4.6rem" }} className="row">
                    <Table.Cell content={this.localize("homeConfigSearchedArticlesNotAvailable", { searchString: this.state.searchString.trim() })} />
                </Table.Row>;
            }

            return this.state.filteredArticles.map((article: IHomeConfigurationArticle) => {
                let description = article.description ? this.converter.makeMarkdown(article.description) : "";

                return <Table.Row design={{ minHeight: "4.6rem" }} className="row" onClick={() => this.onArticleClick(article.articleId)}>
                    <Table.Cell design={{ minWidth: "4rem" }} content={<Checkbox checked={article.isChecked} onChange={(eventDetails: any, eventData: any) => this.onRowCheckboxCheckedChange(eventData, article.articleId)} />} onClick={(eventDetails: any) => eventDetails.stopPropagation()} />
                    <Table.Cell design={{ minWidth: "32%" }} content={article.title} title={article.title} truncateContent={true} />
                    <Table.Cell design={{ minWidth: "34%" }} content={description} title={description} truncateContent={true} />
                    <Table.Cell design={{ minWidth: "30%" }} content={article.imageUrl} title={article.imageUrl} truncateContent={true} />
                </Table.Row>;
            });
        }

        return this.state.articles.map((article: IHomeConfigurationArticle) => {
            let description = article.description ? this.converter.makeMarkdown(article.description) : "";

            return <Table.Row design={{ minHeight: "4.6rem" }} className="row" onClick={() => this.onArticleClick(article.articleId)}>
                <Table.Cell design={{ minWidth: "4rem" }} content={<Checkbox checked={article.isChecked} onChange={(eventDetails: any, eventData: any) => this.onRowCheckboxCheckedChange(eventData, article.articleId)} />} onClick={(eventDetails: any) => eventDetails.stopPropagation()} />
                <Table.Cell design={{ minWidth: "32%" }} content={article.title} title={article.title} truncateContent={true} />
                <Table.Cell design={{ minWidth: "34%" }} content={description} title={description} truncateContent={true} />
                <Table.Cell design={{ minWidth: "30%" }} content={article.imageUrl} title={article.imageUrl} truncateContent={true} />
            </Table.Row>;
        });
    }

    // Renders table.
    renderTable = () => {
        if (this.state.isLoadingArticles) {
            let tableRowSkeleton = <Table.Row design={{ minHeight: "4.6rem" }} className="row">
                <Table.Cell design={{ minWidth: "4rem" }} content={<Skeleton.Line width="2rem" height="1.4rem" />} />
                <Table.Cell design={{ minWidth: "32%" }} content={<Skeleton.Line width="26rem" height="1.4rem" />} />
                <Table.Cell design={{ minWidth: "34%" }} content={<Skeleton.Line width="30rem" height="1.4rem" />} />
                <Table.Cell design={{ minWidth: "30%" }} content={<Skeleton.Line width="25rem" height="1.4rem" />} />
            </Table.Row>;

            return <Skeleton animation="wave">
                <Table className="configurations-table">
                    {this.getTableHeader()}
                    {tableRowSkeleton}
                    {tableRowSkeleton}
                    {tableRowSkeleton}
                    {tableRowSkeleton}
                    {tableRowSkeleton}
                </Table>
            </Skeleton>;
        }

        return <Table className="configurations-table">
            {this.getTableHeader()}
            {this.getTableRows()}
        </Table>;
    }

    // Renders the status bar preview.
    getStatusBarPreview = () => {
        return <Flex vAlign="center" hAlign="center" gap="gap.small">
            <Text content={this.state.homeStatusBarConfiguration.message} weight="semibold" title={this.state.homeStatusBarConfiguration.message} truncated />
            <a href={this.state.homeStatusBarConfiguration.url} target="_blank">{this.state.homeStatusBarConfiguration.linkLabel}</a>
        </Flex>;
    }

    // Gets the home status bar configuration screen.
    getHomeStatusBarConfigurationScreen = () => {
        if (this.state.isLoadingHomeStatusBarConfiguration) {
            return <ContentLoader />;
        }

        return <Flex className="event-bar-configuration" column gap="gap.medium">
            <Text content={this.localize("previewHeadText")} weight="semibold" />
            <Segment design={{ minHeight: "5rem" }} color="brand" content={this.getStatusBarPreview()} />
            <Flex className="home-event-bar-form" hAlign="center">
                <Flex className="task-module-container" column>
                    <Flex className="overflow-y" column gap="gap.medium">
                        <FormInput
                            label={this.localize("homeStatusBarConfigMessageLabel")}
                            placeholder={this.localize("homeStatusBarConfigMessagePlaceholder")}
                            maxLength={StatusBarMessageTextLength}
                            onChange={this.onMessageChange}
                            value={this.state.homeStatusBarConfiguration.message}
                            showSuccessIndicator={false}
                            disabled={this.state.isSubmittingHomeStatusBarConfiguration}
                            required
                            fluid />
                        <FormInput
                            label={this.localize("homeStatusBarConfigLinkLabel")}
                            placeholder={this.localize("homeStatusBarConfigLinkLabelPlaceholder")}
                            maxLength={StatusBarLinkLabelLength}
                            onChange={this.onLinkLabelChange}
                            value={this.state.homeStatusBarConfiguration.linkLabel}
                            showSuccessIndicator={false}
                            disabled={this.state.isSubmittingHomeStatusBarConfiguration}
                            fluid />
                        <FormInput
                            label={this.localize("homeStatusBarConfigUrlLabel")}
                            placeholder={this.localize("homeStatusBarConfigUrlPlaceholder")}
                            onChange={this.onUrlChange}
                            value={this.state.homeStatusBarConfiguration.url}
                            showSuccessIndicator={false}
                            disabled={this.state.isSubmittingHomeStatusBarConfiguration}
                            fluid />
                        <Checkbox disabled={this.state.isSubmittingHomeStatusBarConfiguration} checked={this.state.homeStatusBarConfiguration.isActive} label={this.localize("homeStatusBarConfigActiveLabel")} onChange={this.onHomeStatusBarActiveChange} />
                    </Flex>
                    <Flex.Item push>
                        <Flex gap="gap.small">
                            <Flex.Item push>
                                <Button content={this.localize("resetButtonContent")} disabled={this.state.isSubmittingHomeStatusBarConfiguration} onClick={this.onResetButtonClick} />
                            </Flex.Item>
                            <Button className="athena-button" content={this.localize("submitButtonContent")} disabled={this.state.isSubmittingHomeStatusBarConfiguration} loading={this.state.isSubmittingHomeStatusBarConfiguration} onClick={this.onSubmitStatusBarConfiguration} />
                        </Flex>
                    </Flex.Item>
                </Flex>
            </Flex>
        </Flex>;
    }

    // Renders component.
    render() {
        if (this.state.isUnauthorizedUser) {
            return <NoContent message={this.localize("homeConfigUnauthorizedMessage")} />;
        }

        return (
            <Flex className="home-configuaration" design={{height: "99vh"}} column fill>
                <StatusBar status={this.state.status} isMobile={false} />
                <AthenaSplash description={this.localize("homeConfigurationAthenaSplashDescription")} heading={this.localize("homeConfigurationAthenaSplashHeading")} />
                <Menu
                    design={{minHeight: "4rem", marginTop: "1rem"}}
                    defaultActiveIndex={ConfigureNewToAthenaSectionMenuItemIndex}
                    activeIndex={this.state.menuAciveIndex}
                    items={this.menuItems}
                    onActiveIndexChange={this.onMenuActiveIndexChange}
                />
                {
                    this.state.menuAciveIndex === ConfigureNewToAthenaSectionMenuItemIndex && <>
                        <Segment
                            className="menu-box"
                            content={<Flex vAlign="center">
                                <Button text icon={<AddIcon />} content={this.localize("homeConfigurationNewItemButtonContent")} onClick={this.onNewItemButtonClick} />
                                <Button text icon={<EditIcon />} disabled={!this.state.isEditButtonEnabled || this.state.isDeletingArticles} content={this.localize("editButtonContent")} onClick={this.onEditButtonClick} />
                                <Dialog
                                    cancelButton={this.localize("noLabel")}
                                    confirmButton={this.localize("yesLabel")}
                                    content={this.localize("newHomeConfigItemDeleteConfirmationDialogContent")}
                                    header={this.localize("newHomeConfigItemDeleteConfirmationDialogTitle")}
                                    trigger={<Button text icon={<TrashCanIcon />} loading={this.state.isDeletingArticles} disabled={!this.state.isDeleteButtonEnabled || this.state.isDeletingArticles} content={this.localize("deleteButtonContent")} />}
                                    onConfirm={this.onConfirmDeleteArticlesAsync}
                                />
                                <Flex.Item push>
                                    <Input className="search-box" inverted icon={<SearchIcon />} value={this.state.searchString} placeholder={this.localize("myRequestsFindInputPlaceholder")} disabled={this.state.isLoadingArticles} onChange={(eventDetails: any, eventData: any) => this.onSearchStringChange(eventData.value)} />
                                </Flex.Item>
                            </Flex>
                            }
                        />
                        {this.renderTable()}
                    </>
                }
                {
                    this.state.menuAciveIndex === ConfigureHomeStatusBarMenuItemIndex && this.getHomeStatusBarConfigurationScreen()
                }
            </Flex>
        );
    }
}

export default withTranslation()(withRouter(HomeConfiguration));