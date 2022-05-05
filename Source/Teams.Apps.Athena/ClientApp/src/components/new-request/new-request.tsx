// <copyright file="new-request.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import moment from "moment";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { Button, Checkbox, Flex, FormDropdown, FormInput, FormRadioGroup, Text, TextArea } from "@fluentui/react-northstar";
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import StatusBar from "../common/status-bar/status-bar";
import { IsValidUrl } from "../../helpers/url-helper";
import Constants from "../../constants/constants";
import Loader from "../common/loader/loader";
import RequestType from "../../models/request-type";
import COIType from "../../models/coi-type";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import RequestStatus from "../../models/request-status";
import INews from "../../models/news"
import ICoi from "../../models/coi";
import { getLocalizedCOIType, getLocalizedRequestStatus } from "../../helpers/localization-helper";
import { StatusCodes } from "http-status-codes";
import { createNewsArticleRequestAsync, deleteRequestsAsync, getNewsArticleRequestAsync, saveRequestAsDraftAsync, submitDraftNewsArticleRequestAsync, updateDraftNewsArticleRequestAsync } from "../../api/news-requests-api";
import NoContent from "../common/no-content/no-content";
import { createCoiRequestAsync, getCoiRequestAsync, submitDraftCoiRequestAsync, updateDraftCoiRequestAsync, saveCoiRequestAsDraftAsync, deleteCoiRequestsAsync } from "../../api/coi-requests-api";
import IKeyword from "../../models/keyword";
import KeywordSearchDropdown from "../common/keyword-search-dropdown/keyword-search-dropdown";
import { getAllKeywordsAsync } from "../../api/keyword-api";

import "./new-request.scss";

interface INewRequestState {
    currentPage: number;
    status: IStatusBar;
    isLoadingRequestDetails: boolean;
    request: IRequest;
    selectedCoiType: any;
    isSavingRequestAsDraft: boolean;
    isSubmittingOrUpdatingOrDeletingRequest: boolean;
    selectedKeywords: IKeyword[];
    keywordsData: IKeyword[];
}

interface INewRequestProps extends WithTranslation, RouteComponentProps {
}

interface IRequest {
    id: string;
    title: string;
    description: string;
    coiType: COIType;
    requestType: RequestType;
    keywords: IKeyword[];
    status: number;
    imageUrl: string;
    isImportant: boolean;
    externalLink: string;
    createdAt: Date;
    sumOfRatings: number;
    numberOfRatings: number;
}

enum PageType {
    ChooseRequestType,
    FillRequestDetails,
    DeleteRequestConfirmation,
    RequestDetailsPage
}

const RequestTitleMaxLength: number = 100;
const RequestDescriptionMaxLength: number = 300;
const RequestImageUrlMaxLength: number = 300;
const RequestRedirectionUrlMaxLength: number = 300;
const MaxNewsArticleRating: number = 5;

class NewRequest extends React.Component<INewRequestProps, INewRequestState> {
    readonly localize: TFunction;
    readonly requestTypes: any[] = [];
    readonly coiTypes: any[] = [];
    readonly requestIdToEditOrDelete: string | null;
    readonly requestTypeToEditOrDelete: string | null;

    isSomethingWentWrong: boolean = false;
    isRequestNotFound: boolean = false;

    constructor(props) {
        super(props);

        this.localize = this.props.t;

        this.requestTypes.push({
            key: "news-request-radiogroup-item",
            label: this.localize("requestTypeNews"),
            value: RequestType.News
        },
        {
            key: "coi-request-radiogroup-item",
            label: this.localize("requestTypeCOI"),
            value: RequestType.CommunityOfInterest
        });

        this.coiTypes.push({
            key: "coi-type-public",
            value: COIType.Public,
            header: this.localize("coiTypePublic")
        },
        {
            key: "coi-type-private",
            value: COIType.Private,
            header: this.localize("coiTypePrivate")
        });

        let urlParams = new URLSearchParams(window.location.search);
        this.requestIdToEditOrDelete = urlParams.get(Constants.UrlParamRequestIdToEditOrDeleteRequest);
        this.requestTypeToEditOrDelete = urlParams.get(Constants.UrlParamRequestType);

        let isReadOnlyRequest: string | null = urlParams.get(Constants.UrlParamIsReadonlyToEditOrDeleteRequest);

        this.state = {
            currentPage: this.requestIdToEditOrDelete ? isReadOnlyRequest ? PageType.RequestDetailsPage : PageType.FillRequestDetails : PageType.ChooseRequestType,
            status: { id: 0, message: "", type: ActivityStatus.None },
            isLoadingRequestDetails: this.requestIdToEditOrDelete ? true : false,
            request: { title: "", description: "", externalLink: "", imageUrl: "", requestType: RequestType.News } as IRequest,
            selectedCoiType: undefined,
            isSavingRequestAsDraft: false,
            isSubmittingOrUpdatingOrDeletingRequest: false,
            selectedKeywords: [],
            keywordsData: []
        }
    }

