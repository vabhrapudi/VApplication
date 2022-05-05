// <copyright file="my-requests.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import * as React from 'react';
import { Text, Flex, EyeSlashIcon } from "@fluentui/react-northstar";

interface INoContentProps {
    message: string
}

const NoContent: React.FunctionComponent<INoContentProps> = (props: INoContentProps) => {
    return (
        <Flex column vAlign="center" hAlign="center" fill gap="gap.small">
            <EyeSlashIcon size="largest" />
            <Text content={props.message} weight="semibold" />
        </Flex>
    );
}

export default NoContent;