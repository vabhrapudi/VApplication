// <copyright file="image-helper.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import Constants from "../constants/constants";
import IDiscoveryTreeNodeType from "../models/discovery-tree-node-type";

/**
 * Gets the resource image path.
 * @param nodeTypes The collection of node types.
 * @param nodeTypeId The node type Id of which image path to get.
 */
export const getResourceImagePath = (nodeTypes: IDiscoveryTreeNodeType[], nodeTypeId: number | undefined) => {
    if (nodeTypeId) {
        let nodeType: IDiscoveryTreeNodeType | undefined = nodeTypes
            .find((nodeType: IDiscoveryTreeNodeType) => nodeType.nodeTypeId === nodeTypeId);

        if (nodeType) {
            return Constants.getArtifactsPath + nodeType.icon;
        }
    }

    return "";
}