    // React component life cycle method called when component get mounted. 
    async componentDidMount() {
        microsoftTeams.initialize();
        await this.fetchAllKeywords();

        if (this.requestIdToEditOrDelete) {
            if (this.requestTypeToEditOrDelete === RequestType.News.toString()) {
                await this.getNewsArticleRequestAsync();
            }
            else if (this.requestTypeToEditOrDelete === RequestType.CommunityOfInterest.toString()) {
                await this.getCoiRequestAsync();
            }
        }
    }

    // Fetch all keywords.
    fetchAllKeywords = async () => {
        let response = await getAllKeywordsAsync(this.handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            let keywordsData = response.data as IKeyword[];
            this.setState({ keywordsData })
        }
    }

    // Gets a news article request details.
    getNewsArticleRequestAsync = async () => {
        let response = await await getNewsArticleRequestAsync(this.requestIdToEditOrDelete!, this.handleTokenAccessFailure);

        if (response) {
            if (response.status === StatusCodes.OK) {
                let newsArticleRequest: INews = response.data as INews;

                let keywordIdsStringArray = newsArticleRequest.keywords.map(String);
                let newsKeywords = this.state.keywordsData.filter((keyword: IKeyword) => keywordIdsStringArray.includes(keyword.keywordId));

                this.setState({
                    request: {
                        id: this.requestIdToEditOrDelete,
                        title: newsArticleRequest.title,
                        description: newsArticleRequest.body,
                        externalLink: newsArticleRequest.externalLink,
                        imageUrl: newsArticleRequest.imageUrl,
                        keywords: newsKeywords,
                        status: newsArticleRequest.status,
                        isImportant: newsArticleRequest.isImportant,
                        requestType: RequestType.News,
                        sumOfRatings: newsArticleRequest.sumOfRatings,
                        numberOfRatings: newsArticleRequest.numberOfRatings,
                        createdAt: newsArticleRequest.createdAt
                    } as IRequest,
                    selectedKeywords: newsKeywords,
                    isLoadingRequestDetails: false
                });

                return;
            }
            else if (response.status === StatusCodes.NOT_FOUND) {
                this.isRequestNotFound = true;

                this.setState({
                    isLoadingRequestDetails: false
                });

                return;
            }
        }

        this.isSomethingWentWrong = true;

        this.setState((prevState: INewRequestState) => ({
            isLoadingRequestDetails: false
        }));
    }

    // Gets a COI request.
    getCoiRequestAsync = async () => {
        let response = await getCoiRequestAsync(this.requestIdToEditOrDelete!, this.handleTokenAccessFailure);

        if (response) {
            if (response.status === StatusCodes.OK) {
                let coiRequest: ICoi = response.data as ICoi;

                let keywordIdsStringArray = coiRequest.keywords.map(String);
                let coiKeywords = this.state.keywordsData.filter((keyword: IKeyword) => keywordIdsStringArray.includes(keyword.keywordId));

                this.setState({
                    request: {
                        id: coiRequest.tableId,
                        title: coiRequest.coiName,
                        description: coiRequest.coiDescription,
                        keywords: coiKeywords,
                        coiType: coiRequest.type === COIType.None ? undefined : coiRequest.type,
                        status: coiRequest.status,
                        requestType: RequestType.CommunityOfInterest,
                        createdAt: coiRequest.createdOn
                    } as IRequest,
                    selectedCoiType: this.coiTypes.find((coiType: any) => coiType.value === coiRequest.type),
                    selectedKeywords: coiKeywords,
                    isLoadingRequestDetails: false
                });

                return;
            }
            else if (response.status === StatusCodes.NOT_FOUND) {
                this.isRequestNotFound = true;
                this.setState({ isLoadingRequestDetails: false });

                return;
            }
        }

        this.isSomethingWentWrong = true;
        this.setState({ isLoadingRequestDetails: false });
    }

