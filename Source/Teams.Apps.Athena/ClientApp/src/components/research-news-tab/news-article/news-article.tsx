// <copyright file="news-article.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Flex, Header, Text, Card, Loader, FlagIcon } from "@fluentui/react-northstar";
import { Rating } from '@fluentui/react';
import { IResearchNews } from "../../../models/type";
import { useTranslation } from 'react-i18next';
import { rateNews, updateNewsAsync } from '../../../api/news-api';
import { initializeIcons } from 'office-ui-fabric-react/lib/Icons';
import moment from "moment";
import IStatusBar from "../../../models/status-bar";
import { ActivityStatus } from "../../../models/activity-status";
import { withRouter, RouteComponentProps } from "react-router-dom";
import { StatusCodes } from "http-status-codes";
import CardImage from "../../common/card-image/card-image";
import IKeyword from "../../../models/keyword";
import { IAthenaNewsSource } from "../../../models/athena-news-source";

import "./news-article.scss";

var cloneDeep = require('lodash.clonedeep');
initializeIcons();

interface IProps extends RouteComponentProps {
    newsArticleData: IResearchNews[];
    allKeywords: IKeyword[];
    updateNewsItem: (updatedNewsData: IResearchNews[]) => void;
    displayUpdateStatus: (updatedStatus: IStatusBar) => void;
    newsSources: IAthenaNewsSource[];
    isUserAdmin: boolean;
}

const NewsArticle: React.FunctionComponent<IProps> = (props) => {
    const localize = useTranslation().t;
    const [status] = React.useState<IStatusBar>({ id: 0, message: "", type: ActivityStatus.None });
    const [isLoading, setIsLoading] = React.useState<boolean>();
    const [newsList, setNewsList] = React.useState<IResearchNews[]>(props.newsArticleData);

    React.useEffect(() => {
        setNewsList(props.newsArticleData);
    }, [props.newsArticleData]);

    /**
     * Updates the rating of a news article.
     * @param event The event call when rating changes. 
     * @param rating The rating given.
     * @param tableId The id of news article.
     */
    const handleRatingChange = async (event: any, rating: number, tableId: string) => {
        setIsLoading(!isLoading);
        var index = newsList.findIndex((news: IResearchNews) => news.tableId === tableId);
        var existingNewsData = cloneDeep(newsList) as IResearchNews[];

        var response = await rateNews(tableId, rating, handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            props.displayUpdateStatus({ id: status.id + 1, message: localize("ratingUpdatedSuccessMsg"), type: ActivityStatus.Success });
            setIsLoading(false);

            if (existingNewsData[index].userRating) {
                if (existingNewsData[index].userRating > rating) {
                    existingNewsData[index].sumOfRatings -= (existingNewsData[index].userRating - rating);
                }
                else {
                    existingNewsData[index].sumOfRatings += (rating - existingNewsData[index].userRating);
                }
            }
            else {
                existingNewsData[index].sumOfRatings += rating;
                existingNewsData[index].numberOfRatings += 1;
            }
            existingNewsData[index].userRating = rating;

            props.updateNewsItem(existingNewsData);
        }
        else {
            props.displayUpdateStatus({ id: status.id + 1, message: localize("failedToUpdateRatingText"), type: ActivityStatus.Error });
            setIsLoading(false);
        }
    }

    /**
     * Marks the news article as flagged.
     * @param index The news article's index.
     */
    const handleFlagIconClick = async (index: number) => {
        setIsLoading(true);

        var existingNewsData = cloneDeep(newsList) as IResearchNews[];

        var response = await updateNewsAsync(existingNewsData[index].tableId, true, handleTokenAccessFailure);

        if (response && response.status === StatusCodes.OK) {
            props.displayUpdateStatus({ id: status.id + 1, message: localize("markAsFlaggedSuccessMessage"), type: ActivityStatus.Success });
            existingNewsData[index] = response.data;
            props.updateNewsItem(existingNewsData);
        }
        else {
            props.displayUpdateStatus({ id: status.id + 1, message: localize("markAsFlaggedErrorMessage"), type: ActivityStatus.Error });
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

    // Get keywords string separated by comma.
    const getKeywordsForNewsArticle = (keywords: number[]) => {
        if (!keywords?.length) {
            return "NA";
        }

        let keywordIds = keywords.map(String);

        return props.allKeywords
            ?.filter((keyword: IKeyword) => keywordIds.some((keywordId: string) => keyword.keywordId === keywordId))
            .map((keyword: IKeyword) => keyword.title)
            .join(", ") ?? "";
    }

    /**
     * Returns news source title.
     * @param newsSourceId The news source Id.
     */
    const getNewsSourceTitle = (newsSourceId: number) => {
        var newsSource = props.newsSources.find((newsSource: IAthenaNewsSource) => newsSource.newsSourceId === newsSourceId);
        if (newsSource) {
            return newsSource.title;
        }
        return "NA";
    }

    return (
        <>
            <Loader className="tab-loader rating-loader" hidden={!isLoading} />
            {
                newsList.map((news: IResearchNews, index: number) => {
                    let newsArticleKeywords = !props.allKeywords?.length ? "" : getKeywordsForNewsArticle(news.keywords);

                    return (
                        <Card fluid className="news-card" key={index}>
                            <Flex gap="gap.small" styles={{width:"100%"}}>
                                <Card.Preview horizontal>
                                    <CardImage className="article-image" imageSrc={news.imageUrl} />
                                </Card.Preview>
                                <Flex column gap="gap.smaller" styles={{ width: "100%" }}>
                                    <Flex>
                                        <Flex column>
                                            <Header content={<Text color="brand" content={news.title} />} title={news.title} className="article-heading" onClick={() => { window.open(news.externalLink, "_blank") }} />
                                            <Text content={localize("newsSourceText", { 0: news.publishedDate ? moment(news.publishedDate).format("DD-MMM-YYYY hh:mm A") : "NA", 1: getNewsSourceTitle(news.newsSourceId), 2: isNaN(news.sumOfRatings / news.numberOfRatings) ? 0 : (news.sumOfRatings / news.numberOfRatings).toFixed(1) })} />
                                        </Flex>
                                        <Flex.Item push>
                                            <Flex gap="gap.large" hAlign="center">
                                                {
                                                    news.isImportant &&
                                                    <FlagIcon size="medium" className="important-icon" />
                                                }
                                                {
                                                    !news.isImportant && props.isUserAdmin &&
                                                    <FlagIcon size="medium" className="flag-icon" onClick={() => handleFlagIconClick(index)} title={localize("newRequestMarkAsImportantLabel")} />
                                                }
                                                <Rating
                                                    max={5}
                                                    allowZeroStars
                                                    rating={news.userRating}
                                                    onChange={(event: any, rating?: number, tableId?: string) => handleRatingChange(event, rating ?? 0, news.tableId)}
                                                    className="news-rating"
                                                    disabled={isLoading}
                                                />
                                            </Flex>
                                        </Flex.Item>
                                    </Flex>
                                    <Text content={news.body ? news.body : news.abstract} title={news.body ? news.body : news.abstract} className="article-description" />
                                    {
                                        newsArticleKeywords &&
                                        <Flex gap="gap.smaller">
                                            <Text content={localize("requestDetailsKeywords")} weight="semibold" className="keyword-name" />
                                            <Text content={newsArticleKeywords} title={newsArticleKeywords} className="keyword-display" />
                                        </Flex>
                                    }
                                </Flex>
                            </Flex>
                        </Card>
                    )
                })
            }
        </>
    )
};

export default withRouter(NewsArticle);