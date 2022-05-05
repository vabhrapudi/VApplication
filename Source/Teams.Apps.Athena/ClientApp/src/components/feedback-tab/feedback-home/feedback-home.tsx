// <copyright file="feedback-home.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from 'react';
import { withRouter, RouteComponentProps } from "react-router-dom";
import { useTranslation } from 'react-i18next';
import { AthenaFeedBackEntity, AthenaFeedbackEnum } from '../../../models/athena-feedback';
import { Flex, Text, Image, FilterIcon, Popup, Button } from "@fluentui/react-northstar";
import InfiniteScroll from "react-infinite-scroller";
import StatusBar from "../../common/status-bar/status-bar";
import IStatusBar from "../../../models/status-bar";
import { ActivityStatus } from "../../../models/activity-status";
import { StatusCodes } from "http-status-codes";
import AthenaSplash from '../../athena-splash/athena-splash';
import { getAthenaFeedbacksAsync } from "../../../api/feedback-api";
import { cloneDeep } from "lodash";
import Constants from '../../../constants/constants';
import FeedbackCard from "../feedback-card/feedback-card";
import FilterPopup from '../../common/filter-popup/filter-popup';
import Loader from "../../common/loader/loader";
import { getFeedbackType } from '../../../helpers/localization-helper';
import IFilterItem from '../../../models/filter-item';

import "./feedback-home.scss";

interface IFeedbackHomeProps extends RouteComponentProps {
}

const FeedbackHome: React.FunctionComponent<IFeedbackHomeProps> = (props: IFeedbackHomeProps) => {
    const localize = useTranslation().t;
    const [feedbackData, setFeedbackData] = React.useState<AthenaFeedBackEntity[]>([]);
    const [isLoading, setIsLoading] = React.useState<boolean>(true);
    const [hasMore, setHasMore] = React.useState<Boolean>(true);
    const [key, setKey] = React.useState<number>(0);
    const [status, setStatus] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });
    const [selectedFilters, setSelectedFilters] = React.useState<AthenaFeedbackEnum[]>([]);

    let feedbackFilterList: IFilterItem[] = [
        { key: AthenaFeedbackEnum.Helpful, header: getFeedbackType(AthenaFeedbackEnum.Helpful, localize), isChecked: false } as IFilterItem,
        { key: AthenaFeedbackEnum.NotHelpful, header: getFeedbackType(AthenaFeedbackEnum.NotHelpful, localize), isChecked: false } as IFilterItem,
        { key: AthenaFeedbackEnum.NeedsImprovement, header: getFeedbackType(AthenaFeedbackEnum.NeedsImprovement, localize), isChecked: false } as IFilterItem
    ];

    React.useEffect(() => {
        if (!isLoading) {
            setKey(key + 1);
        }
        getFeedbackData(0);
    }, [selectedFilters]);

    /**
     * Redirects to sign-in page.
     * @param error The error string.
     */
    const handleTokenAccessFailure = (error: string) => {
        props.history.push("/signin");
    }

    /**
     * Fetches feedback data from database.
     * @param pageNumber The page number.
     */
    const getFeedbackData = async (pageNumber: number) => {
        if (pageNumber === 0) {
            setIsLoading(true);
        }
        let response = await getAthenaFeedbacksAsync(pageNumber, selectedFilters, handleTokenAccessFailure);
        if (response && response.status === StatusCodes.OK) {
            setIsLoading(false);
            if (pageNumber === 0) {
                setFeedbackData(response.data);
            }
            else {
                var existingFeedbackData = cloneDeep(feedbackData);
                var updatedFeedbackData = existingFeedbackData.concat(response.data);
                setFeedbackData(updatedFeedbackData);
            }

            if (response.data.length < Constants.lazyLoadFeedbacksCount) {
                setHasMore(false);
            }
        }
        else if (response && response.status === StatusCodes.NOT_FOUND) {
            setIsLoading(false);
        }
        else {
            setIsLoading(false);
            setStatus({ id: status.id + 1, message: localize("generalErrorMessage"), type: ActivityStatus.Error });
        }
    }

    /**
     * Event handler called when feedback filter gets changed.
     * @param feedbackKeysInFilter The selected feedback keys.
     */
    const onFeedbackFilterChange = (feedbackKeysInFilter: any[]) => {
        setSelectedFilters(feedbackKeysInFilter);
        if (hasMore === false) setHasMore(true);
    }

    return (
        <>
            <StatusBar status={status} isMobile={false} />
            <AthenaSplash heading={localize("feedbackTabSplashHeading")} />
            <Flex gap="gap.small" padding="padding.medium" column className="feedback-main-container">
                <Flex gap="gap.small" hAlign="end" vAlign="center">
                    <Popup
                        trigger={<Button
                            className="icon-pointer"
                            text
                            primary={selectedFilters.length !== 0 ?? false}
                            disabled={isLoading}
                            icon={<FilterIcon />}
                            iconOnly
                            content={`${localize("filterButtonContent")} ${selectedFilters.length ? `(${selectedFilters.length})` : ""}`}
                        />}
                        content={<FilterPopup
                            title={localize("feedbackText")}
                            clearText={localize("clearText")}
                            items={feedbackFilterList}
                            disabled={isLoading}
                            selectedFilterItemKeys={selectedFilters}
                            onCheckedChange={(feedbackKeysInFilter) => onFeedbackFilterChange(feedbackKeysInFilter)}
                        />}
                        position="below"
                        align="end"
                    />
                </Flex>
                <Flex fill column gap="gap.small" className="feedback-container">
                    {
                        isLoading ?
                            <Loader />
                            :
                            <>
                                {
                                    feedbackData.length > 0 ?
                                        <div key={key} className="pagination-scroll-area">
                                            <InfiniteScroll
                                                pageStart={0}
                                                initialLoad={false}
                                                loader={<Loader />}
                                                useWindow={false}
                                                loadMore={getFeedbackData}
                                                hasMore={hasMore}
                                            >
                                                <FeedbackCard feedbackData={feedbackData} />
                                                
                                            </InfiniteScroll>
                                        </div>
                                        :
                                        <Flex column gap="gap.medium" padding="padding.medium" hAlign="center" vAlign="center" >
                                            <Image src="Artifacts/Image6.png" className="no-feedback-image" />
                                            <Text content={localize("noFeedbackErrorMsg")} align="center" />
                                        </Flex>
                                }
                            </>
                    }
                </Flex>
            </Flex>
        </>
    )
};

export default withRouter(FeedbackHome);