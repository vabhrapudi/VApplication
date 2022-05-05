// <copyright file="rate-comment.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Flex, TextArea, Text, Chat, CloseIcon, Button, SendIcon, Loader } from "@fluentui/react-northstar";
import { Rating } from '@fluentui/react';
import { useTranslation } from 'react-i18next';
import { initializeIcons } from 'office-ui-fabric-react/lib/Icons';
import { withRouter, RouteComponentProps } from "react-router-dom";
import { StatusCodes } from "http-status-codes";
import { getResearchProjectTableIdAsync, rateResearchProjectAsync } from "../../../api/research-project-api";
import { getResourceCommentsAsync, addResourceCommentAsync } from "../../../api/comments-api";
import { getCoiByTableIdAsync, rateCoiAsync } from "../../../api/coi-requests-api";
import { getEventByTableIdAsync, rateEventAsync } from "../../../api/event-api";
import { getPartnerByTableIdAsync, ratePartnerAsync } from "../../../api/partner-api";
import { getSponsorByTableIdAsync, rateSponsorAsync } from "../../../api/sponsor-api";
import { getResearchRequestByTableIdAsync, rateResearchRequestAsync } from "../../../api/research-request-api";
import { getNewsByTableIdAsync, rateNews } from "../../../api/news-api";
import { ICommentEntity } from "../../../models/comment-entity";
import StatusBar from "../../common/status-bar/status-bar";
import IStatusBar from "../../../models/status-bar";
import { ActivityStatus } from "../../../models/activity-status";
import ContentLoader from "../../common/loader/loader";
import { DiscoveryNodeFileNames } from "../../../models/discovery-tree-node-file-names";
import { ItemType } from "../../../models/discovery-tree-item-type";
import { getResearchProposalByTableIdAsync, rateResearchProposalAsync } from "../../../api/research-proposal-api";
import { cloneDeep } from "lodash";

import "./rate-comment.scss";

initializeIcons();

interface IRateOrCommentProps extends RouteComponentProps {
    itemTableId: string;
    itemJsonFile: string;
    onDisplayDetailsCloseIconClick: () => void;
}

