// <copyright file="approve-reject-task-module.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { useTranslation } from 'react-i18next';
import { withRouter, RouteComponentProps } from "react-router-dom";
import { Flex, Text, Pill, Avatar, Button, Checkbox, Skeleton, SkeletonLine, Label, FormTextArea } from "@fluentui/react-northstar";
import moment from "moment";
import Constants from "../../constants/constants";
import RequestType from "../../models/request-type";
import ICoi from "../../models/coi";
import INews from "../../models/news";
import { getLocalizedCOIType, getLocalizedRequestStatus } from "../../helpers/localization-helper";
import { getCoiRequestByIdAsync, getNewsRequestByIdAsync, approveCoiRequests, rejectCoiRequests, approveNewsArticleRequests, rejectNewsArticleRequests } from "../../api/request-tab-api";
import { StatusCodes } from "http-status-codes";
import StatusBar from "../../components/common/status-bar/status-bar";
import IStatusBar from "../../models/status-bar";
import { ActivityStatus } from "../../models/activity-status";
import { TFunction } from "i18next";
import Loader from "../../components/common/loader/loader";
import RequestStatus from "../../models/request-status";
import Image from "../common/card-image/card-image";
import IUserDetails from "../../models/user-details";
import { getUsersDetailsFromGraphAsync } from "../../api/user-details-api";

import "./approve-reject-task-module.scss";

interface IApproveRejectTaskModuleProps extends RouteComponentProps {
}

enum PageType {
    NewsArticleRequestDetails = "0",
    CoiRequestDetails = "1",
    RejectReason = "2"
}

