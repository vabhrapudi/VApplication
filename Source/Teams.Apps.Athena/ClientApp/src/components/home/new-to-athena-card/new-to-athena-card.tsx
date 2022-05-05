// <copyright file="new-to-athena-card.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Card, Flex, Text, Popup } from "@fluentui/react-northstar";
import CardImage from "../../common/card-image/card-image";

import "./new-to-athena-card.scss";

interface INewToAthenaCardProps {
    header: string;
    description: string;
    imageUrl: string;
}

const NewToAthenaCard: React.FunctionComponent<INewToAthenaCardProps> = (props: INewToAthenaCardProps) => {
    return <Card className="new-to-athena-card" elevated>
        <Flex gap="gap.medium">
            <CardImage className="image" imageSrc={props.imageUrl} />
            <Flex column gap="gap.small">
                <Text className="header" content={props.header} title={props.header} weight="bold" />
                <Popup
                    className="new-to-athena-article-tooltip"
                    on="hover"
                    position="below"
                    align="start"
                    trigger={<div className="description" dangerouslySetInnerHTML={{ __html: props.description }} />}
                    content={{ design: {width: "24rem"}, content: <div dangerouslySetInnerHTML={{ __html: props.description }} /> } } />
            </Flex>
        </Flex>
    </Card>;
}

export default NewToAthenaCard;