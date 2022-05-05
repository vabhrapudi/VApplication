// <copyright file="loader.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from "react";
import { Flex, Loader } from "@fluentui/react-northstar";

interface ILoaderProps {
    className?: string,
    label?: string
}

const ContentLoader: React.FunctionComponent<ILoaderProps> = (props: ILoaderProps): JSX.Element => {
    return (
        <Flex className={props.className ?? ""} vAlign="center" hAlign="center" fill>
            <Loader label={props.label ?? ""} />
        </Flex>
    );
}

export default ContentLoader;