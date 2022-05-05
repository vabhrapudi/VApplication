// <copyright file="your-daily-briefing-card.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Card, Flex, Text } from "@fluentui/react-northstar";
import CardImage from "../../common/card-image/card-image";
import moment from "moment";

import "./your-daily-briefing-card.scss";

interface IYourDailyBriefingCardProps {
    header: string;
    description: string;
    date?: Date;
    imageUrl?: string;
    articleUrl?: string;
}

const YourDailyBriefingCard: React.FunctionComponent<IYourDailyBriefingCardProps> = (props: IYourDailyBriefingCardProps) => {
    return <Card className="your-daily-briefing-card" elevated>
        <Flex column gap="gap.small">
            <Flex gap="gap.small" vAlign="center">
                {
                    props.imageUrl !== undefined && props.imageUrl !== null && <CardImage className="image" imageSrc={props.imageUrl} />
                }
                <Flex column>
                    <Text className={`header${props.articleUrl ? " link" : ""}`} content={props.header} title={props.header} weight="bold" onClick={props.articleUrl ? () => { window.open(props.articleUrl, "_blank") } : () => { }} />
                    {props.date !== undefined && <Text className="date" disabled content={moment(props.date).fromNow()} />}
                </Flex>
            </Flex>
            <Text className="description" content={props.description} title={props.description} />
        </Flex>
    </Card>;
}

export default YourDailyBriefingCard;