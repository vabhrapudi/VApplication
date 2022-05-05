// <copyright file="user-persistent-data.ts" company="NPS Foundation">
// Copyright (c) NPS Foundation.
// Licensed under the MIT license.
// </copyright>

import IDiscoveryTreePersistentData from "./discovery-tree-persistent-data";

export default interface IUserPersistentData {
    discoveryTreePersistentData: IDiscoveryTreePersistentData;
}