    /**
     * Validates fields based on request type and sets the status bar message if any field is invalid.
     * Returns true if all fields are valid. Else returns false.
     * @param isDraft Indicates whether validation to be done on the request which to be submitted as draft.
     */
    validateFields = (isDraft: boolean = false): boolean => {
        if (!this.state.request.title.trim()) {
            this.setState((prevState: INewRequestState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("requestTitleRequiredError"), type: ActivityStatus.Error }
            }));

            return false;
        }

        if (isDraft) {
            if (this.state.request.requestType === RequestType.News) {
                if (this.state.request.externalLink?.trim() && !IsValidUrl(this.state.request.externalLink.trim())) {
                    this.setState((prevState: INewRequestState) => ({
                        status: { id: prevState.status.id + 1, message: this.localize("requestRedirectionUrlRequiredError"), type: ActivityStatus.Error }
                    }));

                    return false;
                }

                if (this.state.request.imageUrl?.trim() && !IsValidUrl(this.state.request.imageUrl.trim())) {
                    this.setState((prevState: INewRequestState) => ({
                        status: { id: prevState.status.id + 1, message: this.localize("requestImageUrlRequiredError"), type: ActivityStatus.Error }
                    }));

                    return false;
                }
            }

            return true;
        }

        if (!this.state.request.description.trim()) {
            this.setState((prevState: INewRequestState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("requestDescriptionRequiredError"), type: ActivityStatus.Error }
            }));

            return false;
        }

        if (!this.state.selectedKeywords?.length) {
            this.setState((prevState: INewRequestState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("createNewRequestKeywordsError"), type: ActivityStatus.Error }
            }));

            return false;
        }

        if (this.state.request.requestType === RequestType.News) {
            if (!this.state.request.externalLink.trim() || !IsValidUrl(this.state.request.externalLink.trim())) {
                this.setState((prevState: INewRequestState) => ({
                    status: { id: prevState.status.id + 1, message: this.localize("requestRedirectionUrlRequiredError"), type: ActivityStatus.Error }
                }));

                return false;
            }

            if (!this.state.request.imageUrl.trim() || !IsValidUrl(this.state.request.imageUrl.trim())) {
                this.setState((prevState: INewRequestState) => ({
                    status: { id: prevState.status.id + 1, message: this.localize("requestImageUrlRequiredError"), type: ActivityStatus.Error }
                }));

                return false;
            }

            return true;
        }

        if (!this.state.selectedCoiType) {
            this.setState((prevState: INewRequestState) => ({
                status: { id: prevState.status.id + 1, message: this.localize("requestCoiTypeRequiredError"), type: ActivityStatus.Error }
            }));

            return false;
        }

        return true;
    }

    /**
     * Validates all required fields and checks whether a request with title already exists. If not, then creates
     * a new request based on type.
     * @param isDraft Indicates whether request to be saved as draft.
     */
    createRequestAsync = async (isDraft: boolean = false) => {
        let isAllMandatoryFieldsFilled: boolean = this.validateFields(isDraft);

        if (!isAllMandatoryFieldsFilled) {
            this.setState({ isSavingRequestAsDraft: false, isSubmittingOrUpdatingOrDeletingRequest: false });
            return;
        }

        let apiResponse: any = undefined;
        let isUpdated: boolean = false;
        let isCreatedAsDraft: boolean = false;
        let isCreatedNewRequest: boolean = false;

        if (this.state.request.requestType === RequestType.News) {
            let newsArticleRequest: INews = {
                newsId: Constants.createdByAthena,
                title: this.state.request.title,
                body: this.state.request.description,
                externalLink: this.state.request.externalLink,
                imageUrl: this.state.request.imageUrl,
                keywordsJson: this.state.selectedKeywords,
                isImportant: this.state.request.isImportant
            } as INews;

            if (this.requestIdToEditOrDelete) {
                newsArticleRequest.tableId = this.state.request.id;

                if (isDraft) {
                    isUpdated = true;
                    apiResponse = await updateDraftNewsArticleRequestAsync(newsArticleRequest, this.handleTokenAccessFailure);
                }
                else {
                    isCreatedNewRequest = true;
                    apiResponse = await submitDraftNewsArticleRequestAsync(newsArticleRequest, this.handleTokenAccessFailure);
                }
            }
            else {
                if (isDraft) {
                    isCreatedAsDraft = true;
                    apiResponse = await saveRequestAsDraftAsync(newsArticleRequest, this.handleTokenAccessFailure);
                }
                else {
                    isCreatedNewRequest = true;
                    apiResponse = await createNewsArticleRequestAsync(newsArticleRequest, this.handleTokenAccessFailure);
                }
            }
        }
        else {
            let coiRequest: ICoi = {
                coiId: Constants.createdByAthena,
                coiName: this.state.request.title,
                coiDescription: this.state.request.description,
                type: this.state.selectedCoiType ? this.state.request.coiType : COIType.None,
                keywordsJson: this.state.selectedKeywords,
            } as ICoi;

            if (this.requestIdToEditOrDelete) {
                coiRequest.tableId = this.state.request.id;

                if (isDraft) {
                    isUpdated = true;
                    apiResponse = await updateDraftCoiRequestAsync(coiRequest, this.handleTokenAccessFailure);
                }
                else {
                    isCreatedNewRequest = true;
                    apiResponse = await submitDraftCoiRequestAsync(coiRequest, this.handleTokenAccessFailure);
                }
            }
            else {
                if (isDraft) {
                    isCreatedAsDraft = true;
                    apiResponse = await saveCoiRequestAsDraftAsync(coiRequest, this.handleTokenAccessFailure);
                }
                else {
                    isCreatedNewRequest = true;
                    apiResponse = await createCoiRequestAsync(coiRequest, this.handleTokenAccessFailure);
                }
            }
        }

        if (apiResponse) {
            if (apiResponse.status === StatusCodes.CREATED || apiResponse.status === StatusCodes.OK) {
                microsoftTeams.tasks.submitTask({
                    data: apiResponse.data,
                    type: this.state.request.requestType === RequestType.News ? RequestType.News : RequestType.CommunityOfInterest,
                    isUpdated,
                    isCreatedAsDraft,
                    isCreatedNewRequest
                });

                return;
            }

            if (apiResponse.status === StatusCodes.CONFLICT) {
                this.setState((prevState: INewRequestState) => ({
                    isSubmittingOrUpdatingOrDeletingRequest: false,
                    isSavingRequestAsDraft: false,
                    status: { id: prevState.status.id + 1, message: this.localize("requestWithTitleExists"), type: ActivityStatus.Error }
                }));

                return;
            }
        }

        this.setState((prevState: INewRequestState) => ({
            isSubmittingOrUpdatingOrDeletingRequest: false,
            isSavingRequestAsDraft: false,
            status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
        }));
    }

    // Gets the rating text for news article request with average rating and max rating.
    getNewsArticleRating = () => {
        if (this.state.request.numberOfRatings === 0) {
            return this.localize("noRatingsGivenYetLabel");
        }

        return this.localize("ratingsLabel", { averageRating: this.state.request.sumOfRatings / this.state.request.numberOfRatings, maxRating: MaxNewsArticleRating });
    }

    // Event handler called when click on 'Next' button to go to request details page.
    onNextButtonClick = () => {
        this.setState({ currentPage: PageType.FillRequestDetails });
    }

    // Event handler called when click on 'Back' button from request details page.
    onBackButtonClick = () => {
        this.setState({ currentPage: PageType.ChooseRequestType });
    }

    /**
     * Event handler called when request type get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onRequestTypeChange = (eventDetails: any, eventData: any) => {
        this.setState((prevState: INewRequestState) => ({
            request: { ...prevState.request, requestType: eventData.value }
        }));
    }

    // Event handler called when request get submitted.
    onSubmitRequest = () => {
        this.setState({ isSubmittingOrUpdatingOrDeletingRequest: true });
        this.createRequestAsync();
    }

    // Event handler called when request to be save as draft.
    onRequestSaveAsDraft = () => {
        this.setState({ isSavingRequestAsDraft: true });
        this.createRequestAsync(true);
    }

    // Event handler called when click on edit request button.
    onEditRequest = () => {
        this.setState({ currentPage: PageType.FillRequestDetails });
    }

    /**
     * Event handler called when request title get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onRequestTitleChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        this.setState((prevState: INewRequestState) => ({
            request: { ...prevState.request, title: eventData.value }
        }));
    }

    /**
     * Event handler called when request description get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onRequestDescriptionChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        this.setState((prevState: INewRequestState) => ({
            request: { ...prevState.request, description: eventData.value }
        }));
    }

    /**
     * Event handler called when news' redirection URL get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onRedirectionUrlChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        this.setState((prevState: INewRequestState) => ({
            request: { ...prevState.request, externalLink: eventData.value }
        }));
    }

    /**
     * Event handler called when news' image URL get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onImageUrlChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        this.setState((prevState: INewRequestState) => ({
            request: { ...prevState.request, imageUrl: eventData.value }
        }));
    }

    /**
     * Event handler called when news important check changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onNewsImportantCheckChange = (eventDetails: React.SyntheticEvent<HTMLElement>, eventData: any) => {
        this.setState((prevState: INewRequestState) => ({
            request: { ...prevState.request, isImportant: eventData.checked }
        }));
    }

    /**
     * Event handler called when COI type get changed.
     * @param eventDetails The event details.
     * @param eventData The event data.
     */
    onCoiTypeChange = (eventDetails: any, eventData: any) => {
        let selectedDropdownItem = this.coiTypes.filter((coiType: any) => coiType.value === eventData.value.value);

        this.setState((prevState: INewRequestState) => ({
            selectedCoiType: selectedDropdownItem[0],
            request: { ...prevState.request, coiType: eventData.value.value }
        }));
    }

    // Event handler called when request to be deleted.
    onDeleteRequest = () => {
        this.setState({ currentPage: PageType.DeleteRequestConfirmation });
    }

    // Event handler called when back button clicked to navigate to request details page.
    onBackToRequestDetailsButtonClick = () => {
        this.setState({ currentPage: PageType.RequestDetailsPage })
    }

    // Event handler called when request deletion is confirmed.
    onDeleteConfirmationClick = async () => {
        this.setState({ isSubmittingOrUpdatingOrDeletingRequest: true });

        var apiResponse: any = undefined;

        if (this.state.request.requestType === RequestType.News) {
            apiResponse = await deleteRequestsAsync([this.requestIdToEditOrDelete!], this.handleTokenAccessFailure);
        }
        else if (this.state.request.requestType === RequestType.CommunityOfInterest) {
            apiResponse = await deleteCoiRequestsAsync([this.requestIdToEditOrDelete!], this.handleTokenAccessFailure);
        }

        if (apiResponse && apiResponse.status === StatusCodes.OK) {
            this.setState({ isSubmittingOrUpdatingOrDeletingRequest: false });

            microsoftTeams.tasks.submitTask({ data: { id: this.requestIdToEditOrDelete }, type: this.state.request.requestType === RequestType.News ? RequestType.News : RequestType.CommunityOfInterest, isDeleted: true });
            return;
        }
        else {
            this.setState((prevState: INewRequestState) => ({
                isSubmittingOrUpdatingOrDeletingRequest: false,
                status: { id: prevState.status.id + 1, message: this.localize("somethingWentWrongMessage"), type: ActivityStatus.Error }
            }));
        }
    }

    // Event handler called when draft request is submitted.
    onSubmitDraftRequest = () => {
        let isAllMandatoryFieldsFilled: boolean = this.validateFields();

        if (!isAllMandatoryFieldsFilled) {
            return;
        }

        this.setState({ isSubmittingOrUpdatingOrDeletingRequest: true });

        this.createRequestAsync();
    }

    /**
     * Event handler called when keyword selection get changed.
     * @param selectedKeywords The array of selected keywords.
     */
    onChangeKeywords = (selectedKeywords: IKeyword[]) => {
        this.setState({
            selectedKeywords
        });
    }

    /**
     * Redirects to the sign-in page when accessing token is failed in API.
     * @param error The error message.
     */
    handleTokenAccessFailure = (error: string) => {
        this.props.history.push("/signin");
    }

    /**
     * Gets the element to be shown on request details page.
     * @param header The header of request field.
     * @param value The value.
     * @param isUrl Indicates whether a value is of URL kind.
     */
    getRequestDetailsElement = (header: string, value: string, isUrl: boolean = false): JSX.Element => {
        return (
            <Flex>
                <Text className="requestDetails-key-column-width" content={header} weight="semibold" truncated />
                <Flex.Item grow>
                    {isUrl ? <a className="word-wrap" href={value} target="_blank">{value}</a> : <Text content={value} />}
                </Flex.Item>
            </Flex>
        );
    }

    // Renders the page which provides option to select request type.
    requestTypeSelectionPage = () => {
        return (<React.Fragment>
            <Flex.Item grow>
                <FormRadioGroup
                    label={this.localize("newRequestTypeLabel")}
                    items={this.requestTypes}
                    checkedValue={this.state.request.requestType}
                    onCheckedValueChange={this.onRequestTypeChange}
                />
            </Flex.Item>
            <Flex.Item push>
                <Flex>
                    <Flex.Item push>
                        <Button className="athena-button" content={this.localize("nextButtonContent")} onClick={this.onNextButtonClick} />
                    </Flex.Item>
                </Flex>
            </Flex.Item>
        </React.Fragment>);
    }

    // Renders page where request details are filled.
    fillRequestDetailsPage = () => {
        return (<React.Fragment>
            <Flex.Item grow>
                <Flex className="overflow-y" column gap="gap.medium">
                    <FormInput
                        label={this.localize("newRequestTitleInputLabel")}
                        placeholder={this.state.request.requestType === RequestType.CommunityOfInterest ? this.localize("newRequestCOITitlePlaceholder") : this.localize("newRequestNewsTitleInputPlaceholder")}
                        maxLength={RequestTitleMaxLength}
                        onChange={this.onRequestTitleChange}
                        value={this.state.request.title}
                        showSuccessIndicator={false}
                        required
                        fluid />
                    <Flex column>
                        <Text content={`${this.localize("newRequestDescriptionInputLabel")}*`} />
                        <TextArea
                            design={{ height: "8.6rem", marginTop: ".4rem" }}
                            placeholder={this.localize("newRequestDescriptionPlaceholder")}
                            maxLength={RequestDescriptionMaxLength}
                            onChange={this.onRequestDescriptionChange}
                            value={this.state.request.description}
                            fluid />
                    </Flex>
                    <KeywordSearchDropdown
                        keywords={this.state.keywordsData}
                        showSlectedKywordPills={true}
                        label={`${this.localize('keywordsText')}*`}
                        getSelectedKywords={this.onChangeKeywords}
                        selectedKeywords={this.state.selectedKeywords}
                    />
                    {
                        this.state.request.requestType === RequestType.News &&
                        <React.Fragment>
                            <FormInput
                                label={this.localize("newRequestRedirectionUrlInputLabel")}
                                placeholder={this.localize("newRequestRedirectionUrlInputPlaceholder")}
                                maxLength={RequestRedirectionUrlMaxLength}
                                onChange={this.onRedirectionUrlChange}
                                value={this.state.request.externalLink}
                                showSuccessIndicator={false}
                                required
                            fluid />
                            <FormInput
                                label={this.localize("newRequestImageUrlInputLabel")}
                                placeholder={this.localize("newRequestImageUrlInputPlaceholder")}
                                maxLength={RequestImageUrlMaxLength}
                                onChange={this.onImageUrlChange}
                                value={this.state.request.imageUrl}
                                showSuccessIndicator={false}
                                required
                                fluid />
                        </React.Fragment>
                    }
                    {
                        this.state.request.requestType === RequestType.CommunityOfInterest &&
                        <FormDropdown
                            label={`${this.localize("newRequestCOITypeDropdownLabel")}*`}
                            placeholder={this.localize("newRequestCOITypePlaceholder")}
                            items={this.coiTypes}
                            onChange={this.onCoiTypeChange}
                            value={this.state.selectedCoiType}
                            className="form-dropdown"
                            fluid />
                    }
                    {
                        this.state.request.requestType === RequestType.News &&
                        <Checkbox label={this.localize("newRequestMarkAsImportantLabel")} checked={this.state.request.isImportant} onChange={this.onNewsImportantCheckChange} />
                    }
                </Flex>
            </Flex.Item>
            <Flex.Item push>
                <Flex gap="gap.small">
                    {
                        !this.requestIdToEditOrDelete &&
                        <>
                            <Flex.Item push>
                                <Button
                                    content={this.localize("backButtonContent")}
                                    disabled={this.state.isSavingRequestAsDraft || this.state.isSubmittingOrUpdatingOrDeletingRequest}
                                    onClick={this.onBackButtonClick}
                            />
                            </Flex.Item>
                            <Button
                                content={this.localize("saveAsDraftButtonContent")}
                                disabled={this.state.isSavingRequestAsDraft || this.state.isSubmittingOrUpdatingOrDeletingRequest}
                                loading={this.state.isSavingRequestAsDraft}
                                onClick={this.onRequestSaveAsDraft} />
                            <Button
                                className="athena-button"
                                content={this.localize("submitButtonContent")}
                                disabled={this.state.isSavingRequestAsDraft || this.state.isSubmittingOrUpdatingOrDeletingRequest}
                                loading={this.state.isSubmittingOrUpdatingOrDeletingRequest}
                                onClick={this.onSubmitRequest} />
                        </>
                    }
                    {
                        this.requestIdToEditOrDelete &&
                        <>
                            {
                                this.state.request.status === RequestStatus.Draft &&
                                <Flex.Item push>
                                    <Button
                                        content={this.localize("saveAsDraftButtonContent")}
                                        disabled={this.state.isSavingRequestAsDraft || this.state.isSubmittingOrUpdatingOrDeletingRequest}
                                        loading={this.state.isSavingRequestAsDraft}
                                        onClick={this.onRequestSaveAsDraft} />
                                </Flex.Item>
                            }
                            <Flex.Item push>
                                <Button
                                    className="athena-button"
                                    content={this.localize("submitButtonContent")}
                                    disabled={this.state.isSubmittingOrUpdatingOrDeletingRequest}
                                    loading={this.state.isSubmittingOrUpdatingOrDeletingRequest}
                                    onClick={this.onSubmitDraftRequest} />
                            </Flex.Item>
                        </>
                    }
                </Flex>
            </Flex.Item>
        </React.Fragment>);
    }

    // Renders the component which asks for confirmation to delete a request.
    deleteConfirmationPage = () => {
        return <Flex column fill>
            <Text content={this.localize("deleteRequestConfirmationMessage")} weight="semibold" />
            <Flex.Item push>
                <Flex gap="gap.small">
                    <Flex.Item push>
                        <Button
                            content={this.localize("backButtonContent")}
                            disabled={this.state.isSubmittingOrUpdatingOrDeletingRequest}
                            onClick={this.onBackToRequestDetailsButtonClick} />
                    </Flex.Item>
                    <Button
                        className="athena-button"
                        content={this.localize("okButtonContent")}
                        disabled={this.state.isSubmittingOrUpdatingOrDeletingRequest}
                        loading={this.state.isSubmittingOrUpdatingOrDeletingRequest}
                        onClick={this.onDeleteConfirmationClick} />
                </Flex>
            </Flex.Item>
        </Flex>
    }

    /**
     * Gets the comma separated keywords string.
     * @param keywords The keywords.
     */
    getKeywords = (keywords: IKeyword[] | undefined): string => {
        if (keywords && keywords.length > 0) {
            return keywords?.map((keyword: IKeyword) => keyword.title).join(", ");
        }
        return "NA";
    }

    // Renders page by current page type.
    renderPageByType = () => {
        if (this.state.currentPage === PageType.ChooseRequestType) {
            return this.requestTypeSelectionPage();
        }
        else if (this.state.currentPage === PageType.FillRequestDetails) {
            return this.fillRequestDetailsPage();
        }
        else if (this.state.currentPage === PageType.RequestDetailsPage) {
            return this.renderRequestDetails();
        }
        else {
            return this.deleteConfirmationPage();
        }
    }

    // Renders request details.
    renderRequestDetails = () => {
        return <React.Fragment>
            <Text content={this.state.request.requestType === RequestType.News ? this.localize("newsRequestDetailsTitle") : this.localize("coiRequestDetailsTitle")} weight="semibold" />
            <Flex className="overflow-y" fill column gap="gap.small">
                {this.getRequestDetailsElement(this.localize("requestDetailsTitle"), this.state.request.title)}
                {this.getRequestDetailsElement(this.localize("requestDetailsDescription"), this.state.request.description)}
                {this.getRequestDetailsElement(this.localize("requestDetailsKeywords"), this.getKeywords(this.state.request.keywords))}
                {this.state.request.requestType === RequestType.CommunityOfInterest && this.getRequestDetailsElement(this.localize("requestDetailsCOIType"), getLocalizedCOIType(this.state.request.coiType, this.localize))}
                {this.state.request.requestType === RequestType.News && this.getRequestDetailsElement(this.localize("requestDetailsRedirectionUrl"), this.state.request.externalLink, true)}
                {this.state.request.requestType === RequestType.News && this.getRequestDetailsElement(this.localize("requestDetailsImageUrl"), this.state.request.imageUrl, true)}
                {this.state.request.requestType === RequestType.News && this.state.request.status === RequestStatus.Approved && this.getRequestDetailsElement(`${this.localize("ratedByUsersLabel")}:`, this.getNewsArticleRating(), false)}
                {this.state.request.requestType === RequestType.News && this.getRequestDetailsElement(this.localize("requestDetailsImportant"), this.state.request.isImportant ? this.localize("yesLabel") : this.localize("noLabel"))}
                {this.getRequestDetailsElement(this.localize("requestDetailsStatus"), getLocalizedRequestStatus(this.state.request.status, this.localize))}
                {this.getRequestDetailsElement(this.localize("requestDetailsCreatedOn"), this.state.request.createdAt ? moment(this.state.request.createdAt).format("DD-MMM-YYYY, hh:mm A") : "NA")}
            </Flex>
            <Flex.Item push>
                <Flex gap="gap.small">
                    {
                        this.state.request.status === RequestStatus.Draft &&
                        <Flex.Item push>
                            <Button className="athena-button" content={this.localize("deleteButtonContent")} onClick={this.onDeleteRequest} />
                        </Flex.Item>
                    }
                    {
                        this.state.request.status === RequestStatus.Draft &&
                        <Flex.Item push>
                            <Button className="athena-button" content={this.localize("editButtonContent")} onClick={this.onEditRequest} />
                        </Flex.Item>
                    }
                </Flex>
            </Flex.Item>
        </React.Fragment>;
    }

    // Renders component
    render() {
        if (this.requestIdToEditOrDelete && this.state.isLoadingRequestDetails) {
            return <Flex className="task-module-container" column fill gap="gap.medium">
                <Loader />
            </Flex>;
        }

        if (this.requestIdToEditOrDelete && !this.state.isLoadingRequestDetails) {
            if (this.isSomethingWentWrong) {
                return <Flex className="task-module-container" column fill gap="gap.medium">
                    <NoContent message={this.localize("somethingWentWrongMessage")} />;
                </Flex>;
            }
            else if (this.isRequestNotFound) {
                return <Flex className="task-module-container" column fill gap="gap.medium">
                    <NoContent message={this.localize("failedToLoadRequestDetails")} />;
                </Flex>;
            }
        }

        return (
            <Flex className="task-module-container new-request" column fill gap="gap.medium">
                <StatusBar status={this.state.status} isMobile={false} />
                { this.renderPageByType() }
            </Flex>
        );
    }
}

export default withTranslation()(withRouter(NewRequest));