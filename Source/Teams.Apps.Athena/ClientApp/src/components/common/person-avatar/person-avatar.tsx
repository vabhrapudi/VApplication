// <copyright file="person-avatar.tsx" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// </copyright>

import * as React from "react";
import { Avatar } from "@fluentui/react-northstar";

import "./person-avatar.scss";

interface IPersonAvatarProps {
    profilePhoto: string;
    userName: string;
}

export const PersonAvatar: React.FunctionComponent<IPersonAvatarProps> = (props: IPersonAvatarProps) => {

    return (
        <div>
            {
                props.profilePhoto ? <Avatar name={props.userName} image={props.profilePhoto} />
                    : <Avatar name={props.userName} className="avatar-icon" />
            }
        </div>
    )
}

export default PersonAvatar;