const RateOrComment: React.FunctionComponent<IRateOrCommentProps> = (props) => {

    const localize = useTranslation().t;
    const [isLoading, setIsLoading] = React.useState<boolean>(false);
    const [commentsData, setCommentsData] = React.useState<ICommentEntity[]>([]);
    const [nodeDetails, setNodeDetails] = React.useState<any>(undefined);
    const [isPageLoading, setIsPageLoading] = React.useState<boolean>(true);
    const [comment, setComment] = React.useState<string>("");
    const [status, setStatus] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });

    React.useEffect(() => {
        fetchComments();
        getItemDetails();
    }, [props.itemTableId, props.itemJsonFile]);

    const fetchComments = async () => {
        setIsPageLoading(true);

        let resourceTypeId: number = 0;

        if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchProjects) {
            resourceTypeId = ItemType.ResearchProject;
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaNewsArticles) {
            resourceTypeId = ItemType.News;
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaCommunities) {
            resourceTypeId = ItemType.COI;
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchRequests) {
            resourceTypeId = ItemType.ResearchRequest;
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchProposals) {
            resourceTypeId = ItemType.ResearchProposal;
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaPartners) {
            resourceTypeId = ItemType.Partner;
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaSponsors) {
            resourceTypeId = ItemType.Sponsor;
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaEvents) {
            resourceTypeId = ItemType.Event;
        }

        if (resourceTypeId) {
            let response = await getResourceCommentsAsync(props.itemTableId, resourceTypeId, handleTokenAccessFailure);

            if (response && response.status === StatusCodes.OK) {
                setCommentsData(response.data);
            }
            else {
                setStatus((prevState: IStatusBar) => ({ id: prevState.id + 1, message: localize("failedToLoadCommentsError"), type: ActivityStatus.Error }));
            }
        }
        
        setIsPageLoading(false);
    }

    const getItemDetails = async () => {
        setIsPageLoading(true);

        let response;

        if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchProjects) {
            response = await getResearchProjectTableIdAsync(props.itemTableId, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaNewsArticles) {
            response = await getNewsByTableIdAsync(props.itemTableId, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaCommunities) {
            response = await getCoiByTableIdAsync(props.itemTableId, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchRequests) {
            response = await getResearchRequestByTableIdAsync(props.itemTableId, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchProposals) {
            response = await getResearchProposalByTableIdAsync(props.itemTableId, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaPartners) {
            response = await getPartnerByTableIdAsync(props.itemTableId, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaSponsors) {
            response = await getSponsorByTableIdAsync(props.itemTableId, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaEvents) {
            response = await getEventByTableIdAsync(props.itemTableId, handleTokenAccessFailure);
        }

        if (response && response.status === StatusCodes.OK) {
            setNodeDetails(response.data);
        }
        else {
            setStatus((prevState: IStatusBar) => ({ id: prevState.id + 1, message: localize("failedToLoadNodeDetails"), type: ActivityStatus.Error }));
        }
        setIsPageLoading(false);
    }

    const addUserComment = async () => {
        if (comment) {
            setIsLoading(true);

            let resourceTypeId: number = 0;

            if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchProjects) {
                resourceTypeId = ItemType.ResearchProject;
            }
            else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaNewsArticles) {
                resourceTypeId = ItemType.News;
            }
            else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaCommunities) {
                resourceTypeId = ItemType.COI;
            }
            else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchRequests) {
                resourceTypeId = ItemType.ResearchRequest;
            }
            else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchProposals) {
                resourceTypeId = ItemType.ResearchProposal;
            }
            else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaPartners) {
                resourceTypeId = ItemType.Partner;
            }
            else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaSponsors) {
                resourceTypeId = ItemType.Sponsor;
            }
            else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaEvents) {
                resourceTypeId = ItemType.Event;
            }

            if (resourceTypeId) {
                let response = await addResourceCommentAsync(props.itemTableId, resourceTypeId, comment, handleTokenAccessFailure);
                if (response && response.status === StatusCodes.OK) {
                    setComment("");
                    var existingComments = cloneDeep(commentsData);
                    existingComments.push(response.data);
                    setCommentsData(existingComments);
                    setStatus((prevState: IStatusBar) => ({ id: prevState.id + 1, message: localize("addCommentSuccessMsg"), type: ActivityStatus.Success }));
                }
                else {
                    setStatus((prevState: IStatusBar) => ({ id: prevState.id + 1, message: localize("failedToAddCommentText"), type: ActivityStatus.Error }));
                }
            }

            setIsLoading(false);
        }
    }

    /**
     * Updates the rating of a news article.
     * @param event The event call when rating changes. 
     * @param rating The rating given.
     */
    const handleRatingChange = async (event: any, rating: number) => {
        setIsLoading(true);

        let response;

        if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchProjects) {
            response = await rateResearchProjectAsync(props.itemTableId, rating, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaNewsArticles) {
            response = await rateNews(props.itemTableId, rating, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaCommunities) {
            response = await rateCoiAsync(props.itemTableId, rating, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchRequests) {
            response = await rateResearchRequestAsync(props.itemTableId, rating, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaResearchProposals) {
            response = await rateResearchProposalAsync(props.itemTableId, rating, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaPartners) {
            response = await ratePartnerAsync(props.itemTableId, rating, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaSponsors) {
            response = await rateSponsorAsync(props.itemTableId, rating, handleTokenAccessFailure);
        }
        else if (props.itemJsonFile === DiscoveryNodeFileNames.AthenaEvents) {
            response = await rateEventAsync(props.itemTableId, rating, handleTokenAccessFailure);
        }

        if (response && response.status === StatusCodes.OK) {
            setStatus((prevState: IStatusBar) => ({ id: prevState.id + 1, message: localize("ratingUpdatedSuccessMsg"), type: ActivityStatus.Success }));
            var existingNodeDetails = cloneDeep(nodeDetails);

            if (existingNodeDetails?.userRating) {
                if (existingNodeDetails?.userRating > rating) {
                    existingNodeDetails.sumOfRatings! -= (existingNodeDetails?.userRating - rating);
                }
                else {
                    existingNodeDetails.sumOfRatings! += (rating - existingNodeDetails?.userRating);
                }
            }
            else {
                existingNodeDetails.sumOfRatings! += rating;
                existingNodeDetails.numberOfRatings! += 1;
            }
            existingNodeDetails.userRating! = rating;

            setNodeDetails(existingNodeDetails);
        }
        else {
            setStatus((prevState: IStatusBar) => ({ id: prevState.id + 1, message: localize("failedToUpdateRatingText"), type: ActivityStatus.Error }));
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

    // Returns average rating of research project.
    const getAverageRating = () => {
        if (nodeDetails?.numberOfRatings && nodeDetails?.sumOfRatings) {
            if (!isNaN(nodeDetails.sumOfRatings / nodeDetails.numberOfRatings)) {
                return (nodeDetails.sumOfRatings / nodeDetails?.numberOfRatings).toFixed(1);
            }
        }
        return 0;
    }

    return (
        <Flex className="rate-comment-pane">
            <StatusBar status={status} isMobile={false} />
            {
                isPageLoading ?
                    <ContentLoader />
                    :
                    <>
                        <Flex vAlign="center">
                            <Loader className="details-pane-loader" hidden={!isLoading} />
                        </Flex>
                        <Flex column gap="gap.medium" padding="padding.medium" className="comments-sub-container">
                            <Flex vAlign="center">
                                <Text content={localize("researchProjectRatingText", { 0: getAverageRating() })} size = "large" weight = "bold" />
                                <Flex.Item push>
                                    <CloseIcon onClick={props.onDisplayDetailsCloseIconClick} className="icon-pointer" />
                                </Flex.Item>
                            </Flex>
                            <Flex hAlign="center">
                                <Rating
                                    max={5}
                                    allowZeroStars
                                    rating={nodeDetails?.userRating ?? 0}
                                    onChange={(event: any, rating?: number) => handleRatingChange(event, rating ?? 0)}
                                    className="research-rating"
                                    disabled={isLoading}
                                />
                            </Flex>
                            <Text content="Comments:" weight="semibold" />
                            <Flex column gap="gap.medium" className="comments-container">
                                {
                                    commentsData.length > 0 && commentsData.map((item, index) => <Chat.Message
                                        content={{
                                            content: (
                                                <div>
                                                    {item.comment}
                                                </div>
                                            ),
                                        }}
                                        author={item.userName}
                                    />)
                                }
                            </Flex>
                            <Flex.Item push>
                                <Flex>
                                    <TextArea value={comment} onChange={(event: any) => { setComment(event.target.value); }} fluid placeholder={localize('commentTextAreaPlaceholder')} disabled={isLoading} />
                                    <Flex.Item push>
                                        <Button text iconOnly icon={<SendIcon />} className="icon-pointer" onClick={addUserComment} disabled={isLoading} />
                                    </Flex.Item>
                                </Flex>
                            </Flex.Item>
                        </Flex>
                    </>
            }
        </Flex>
    )
};

export default withRouter(RateOrComment);