const ApproveRejectRequest: React.FunctionComponent<IApproveRejectTaskModuleProps> = (props: IApproveRejectTaskModuleProps) => {
    const localize: TFunction = useTranslation().t;
    const [coiRequestDetails, setCoiRequestDetails] = React.useState({} as ICoi);
    const [newsRequestDetails, setNewsRequestDetails] = React.useState({} as INews);
    const [statusBar, setStatusBar] = React.useState({ id: 0, message: "", type: ActivityStatus.None } as IStatusBar);
    const [isFailedToGetRequestDetails, setFailedToGetRequestDetails] = React.useState(false);
    const [isApprovingRequest, setIsApprovingRequest] = React.useState(false);
    const [isRejectingRequest, setIsRejectingRequest] = React.useState(false);
    const [isLoadingRequestDetails, setLoadingRequestDetails] = React.useState(true);
    const [pageType, setPageType] = React.useState("");
    const [userDetails, setUserDetails] = React.useState({ displayName: "NA" } as IUserDetails);
    const [isLoadingUserDetails, setLoadingUserDetails] = React.useState(true);
    const [rejectReason, setRejectReason] = React.useState("");
    const [requestType, setRequestType] = React.useState("");
    const [requestId, setRequestId] = React.useState("");
    const [isNewsArticleImportant, setNewsArticleImportant] = React.useState(false);

    React.useEffect(() => {
        microsoftTeams.initialize();

        let urlParams = new URLSearchParams(window.location.search);
        let requestIdToApproveOrReject = urlParams.get(Constants.UrlParamRequestIdToApproveOrRejectRequest) ?? "";
        setRequestId(requestIdToApproveOrReject);

        let requestTypeParam = urlParams.get(Constants.UrlParamRequestType) ?? "";

        if (requestTypeParam === RequestType.News.toString()) {
            setRequestType(RequestType.News.toString());
            setPageType(PageType.NewsArticleRequestDetails);

            getNewsArticleRequestAsync(requestIdToApproveOrReject);
        }
        else if (requestTypeParam === RequestType.CommunityOfInterest.toString()) {
            setRequestType(RequestType.CommunityOfInterest.toString());
            setPageType(PageType.CoiRequestDetails);

            getCoiRequestAsync(requestIdToApproveOrReject);
        }
    }, []);

    const getCoiRequestAsync = async (requestId: string) => {
        let response = await getCoiRequestByIdAsync(requestId);

        setLoadingRequestDetails(false);

        if (response && response.status === StatusCodes.OK) {
            let data = response.data as ICoi;

            setCoiRequestDetails(data);
            getUserDetails(data.createdBy);
        }
        else {
            setFailedToGetRequestDetails(true);
        }
    }

    const getNewsArticleRequestAsync = async (requestId: string) => {
        let response = await getNewsRequestByIdAsync(requestId);

        setLoadingRequestDetails(false);

        if (response && response.status === StatusCodes.OK) {
            let data = response.data as INews;

            setNewsArticleImportant(data.isImportant);
            setNewsRequestDetails(data);
            getUserDetails(data.createdBy);
        }
        else {
            setFailedToGetRequestDetails(true);
        }
    }

    const getUserDetails = async (userId: string) => {
        let response = await getUsersDetailsFromGraphAsync([userId], handleTokenAccessFailure);

        setLoadingUserDetails(false);

        if (response && response.status === StatusCodes.OK && response.data && response.data.length > 0) {
            setUserDetails(response.data[0] as IUserDetails);
        }
    }

    const approveNewsArticleRequestAsync = async (newsTableId: string) => {
        setIsApprovingRequest(true);

        let response = await approveNewsArticleRequests([newsTableId], handleTokenAccessFailure, isNewsArticleImportant);

        if (response && response.status === StatusCodes.OK) {
            if (response.data === true) {
                microsoftTeams.tasks.submitTask({
                    data: "Approved",
                    type: RequestType.News
                });
            }
            else {
                setIsApprovingRequest(false);
                setStatusBar({ id: statusBar.id + 1, message: localize("requestAlreadyApprovedOrRejected"), type: ActivityStatus.Error });
            }
        }
        else {
            setIsApprovingRequest(false);
            setStatusBar({ id: statusBar.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
        }
    }

    const rejectNewsArticleRequestAsync = async (newsTableId: string) => {
        setIsRejectingRequest(true);

        let response = await rejectNewsArticleRequests([newsTableId], rejectReason, handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            if (response.data === true) {
                microsoftTeams.tasks.submitTask({
                    data: "Rejected",
                    type: RequestType.News
                });
            }
            else {
                setIsRejectingRequest(false);
                setStatusBar({ id: statusBar.id + 1, message: localize("requestAlreadyApprovedOrRejected"), type: ActivityStatus.Error });
            }
        }
        else {
            setIsRejectingRequest(false);
            setStatusBar({ id: statusBar.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
        }
    }

    const approveCoiRequestAsync = async (coiTableId: string) => {
        setIsApprovingRequest(true);

        let response = await approveCoiRequests([coiTableId], handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            if (response.data === true) {
                microsoftTeams.tasks.submitTask({
                    data: "Approved",
                    type: RequestType.CommunityOfInterest
                });
            }
            else {
                setIsApprovingRequest(false);
                setStatusBar({ id: statusBar.id + 1, message: localize("requestAlreadyApprovedOrRejected"), type: ActivityStatus.Error });
            }
        }
        else {
            setIsApprovingRequest(false);
            setStatusBar({ id: statusBar.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
        }
    }

    const rejectCoiRequestAsync = async (coiTableId: string) => {
        setIsRejectingRequest(true);

        let response = await rejectCoiRequests([coiTableId], rejectReason, handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            if (response.data === true) {
                microsoftTeams.tasks.submitTask({
                    data: "Rejected",
                    type: RequestType.CommunityOfInterest
                });
            }
            else {
                setIsRejectingRequest(false);
                setStatusBar({ id: statusBar.id + 1, message: localize("requestAlreadyApprovedOrRejected"), type: ActivityStatus.Error });
            }
        }
        else {
            setIsRejectingRequest(false);
            setStatusBar({ id: statusBar.id + 1, message: localize("somethingWentWrongMessage"), type: ActivityStatus.Error });
        }
    }

    const onChangeRequestRejectReason = (eventDetails: any, eventData: any) => {
        setRejectReason(eventData?.value ?? "");
    }

    const onBackButtonClick = () => {
        setRejectReason("");

        if (requestType === RequestType.News.toString()) {
            setPageType(PageType.NewsArticleRequestDetails);
        }
        else {
            setPageType(PageType.CoiRequestDetails);
        }
    }

    const onRejectRequestConfirmed = () => {
        if (requestType === RequestType.News.toString()) {
            rejectNewsArticleRequestAsync(requestId);
        }
        else {
            rejectCoiRequestAsync(requestId);
        }
    }

    const onRejectButtonClick = () => {
        setRejectReason("");
        setPageType(PageType.RejectReason);
    }

    const onNewsArticleImportantChange = (eventDetails: any, eventData: any) => {
        setNewsArticleImportant(eventData.checked);
    }

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    const renderNewsArticleRequestDetails = () => {
        return <Flex column className="task-module-container approve-reject-task-module" fill>
            <StatusBar isMobile={false} status={statusBar} />
            <Flex className="container" column gap="gap.medium">
                <Flex vAlign="center">
                    <Pill className="news-request-pill">{localize("newsRequestText")}</Pill>
                    {
                        newsRequestDetails.isImportant === true &&
                        <Pill className="important-news-request-pill">{localize("importantText")}</Pill>
                    }
                    <Flex.Item push>
                        <Label className="status-label" circular color={getStatusColor(newsRequestDetails.status)} content={getLocalizedRequestStatus(newsRequestDetails.status, localize)} />
                    </Flex.Item>
                </Flex>
                <Flex column>
                    <Flex vAlign="center" gap="gap.small">
                        <Text content={newsRequestDetails.title} weight="semibold" size="large" />
                        <Flex.Item push>
                            <a href={newsRequestDetails.externalLink} target="_blank">{localize("openLinkText")}</a>
                        </Flex.Item>
                    </Flex>
                    <Flex vAlign="center" gap="gap.smaller">
                        <Avatar name={userDetails.displayName} image={userDetails.profileImage} size="small" />
                        {
                            isLoadingUserDetails &&
                            <Skeleton animation="wave">
                                <Flex vAlign="center" gap="gap.smaller">
                                    <SkeletonLine width="4rem" height="1.2rem" />
                                    <Text content={` | ${moment(newsRequestDetails.createdAt).format("DD-MMM-YYYY hh:mm A")}`} size="small" />
                                </Flex>
                            </Skeleton>
                        }
                        {
                            !isLoadingUserDetails &&
                            <Text content={`${userDetails.displayName} | ${moment(newsRequestDetails.createdAt).format("DD-MMM-YYYY hh:mm A")}`} size="small" />
                        }
                    </Flex>
                </Flex>
                <Image className="news-image" imageSrc={newsRequestDetails.imageUrl} />
                <Text content={newsRequestDetails.body} />
            </Flex>
            {
                newsRequestDetails.status === RequestStatus.Pending &&
                <Flex.Item push>
                    <Flex column>
                        {
                            newsRequestDetails.isImportant &&
                            <Text className="mark-as-important-admin-note" content={localize("markAsImportantAdminNote")} weight="semibold" />
                        }
                        <Flex vAlign="center" gap="gap.small">
                            <Checkbox checked={isNewsArticleImportant} label={localize("newRequestMarkAsImportantLabel")} onChange={onNewsArticleImportantChange} />
                            <Flex.Item push>
                                <Button content={localize("rejectText")} onClick={onRejectButtonClick} disabled={isApprovingRequest} />
                            </Flex.Item>
                            <Button className="athena-button" content={localize("approveText")} onClick={() => approveNewsArticleRequestAsync(newsRequestDetails.tableId)} loading={isApprovingRequest} disabled={isApprovingRequest} />
                        </Flex>
                    </Flex>
                </Flex.Item>
            }
        </Flex>;
    }

    const getStatusColor = (status: RequestStatus) => {
        switch (status) {
            case RequestStatus.Pending:
                return 'yellow';
            case RequestStatus.Approved:
                return 'green';
            case RequestStatus.Rejected:
                return 'red';
            default:
                return 'grey';
        }
    }

    const renderCoiDetails = () => {
        return <Flex column className="task-module-container approve-reject-task-module">
            <StatusBar isMobile={false} status={statusBar} />
            <Flex className="container" column gap="gap.medium">
                <Flex vAlign="center">
                    <Pill>{`${localize("coiRequestText")} ${getLocalizedCOIType(coiRequestDetails.type, localize)}`}</Pill>
                    <Flex.Item push>
                        <Label className="status-label" circular color={getStatusColor(coiRequestDetails.status)} content={getLocalizedRequestStatus(coiRequestDetails.status, localize)} />
                    </Flex.Item>
                </Flex>
                <Flex column>
                    <Text content={localize("coiNameLabel")} weight="semibold" />
                    <Text content={coiRequestDetails.coiName} />
                </Flex>
                <Flex column gap="gap.small">
                    <Text content={localize("requestorNameLabel")} weight="semibold" />
                    <Flex vAlign="center" gap="gap.smaller">
                        <Avatar name={userDetails.displayName} image={userDetails.profileImage} size="small" />
                        {
                            !isLoadingUserDetails &&
                            <Text content={userDetails.displayName} />
                        }
                    </Flex>
                </Flex>
                <Flex column>
                    <Text content={localize("requestedOnLabel")} weight="semibold" />
                    <Text content={coiRequestDetails.createdOn ? moment(coiRequestDetails.createdOn).format("DD-MMM-YYYY hh:mm A") : "NA"} />
                </Flex>
                <Flex column>
                    <Text content={localize("requestDetailsDescription")} weight="semibold" />
                    <Text content={coiRequestDetails.coiDescription} />
                </Flex>
            </Flex>
            {
                coiRequestDetails.status === RequestStatus.Pending &&
                <Flex.Item push>
                    <Flex vAlign="center" gap="gap.small">
                        <Flex.Item push>
                            <Button content={localize("rejectText")} onClick={onRejectButtonClick} disabled={isApprovingRequest} />
                        </Flex.Item>
                        <Button className="athena-button" content={localize("approveText")} onClick={() => approveCoiRequestAsync(coiRequestDetails.tableId)} loading={isApprovingRequest} disabled={isApprovingRequest} />
                    </Flex>
                </Flex.Item>
            }
        </Flex>;
    }

    const renderRequestRejectReasonPage = () => {
        return <Flex className="task-module-container approve-reject-task-module" column fill>
            <StatusBar isMobile={false} status={statusBar} />
            <FormTextArea
                className="reject-request-reason-textarea"
                label={`${localize("rejectRequestReasonLabel")}*`}
                fluid
                onChange={onChangeRequestRejectReason}
            />
            <Flex.Item push>
                <Flex vAlign="center" gap="gap.small">
                    <Flex.Item push>
                        <Button content={localize("backButtonContent")} onClick={onBackButtonClick} disabled={isRejectingRequest} />
                    </Flex.Item>
                    <Button className="athena-button" content={localize("rejectText")} onClick={onRejectRequestConfirmed} loading={isRejectingRequest} disabled={isRejectingRequest || !rejectReason?.trim()} />
                </Flex>
            </Flex.Item>
        </Flex>
    }

    if (isLoadingRequestDetails) {
        return <Loader className="task-module-container" />;
    }

    if (isFailedToGetRequestDetails) {
        return <Flex column className="task-module-container approve-reject-task-module" vAlign="center" hAlign="center">
            <Text content={localize("failedToFetchText")} weight="semibold" error />
        </Flex>
    }

    if (pageType === PageType.NewsArticleRequestDetails) {
        return renderNewsArticleRequestDetails();
    }
    else if (pageType === PageType.CoiRequestDetails) {
        return renderCoiDetails();
    }
    else if (pageType === PageType.RejectReason) {
        return renderRequestRejectReasonPage();
    }

    return <></>;
}

export default withRouter(ApproveRejectRequest);